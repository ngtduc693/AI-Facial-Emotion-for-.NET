using Emgu.CV;

namespace AI.Facial.Emotion.Interface;

internal interface IEmotionRecognizer
{
    string PredictEmotion(Mat faceImage);
}