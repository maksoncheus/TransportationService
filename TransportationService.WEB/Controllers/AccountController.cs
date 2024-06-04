using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Net;
using TransportationService.WEB.Configuration;
using TransportationService.WEB.Data.Entities;
using TransportationService.WEB.Models;
using TransportationService.WEB.Services;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Microsoft.AspNetCore.Authorization;

namespace TransportationService.WEB.Controllers
{
    /// <summary>
    /// Контроллер "Аккаунт". Содержит методы для авторизация, регистрации, выхода из профиля.
    /// </summary>
    public class AccountController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly MailService _mailService;
        public AccountController(SignInManager<User> signInManager,
            UserManager<User> userManager,
            MailService mailService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _mailService = mailService;
        }
        /// <summary>
        /// Получить страницу "Регистрация".
        /// </summary>
        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegistrationViewModel());
        }
        /// <summary>
        /// Отправка формы на странице "Регистрация". Метод регистрирует нового пользователя с указанными данными.
        /// </summary>
        /// <param name="vm">Модель представления "<see cref="RegistrationViewModel">Регистрация</see>"</param>
        [HttpPost]
        public async Task<IActionResult> Register(RegistrationViewModel vm)
        {
            if (ModelState.IsValid)
            {
                User user = new User()
                {
                    UserName = vm.Email,
                    FirstName = vm.FirstName,
                    LastName = vm.LastName,
                    Patronymic = vm.Patronymic,
                    Email = vm.Email,
                    PhoneNumber = vm.PhoneNumber,
                    PhoneNumberConfirmed = true,
                    EmailConfirmed = false
                };
                var result = await _userManager.CreateAsync(user, vm.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "User");
                    string confirmLink = await GenerateEmailConfirmationLink(user);
                    _mailService.SendConfirmationLink(confirmLink, user.Email);
                    string message = "<h3>Вы успешно зарегистрировались!</h3><br> Проверьте почту и подтвердите свой аккаунт";
                    return RedirectToAction("Message", "Home", new { msg = message });
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(vm);
        }
        /// <summary>
        /// Метод авторизации.
        /// </summary>
        /// <param name="authString">Строка входа (может быть номер телефона или адрес электронной почты).</param>
        /// <param name="password">Пароль.</param>
        [HttpGet]
        public async Task<IActionResult> Authenticate(string authString, string password)
        {
            try
            {
                User? user = await _userManager.Users.FirstOrDefaultAsync(u =>
                u.PhoneNumber == authString
                ||
                u.Email == authString);
                if (user == null)
                {
                    return BadRequest("Пользователь с такими данными не найден");
                }
                var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, true);
                    return Ok();
                }
                if (result.IsNotAllowed)
                {
                    return BadRequest("Эл. почта не подтверждена!");
                }
                if (result == Microsoft.AspNetCore.Identity.SignInResult.Failed)
                    return BadRequest("Неправильный пароль");
                return BadRequest();
            }
            catch
            {
                return BadRequest("Что-то пошло не так");
            }
        }
        /// <summary>
        /// Выйти из профиля.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home", new {area=""});
        }
        /// <summary>
        /// Метод подтверждения аккаунта через ссылку на электронной почте.
        /// </summary>
        /// <param name="userId">ID пользователя.</param>
        /// <param name="token">Уникальный токен подтверждения.</param>
        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            User? user = await _userManager.FindByIdAsync(userId);
            string message;
            if(user == null)
            {
                message = "<h3>Такой пользователь не был найден. Обратитесь к администратору</h3>";
            }
            else
            {
                var result = await _userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                    message = "<h3>Электронная почта успешно подтверждена. Теперь вы можете авторизоваться на сайте</h3>";
                else
                    message = string.Join(", ", result.Errors);
            }
            return RedirectToAction("Message", "Home", new { msg = message });
        }
        /// <summary>
        /// Сгенерировать строку подтверждения E-mail для указаного пользователя.
        /// </summary>
        /// <param name="user">Пользователь, для которого требуется сгенировать ссылку.</param>
        /// <exception cref="ArgumentNullException">Строка не была сгенерирована.</exception>
        private async Task<string> GenerateEmailConfirmationLink(User user)
        {
            string confirmationToken = await _userManager
                 .GenerateEmailConfirmationTokenAsync(user);

            string? confirmationLink = Url.Action("ConfirmEmail",
              "Account", new
              {
                  userid = user.Id,
                  token = confirmationToken
              },
              protocol: HttpContext.Request.Scheme);
            if (confirmationLink == null) throw new ArgumentNullException(nameof(confirmationLink));
            return confirmationLink;
        }
        /// <summary>
        /// Получить страницу "Сброс пароля".
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetResetPasswordLink()
        {
            return View();
        }
        /// <summary>
        /// Отправка формы на странице "Сброс пароля".
        /// </summary>
        /// <param name="authString">Строка авторизации. Это может быть адрес электронной почты или номер телефона.</param>
        [HttpPost]
        public async Task<IActionResult> GetResetPasswordLink(string? authString)
        {
            if(authString == null) return Forbid();
            User? user = await _userManager.FindByEmailAsync(authString);
            if (user == null)
                user = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == authString);
            if (user == null)
                return NotFound();
            string link = await GenerateResetPasswordLink(user);
            _mailService.SendResetLink(link, user.Email);
            string message = "<h3>На вашу почту было отправлено письмо с ссылкой для сброса пароля.</h3> <br>" +
                "Не забудьте активировать аккаунт, если этого ещё не сделали";
            return RedirectToAction("Message", "Home", new {msg = message });
        }
        /// <summary>
        /// Сгенерировать ссылку сброса пароля для указанного пользователя.
        /// </summary>
        /// <param name="user">Пользователь, для которого требуется сгенерировать ссылку.</param>
        /// <exception cref="ArgumentNullException">Ссылка не была сгенерирована.</exception>
        private async Task<string> GenerateResetPasswordLink(User user)
        {
            string resetToken = await _userManager
                 .GeneratePasswordResetTokenAsync(user);

            string? confirmationLink = Url.Action("ResetPassword",
              "Account", new
              {
                  userid = user.Id,
                  token = resetToken
              },
              protocol: HttpContext.Request.Scheme);
            if (confirmationLink == null) throw new ArgumentNullException(nameof(confirmationLink));
            return confirmationLink;
        }
        /// <summary>
        /// Найти пользователя.
        /// </summary>
        /// <param name="authString">Строка авторизации. Это может быть номер телефона или адрес электронной почты.</param>
        [HttpGet]
        public async Task<IActionResult> FindSuggestedUser(string? authString)
        {
            if (authString == null)
                return BadRequest("Укажите почту!");
            User? user = await _userManager.FindByEmailAsync(authString);
            if (user == null)
                user = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == authString);
            if (user == null)
                return BadRequest("Такого пользователя не найдено");
            return Ok();
        }
        /// <summary>
        /// Сбросить пароль.
        /// </summary>
        /// <param name="userId">ID пользователя.</param>
        /// <param name="token">Уникальный токен сброса пароля.</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(string userId, string token)
        {
            User? user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                ResetPasswordViewModel vm = new() { Id = userId, Token = token };
                return View(vm);
            }

            return RedirectToAction("Index", "Home");
        } 
        /// <summary>
        /// Отправка формы на странице "Сброс пароля".
        /// </summary>
        /// <param name="vm">Модель представления "<see cref="ResetPasswordViewModel">Сброс пароля</see>"</param>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel vm)
        {
            User? user = await _userManager.FindByIdAsync(vm.Id);
            if (user != null)
            {
                IdentityResult result = await _userManager.ResetPasswordAsync(user, vm.Token, vm.Password);
                if (result.Succeeded)
                {
                    string message = "<h3>Вы успешно изменили пароль своей учетной записи.</h3><br>" +
                        " Теперь вы можете авторизоваться, используя новые данные";
                    return RedirectToAction("Message", "Home", new {msg = message});
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                return View(vm);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
