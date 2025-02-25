using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace AI.Facial.Emotion.Interface;

internal interface IEmotionRecognizer
{
    string PredictEmotion(Image<Rgb24> faceImage);
}