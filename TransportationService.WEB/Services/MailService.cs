using System.Net.Mail;
using System.Net;
using TransportationService.WEB.Configuration;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;

namespace TransportationService.WEB.Services
{
    /// <summary>
    /// Сервис для отправки электронной почты.
    /// </summary>
    public class MailService
    {
        private readonly ILogger<MailService> _logger;
        private readonly SMTPSettings _smtpSettings;
        public MailService(ILogger<MailService> logger, SMTPSettings smtpSettings)
        {
            _logger = logger;
            _smtpSettings = smtpSettings;
        }
        /// <summary>
        /// Отправить ссылку подтверждения по указанному адресу электронной почты.
        /// </summary>
        /// <param name="link">Ссылка подтверждения.</param>
        /// <param name="emailTo">Адрес электронной почты, на который требуется отправить ссылку.</param>
        public void SendConfirmationLink(string link, string emailTo)
        {
            try
            {
                using (MailMessage mm = new MailMessage(
                    new MailAddress(_smtpSettings.Email, "TransportationService"), new MailAddress(emailTo)))
                {
                    //Заголовок сообщения
                    mm.Subject = "Подтверждение почты TransportationService";
                    //Тело сообщения
                    mm.Body = "Благодарим вас за регистрацию в сервисе TransportationService!"
                        + Environment.NewLine +
                        $"Для подтверждения аккаунта пройдите по ссылке: {link}";
                    mm.IsBodyHtml = false;
                    SendMessage(mm);
                }
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.Message);
            }
        }
        /// <summary>
        /// Отправить ссылку сброса пароля по указанному адресу электронной почты.
        /// </summary>
        /// <param name="link">Ссылка сброса пароля.</param>
        /// <param name="emailTo">Адрес, на который требуется отправить сообщение.</param>
        public void SendResetLink(string link, string emailTo)
        {
            try
            {
                using (MailMessage mm = new MailMessage(
                    new MailAddress(_smtpSettings.Email, "TransportationService"), new MailAddress(emailTo)))
                {
                    mm.Subject = "Сброс пароля TransportationService";
                    mm.Body = "Для вашего аккаунта был запрошен сброс пароля"
                        + Environment.NewLine +
                        $"Для сброса пройдите по ссылке: {link}"
                        + Environment.NewLine +
                        "Если вы не запрашивали сброс пароля, проигнорируйте данное сообщение";
                    mm.IsBodyHtml = false;
                    SendMessage(mm);
                }
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.Message);
            }
        }
        public void SendOrderMessage(string linkToHome, string orderNumber, string emailTo)
        {
            try
            {
                using (MailMessage mm = new MailMessage(
                    new MailAddress(_smtpSettings.Email, "TransportationService"), new MailAddress(emailTo)))
                {
                    mm.Subject = "Ваш заказ в сервисе TransportationService";
                    mm.Body = "Спасибо, что воспользовались нашим сервисом грузоперевозок!"
                        + Environment.NewLine +
                        $"Уникальный номер вашего заказа: {orderNumber}"
                        + Environment.NewLine +
                        "Вы можете посмотреть статус и подробную информацию о вашем заказе в личном профиле на сайте либо воспользовавшись специальной формой на главной странице."
                        + Environment.NewLine +
                        $"{linkToHome}";
                    mm.IsBodyHtml = false;
                    SendMessage(mm);
                }
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.Message);
            }
        }
        /// <summary>
        /// Отправить сообщение.
        /// </summary>
        /// <param name="mm">Экземпляр класса <see cref="MailMessage"></see>, который нужно отправить</param>
        private void SendMessage(MailMessage mm)
        {
            using (SmtpClient smtp = new SmtpClient())
            {
                smtp.Host = _smtpSettings.Host;
                smtp.EnableSsl = true;
                NetworkCredential NetworkCred = new NetworkCredential(_smtpSettings.Email, _smtpSettings.Password);
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = NetworkCred;
                smtp.Port = _smtpSettings.Port;
                smtp.Send(mm);
            }
        }
    }
}
