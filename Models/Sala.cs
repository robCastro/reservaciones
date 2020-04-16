using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace reservacion.Models{
	public class Sala{
		public int ID {get; set;}
		
		[Display(Name = "Nombre")]
		public string Nombre {get; set;}
		
		[Display(Name = "Ubicacion")]
		public string DescripcionUbicacion {get; set;}

		public List<Reservacion> Reservaciones {get;set;}
	}
}