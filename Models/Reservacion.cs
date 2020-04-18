using System;
using System.ComponentModel.DataAnnotations;
using reservacion.Areas.Identity.Data;
using reservacion.Models.CustomValidations;

namespace reservacion.Models{
	//[MetadataType(typeof(ReservacionMetadata))]
	public class Reservacion{
		public int ID {get; set;}
		
		[Display(Name = "Creada el")]
		public DateTime FechaReservacion {get; set;}
		
		[DataType(DataType.DateTime)]
        [Display(Name = "Desde")]
		public DateTime ReservacionDesde {get; set;}

		[DataType(DataType.DateTime)]
        [Display(Name = "Hasta")]
		[ReservacionDesdeValidator]
		public DateTime ReservacionHasta {get; set;}

		public string UserId {get; set;}
        public User User {get; set;}

        public int SalaId {get; set;}
        public Sala Sala {get; set;}

	}
}