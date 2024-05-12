using System.ComponentModel.DataAnnotations;

namespace TransportationService.WEB.Areas.Profile.Models
{
    public class PersonalDataViewModel
    {
        [Required(ErrorMessage = "Укажите номер телефона")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Укажите корректный номер телефона")]
        [Display(Name = "Номер телефона")]
        public string PhoneNumber { get; set; } = null!;
        [Display(Name = "Имя")]
        public string? FirstName { get; set; }
        [Display(Name = "Фамилия")]
        public string? LastName { get; set; }
        [Display(Name = "Отчество (при наличии)")]
        public string? Patronymic { get; set; }
    }
}
