using AI.Facial.Emotion.Helpers;
using AI.Facial.Emotion.Interface;
using AI.Facial.Emotion.Models;

namespace AI.Facial.Emotion;

public class EmotionAnalyzer : IEmotionAnalyzer
{
    private readonly FaceDetector _faceDetector;
    private readonly EmotionRecognizer _emotionRecognizer;
    private readonly Configuration _configuration;

    public EmotionAnalyzer()
    {
        _faceDetector = new FaceDetector();
        _emotionRecognizer = new EmotionRecognizer();
        _configuration = new Configuration() { Threshold = 0.5f, NmsThreshold = 0.3f, topK = 5000 };
    }
    public EmotionAnalyzer(Configuration configuration)
    {
        _faceDetector = new FaceDetector();
        _emotionRecognizer = new EmotionRecognizer();
        _configuration = configuration;
    }

    public async Task<EmotionResult> AnalyzeEmotionFromUrlAsync(string imageUrl)
    {
        using HttpClient httpClient = new();
        byte[] imageBytes = await httpClient.GetByteArrayAsync(imageUrl);
        return Analyze(imageBytes);
    }

    public Task<EmotionResult> AnalyzeEmotionFromBase64Async(string base64Image)
    {
        byte[] imageBytes = Convert.FromBase64String(base64Image);
        return Task.FromResult(Analyze(imageBytes));
    }

    public async Task<EmotionResult> AnalyzeEmotionFromStreamAsync(Stream fileStream)
    {
        using MemoryStream memoryStream = new();
        await fileStream.CopyToAsync(memoryStream);
        return Analyze(memoryStream.ToArray());
    }

    private EmotionResult Analyze(byte[] imageData)
    {
        List<Emgu.CV.Mat> detectedFaces = _faceDetector.DetectFaces(imageData, _configuration.Threshold, _configuration.NmsThreshold, _configuration.topK);

        if (detectedFaces.Count == 0)
        {
            throw new Exception(ErrorMessage.IMG_NO_FACE);
        }

        Emgu.CV.Mat faceBox = detectedFaces.First();

        string emotionScores = _emotionRecognizer.PredictEmotion(faceBox);
        return new EmotionResult
        {
            Emotion = emotionScores,
        };
    }
}