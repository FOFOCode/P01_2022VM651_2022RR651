using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace P01_2022VM651_2022RR651.Models
{
    public class Reservas
    {
        [Key]
        public int ReservaID { get; set; }
        public int UsuarioID { get; set; }
        public int EspacioID { get; set; }
        public DateTime? FechaReserva { get; set; }
        public string? Estado { get; set; }
    }
}
