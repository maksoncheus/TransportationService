using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TransportationService.WEB.Data;
using TransportationService.WEB.Data.Entities;
using TransportationService.WEB.Helpers;

namespace TransportationService.WEB.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext<User> _context;
        private readonly UserManager<User> _userManager;
        public UsersController(ApplicationDbContext<User> context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Authorize(Roles = "Administrator, Manager")]
        public async Task<IActionResult> Index(
            string currentFilter,
            string searchString,
            int? pageNumber)
        {

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var users = await _context.Users.ToListAsync();
            if (!string.IsNullOrEmpty(searchString))
            {
                users = users.Where(s => s.Email.Contains(searchString) || s.PhoneNumber.Contains(searchString)).ToList();
            }
            int pageSize = 6;
            return View(await PaginatedList<User>.CreateAsync(users, pageNumber ?? 1, pageSize));
        }
        [HttpPost]
        public async Task<IActionResult> Promote(string id)
        {
            try
            {
                User? user = await _context.Users.FindAsync(id);
                if (user == null) return BadRequest("Пользователь не найден");
                if (!await _userManager.IsInRoleAsync(user, "User"))
                {
                    return BadRequest("Пользователя нельзя повысить");
                }
                await _userManager.RemoveFromRoleAsync(user, "User");
                await _userManager.AddToRoleAsync(user, "Manager");
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Demote(string id)
        {
            try
            {
                User? user = await _context.Users.FindAsync(id);
                if (user == null) return BadRequest("Пользователь не найден");
                if (!await _userManager.IsInRoleAsync(user, "Manager"))
                {
                    return BadRequest("Пользователя нельзя понизить");
                }
                await _userManager.RemoveFromRoleAsync(user, "Manager");
                await _userManager.AddToRoleAsync(user, "User");
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
