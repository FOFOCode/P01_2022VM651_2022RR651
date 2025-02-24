using System.ComponentModel.DataAnnotations;

namespace P01_2022VM651_2022RR651.Models
{
	public class EspaciosParqueo
	{
		[Key]
		public int  EspacioID { get; set; }
		public int SucursalID { get; set; }
		public int ? NumeroEspacio { get; set; }
		public string ? Ubicacion { get; set; }

		public decimal ? CostoPorHora { get; set; }

		public string ? Estado { get; set; }






	}
}
