using System;
using System.ComponentModel.DataAnnotations;
using reservacion.Areas.Identity.Data;

namespace reservacion.Models{
	public class Reservacion{
		public int ID {get; set;}
		
		[Display(Name = "Creada el")]
		public DateTime FechaReservacion {get; set;}
		
        [Display(Name = "Desde")]
		public DateTime ReservacionDesde {get; set;}

        [Display(Name = "Hasta")]
		public DateTime ReservacionHasta {get; set;}

		public string UserId {get; set;}
        public User User {get; set;}

        public int SalaId {get; set;}
        public Sala Sala {get; set;}

	}
}