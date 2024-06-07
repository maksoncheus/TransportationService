using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TransportationService.WEB.Data.Enums;

namespace TransportationService.WEB.Data.Entities
{
    public class TransportOrder
    {
        [Key]
        [DisplayName("Номер заказа")]
        public string Number { get; set; } = null!;
        [DisplayName("Длина")]
        [Required]
        public double Length { get; set; }
        [Required]
        [DisplayName("Ширина")]
        public double Width { get; set; }
        [DisplayName("Высота")]
        [Required]
        public double Height { get; set; }
        [DisplayName("Вес")]
        [Required]
        public double Weight { get; set; }
        [DisplayName("Адрес отправки")]
        [Required]
        public string FromAddress { get; set; } = null!;
        [DisplayName("Адрес получения")]
        [Required]
        public string ToAddress { get; set; } = null!;
        [DisplayName("Дата доставки")]
        [Required]
        public DateOnly DeliveryDate { get; set; }
        [Required]
        public TimeOnly DeliveryMinTime { get; set; }
        [Required]
        public TimeOnly DeliveryMaxTime { get; set; }
        public virtual Status Status { get; set; }
        [DisplayName("Итоговая цена")]
        public double Price { get; set; }

        [Display(Name = "Номер заказчика")]
        public virtual User Customer { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
