using System;
using System.Linq;
using System.Numerics.Tensors;
using System.Text.RegularExpressions;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using Size = SixLabors.ImageSharp.Size;
namespace WinForms;

public class FaceDetector
{
    private readonly InferenceSession _session;
    private readonly float _scoreThreshold = 0.5f;

    public FaceDetector()
    {
        _session = new InferenceSession(Utils.LoadEmbeddedResource("WinForms.Resources.detection.onnx"));
    }
    
    private float[][] ApplyNMS(float[] allBboxes, float[] allScores, float threshold)
    {
        List<float[]> filteredBoxes = new List<float[]>();
        bool[] suppressed = new bool[allScores.Length];

        for (int i = 0; i < allScores.Length; i++)
        {
            if (suppressed[i]) continue;
            float[] boxA = allBboxes.Skip(i * 4).Take(4).ToArray();
            float scoreA = allScores[i];

            filteredBoxes.Add(boxA);

            for (int j = i + 1; j < allScores.Length; j++)
            {
                float[] boxB = allBboxes.Skip(j * 4).Take(4).ToArray();
                float iou = ComputeIoU(boxA, boxB);
                if (iou > threshold) suppressed[j] = true;
            }
        }

        return filteredBoxes.ToArray();
    }

    private float ComputeIoU(float[] boxA, float[] boxB)
    {
        float x1 = Math.Max(boxA[0], boxB[0]);
        float y1 = Math.Max(boxA[1], boxB[1]);
        float x2 = Math.Min(boxA[2], boxB[2]);
        float y2 = Math.Min(boxA[3], boxB[3]);

        float intersection = Math.Max(0, x2 - x1) * Math.Max(0, y2 - y1);
        float areaA = (boxA[2] - boxA[0]) * (boxA[3] - boxA[1]);
        float areaB = (boxB[2] - boxB[0]) * (boxB[3] - boxB[1]);

        return intersection / (areaA + areaB - intersection);
    }

    public float[][] DetectFaces(Image<Rgb24> image)
    {
        int inputSize = 640;

        image.Mutate(ctx => ctx.Resize(new Size(inputSize, inputSize)));
        var tensor = new DenseTensor<float>(new[] { 1, 3, inputSize, inputSize });

        for (int y = 0; y < inputSize; y++)
        {
            for (int x = 0; x < inputSize; x++)
            {
                var pixel = image[x, y];
                tensor[0, 0, y, x] = (pixel.B - 127.5f) / 128.0f;
                tensor[0, 1, y, x] = (pixel.G - 127.5f) / 128.0f;
                tensor[0, 2, y, x] = (pixel.R - 127.5f) / 128.0f;
            }
        }

        var inputs = new List<NamedOnnxValue>
        {
            NamedOnnxValue.CreateFromTensor(_session.InputMetadata.Keys.First(), tensor)
        };

        using var results = _session.Run(inputs);

     var allBboxes = results
            .Where(r => r.Name.Contains("bbox", StringComparison.OrdinalIgnoreCase))
            .SelectMany(r => r.AsEnumerable<float>())
            .ToArray();

        var allScores = results
            .Where(r => r.Name.Contains("score", StringComparison.OrdinalIgnoreCase))
            .SelectMany(r => r.AsEnumerable<float>())
            .ToArray();

        if (allBboxes.Length == 0 || allScores.Length == 0)
        {
            Console.WriteLine("No valid boxes detected!");
            return Array.Empty<float[]>();
        }

        return ApplyNMS(allBboxes, allScores, 0.4f);
    }
}