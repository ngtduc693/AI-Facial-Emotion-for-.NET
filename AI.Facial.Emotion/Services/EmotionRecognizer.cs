using AI.Facial.Emotion.Helpers;
using AI.Facial.Emotion.Interface;
using AI.Facial.Emotion.Models;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using System.Drawing;
using System.Reflection;

namespace AI.Facial.Emotion;

internal class EmotionRecognizer : IEmotionRecognizer
{
    private readonly string[] _emotionLabels = { "ANGER", "DISGUST", "FEAR", "HAPPINESS", "NEUTRAL", "SADNESS", "SURPRISE" };

    private static readonly Lazy<InferenceSession> _session = new(() =>
    {
        var assembly = Assembly.GetExecutingAssembly();
        var modelData = assembly.LoadEmbeddedResource(ModelResourceNames.EmotionModel);
        return new InferenceSession(modelData);
    });

    public string PredictEmotion(Mat faceImage)
    {
        var inputTensor = PreprocessFace(faceImage);
        var inputs = new List<NamedOnnxValue> { NamedOnnxValue.CreateFromTensor(_session.Value.InputMetadata.Keys.First(), inputTensor) };
        using var results = _session.Value.Run(inputs);
        var scores = results.First().AsEnumerable<float>().ToArray();
        return _emotionLabels[Softmax(scores).MaxIndex()];
    }

    private DenseTensor<float> PreprocessFace(Mat faceImage)
    {
        using var resizedImage = new Mat();
        CvInvoke.Resize(faceImage, resizedImage, new Size(260, 260));

        using var rgbImage = new Mat();
        CvInvoke.CvtColor(resizedImage, rgbImage, faceImage.NumberOfChannels == 1 ? ColorConversion.Gray2Rgb : ColorConversion.Bgr2Rgb);

        if (faceImage.NumberOfChannels is not (1 or 3)) throw new Exception(ErrorMessage.IMG_UNSUPPORTED);

        var tensor = new DenseTensor<float>([1, 3, 260, 260]);
        ReadOnlySpan<byte> imageData = rgbImage.ToImage<Rgb, byte>().Bytes;

        for (int y = 0; y < 260; y++)
            for (int x = 0; x < 260; x++)
            {
                int index = (y * 260 + x) * 3;
                tensor[0, 0, y, x] = imageData[index] / 255f;
                tensor[0, 1, y, x] = imageData[index + 1] / 255f;
                tensor[0, 2, y, x] = imageData[index + 2] / 255f;
            }

        return tensor;
    }

    private static float[] Softmax(float[] scores)
    {
        float max = scores.Max(), sum = scores.Sum(s => MathF.Exp(s - max));
        return scores.Select(s => MathF.Exp(s - max) / sum).ToArray();
    }
}