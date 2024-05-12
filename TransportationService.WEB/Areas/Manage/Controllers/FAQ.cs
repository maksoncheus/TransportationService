using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TransportationService.WEB.Areas.Manage.Models;
using TransportationService.WEB.Data;
using TransportationService.WEB.Data.Entities;

namespace TransportationService.WEB.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class FAQController : Controller
    {
        private readonly ApplicationDbContext<User> _context;
        public FAQController(ApplicationDbContext<User> context)
        {
            _context = context;
        }
        [HttpGet]
        [Authorize(Roles = "Administrator,Manager")]
        public IActionResult Add()
        {
            return View(new AddFAQViewModel());
        }
        [HttpPost]
        [Authorize(Roles = "Administrator,Manager")]
        public async Task<IActionResult> Add(AddFAQViewModel vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _context.SupportFAQ.AddAsync(new() { Question = vm.Question, Answer = vm.Answer });
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Support", "Home", new { area = string.Empty });
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
        public async Task<IActionResult> Delete(int id)
        {
            SupportFAQ? fAQ = await _context.SupportFAQ.FindAsync(id);
            if (fAQ == null) return NotFound();
            try
            {
                _context.SupportFAQ.Remove(fAQ);
                await _context.SaveChangesAsync();
                return RedirectToAction("Support", "Home", new { area = string.Empty });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Authorize(Roles = "Administrator,Manager")]
        public async Task<IActionResult> Edit(int id)
        {
            SupportFAQ? fAQ = await _context.SupportFAQ.FindAsync(id);
            if (fAQ == null) return NotFound();
            return View(fAQ);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator,Manager")]
        public async Task<IActionResult> Edit(SupportFAQ model)
        {
            SupportFAQ? fAQ = await _context.SupportFAQ.FindAsync(model.Id);
            if (fAQ == null) return NotFound();
            fAQ.Question = model.Question;
            fAQ.Answer = model.Answer;
            _context.Entry(fAQ).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return RedirectToAction("Support", "Home", new { area = string.Empty });
        }
    }
}
