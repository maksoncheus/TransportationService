using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.Globalization;
using TransportationService.WEB.Data;
using TransportationService.WEB.Data.Entities;
using TransportationService.WEB.Data.Enums;
using TransportationService.WEB.Models;
using TransportationService.WEB.Services;

namespace TransportationService.WEB.Controllers
{
    /// <summary>
    /// Контроллер "Заказы". Содержит методы для просмотра, добавления, удаления и редактирования заказов.
    /// </summary>
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext<User> _context;
        private readonly UserManager<User> _userManager;
        private readonly MailService _mailService;
        public OrderController(ApplicationDbContext<User> context, UserManager<User> userManager, MailService mailService)
        {
            _context = context;
            _userManager = userManager;
            _mailService = mailService;
        }
        public async Task<double> GetCargoPrice(Guid id)
        {
            Cargo? cargo = await _context.Cargos.FindAsync(id);
            return cargo.Price;
        }
        public async Task<double> GetCargoQuantity(Guid id)
        {
            Cargo? cargo = await _context.Cargos.FindAsync(id);
            return cargo.RemainQuantity;
        }
        /// <summary>
        /// Страница оформления заказа на доставку определенного груза.
        /// </summary>
        /// <param name="cargoId">ID груза.</param>
        public async Task<IActionResult> Cargo(Guid? cargoId = null)
        {
            if (cargoId == null) { return View(new CargoOrderViewModel()); }
            Cargo? cargo = await _context.Cargos.FindAsync(cargoId);
            if (cargo == null) return NotFound();
            return View(new CargoOrderViewModel() { CargoId = cargo.Id });
        }
        /// <summary>
        /// Оформить заказ на доставку определенного груза.
        /// </summary>
        /// <param name="model">Модель представления "<see cref="CargoOrderViewModel">Заказ на доставку</see>"</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CargoOrder(CargoOrderViewModel model)
        {
            Cargo? cargo = await _context.Cargos.FindAsync(model.CargoId);
            if (cargo == null) return NotFound();
            //HTML плохо работает с дробными числами. Поэтому получаем вес как строку и пытаемся преобразовать в тип System.Double
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

            if (!ModelState.IsValid) return View("Cargo", model);
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
                    Price = double.Parse(model.Price, CultureInfo.InvariantCulture),
                    Status = Status.Ordered,
                    TimeStamp = DateTime.Now
                };
                _context.CargoOrders.Add(order);
                cargo.RemainQuantity -= weight;
                _context.Entry(cargo).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                await _context.SaveChangesAsync();
                _mailService.SendOrderMessage(Url.Action("Index", "Home", new { area=""}, protocol: HttpContext.Request.Scheme),order.Number, order.Customer.Email);
                return RedirectToAction("Message", "Home", new { area = "", msg = $"<h3>Номер вашего заказа: <strong>{order.Number}</strong>. Вы можете отследить его выполнение на вкладке \"Мои заказы\"</h3>" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        public IActionResult Transport(string fromAddress = "", string toAddress = "")
        {
            return View(new TransportOrderViewModel() { FromAddress = fromAddress, ToAddress = toAddress });
        }
        [HttpPost]
        public async Task<IActionResult> TransportOrder(TransportOrderViewModel model)
        {
            if (double.TryParse(model.Width, out double width))
            {
                if (width <= 0) ModelState.AddModelError(string.Empty, "Ширина должна быть положительной");
            }
            else ModelState.AddModelError(string.Empty, "Введенная ширина некорректна");

            if (double.TryParse(model.Length, out double length))
            {
                if (length <= 0) ModelState.AddModelError(string.Empty, "Длина должна быть положительной");
            }
            else ModelState.AddModelError(string.Empty, "Введенная длина некорректна");

            if (double.TryParse(model.Height, out double height))
            {
                if (height <= 0) ModelState.AddModelError(string.Empty, "Высота должна быть положительной");
            }
            else ModelState.AddModelError(string.Empty, "Введенная высота некорректна");

            if (double.TryParse(model.Weight, out double weight))
            {
                if (weight <= 0) ModelState.AddModelError(string.Empty, "Масса должна быть положительной");
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

            if (!ModelState.IsValid) return View("Transport", model);
            try
            {
                string orderId = "TNRT-" + Guid.NewGuid().ToString();
                TransportOrder order = new()
                {
                    Number = orderId,
                    Width = width,
                    Height = height,
                    Length = length,
                    Weight = weight,
                    FromAddress = model.FromAddress,
                    ToAddress = model.ToAddress,
                    DeliveryDate = model.DeliveryDate,
                    DeliveryMinTime = model.DeliveryMinTime,
                    DeliveryMaxTime = model.DeliveryMaxTime,
                    Customer = user,
                    Price = double.Parse(model.Price, CultureInfo.InvariantCulture),
                    Status = Status.Ordered,
                    TimeStamp = DateTime.Now
                };
                _context.TransportOrders.Add(order);
                await _context.SaveChangesAsync();
                _mailService.SendOrderMessage(Url.Action("Index", "Home", new { area=""}, protocol: HttpContext.Request.Scheme),order.Number, order.Customer.Email);
                return RedirectToAction("Message", "Home", new { area = "", msg = $"<h3>Номер вашего заказа: <strong>{order.Number}</strong>. Вы можете отследить его выполнение на вкладке \"Мои заказы\"</h3>" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<string> GetOrder(string orderId)
        {
            CargoOrder? cargo = await _context.CargoOrders.FindAsync(orderId);
            if (cargo != null)
            {
                string status = cargo.Status switch
                {
                    (Status)0 => "Отменен компанией",
                    (Status)1 => "Отменен пользователем",
                    (Status)2 => "Получен организацией",
                    (Status)3 => "Одобрен организацией",
                    (Status)4 => "В работе",
                    (Status)5 => "Доставлен",
                    _ => "Не удалось получить статус"
                };
                return $"<div class=\"container p-2\">\r\n" +
                    $"\r\n<div class=\"row\">\r\n<article class=\"card bg-transparent\">\r\n" +
                    $"<div class=\"card-body bg-transparent\">\r\n<h6>{cargo.Number}</h6>\r\n<article class=\"card bg-transparent\">\r\n" +
                    $"<div class=\"card-body row bg-transparent\">\r\n<div class=\"col\"> <strong>Груз: </strong> <br>{cargo.Cargo.Name} | {cargo.Weight} т.</div>\r\n" +
                    $"</div>\r\n</article>\r\n<article class=\"card bg-transparent\">\r\n" +
                    $"<div class=\"card-body row bg-transparent\">\r\n" +
                    $"<div class=\"col\"> <strong>Адрес:</strong> <br>{cargo.DeliveryAddress}</div>" +
                    $"<div class=\"col\"> <strong>Цена:</strong> <br>{cargo.Price}</div>" + 
                    $"</div>\r\n" +
                    $"</article>\r\n" +
                    $"<article class=\"card bg-transparent\">\r\n<div class=\"card-body row bg-transparent\">\r\n" +
                    $"<div class=\"col\"> <strong>Ожидаемое время получения:</strong>" +
                    $"<br>{cargo.DeliveryDate} {cargo.DeliveryMinTime}-{cargo.DeliveryMaxTime}</div>" +
                    $"\r\n<div class=\"col\"> <strong>Статус:</strong> <br> {status}</div>\r\n</div>\r\n</article>" +
                    $"\r\n</div>\r\n</article>\r\n</div>\r\n</ div >";
            }
            TransportOrder? transport = await _context.TransportOrders.FindAsync(orderId);
            if (transport != null)
            {
                string status = transport.Status switch
                {
                    (Status)0 => "Отменен компанией",
                    (Status)1 => "Отменен пользователем",
                    (Status)2 => "Получен организацией",
                    (Status)3 => "Одобрен организацией",
                    (Status)4 => "В работе",
                    (Status)5 => "Доставлен",
                    _ => "Не удалось получить статус"
                };
                return $"<div class=\"container p-2\">\r\n" +
                    $"<div class=\"row\">\r\n<article class=\"card bg-transparent\">\r\n" +
                    $"<div class=\"card-body bg-transparent\">\r\n" +
                    $"<h6>{transport.Number}</h6>" +
                    $"<article class=\"card bg-transparent\">\r\n<div class=\"card-body row bg-transparent\">" +
                    $"\r\n<div class=\"col\"><strong>Параметры:</strong> <br>{transport.Length}x{transport.Width}" +
                    $"x{transport.Height} м {transport.Weight} кг</div>\r\n</div>\r\n</article>" +
                    $"\r\n<article class=\"card bg-transparent\">\r\n" +
                    $"<div class=\"card-body row bg-transparent\">\r\n" +
                    $"<div class=\"col\"> <strong>Откуда:</strong> <br>{transport.FromAddress}</div>\r\n" +
                    $"</div>\r\n</article>\r\n<article class=\"card bg-transparent\">\r\n" +
                    $"<div class=\"card-body row bg-transparent\">\r\n" +
                    $"<div class=\"col\"> <strong>Куда:</strong> <br>{transport.ToAddress}</div>\r\n" +
                    $"</div>\r\n</article>\r\n<article class=\"card bg-transparent\">\r\n" +
                    $"<div class=\"card-body row bg-transparent\">\r\n" +
                    $"<div class=\"col\"> <strong>Цена:</strong> <br>{transport.Price}</div>\r\n" +
                    $"</div>\r\n</article>\r\n<article class=\"card bg-transparent\">\r\n" +
                    $"<div class=\"card-body row bg-transparent\">\r\n" +
                    $"<div class=\"col\"> <strong>Ожидаемое время получения:</strong>" +
                    $"<br>{transport.DeliveryDate} {transport.DeliveryMinTime}-{transport.DeliveryMaxTime}</div>\r\n" +
                    $"<div class=\"col\"> <strong>Статус:</strong> <br> {status}</div>\r\n</div>\r\n</article>\r\n" +
                    $"</div>\r\n</article>\r\n</div>\r\n</div>";
            }
            return $"<h4>К сожалению, нам <span class=\"text-danger\">не удалось</span> найти ваш заказ. Проверьте номер заказа и попытайтесь снова</h4>";
        }

    }
}
