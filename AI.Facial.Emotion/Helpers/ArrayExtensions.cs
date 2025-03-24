namespace AI.Facial.Emotion.Helpers;

public static class ArrayExtensions
{
    public static int MaxIndex(this float[] scores) => Array.IndexOf(scores, scores.Max());
}