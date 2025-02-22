using AI.Facial.Emotion.Models;

namespace AI.Facial.Emotion.Interface;

public interface IEmotionAnalyzer
{
    Task<EmotionResult> AnalyzeEmotionFromUrlAsync(string imageUrl);
    Task<EmotionResult> AnalyzeEmotionFromBase64Async(string base64Image);
    Task<EmotionResult> AnalyzeEmotionFromStreamAsync(Stream fileStream);
}