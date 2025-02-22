using System.Reflection;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.IO;
using Image = SixLabors.ImageSharp.Image;
using Point = SixLabors.ImageSharp.Point;
using Rectangle = SixLabors.ImageSharp.Rectangle;

namespace WinForms;

public class EmotionAnalyzer
{
    private readonly InferenceSession _faceDetector;
    private readonly InferenceSession _emotionModel;
    private static readonly string[] EmotionLabels = { "Anger", "Disgust", "Fear", "Happy", "Neutral", "Sad", "Surprise" };

    public EmotionAnalyzer()
    {
        _faceDetector = new InferenceSession(LoadEmbeddedResource("WinForms.Resources.detection.onnx"));
        _emotionModel = new InferenceSession(LoadEmbeddedResource("WinForms.Resources.emotion.onnx"));
    }
    
    public async Task<EmotionResult> AnalyzeEmotionFromUrlAsync(string imageUrl)
    {
        using var httpClient = new HttpClient();
        var imageBytes = await httpClient.GetByteArrayAsync(imageUrl);
        return Analyze(imageBytes);
    }

    public Task<EmotionResult> AnalyzeEmotionFromBase64Async(string base64Image)
    {
        var imageBytes = Convert.FromBase64String(base64Image);
        return Task.FromResult(Analyze(imageBytes));
    }

    public async Task<EmotionResult> AnalyzeEmotionFromStreamAsync(Stream fileStream)
    {
        using var memoryStream = new MemoryStream();
        await fileStream.CopyToAsync(memoryStream);
        return Analyze(memoryStream.ToArray());
    }

    public EmotionResult Analyze(byte[] imageData)
    {
        using var image = Image.Load<Rgb24>(imageData);
        float originalWidth = image.Width;
        float originalHeight = image.Height;
        var inputTensor = PreprocessImageForFaceDetection(image);
        var x = _faceDetector.InputMetadata.Keys.First();
        var faceDetectionResults =
            _faceDetector.Run(new List<NamedOnnxValue> { NamedOnnxValue.CreateFromTensor(_faceDetector.InputMetadata.Keys.First(), inputTensor) });

        var boxes = (DenseTensor<float>)faceDetectionResults.First().AsTensor<float>();
        var scores = (DenseTensor<float>)faceDetectionResults.ElementAt(1).AsTensor<float>();

        if (scores.Length == 0 || boxes.Length == 0)
            return new EmotionResult { Emotion = "No face detected" };

        int bestFaceIndex = Array.IndexOf(scores.ToArray(), scores.Max());
        var faceBox = boxes.ToArray().Skip(bestFaceIndex * 4).Take(4).ToArray();
        
        float scaleX = 640f / originalWidth;
        float scaleY = 640f / originalHeight;
        
        var x1 = (int)(faceBox[0] * originalWidth);
        var y1 = (int)(faceBox[1] * originalHeight);
        var x2 = (int)((faceBox[0] + faceBox[2]) * originalWidth);
        var y2 = (int)((faceBox[1] + faceBox[3]) * originalHeight);

        if (x2 <= x1 || y2 <= y1)
            return new EmotionResult { Emotion = "Invalid face coordinates" };

        var faceImage = image.Clone(ctx => ctx.Crop(new Rectangle(x1, y1, x2 - x1, y2 - y1)));   
        faceImage.Mutate(ctx => ctx.Resize(260, 260));
        faceImage.Save("debug_face_image.png");

        var emotionTensor = PreprocessImageForEmotionModel(faceImage);
        var emotionResults = _emotionModel.Run(new List<NamedOnnxValue> { NamedOnnxValue.CreateFromTensor(_emotionModel.InputMetadata.Keys.First(), emotionTensor) });
        
        var emotionScores = (DenseTensor<float>)emotionResults.First().AsTensor<float>();
        
        float[] softmaxScores = Softmax(emotionScores.ToArray());
        int emotionIndex = Array.IndexOf(softmaxScores, softmaxScores.Max());
        return new EmotionResult { Emotion = EmotionLabels[emotionIndex] };
    }
    
    private float[] Softmax(float[] scores)
    {
        float maxScore = scores.Max();
        float[] expScores = scores.Select(s => (float)Math.Exp(s - maxScore)).ToArray();
        float sumExpScores = expScores.Sum();
        return expScores.Select(s => s / sumExpScores).ToArray();
    }

    private DenseTensor<float> PreprocessImageForFaceDetection(Image<Rgb24> image)
    {
        const int inputSize = 640;
        var resized = ResizeWithPadding(image, inputSize, inputSize);
        var tensor = new DenseTensor<float>(new[] { 1, 3, inputSize, inputSize });

        resized.ProcessPixelRows(accessor =>
        {
            for (int y = 0; y < inputSize; y++)
            {
                var row = accessor.GetRowSpan(y);
                for (int x = 0; x < inputSize; x++)
                {
                    tensor[0, 0, y, x] = ((row[x].R / 255f) - 0.485f) / 0.229f;
                    tensor[0, 1, y, x] = ((row[x].G / 255f) - 0.456f) / 0.224f;
                    tensor[0, 2, y, x] = ((row[x].B / 255f) - 0.406f) / 0.225f;
                }
            }
        });

        return tensor;
    }

    private DenseTensor<float> PreprocessImageForEmotionModel(Image<Rgb24> image)
    {
        var tensor = new DenseTensor<float>(new[] { 1, 3, 260, 260 });

        var resized = image.Clone(ctx => ctx.Resize(260, 260));

        resized.ProcessPixelRows(accessor =>
        {
            for (int y = 0; y < 260; y++)
            {
                var row = accessor.GetRowSpan(y);
                for (int x = 0; x < 260; x++)
                {
                    tensor[0, 0, y, x] = ((row[x].R / 255f) - 0.485f) / 0.229f;
                    tensor[0, 1, y, x] = ((row[x].G / 255f) - 0.456f) / 0.224f;
                    tensor[0, 2, y, x] = ((row[x].B / 255f) - 0.406f) / 0.225f;
                }
            }
        });

        return tensor;
    }
    
    private Image<Rgb24> ResizeWithPadding(Image<Rgb24> image, int targetWidth, int targetHeight)
    {
        float imageRatio = (float)image.Height / image.Width;
        float targetRatio = (float)targetHeight / targetWidth;
    
        int newWidth, newHeight;
        if (imageRatio > targetRatio)
        {
            newHeight = targetHeight;
            newWidth = (int)(targetHeight / imageRatio);
        }
        else
        {
            newWidth = targetWidth;
            newHeight = (int)(targetWidth * imageRatio);
        }

        var resized = image.Clone(ctx => ctx.Resize(newWidth, newHeight));
        var padded = new Image<Rgb24>(targetWidth, targetHeight);
        padded.Mutate(ctx => ctx.DrawImage(resized, new Point((targetWidth - newWidth) / 2, (targetHeight - newHeight) / 2), 1f));

        return padded;
    }
    
    private static byte[] LoadEmbeddedResource(string resourceName)
    {
        var assembly = Assembly.GetExecutingAssembly();
        using var stream = assembly.GetManifestResourceStream(resourceName);
        if (stream == null)
            throw new FileNotFoundException($"Embedded resource {resourceName} not found.");

        using var memoryStream = new MemoryStream();
        stream.CopyTo(memoryStream);
        return memoryStream.ToArray();
    }
}