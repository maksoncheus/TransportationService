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
        public Cargo Cargo { get; set; }
        public Status Status { get; set; }
    }
}
