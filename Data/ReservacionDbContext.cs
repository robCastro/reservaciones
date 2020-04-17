using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using reservacion.Areas.Identity.Data;

namespace reservacion.Data
{
    public class ReservacionDbContext: IdentityDbContext<User, IdentityRole, string>
    {
        public ReservacionDbContext(DbContextOptions<ReservacionDbContext>options): base(options){
        }
        public DbSet<reservacion.Models.Sala> Sala {get; set;}
        public DbSet<reservacion.Models.Reservacion> Reservacion {get; set;}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}