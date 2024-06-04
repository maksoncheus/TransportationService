using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TransportationService.WEB.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class MainController : Controller
    {
        [Authorize(Roles = "Administrator,Manager")]
        public IActionResult Index()
        {
            return View();
        }
    }
}