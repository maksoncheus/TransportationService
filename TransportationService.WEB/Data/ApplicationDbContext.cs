using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TransportationService.WEB.Data.Entities;

namespace TransportationService.WEB.Data
{
    public class ApplicationDbContext<User> : IdentityDbContext<User, IdentityRole, string> where User: IdentityUser
    {
        override public DbSet<User> Users { get; set; }
        public DbSet<Cargo> Cargos { get; set; }
        public DbSet<TransportOrder> TransportOrders { get; set; }
        public DbSet<CargoOrder> CargoOrders { get; set; }
        public DbSet<SupportFAQ> SupportFAQ { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext<User>> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
