using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace TransportationService.WEB.Areas.Manage.Models
{
    public class AddServiceViewModel
    {
        [Required(ErrorMessage = "Укажите название")]
        [DisplayName("Название")]
        public string Name { get; set; } = null!;
        [Required(ErrorMessage = "Укажите цену за тонну")]
        [DisplayName("Цена за тонну")]
        [Range(0, 999999)]
        public int Price { get; set; }
        [Required(ErrorMessage = "Укажите начальное количество")]
        [DisplayName("Начальное количество на складе (в тоннах)")]
        [Range(0, 999999)]
        public int Quantity { get; set; }
    }
}
