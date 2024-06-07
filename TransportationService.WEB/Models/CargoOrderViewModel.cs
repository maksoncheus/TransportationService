using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using TransportationService.WEB.Data.Entities;
using TransportationService.WEB.Data.Enums;

namespace TransportationService.WEB.Models
{
    public class CargoOrderViewModel
    {
        [Required(ErrorMessage = "Укажите массу")]
        [DisplayName("Масса (в тоннах)")]
        public string Weight { get; set; } = null!;
        [Required(ErrorMessage = "Укажите адрес доставки")]
        [DisplayName("Адрес доставки")]
        public string DeliveryAddress { get; set; } = null!;
        [Required(ErrorMessage = "Укажите дату доставки")]
        [DisplayName("Дата получения")]
        public DateOnly DeliveryDate { get; set; }
        [DisplayName("Время получения - минимум")]
        [Required(ErrorMessage = "Укажите диапазон времени для доставки")]
        public TimeOnly DeliveryMinTime { get; set; }
        [Required(ErrorMessage = "Укажите диапазон времени для доставки")]
        [DisplayName("Время получения - максимум")]
        public TimeOnly DeliveryMaxTime { get; set; }
        [Required(ErrorMessage = "Выберите товар")]
        [DisplayName("Наименование груза")]
        public Guid CargoId { get; set; }
        [DisplayName("Итоговая цена")]
        [Required(ErrorMessage = "Цена не указана")]
        public string Price { get; set; }
    }
}
