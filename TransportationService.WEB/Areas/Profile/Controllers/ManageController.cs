using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Newtonsoft.Json;
using System.Text;
using TransportationService.WEB.Areas.Profile.Models;
using TransportationService.WEB.Data;
using TransportationService.WEB.Data.Entities;
using TransportationService.WEB.Services;

namespace TransportationServiceWEB.Areas.Profile.Controllers
{
    /// <summary>
    /// Контроллер "Мой профиль". Содержит методы для изменения данных в профиле пользователя.
    /// </summary>
    [Area("Profile")]
    public class ManageController : Controller
    {
        private readonly ApplicationDbContext<User> _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly MailService _mailService;
        public ManageController(ApplicationDbContext<User> context, UserManager<User> userManager, SignInManager<User> signInManager, MailService mailService)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _mailService = mailService;
        }
        /// <summary>
        /// Основная страница профиля с личными данными.
        /// </summary>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Main()
        {
            User? currentUser = await _userManager.GetUserAsync(HttpContext.User);
            if (currentUser == null) return Forbid();
            PersonalDataViewModel model = new()
            {
                FirstName = currentUser.FirstName,
                LastName = currentUser.LastName,
                Patronymic = currentUser.Patronymic
            };
            return View(model);
        }
        /// <summary>
        /// Отправка формы на главной странице. Изменение основных данных учетной записи.
        /// </summary>
        /// <param name="model">Модель представления</param>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Main(PersonalDataViewModel model)
        {
            try
            {
                if (!ModelState.IsValid) return View(model);
                User? currentUser = await _userManager.GetUserAsync(HttpContext.User);
                if (currentUser == null) return Forbid();
                currentUser.FirstName = model.FirstName;
                currentUser.LastName = model.LastName;
                currentUser.Patronymic = model.Patronymic;
                _context.Entry(currentUser).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                await _signInManager.RefreshSignInAsync(currentUser);
                ViewData[AppConstValues.SuccessString] = "Данные успешно изменены";
                return View(model);
            }
            catch (Exception ex)
            {
                ViewData[AppConstValues.ErrorString] = ex.Message;
                return View(model);
            }
        }
        /// <summary>
        /// Страница "Изменить адрес электронной почты".
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Email()
        {
            User? currentUser = await _userManager.GetUserAsync(HttpContext.User);
            if (currentUser == null) return Forbid();
            EmailViewModel model = new()
            {
                OldEmail = currentUser.Email,
                IsEmailConfirmed = currentUser.EmailConfirmed
            };
            return View(model);
        }
        /// <summary>
        /// Изменить адрес электронной почты.
        /// </summary>
        /// <param name="model">Модель представления.</param>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Email(EmailViewModel model)
        {
            try
            {
                if (!ModelState.IsValid) return View(model);
                User? currentUser = await _userManager.GetUserAsync(HttpContext.User);
                if (currentUser == null) return Forbid();
                currentUser.Email = model.NewEmail;
                currentUser.UserName = model.NewEmail;
                currentUser.EmailConfirmed = false;
                _context.Entry(currentUser).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                await _signInManager.RefreshSignInAsync(currentUser);
                model.OldEmail = model.NewEmail;
                model.NewEmail = string.Empty;
                model.IsEmailConfirmed = currentUser.EmailConfirmed;
                string confirmLink = await GenerateEmailConfirmationLink(currentUser);
                _mailService.SendConfirmationLink(confirmLink, currentUser.Email);
                ViewData[AppConstValues.SuccessString] = "Данные успешно изменены. Проверьте новую почту для подтверждения";
                return View(model);
            }
            catch (Exception ex)
            {
                ViewData[AppConstValues.ErrorString] = ex.Message;
                return View(model);
            }
        }
        /// <summary>
        /// Страница смены пароля.
        /// </summary>
        [HttpGet]
        public IActionResult Password()
        {
            return View(new PasswordViewModel());
        }
        /// <summary>
        /// Сменить пароль.
        /// </summary>
        /// <param name="model">Модель представления.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Password(PasswordViewModel model)
        {
            try
            {
                if (!ModelState.IsValid) return View(model);
                User? currentUser = await _userManager.GetUserAsync(HttpContext.User);
                if (currentUser == null) return Forbid();
                var result = await _userManager.ChangePasswordAsync(currentUser, model.OldPassword, model.NewPassword);
                if(result.Succeeded)
                {
                    await _context.SaveChangesAsync();
                    ViewData[AppConstValues.SuccessString] = "Данные успешно изменены.";
                    return View(model);
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            catch (Exception ex)
            {
                ViewData[AppConstValues.ErrorString] = ex.Message;
                return View(model);
            }
            return View(model);
        }
        public IActionResult Account()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> DownloadData()
        {
            User? currentUser = await _userManager.GetUserAsync(HttpContext.User);
            if (currentUser == null) return Forbid();
            var user = new
            {
                currentUser.Email,
                currentUser.FirstName,
                currentUser.Patronymic,
                currentUser.LastName,
                currentUser.PhoneNumber
            };
            string serializedUser = JsonConvert.SerializeObject(user);
            var content = new MemoryStream(Encoding.UTF8.GetBytes(serializedUser));
            var contentType = "APPLICATION/octet-stream";
            return File(content, contentType, "profile.json");
        }
        [HttpGet]
        public async Task<IActionResult> RemoveProfile()
        {
            User? currentUser = await _userManager.GetUserAsync(HttpContext.User);
            if (currentUser == null) return Forbid();
            await _userManager.DeleteAsync(currentUser);
            return RedirectToAction("LogOut", "Account", new { area = "" });
        }
        private async Task<string> GenerateEmailConfirmationLink(User user)
        {
            string confirmationToken = await _userManager
                 .GenerateEmailConfirmationTokenAsync(user);

            string? confirmationLink = Url.Action("ConfirmEmail",
              "Account", new
              {
                  Area = "",
                  userid = user.Id,
                  token = confirmationToken
              },
              protocol: HttpContext.Request.Scheme);
            if (confirmationLink == null) throw new ArgumentNullException(nameof(confirmationLink));
            return confirmationLink;
        }
    }
}
