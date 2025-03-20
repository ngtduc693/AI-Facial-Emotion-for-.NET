using Example.gRPC;
using Example.WebApps.Models;
using Google.Protobuf;
using Grpc.Core;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Web;

namespace Example.WebApps.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly EmotionDetector.EmotionDetectorClient _client;
        public HomeController(ILogger<HomeController> logger, EmotionDetector.EmotionDetectorClient client)
        {
            _logger = logger;
            _client = client;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [HttpPost]
        public async Task<IActionResult> Analyze()
        {
            try
            {
                using var memoryStream = new MemoryStream();
                await Request.Body.CopyToAsync(memoryStream);
                byte[] imageData = memoryStream.ToArray();

                if (imageData == null || imageData.Length == 0)
                {
                    Console.WriteLine("Image data is null or empty");
                    return BadRequest(new { error = "Image data is null or empty" });
                }

                using var call = _client.AnalyzeEmotionStream();
                await call.RequestStream.WriteAsync(new EmotionRequest
                {
                    ImageData = ByteString.CopyFrom(imageData)
                });
                await call.RequestStream.CompleteAsync();

                string emotion = "Unknown";
                await foreach (var response in call.ResponseStream.ReadAllAsync())
                {
                    emotion = response.Emotion;
                    break;
                }

                return Json(new { emotion });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
