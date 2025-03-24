using AI.Facial.Emotion.Helpers;
using AI.Facial.Emotion.Models;
using Emgu.CV;
using Emgu.CV.CvEnum;
using System.Drawing;
using System.Reflection;

namespace AI.Facial.Emotion;

internal class FaceDetector
{
    private const int RequiredSize = 320;


    public List<Mat> DetectFaces(ReadOnlySpan<byte> imageBytes, Configuration configuration)
    {
        using var image = new Mat();
        CvInvoke.Imdecode(imageBytes.ToArray(), ImreadModes.Color, image);

        if (image.IsEmpty) throw new Exception(ErrorMessage.IMG_COULD_LOAD);
        if (image.Width != image.Height) throw new Exception($"{ErrorMessage.IMG_INVALID_RATIO} {image.Width}x{image.Height}");

        using var processedImage = image.Width != RequiredSize || image.Height != RequiredSize
            ? (new Mat().Also(m => CvInvoke.Resize(image, m, new Size(RequiredSize, RequiredSize))))
            : image;

        using var model = InitializeFaceDetectorModel(new Size(RequiredSize, RequiredSize), configuration);
        using var faces = new Mat();
        model.Detect(processedImage, faces);

        return CropFaces(processedImage, faces);
    }

    private static readonly Lazy<string> _modelPath = new(() =>
    {
        var resourceName = ModelResourceNames.FaceDetectionModel;
        var tempPath = Path.Combine(Path.GetTempPath(), "detectionv2.onnx");
        if (!File.Exists(tempPath))
        {
            var assembly = Assembly.GetExecutingAssembly();
            var modelData = assembly.LoadEmbeddedResource(resourceName);
            File.WriteAllBytes(tempPath, modelData);
        }
        return tempPath;
    });

    private FaceDetectorYN InitializeFaceDetectorModel(Size inputSize, Configuration configuration) =>
        new(
            _modelPath.Value,
            string.Empty,
            inputSize,
            configuration.Threshold,
            configuration.NmsThreshold,
            configuration.TopK,
            Emgu.CV.Dnn.Backend.Default,
            configuration.TargetHadware);

    private List<Mat> CropFaces(Mat image, Mat faces)
    {
        if (faces is null || faces.IsEmpty || faces.Rows <= 0) throw new Exception(ErrorMessage.IMG_NO_FACE);

        var facesData = (float[,])faces.GetData(jagged: true);
        var faceImages = new List<Mat>(facesData.GetLength(0));

        for (int i = 0; i < facesData.GetLength(0); i++)
        {
            int x = Math.Max(0, (int)facesData[i, 0]);
            int y = Math.Max(0, (int)facesData[i, 1]);
            int width = Math.Min((int)facesData[i, 2], image.Width - x);
            int height = Math.Min((int)facesData[i, 3], image.Height - y);

            faceImages.Add(new Mat(image, new Rectangle(x, y, width, height)));
        }
        return faceImages;
    }
}