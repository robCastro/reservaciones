using Microsoft.EntityFrameworkCore;

namespace reservacion.Data
{
    public class ReservacionDbContext: DbContext
    {
        public ReservacionDbContext(DbContextOptions<ReservacionDbContext>options): base(options){
        }
        public DbSet<reservacion.Models.Sala> Sala {get; set;}
    }
}