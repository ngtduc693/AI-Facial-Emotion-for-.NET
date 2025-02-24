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

    public float[][] DetectFaces(Image<Rgb24> image)
    {
        int inputSize = 640;
        var inputTensor = PreprocessImage(image, inputSize);
        
        var inputs = new List<NamedOnnxValue>
        {
            NamedOnnxValue.CreateFromTensor(_session.InputMetadata.Keys.First(), inputTensor)
        };

        using var results = _session.Run(inputs);
        var bboxResults = results
            .Where(r => Regex.IsMatch(r.Name, @"^bbox.*$", RegexOptions.IgnoreCase))
            .Select(r => r.AsTensor<float>().ToArray())
            .ToList();
        var scoreResults = results
            .Where(r => Regex.IsMatch(r.Name, @"^score.*$", RegexOptions.IgnoreCase))
            .Select(r => r.AsTensor<float>().ToArray())
            .ToList();
        var allBboxes = bboxResults.SelectMany(b => b).ToArray();
        var allScores = scoreResults.SelectMany(s => s).ToArray();
        float _scoreThreshold = 0.5f;
        if (allBboxes.Length == 0 || allScores.Length == 0)
        {
            Console.WriteLine("Box and score not valid!");
            return null;
        }
        int numDetections = Math.Min(allBboxes.Length / 4, allScores.Length);

        var boxResults = Enumerable.Range(0, numDetections)
            .Select(i => new { Box = allBboxes.Skip(i * 4).Take(4).ToArray(), Score = allScores[i] })
            .Where(x => x.Score > _scoreThreshold)
            .OrderByDescending(x => x.Score)
            .Select(x => x.Box)
            .ToArray();
        return boxResults;
    }

    private DenseTensor<float> PreprocessImage(Image<Rgb24> image, int size)
    {
        image.Mutate(x => x.Resize(new Size(size, size)));
        var tensor = new DenseTensor<float>(new[] { 1, 3, size, size });

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
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