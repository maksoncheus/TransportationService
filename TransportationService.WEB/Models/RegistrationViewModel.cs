using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TransportationService.WEB.Models
{
    public class RegistrationViewModel
    {
        [Required(ErrorMessage = "Укажите номер телефона")]
        [DisplayName("Номер телефона")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Укажите адрес электронной почты")]
        [DisplayName("Эл. почта")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DisplayName("Пароль")]
        [Required(ErrorMessage = "Укажите пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DisplayName("Имя")]
        public string? FirstName { get; set; }
        [DisplayName("Отчество")]
        public string? Patronymic { get; set; }
        [DisplayName("Фамилия")]
        public string? LastName { get; set; }

    }
}
