using System.Reflection;
using AI.Facial.Emotion.Helpers;
using AI.Facial.Emotion.Models;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using System.IO;
using AI.Facial.Emotion.Interface;

namespace AI.Facial.Emotion;

public class EmotionAnalyzer : IEmotionAnalyzer
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

    private EmotionResult Analyze(byte[] imageData, float threshold = 0.5f, float nmsThreshold = 0.3f, int topK = 5000)
    {
        var detectedFaces = _faceDetector.DetectFaces(imageData, threshold, nmsThreshold, topK);

        if (detectedFaces.Count == 0)
        {
            throw new Exception(ErrorMessage.IMG_NO_FACE);
        }

        var faceBox = detectedFaces.First();       
        
        var emotionScores = _emotionRecognizer.PredictEmotion(faceBox);
        return new EmotionResult
        {
            Emotion = emotionScores,
        };
    }
}