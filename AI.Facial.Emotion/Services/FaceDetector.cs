using AI.Facial.Emotion.Helpers;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Dnn;
using System.Drawing;

namespace AI.Facial.Emotion;

public class FaceDetector
{
    private const int RequiredSize = 320;

    public List<Mat> DetectFaces(byte[] imageBytes, float threshold, float nmsThreshold, int topK)
    {
        using var image = new Mat();

        CvInvoke.Imdecode(imageBytes, ImreadModes.Color, image);

        if (image.IsEmpty)
        {
            throw new Exception(ErrorMessage.IMG_COULD_LOAD);
        }
        if (image.Width != image.Height)
        {
            throw new Exception(ErrorMessage.IMG_INVALID_RATIO +
                              $"{image.Width}x{image.Height}");
        }
        Mat processedImage;
        if (image.Width != RequiredSize || image.Height != RequiredSize)
        {
            processedImage = new Mat();
            CvInvoke.Resize(image, processedImage, new Size(RequiredSize, RequiredSize));
        }
        else
        {
            processedImage = image;
        }

        using var model = InitializeFaceDetectionModel(new Size(RequiredSize, RequiredSize), threshold, nmsThreshold, topK);
        var faces = new Mat();
        model.Detect(processedImage, faces);

        var facesList = CropFaces(processedImage, faces);

        if (processedImage != image)
        {
            processedImage.Dispose();
        }
        return facesList;
    }

    private FaceDetectorYN InitializeFaceDetectionModel(Size inputSize, float threshold, float nmsThreshold, int topK)
    {
        return new FaceDetectorYN(
            model: Utils.ExtractEmbeddedResource("AI.Facial.Emotion.Resources.detectionv2.onnx"),
            config: string.Empty,
            inputSize: inputSize,
            scoreThreshold: threshold,
            nmsThreshold: nmsThreshold,
            topK: topK,
            backendId: Emgu.CV.Dnn.Backend.Default,
            targetId: Target.Cpu);
    }

    private List<Mat> CropFaces(Mat image, Mat faces)
    {
        var faceImage = new List<Mat>();
        if (faces == null || faces.IsEmpty || faces.Rows <= 0)
        {
            throw new Exception(ErrorMessage.IMG_NO_FACE);
        }

        var facesData = (float[,])faces.GetData(jagged: true);

        for (var i = 0; i < facesData.GetLength(0); i++)
        {
            int x = (int)facesData[i, 0];
            int y = (int)facesData[i, 1];
            int width = (int)facesData[i, 2];
            int height = (int)facesData[i, 3];
            float confidence = facesData[i, 14];

            x = Math.Max(0, x);
            y = Math.Max(0, y);
            width = Math.Min(width, image.Width - x);
            height = Math.Min(height, image.Height - y);

            var faceRectangle = new Rectangle(x, y, width, height);
            faceImage.Add(new Mat(image, faceRectangle));
        }
        return faceImage;
    }
}