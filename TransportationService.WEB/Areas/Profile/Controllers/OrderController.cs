using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TransportationService.WEB.Data.Entities;
using TransportationService.WEB.Data;
using TransportationService.WEB.Services;

namespace TransportationService.WEB.Areas.Profile.Controllers
{
    [Area("Profile")]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext<User> _context;
        private readonly UserManager<User> _userManager;
        public OrderController(ApplicationDbContext<User> context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [Authorize]
        public IActionResult All()
        {
            return View();
        }
    }
}
