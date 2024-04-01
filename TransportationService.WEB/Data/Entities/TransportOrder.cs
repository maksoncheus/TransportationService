using System.ComponentModel.DataAnnotations;
using TransportationService.WEB.Data.Enums;

namespace TransportationService.WEB.Data.Entities
{
    public class TransportOrder
    {
        [Key]
        [Display(Name = "Номер заказа")]
        public string Number { get; set; } = null!;
        public double Length { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public string FromAddress { get; set; } = null!;
        public string ToAddress { get; set; } = null!;
        public DateTime DeliveryDate { get; set; }
        public Status Status { get; set; }


        [Display(Name = "Номер заказчика")]
        public User? Customer { get; set; }
    }
}
