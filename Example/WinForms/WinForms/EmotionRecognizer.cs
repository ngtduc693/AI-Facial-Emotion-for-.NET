using System;
using System.Linq;
using System.Numerics.Tensors;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using Size = SixLabors.ImageSharp.Size;
using System.Reflection;
using Microsoft.ML.OnnxRuntime;
namespace WinForms;

public class EmotionRecognizer
{
    private readonly InferenceSession _session;
    private readonly string[] _emotionLabels = { "ANGER", "DISGUST", "FEAR", "HAPPINESS", "NEUTRAL", "SADNESS", "SURPRISE" };

    public EmotionRecognizer()
    {
        _session = new InferenceSession(Utils.LoadEmbeddedResource("WinForms.Resources.emotion.onnx"));
    }

    public string PredictEmotion(Image<Rgb24> faceImage)
    {
        var inputTensor = PreprocessFace(faceImage);
        var inputs = new List<NamedOnnxValue>
        {
            NamedOnnxValue.CreateFromTensor(_session.InputMetadata.Keys.First(), inputTensor)
        };

        using var results = _session.Run(inputs);
        var scores = results.First().AsEnumerable<float>().ToArray();
        int maxIndex = Array.IndexOf(scores, scores.Max());
        return _emotionLabels[maxIndex];
    }

    private DenseTensor<float> PreprocessFace(Image<Rgb24> image)
    {
        image.Mutate(x => x.Resize(new Size(260, 260)));
        var tensor = new DenseTensor<float>(new[] { 1, 3, 260, 260 });

        for (int y = 0; y < 260; y++)
        {
            for (int x = 0; x < 260; x++)
            {
                var pixel = image[x, y];
                tensor[0, 0, y, x] = pixel.R / 255f;
                tensor[0, 1, y, x] = pixel.G / 255f;
                tensor[0, 2, y, x] = pixel.B / 255f;
            }
        }
        return tensor;
    }
}