using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using TransportationService.WEB.Data.Entities;
using TransportationService.WEB.Data.Enums;

namespace TransportationService.WEB.Models
{
    public class TransportOrderViewModel
    {
        [DisplayName("Длина")]
        [Required(ErrorMessage = "Укажите все параметры груза")]
        public double Length { get; set; }
        [Required(ErrorMessage = "Укажите все параметры груза")]
        [DisplayName("Ширина")]
        public double Width { get; set; }
        [DisplayName("Высота")]
        [Required(ErrorMessage = "Укажите все параметры груза")]
        public double Height { get; set; }
        [DisplayName("Вес (в тоннах)")]
        [Required(ErrorMessage = "Укажите массу")]
        public string Weight { get; set; }
        [DisplayName("Адрес отправки")]
        [Required(ErrorMessage = "Укажите адрес отправки")]
        public string FromAddress { get; set; } = null!;
        [DisplayName("Адрес получения")]
        [Required(ErrorMessage = "Укажите адрес получения")]
        public string ToAddress { get; set; } = null!;
        [DisplayName("Дата доставки")]
        [Required(ErrorMessage = "Укажите дату доставки")]
        public DateOnly DeliveryDate { get; set; }
        [DisplayName("Время получения - минимум")]
        [Required(ErrorMessage = "Укажите диапазон времени получения груза")]
        public TimeOnly DeliveryMinTime { get; set; }
        [DisplayName("Время получения - максимум")]
        [Required(ErrorMessage = "Укажите диапазон времени получения груза")]
        public TimeOnly DeliveryMaxTime { get; set; }
        [DisplayName("Итоговая цена")]
        public double Price { get; set; }
    }
}
