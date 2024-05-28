using System.ComponentModel.DataAnnotations;

namespace TransportationService.WEB.Areas.Profile.Models
{
    public class PersonalDataViewModel
    {
        [Display(Name = "Имя")]
        public string? FirstName { get; set; }
        [Display(Name = "Фамилия")]
        public string? LastName { get; set; }
        [Display(Name = "Отчество (при наличии)")]
        public string? Patronymic { get; set; }
    }
}
