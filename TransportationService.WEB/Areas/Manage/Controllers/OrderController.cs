using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TransportationService.WEB.Data.Entities;
using TransportationService.WEB.Data;
using Microsoft.EntityFrameworkCore;
using TransportationService.WEB.Helpers;
using TransportationService.WEB.Data.Enums;

namespace TransportationService.WEB.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext<User> _context;
        private readonly UserManager<User> _userManager;
        public OrderController(ApplicationDbContext<User> context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        //TODO : MAKE IT WORK
        [Authorize(Roles = "Administrator,Manager")]
        public async Task<IActionResult> Cargo(
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

            var orders = await _context.CargoOrders.ToListAsync();
            if (!string.IsNullOrEmpty(searchString))
            {
                orders = orders.Where(o => o.Number.Contains(searchString) || o.Customer.PhoneNumber.Contains(searchString)).ToList();
            }
            int pageSize = 6;
            return View(await PaginatedList<CargoOrder>.CreateAsync(orders, pageNumber ?? 1, pageSize));
        }
        [Authorize(Roles = "Administrator,Manager")]
        public async Task<IActionResult> Transport(
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

            var orders = await _context.TransportOrders.ToListAsync();
            if (!string.IsNullOrEmpty(searchString))
            {
                orders = orders.Where(o => o.Number.Contains(searchString) || o.Customer.PhoneNumber.Contains(searchString)).ToList();
            }
            int pageSize = 6;
            return View(await PaginatedList<TransportOrder>.CreateAsync(orders, pageNumber ?? 1, pageSize));
        }
        [HttpPost]
        public async Task<IActionResult> ChangeStatus(string? id, int newStatus)
        {
            if (id == null) return BadRequest();
            CargoOrder? cargo = await _context.CargoOrders.FindAsync(id);
            if (cargo != null)
            {
                cargo.Status = (Status)newStatus;
                _context.Entry(cargo).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok();
            }
            TransportOrder? transport = await _context.TransportOrders.FindAsync(id);
            if (transport != null)
            {
                transport.Status = (Status)newStatus;
                _context.Entry(transport).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok();
            }
            return BadRequest();
        }
    }
}
