

using System.ComponentModel.DataAnnotations;

namespace TransportationService.WEB.Areas.Profile.Models
{
    public class PasswordViewModel
    {
        [Required(ErrorMessage = "Укажите старый пароль")]
        [Display(Name = "Старый пароль")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }
        [Required(ErrorMessage = "Укажите новый пароль")]
        [Display(Name = "Новый пароль")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [Required(ErrorMessage = "Пароль не подтвержден")]
        [Display(Name = "Подтвердите пароль")]
        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword), ErrorMessage = "Пароли не совпадают")]
        public string ConfirmNewPassword { get; set; }
    }
}
