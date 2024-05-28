using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TransportationService.WEB.Models;

namespace TransportationService.WEB.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Service()
        {
            return View();
        }
        public IActionResult Support()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Message(string msg)
        {
            return View(model: msg);
        }
        public IActionResult GetMessage(string msg)
        {
            return Content(msg, "text/html");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
