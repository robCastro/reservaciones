using System;
using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

namespace reservacion.Models{
	public class Sala{
		public int ID {get; set;}
		
		[Display(Name = "Nombre")]
		public string nombre {get; set;}
		
		[Display(Name = "Ubicacion")]
		public string descripcionUbicacion {get; set;}
	}
}