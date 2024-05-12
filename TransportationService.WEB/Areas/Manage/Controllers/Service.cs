using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TransportationService.WEB.Areas.Manage.Models;
using TransportationService.WEB.Data.Entities;
using TransportationService.WEB.Data;

namespace TransportationService.WEB.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class ServiceController : Controller
    {
        private readonly ApplicationDbContext<User> _context;
        public ServiceController(ApplicationDbContext<User> context)
        {
            _context = context;
        }
        [HttpGet]
        [Authorize(Roles = "Administrator,Manager")]
        public IActionResult Add()
        {
            return View(new AddServiceViewModel());
        }
        [HttpPost]
        [Authorize(Roles = "Administrator,Manager")]
        public async Task<IActionResult> Add(AddServiceViewModel vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _context.Cargos.AddAsync(new() { Name = vm.Name, Price = vm.Price, RemainQuantity = vm.Quantity });
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Service", "Home", new { area = string.Empty });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(vm);
        }
        [HttpPost]
        [Authorize(Roles = "Administrator,Manager")]
        public async Task<IActionResult> Delete(Guid id)
        {
            Cargo? cargo = await _context.Cargos.FindAsync(id);
            if (cargo == null) return NotFound();
            try
            {
                _context.Cargos.Remove(cargo);
                await _context.SaveChangesAsync();
                return RedirectToAction("Service", "Home", new { area = string.Empty });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Authorize(Roles = "Administrator,Manager")]
        public async Task<IActionResult> Edit(Guid id)
        {
            Cargo? cargo = await _context.Cargos.FindAsync(id);
            if (cargo == null) return NotFound();
            return View(cargo);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator,Manager")]
        public async Task<IActionResult> Edit(Cargo model)
        {
            Cargo? cargo = await _context.Cargos.FindAsync(model.Id);
            if (cargo == null) return NotFound();
            cargo.Name = model.Name;
            cargo.Price = model.Price;
            cargo.RemainQuantity = model.RemainQuantity;
            _context.Entry(cargo).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return RedirectToAction("Service", "Home", new { area = string.Empty });
        }
    }
}
