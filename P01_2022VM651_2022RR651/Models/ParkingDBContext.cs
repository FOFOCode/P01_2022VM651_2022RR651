using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer;

namespace P01_2022VM651_2022RR651.Models
{
    public class ParkingDBContext: DbContext
    {
        public ParkingDBContext(DbContextOptions<ParkingDBContext> options) : base(options)
        {
        }
        public DbSet<Usuarios> Usuarios { get; set; }
        public DbSet<Reservas> Reservas { get; set; }
        public DbSet<EspaciosParqueo> Espacios { get; set; }
    }
}
