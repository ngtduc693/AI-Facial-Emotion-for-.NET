using AI.Facial.Emotion.Models;

namespace AI.Facial.Emotion.Interface;

public interface IEmotionAnalyzer
{
    Task<string> AnalyzeEmotionFromUrlAsync(string imageUrl);
    Task<string> AnalyzeEmotionFromBase64Async(string base64Image);
    Task<string> AnalyzeEmotionFromStreamAsync(Stream fileStream);
}