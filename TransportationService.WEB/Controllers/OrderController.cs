using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using TransportationService.WEB.Data;
using TransportationService.WEB.Data.Entities;
using TransportationService.WEB.Data.Enums;
using TransportationService.WEB.Models;

namespace TransportationService.WEB.Controllers
{
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext<User> _context;
        private readonly UserManager<User> _userManager;
        public OrderController(ApplicationDbContext<User> context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        
        public async Task<IActionResult> Cargo(Guid cargoId)
        {
            Cargo? cargo = await _context.Cargos.FindAsync(cargoId);
            if(cargo == null) return NotFound();
            return View(new CargoOrderViewModel() { CargoId = cargo.Id});
        }
        [HttpPost]
        public async Task<IActionResult> CargoOrder(CargoOrderViewModel model)
        {
            Cargo? cargo = await _context.Cargos.FindAsync(model.CargoId);
            if (cargo == null) return NotFound();

            if (double.TryParse(model.Weight, out double weight))
            {
                
                if (weight <= 0) ModelState.AddModelError(string.Empty, "Масса должна быть положительной");

                if (weight > cargo.RemainQuantity)
                    ModelState.AddModelError(string.Empty, "На складе нет столько товара");
            }
            else ModelState.AddModelError(string.Empty, "Введенная масса некорректна");
            User? user = null;
            if (User.Identity.IsAuthenticated)
            {
                user = await _userManager.GetUserAsync(User);
            }
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Сначала авторизуйтесь");
            }
            
            if(!ModelState.IsValid) return View("Cargo", model);
            try
            {
                string orderId = "DLVR-" + Guid.NewGuid().ToString();
                CargoOrder order = new()
                {
                    Number = orderId,
                    Weight = weight,
                    DeliveryAddress = model.DeliveryAddress,
                    DeliveryDate = model.DeliveryDate,
                    DeliveryMinTime = model.DeliveryMinTime,
                    DeliveryMaxTime = model.DeliveryMaxTime,
                    Cargo = cargo,
                    Customer = user,
                    Price = model.Price,
                    Status = Status.Ordered
                };
                _context.CargoOrders.Add(order);
                cargo.RemainQuantity -= weight;
                _context.Entry(cargo).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                await _context.SaveChangesAsync();
                return RedirectToAction("Message", "Home", new { area = "", msg = $"Номер вашего заказа: {order.Number}. Вы можете отследить его выполнение на вкладке \"Мои заказы\"" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
