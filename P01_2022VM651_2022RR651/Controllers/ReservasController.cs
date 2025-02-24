using Microsoft.AspNetCore.Mvc;
using P01_2022VM651_2022RR651.Models;

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
		[Route("GetActiveReservations/{usuarioId}")]
		public IActionResult GetActiveReservations(int usuarioId)
		{
			var reservasActivas = _reservaContexto.Reservas
				.Where(r => r.UsuarioID == usuarioId && r.Estado == "Activa")
				.ToList();

			if (!reservasActivas.Any())
			{
				return NotFound("No hay reservas activas para este usuario.");
			}
			return Ok(reservasActivas);
		}

	}
}
