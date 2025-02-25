using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using P01_2022VM651_2022RR651.Models;
using Microsoft.EntityFrameworkCore;

namespace P01_2022VM651_2022RR651.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EspaciosParqueoController : ControllerBase
    {
        private readonly ParkingDBContext _ParkinDbcontext;

        public EspaciosParqueoController(ParkingDBContext ParkinDbcontext)
        {
            _ParkinDbcontext = ParkinDbcontext;
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            var listadoEspaciosParqueo = (from ep in _ParkinDbcontext.EspaciosParqueo
                                          join s in _ParkinDbcontext.Sucursales on ep.SucursalID equals s.SucursalID
                                          select new
                                          {
                                              ep.EspacioID,
                                              Sucursal = s.Nombre,
                                              ep.NumeroEspacio,
                                              ep.Ubicacion,
                                              ep.CostoPorHora,
                                              ep.Estado
                                          }).ToList();

            if (listadoEspaciosParqueo.Count() == 0)
            {
                return NotFound();
            }

            return Ok(listadoEspaciosParqueo);
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult GuardarEspacioParqueo([FromBody]EspaciosParqueo espaciosParqueo)
        {
            try
            {
                _ParkinDbcontext.EspaciosParqueo.Add(espaciosParqueo);
                _ParkinDbcontext.SaveChanges();
                return Ok(espaciosParqueo);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("ActualizarSucursal/{id}")]
        public IActionResult ActualizarSucursal(int id, [FromBody] Sucursales sucursalModificar)
        {
            Sucursales? sucursalActual = (from s in _ParkinDbcontext.Sucursales
                                        where s.SucursalID == id
                                        select s).FirstOrDefault();

            if (sucursalActual == null)
            {
                return NotFound();
            }

            sucursalActual.Nombre = sucursalModificar.Nombre;
            sucursalActual.Direccion = sucursalModificar.Direccion;
            sucursalActual.Telefono = sucursalModificar.Telefono;
            sucursalActual.AdministradorID = sucursalModificar.AdministradorID;
            sucursalActual.NumeroEspacios = sucursalModificar.NumeroEspacios;

            _ParkinDbcontext.Entry(sucursalActual).State = EntityState.Modified;
            _ParkinDbcontext.SaveChanges();

            return Ok(sucursalActual);
        }

        [HttpGet]
        [Route("Get/{id}")]
        public IActionResult GetSucursalById(int id)
        {
            var sucursal = (from s in _ParkinDbcontext.Sucursales
                            join u in _ParkinDbcontext.Usuarios on s.AdministradorID equals u.UsuarioID
                            where s.SucursalID == id
                            select new
                            {
                                s.SucursalID,
                                s.Nombre,
                                s.Direccion,
                                s.Telefono,
                                Administrador = u.Nombre,  
                                AdministradorTelefono = u.Telefono, 
                                s.NumeroEspacios,
                                EspaciosParqueo = _ParkinDbcontext.EspaciosParqueo
                                                                .Where(ep => ep.SucursalID == s.SucursalID)
                                                                .Select(ep => new
                                                                {
                                                                    ep.EspacioID,
                                                                    ep.NumeroEspacio,
                                                                    ep.Ubicacion,
                                                                    ep.CostoPorHora,
                                                                    ep.Estado
                                                                }).ToList()
                            }).FirstOrDefault();

            if (sucursal == null)
            {
                return NotFound();
            }

            return Ok(sucursal);
        }


        [HttpDelete]
        [Route("EliminarSucursal/{id}")]
        public IActionResult EliminarSucursal(int id)
        {
            var sucursal = _ParkinDbcontext.Sucursales
                                          .Where(s => s.SucursalID == id)
                                          .FirstOrDefault();

            if (sucursal == null)
            {
                return NotFound();
            }

            _ParkinDbcontext.Sucursales.Remove(sucursal);
            _ParkinDbcontext.SaveChanges();

            return Ok(new { message = "Sucursal eliminada exitosamente" });
        }

        [HttpPost]
        [Route("RegistrarEspacio")]
        public IActionResult RegistrarEspacioParqueo([FromBody] EspaciosParqueo espaciosParqueo)
        {
            try
            {
                
                var sucursal = _ParkinDbcontext.Sucursales
                                               .FirstOrDefault(s => s.SucursalID == espaciosParqueo.SucursalID);

                if (sucursal == null)
                {
                    return BadRequest("La sucursal especificada no existe.");
                }

                
                bool espacioExistente = _ParkinDbcontext.EspaciosParqueo
                                                        .Any(ep => ep.SucursalID == espaciosParqueo.SucursalID && ep.NumeroEspacio == espaciosParqueo.NumeroEspacio);

                if (espacioExistente)
                {
                    return BadRequest("Ya existe un espacio con ese número en la sucursal.");
                }

                
                if (espaciosParqueo.Estado != "Disponible" && espaciosParqueo.Estado != "Ocupado")
                {
                    return BadRequest("El estado debe ser 'Disponible' o 'Ocupado'.");
                }

                
                if (espaciosParqueo.CostoPorHora <= 0)
                {
                    return BadRequest("El costo por hora debe ser un valor positivo.");
                }

                
                var nuevoEspacio = new EspaciosParqueo
                {
                    SucursalID = espaciosParqueo.SucursalID,
                    NumeroEspacio = espaciosParqueo.NumeroEspacio,
                    Ubicacion = espaciosParqueo.Ubicacion,
                    CostoPorHora = espaciosParqueo.CostoPorHora,
                    Estado = espaciosParqueo.Estado
                };

                _ParkinDbcontext.EspaciosParqueo.Add(nuevoEspacio);
                _ParkinDbcontext.SaveChanges();

                return Ok(new
                {
                    message = "Espacio de parqueo registrado exitosamente.",
                    espacio = nuevoEspacio
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("EspaciosDisponibles")]
        public IActionResult ObtenerEspaciosDisponibles()
        {
            var espaciosDisponibles = (from ep in _ParkinDbcontext.EspaciosParqueo
                                       join s in _ParkinDbcontext.Sucursales on ep.SucursalID equals s.SucursalID
                                       where ep.Estado == "Disponible"
                                       select new
                                       {
                                           ep.EspacioID,
                                           Sucursal = s.Nombre,
                                           ep.NumeroEspacio,
                                           ep.Ubicacion,
                                           ep.CostoPorHora,
                                           ep.Estado
                                       }).ToList();

            if (espaciosDisponibles.Count == 0)
            {
                return NotFound("No hay espacios de parqueo disponibles en este momento.");
            }

            return Ok(espaciosDisponibles);
        }

        [HttpPut]
        [Route("ActualizarEspacio/{id}")]
        public IActionResult ActualizarEspacio(int id, [FromBody] EspaciosParqueo espacioModificar)
        {
            var espacioActual = (from ep in _ParkinDbcontext.EspaciosParqueo
                                 where ep.EspacioID == id
                                 select ep).FirstOrDefault();

            if (espacioActual == null)
            {
                return NotFound("El espacio de parqueo no existe.");
            }

            if (espacioModificar.CostoPorHora <= 0)
            {
                return BadRequest("El costo por hora debe ser un valor positivo.");
            }

            espacioActual.NumeroEspacio = espacioModificar.NumeroEspacio;
            espacioActual.Ubicacion = espacioModificar.Ubicacion;
            espacioActual.CostoPorHora = espacioModificar.CostoPorHora;
            espacioActual.Estado = espacioModificar.Estado;

            _ParkinDbcontext.EspaciosParqueo.Update(espacioActual);
            _ParkinDbcontext.SaveChanges();

            return Ok(new { message = "Espacio de parqueo actualizado exitosamente.", espacioActual });
        }

        [HttpDelete]
        [Route("EliminarEspacio/{id}")]
        public IActionResult EliminarEspacio(int id)
        {
            var espacio = (from ep in _ParkinDbcontext.EspaciosParqueo
                           where ep.EspacioID == id
                           select ep).FirstOrDefault();

            if (espacio == null)
            {
                return NotFound("El espacio de parqueo no existe.");
            }

            _ParkinDbcontext.EspaciosParqueo.Remove(espacio);
            _ParkinDbcontext.SaveChanges();

            return Ok(new { message = "Espacio de parqueo eliminado exitosamente." });
        }

        [HttpPost]
        [Route("AddSucursal")]
        public IActionResult AddSucursal([FromBody] Sucursales nuevaSucursal)
        {
            try
            {
                _ParkinDbcontext.Sucursales.Add(nuevaSucursal);
                _ParkinDbcontext.SaveChanges();
                return Ok(nuevaSucursal);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
