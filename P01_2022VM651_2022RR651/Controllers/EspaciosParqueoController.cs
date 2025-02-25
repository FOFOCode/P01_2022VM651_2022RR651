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
        [Route("Actualizar/{id}")]
        public IActionResult ActualizarEspacioParqueo(int id, [FromBody] EspaciosParqueo espaciosParqueoModificar)
        {
            EspaciosParqueo? espaciosParqueoActual = (from ep in _ParkinDbcontext.EspaciosParqueo where ep.EspacioID == id
                                                       select ep).FirstOrDefault();

            if(espaciosParqueoActual == null)
            {
                return NotFound();
            }

            espaciosParqueoActual.EspacioID = espaciosParqueoModificar.EspacioID;
            espaciosParqueoActual.SucursalID = espaciosParqueoModificar.SucursalID;
            espaciosParqueoActual.NumeroEspacio = espaciosParqueoModificar.NumeroEspacio;
            espaciosParqueoActual.Ubicacion = espaciosParqueoModificar.Ubicacion;
            espaciosParqueoActual.CostoPorHora = espaciosParqueoModificar.CostoPorHora;
            espaciosParqueoActual.Estado = espaciosParqueoModificar.Estado;

            _ParkinDbcontext.Entry(espaciosParqueoActual).State = EntityState.Modified;
            _ParkinDbcontext.SaveChanges();

            return Ok(espaciosParqueoActual);
        }


    }
}
