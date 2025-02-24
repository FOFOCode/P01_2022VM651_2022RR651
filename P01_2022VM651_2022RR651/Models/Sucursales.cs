using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace P01_2022VM651_2022RR651.Models
{
    public class Sucursales
    {
        [Key]
        public int SucursalID { get; set; }
        public string? Nombre { get; set; }
        public string? Direccion { get; set; }
        public string? Telefono { get; set; }
        public int AdministradorID { get; set; }
        public int? NumeroEspacios { get; set; }
    }
}
