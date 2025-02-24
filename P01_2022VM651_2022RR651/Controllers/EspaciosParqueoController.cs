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
            List<EspaciosParqueo> listadoEspaciosParquo = (from ep in _ParkinDbcontext.Espacios select ep).ToList();

            if(listadoEspaciosParquo.Count() == 0)
            {
                return NotFound();
            }

            return Ok(listadoEspaciosParquo);
        }


    }
}
