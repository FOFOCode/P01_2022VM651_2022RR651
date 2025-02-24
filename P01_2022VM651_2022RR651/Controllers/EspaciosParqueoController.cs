﻿using Microsoft.AspNetCore.Http;
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

        //[HttpPost]
        //[Route("Add")]
        //public IActionResult GuardarEspacioParqueo(EspaciosParqueo espaciosParqueo)
        //{
        //    _ParkinDbcontext.Espacios.Add(espaciosParqueo);
        //    _ParkinDbcontext.SaveChanges();
        //    return Ok();
        //}


    }
}
