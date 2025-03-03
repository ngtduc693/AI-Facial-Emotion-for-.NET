using Emgu.CV.Dnn;

namespace AI.Facial.Emotion.Models;

public class Configuration
{
    public float Threshold { get; set; }
    public float NmsThreshold { get; set; }
    public int TopK { get; set; }
    public Target TargetHadware { get; set; }
}
