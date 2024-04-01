﻿using System.ComponentModel.DataAnnotations;

namespace TransportationService.WEB.Data.Entities
{
    public class Cargo
    {
        public Guid Id { get; set; }
        [Display(Name = "Название груза")]
        public string Name { get; set; } = null!;
        [Display(Name = "Остаток на складе (в тоннах)")]
        public double RemainQuantity { get; set; }
    }
}