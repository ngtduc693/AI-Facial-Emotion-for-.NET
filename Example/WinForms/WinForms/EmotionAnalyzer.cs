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
    private readonly EmotionRecognizer _emotionRecognizer;
    public EmotionAnalyzer()
    {
        _faceDetector = new FaceDetector();
        _emotionRecognizer = new EmotionRecognizer();
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
        {
            return new EmotionResult { };
        }

        var faceBox = detectedFaces[0];

        bool isRelative = faceBox[0] < 1.0f && faceBox[1] < 1.0f;

        float x1, y1, x2, y2;
        if (isRelative)
        {
            x1 = (faceBox[0] - faceBox[2] / 2) * image.Width;
            y1 = (faceBox[1] - faceBox[3] / 2) * image.Height;
            x2 = (faceBox[0] + faceBox[2] / 2) * image.Width;
            y2 = (faceBox[1] + faceBox[3] / 2) * image.Height;
        }
        else
        {
            x1 = faceBox[0];
            y1 = faceBox[1];
            x2 = faceBox[2];
            y2 = faceBox[3];
        }

        int x1_int = Math.Max(0, Math.Min((int)((faceBox[0] - (faceBox[2] / 2)) * image.Width), image.Width - 1));
        int y1_int = Math.Max(0, Math.Min((int)((faceBox[1] - (faceBox[3] / 2)) * image.Height), image.Height - 1));
        int x2_int = Math.Max(0, Math.Min((int)((faceBox[0] + (faceBox[2] / 2)) * image.Width), image.Width - 1));
        int y2_int = Math.Max(0, Math.Min((int)((faceBox[1] + (faceBox[3] / 2)) * image.Height), image.Height - 1));

        var faceImage = image.Clone(ctx => ctx.Crop(new Rectangle(
            x1_int, 
            y1_int, 
            Math.Max(1, x2_int - x1_int),
            Math.Max(1, y2_int - y1_int) 
        )));

        faceImage.Save("debug.jpg");
        
        var emotionScores = _emotionRecognizer.PredictEmotion(faceImage);
        return new EmotionResult
        {
            Emotion = emotionScores,
        };
    }
}