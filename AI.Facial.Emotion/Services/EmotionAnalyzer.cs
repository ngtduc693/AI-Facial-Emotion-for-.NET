using AI.Facial.Emotion.Helpers;
using AI.Facial.Emotion.Interface;
using AI.Facial.Emotion.Models;
using Emgu.CV.Dnn;
namespace AI.Facial.Emotion;

public class EmotionAnalyzer : IEmotionAnalyzer
{
    private readonly FaceDetector _faceDetector = new();
    private readonly EmotionRecognizer _emotionRecognizer = new();
    private readonly Configuration _configuration;

    public EmotionAnalyzer(Configuration? configuration = null) =>
        _configuration = configuration ?? new() { Threshold = 0.5f, NmsThreshold = 0.3f, TopK = 5000, TargetHadware = Target.Cpu };

    public async Task<string> AnalyzeEmotionFromUrlAsync(string imageUrl) =>
        Analyze(await new HttpClient().GetByteArrayAsync(imageUrl));

    public Task<string> AnalyzeEmotionFromBase64Async(string base64Image) =>
        Task.FromResult(Analyze(Convert.FromBase64String(base64Image)));

    public async Task<string> AnalyzeEmotionFromStreamAsync(Stream fileStream) =>
        Analyze(await ReadToEndAsArrayAsync(fileStream));

    private string Analyze(byte[] imageData)
    {
        var detectedFaces = _faceDetector.DetectFaces(imageData, _configuration);
        if (detectedFaces.Count == 0) throw new Exception(ErrorMessage.IMG_NO_FACE);

        return _emotionRecognizer.PredictEmotion(detectedFaces[0]);
    }
    private static async Task<byte[]> ReadToEndAsArrayAsync(Stream stream)
    {
        using var memoryStream = new MemoryStream();
        await stream.CopyToAsync(memoryStream);
        return memoryStream.ToArray();
    }
}