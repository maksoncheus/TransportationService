using System.ComponentModel.DataAnnotations;

namespace TransportationService.WEB.Areas.Profile.Models
{
    public class EmailViewModel
    {
        [Display(Name = "Текущий адрес электронной почты")]
        public string? OldEmail { get; set; }
        [Required(ErrorMessage = "Укажите новый адрес электронной почты")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Новый адрес электронной почты")]
        public string NewEmail { get; set; }
        [Display(Name = "Почта подтверждена")]
        public bool IsEmailConfirmed { get; set; }
    }
}
