using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using reservacion.Data;

namespace reservacion.Models.CustomValidations{    
    
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
    public class ReservacionDesdeValidator : ValidationAttribute
    {
        public ReservacionDesdeValidator()
        {
            
        }

        public DateTime ReservacionDesde;
        public DateTime ReservacionHasta;

        public string GetErrorMessage(int tipo){
            string msj = "";
            if(tipo == 1)
                msj = $"Reservacion {ReservacionDesde} - {ReservacionHasta} choca con otra reservacion para esta sala.";
            if(tipo == 2)
                msj = "Ud tiene reservaciones en otra sala hechas que chocan con estas";
            return msj;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext){
            ReservacionDbContext context = (ReservacionDbContext)validationContext.GetService(typeof(ReservacionDbContext));
            Reservacion Reservacion = (Reservacion)validationContext.ObjectInstance;
            ReservacionDesde = Reservacion.ReservacionDesde;
            ReservacionHasta = Reservacion.ReservacionHasta;
            
            // Reservaciones en rango de fechas, excepto la reservacion en edicion
            // no filtro de una vez por sala
            // porque reutilizo esto para filtrar por usuario
            var queryReservaciones = context.Reservacion.Where( r => (
                (r.ReservacionDesde >= ReservacionDesde && r.ReservacionDesde <= ReservacionHasta)
                ||
                (r.ReservacionHasta >= ReservacionDesde && r.ReservacionHasta <= ReservacionHasta)
                ||
                (r.ReservacionDesde <= ReservacionDesde && r.ReservacionHasta >= ReservacionHasta)
            ) && r.ID != Reservacion.ID);
            
            // Reservaciones en especifico de la sala dentro rango de fechas
            var queryReservacionesSala = queryReservaciones.Where(r =>r.SalaId == Reservacion.SalaId);
            
            // Si hay reservaciones para sala actual
            var countReservacionesSala = queryReservacionesSala.Count();
            if(queryReservacionesSala.Count() > 0){
                return new ValidationResult(GetErrorMessage(1));
            }
            else{
                // No hay reservaciones en la sala, pero aun no se valida si el usuario tiene otras
                // reservaciones que choquen con los rangos; la siguiente consulta toma todas las
                // reservaciones dentro del rango y las filtra por usuario
                var queryReservacionesUsuario = queryReservaciones.Where(r => 
                    r.UserId == Reservacion.UserId
                );
                var countReservacionesUsuario = queryReservacionesUsuario.Count();
                if(queryReservacionesUsuario.Count() > 0){
                    // Usuario tiene otras reservaciones en el rango
                    return new ValidationResult(GetErrorMessage(2));
                }
            }
            return ValidationResult.Success;
        }
    }
}