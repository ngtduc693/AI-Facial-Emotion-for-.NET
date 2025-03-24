using Emgu.CV;

namespace AI.Facial.Emotion.Helpers;

public static class MatExtensions
{
    public static Mat Also(this Mat mat, Action<Mat> action)
    {
        action(mat);
        return mat;
    }
}