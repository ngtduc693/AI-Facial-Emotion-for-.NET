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
    private readonly FaceDetector _faceDetector;
    private readonly EmotionRecognizer _emotionDetector;
    public EmotionAnalyzer()
    {
        _faceDetector = new FaceDetector();
        _emotionDetector = new EmotionRecognizer();
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
        var detectedFaces = _faceDetector.DetectFaces(image);

        if (detectedFaces.Length == 0)
            throw new Exception("No face detected");
        
        var faceBox = detectedFaces[0];
        
        int x1 = (faceBox[0] > 1) ? (int)faceBox[0] : (int)(faceBox[0] * image.Width);
        int y1 = (faceBox[1] > 1) ? (int)faceBox[1] : (int)(faceBox[1] * image.Height);
        int x2 = (faceBox[2] > 1) ? (int)faceBox[2] : (int)(faceBox[2] * image.Width);
        int y2 = (faceBox[3] > 1) ? (int)faceBox[3] : (int)(faceBox[3] * image.Height);
        if (x2 <= x1 || y2 <= y1)
            throw new Exception("Face coordinates are invalid");
        
        var faceImage = image.Clone(ctx => ctx.Crop(new Rectangle(x1, y1, x2 - x1, y2 - y1)));   
        faceImage.Mutate(ctx => ctx.Resize(260, 260));
        faceImage.Save("debug_face_image.png");

        var emotionResult = _emotionDetector.PredictEmotion(faceImage);
        
        return new EmotionResult { Emotion = emotionResult};
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
    
    
}