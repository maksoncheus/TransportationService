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
        public string Length { get; set; } = null!;
        [Required(ErrorMessage = "Укажите все параметры груза")]
        [DisplayName("Ширина")]
        public string Width { get; set; } = null!;
        [DisplayName("Высота")]
        [Required(ErrorMessage = "Укажите все параметры груза")]
        public string Height { get; set; } = null!;
        [DisplayName("Вес (в килограммах)")]
        [Required(ErrorMessage = "Укажите массу")]
        public string Weight { get; set; } = null!;
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
        [Required(ErrorMessage = "Цена не указана")]
        [DisplayName("Итоговая цена")]
        public string Price { get; set; }
    }
}
