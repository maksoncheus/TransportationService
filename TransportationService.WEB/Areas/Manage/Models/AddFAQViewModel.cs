using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace TransportationService.WEB.Areas.Manage.Models
{
    public class AddFAQViewModel
    {
        [Required]
        [MinLength(10)]
        [DisplayName("Вопрос")]
        public string Question { get; set; } = null!;
        [Required]
        [MinLength(10)]
        [DisplayName("Ответ")]
        public string Answer { get; set; } = null!;
    }
}
