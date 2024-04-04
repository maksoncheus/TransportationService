using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TransportationService.WEB.Models
{
    public class ResetPasswordViewModel
    {
        [Required]
        public string Id { get; set; }
        [Required]
        [DisplayName("Новый пароль")]
        public string Password { get; set; }
        [Required]
        [Compare(nameof(Password))]
        [DisplayName("Подтвердите пароль")]
        public string ConfirmPassword { get; set; }
        [Required]
        public string Token { get; set; }
    }
}
