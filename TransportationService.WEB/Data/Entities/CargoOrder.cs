using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TransportationService.WEB.Data.Enums;

namespace TransportationService.WEB.Data.Entities
{
    public class CargoOrder
    {
        [Key]
        [Display(Name = "Номер заказа")]
        public string Number { get; set; } = null!;
        public double Weight { get; set; }
        public string DeliveryAddress { get; set; } = null!;
        public DateTime DeliveryDateTime { get; set; }
        public virtual Cargo Cargo { get; set; }
        public virtual Status Status { get; set; }
        public virtual User Customer { get; set; }
        [DisplayName("Итоговая цена")]
        public double Price { get; set; }
    }
}
