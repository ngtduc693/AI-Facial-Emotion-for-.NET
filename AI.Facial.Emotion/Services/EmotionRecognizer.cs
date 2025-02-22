using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using AI.Facial.Emotion.Helpers;
using AI.Facial.Emotion.Interface;
using Emgu.CV.CvEnum;
using Emgu.CV;
using Emgu.CV.Structure;

namespace AI.Facial.Emotion;

internal class EmotionRecognizer : IEmotionRecognizer
{
    private readonly InferenceSession _session;
    private readonly string[] _emotionLabels = { "ANGER", "DISGUST", "FEAR", "HAPPINESS", "NEUTRAL", "SADNESS", "SURPRISE" };

    public EmotionRecognizer()
    {
        _session = new InferenceSession(Utils.LoadEmbeddedResource("AI.Facial.Emotion.Resources.emotion.onnx"));
    }

    public string PredictEmotion(Mat faceImage)
    {
        var inputTensor = PreprocessFace(faceImage);
        var inputs = new List<NamedOnnxValue>
        {
            NamedOnnxValue.CreateFromTensor(_session.InputMetadata.Keys.First(), inputTensor)
        };

        using var results = _session.Run(inputs);
        var scores = results.First().AsEnumerable<float>().ToArray();
        scores = Softmax(scores);
        int maxIndex = Array.IndexOf(scores, scores.Max());
        return _emotionLabels[maxIndex];
    }

    private DenseTensor<float> PreprocessFace(Mat faceImage)
    {
        Mat resizedImage = new Mat();
        CvInvoke.Resize(faceImage, resizedImage, new System.Drawing.Size(260, 260));

        Mat rgbImage = new Mat();
        if (faceImage.NumberOfChannels == 1)
        {
            CvInvoke.CvtColor(resizedImage, rgbImage, ColorConversion.Gray2Rgb);
        }
        else if (faceImage.NumberOfChannels == 3)
        {
            CvInvoke.CvtColor(resizedImage, rgbImage, ColorConversion.Bgr2Rgb);
        }
        else
        {
            throw new Exception(ErrorMessage.IMG_UNSUPPORTED);
        }

        var tensor = new DenseTensor<float>(new[] { 1, 3, 260, 260 });

        byte[] imageData = rgbImage.ToImage<Rgb, byte>().Bytes;
        int width = rgbImage.Width;
        int height = rgbImage.Height;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int index = (y * width + x) * 3;
                tensor[0, 0, y, x] = imageData[index] / 255f;
                tensor[0, 1, y, x] = imageData[index + 1] / 255f;
                tensor[0, 2, y, x] = imageData[index + 2] / 255f;
            }
        }

        resizedImage.Dispose();
        rgbImage.Dispose();

        return tensor;
    }

    private float[] Softmax(float[] scores)
    {
        float max = scores.Max();
        float sum = scores.Sum(s => (float)Math.Exp(s - max));
        return scores.Select(s => (float)Math.Exp(s - max) / sum).ToArray();
    }
}