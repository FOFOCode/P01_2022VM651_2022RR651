using Microsoft.AspNetCore.Mvc;
using P01_2022VM651_2022RR651.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace P01_2022VM651_2022RR651.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ReservasController : Controller
	{
		private readonly ParkingDBContext _reservaContexto;

		public ReservasController(ParkingDBContext reservaContexto)
		{
			_reservaContexto = reservaContexto;
		}
		[HttpGet]
		[Route("GetReservacionesActivas/{usuarioId}")]
		public IActionResult GetReservacionesActivas(int usuarioId)
		{
			var reservasActivas = (from r in _reservaContexto.Reservas
								   where r.UsuarioID == usuarioId && r.Estado == "Activa"
								   select r).ToList();

			if (!reservasActivas.Any())
			{
				return NotFound("No hay reservas activas para este usuario.");
			}
			return Ok(reservasActivas);
		}

		[HttpGet]
		[Route("GetEspaciosReservadosPorDia")]
		public IActionResult GetEspaciosReservadosPorDia()
		{
			var reservasPorDia = (from r in _reservaContexto.Reservas
								  join e in _reservaContexto.EspaciosParqueo on r.EspacioID equals e.EspacioID
								  join s in _reservaContexto.Sucursales on e.SucursalID equals s.SucursalID
								  where r.FechaReserva.HasValue && r.Estado == "Activa"
								  group new { r, s } by r.FechaReserva.Value.Date into g
								  select new
								  {
									  Fecha = g.Key,
									  TotalReservas = g.Count(),
									  Sucursales = g.GroupBy(x => x.s.Nombre)
													.Select(sg => new
													{
														NombreSucursal = sg.Key,
														TotalReservasSucursal = sg.Count(),
														Espacios = sg.Select(x => new
														{
															x.r.EspacioID,
															x.r.UsuarioID
														}).ToList()
													}).ToList()
								  }).ToList();

			if (!reservasPorDia.Any())
			{
				return NotFound();
			}

			return Ok(reservasPorDia);
		}

		[HttpGet]
		[Route("GetEspaciosReservadosPorRango")]
		public IActionResult GetEspaciosReservadosPorRango(DateTime fechaInicio, DateTime fechaFin, int sucursalId)
		{
			var reservas = (from r in _reservaContexto.Reservas
							join e in _reservaContexto.EspaciosParqueo on r.EspacioID equals e.EspacioID
							join s in _reservaContexto.Sucursales on e.SucursalID equals s.SucursalID
							where r.FechaReserva >= fechaInicio && r.FechaReserva <= fechaFin
								  && s.SucursalID == sucursalId
								  && r.Estado == "Activa"
							select new
							{
								r.FechaReserva,
								r.EspacioID,
								r.UsuarioID
							}).ToList();

			if (!reservas.Any())
			{
				return NotFound();
			}

			return Ok(reservas);
		}

		[HttpPost]
		[Route("CancelarReserva")]
		public IActionResult CancelarReserva(int reservaID)
		{
			var reserva = _reservaContexto.Reservas.FirstOrDefault(r => r.ReservaID == reservaID && r.Estado == "Activa");

			if (reserva == null)
			{
				return NotFound("Reserva no encontrada o ya ha sido cancelada/completada.");
			}

			reserva.Estado = "Cancelada";
			_reservaContexto.SaveChanges();

			return Ok("Reserva cancelada exitosamente.");
		}

		[HttpPost]
		[Route("ReservarEspacio")]
		public IActionResult ReservarEspacio(int usuarioId, int espacioId, DateTime fechaReserva, int cantidadHoras, TimeSpan horaReserva)
		{
			var espacioDisponible = _reservaContexto.EspaciosParqueo
				.FirstOrDefault(e => e.EspacioID == espacioId && e.Estado == "Disponible");

			if (espacioDisponible == null)
			{
				return NotFound(new { Message = "El espacio no está disponible." });
			}

			var nuevaReserva = new Reservas
			{
				UsuarioID = usuarioId,
				EspacioID = espacioId,
				FechaReserva = fechaReserva.Add(horaReserva), 
				CantidadHoras = cantidadHoras,
				Estado = "Activa"
			};

			
			espacioDisponible.Estado = "Ocupado";

			
			_reservaContexto.Reservas.Add(nuevaReserva);
			_reservaContexto.SaveChanges();

			return Ok(new { Message = "Reserva realizada con éxito." });
		}



	}
}
