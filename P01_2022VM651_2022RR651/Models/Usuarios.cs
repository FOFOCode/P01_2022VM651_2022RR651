using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;


namespace P01_2022VM651_2022RR651.Models
{
    public class Usuarios
    {
        [Key]
        public int UsuarioID { get; set; }
        public string? Nombre { get; set; }
        public string? Correo { get; set; }
        public string? Telefono { get; set; }
        public string? Contraseña { get; set; }
        public string? Rol { get; set; }
    }
}
