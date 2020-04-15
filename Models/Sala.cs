using System;
using System.ComponentModel.DataAnnotations;

namespace reservacion.Models{
	public class Sala{
		public int ID {get; set;}
		public string nombre {get; set;}
		public string descripcionUbicacion {get; set;}
	}
}