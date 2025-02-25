using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P01_2022VM651_2022RR651.Models;

namespace P01_2022VM651_2022RR651.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly ParkingDBContext _ParkinDbcontext;

        public UsuariosController(ParkingDBContext ParkinDbcontext)
        {
            _ParkinDbcontext = ParkinDbcontext;
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult AddUsuario([FromBody] Usuarios usuario)
        {
            try
            {
                _ParkinDbcontext.Usuarios.Add(usuario);
                _ParkinDbcontext.SaveChanges();
                return Ok("Usuario agregado correctamente");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetAllUsuarios")]
        public IActionResult GetAllUsuarios()
        {
            List<Usuarios> usuariosActuales = (from u in _ParkinDbcontext.Usuarios
                                               select u).ToList();

            if (usuariosActuales.Count == 0)
            {
                return NotFound("No hay usuarios registrados");
            }

            return Ok(usuariosActuales);
        }

        [HttpGet]
        [Route("GetUsuario/{id}")]
        public IActionResult GetUsuario(int id)
        {
            var usuario = (from u in _ParkinDbcontext.Usuarios
                           join s in _ParkinDbcontext.Sucursales on u.UsuarioID equals s.AdministradorID into sucursales
                           from s in sucursales.DefaultIfEmpty()
                           where u.UsuarioID == id
                           select new
                           {
                               u.UsuarioID,
                               u.Nombre,
                               u.Correo,
                               u.Telefono,
                               u.Rol,
                               Sucursal = s != null ? s.Nombre : "Sin sucursal asignada"
                           }).FirstOrDefault();

            if (usuario == null)
            {
                return NotFound($"Usuario con ID {id} no encontrado.");
            }

            return Ok(usuario);
        }

        [HttpPut]
        [Route("UpdateUsuario/{id}")]
        public IActionResult UpdateUsuario(int id, [FromBody] Usuarios usuario)
        {

            var existingUser = (from u in _ParkinDbcontext.Usuarios
                                where u.UsuarioID == id
                                select u).FirstOrDefault();


            existingUser.Nombre = usuario.Nombre;
            existingUser.Correo = usuario.Correo;
            existingUser.Telefono = usuario.Telefono;
            existingUser.Rol = usuario.Rol;

            _ParkinDbcontext.Entry(existingUser).State = EntityState.Modified;
            _ParkinDbcontext.SaveChanges();

            return Ok(existingUser);
        }

        [HttpDelete]
        [Route("DeleteUsuario/{id}")]
        public IActionResult DeleteUsuario(int id)
        {
            var usuario = (from u in _ParkinDbcontext.Usuarios
                           where u.UsuarioID == id
                           select u).FirstOrDefault();

            if (usuario == null)
            {
                return NotFound();

                _ParkinDbcontext.Usuarios.Remove(usuario);
                _ParkinDbcontext.SaveChanges();
            }
            
            return Ok(usuario);
        }

        [HttpPost]
        [Route("IniciarSesion")]
        public IActionResult IniciarSesion([FromBody] Login login)
        {
            
            var usuario = _ParkinDbcontext.Usuarios
                                          .Where(u => u.Correo == login.Correo)
                                          .FirstOrDefault();


            if (usuario == null)
            {
                return Unauthorized("Credenciales inválidas");
            }

            
            if (usuario.Contraseña == login.Contrasena)
            {
                
                return Ok("Credenciales válidas");
            }

            
            return Unauthorized("Credenciales inválidas");
        }







    }
}
