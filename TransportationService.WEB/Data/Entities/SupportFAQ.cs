using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TransportationService.WEB.Data.Entities
{
    public class SupportFAQ
    {
        public int Id { get; set; }
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
