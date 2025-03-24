using Example.gRPC;
using Grpc.Core;
using AI.Facial.Emotion;

namespace Example.gRPC.Services;

public class EmotionDetectorService : EmotionDetector.EmotionDetectorBase
{
    private EmotionAnalyzer _analyzer;

    public EmotionDetectorService() => _analyzer = new EmotionAnalyzer();

    public override async Task AnalyzeEmotionStream(
        IAsyncStreamReader<EmotionRequest> requestStream,
        IServerStreamWriter<EmotionResponse> responseStream,
        ServerCallContext context)
    {
        await foreach (var request in requestStream.ReadAllAsync())
        {
            using var stream = new MemoryStream(request.ImageData.ToByteArray());
            var emotion = await _analyzer.AnalyzeEmotionFromStreamAsync(stream);
            await responseStream.WriteAsync(new EmotionResponse { Emotion = emotion });
        }
    }
}
