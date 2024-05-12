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
        public double Length { get; set; }
        [DisplayName("Ширина")]
        public double Width { get; set; }
        [DisplayName("Высота")]
        public double Height { get; set; }
        [DisplayName("Адрес отправки")]
        public string FromAddress { get; set; } = null!;
        [DisplayName("Адрес получения")]
        public string ToAddress { get; set; } = null!;
        [DisplayName("Дата доставки")]
        public DateTime DeliveryDate { get; set; }
        public virtual Status Status { get; set; }
        [DisplayName("Итоговая цена")]
        public double Price { get; set; }

        [Display(Name = "Номер заказчика")]
        public virtual User Customer { get; set; }
    }
}
