using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace TransportationService.WEB.Data.Entities
{
    public class User : IdentityUser
    {
        [Display(Name = "Имя")]
        public string? FirstName { get; set; }
        [Display(Name = "Отчество")]
        public string? Patronymic { get; set; }
        [Display(Name = "Фамилия")]
        public string? LastName { get; set; }
        public virtual ICollection<TransportOrder> TransportOrders { get; set; }
        public virtual ICollection<CargoOrder> CargoOrders { get; set; }
    }
}
