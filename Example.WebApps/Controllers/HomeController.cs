using Example.WebApps.Models;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Web;
using AI.Facial.Emotion;

namespace Example.WebApps.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly EmotionAnalyzer _emotionAnalyzer;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _emotionAnalyzer = new EmotionAnalyzer();
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
        public async Task<JsonResult> UploadImageAsync([FromBody] ImageUploadModel model)
        {
            try
            {
                if (model?.ImageData != null)
                {
                    string base64Data = model.ImageData.Split(',')[1];
                    var result =_emotionAnalyzer.AnalyzeEmotionFromBase64Async(base64Data).Result.Emotion;

                    return Json(new { success = true, message = "Emotion is " + result });
                }
                return Json(new { success = false, message = "Invalid data!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }
    }
}
