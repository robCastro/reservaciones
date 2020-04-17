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

        public string GetErrorMessage() =>
        $"Reservacion {ReservacionDesde} - {ReservacionHasta} choca con otra reservacion para esta sala.";

        protected override ValidationResult IsValid(object value, ValidationContext validationContext){
            ReservacionDbContext context = (ReservacionDbContext)validationContext.GetService(typeof(ReservacionDbContext));
            Reservacion Reservacion = (Reservacion)validationContext.ObjectInstance;
            ReservacionDesde = Reservacion.ReservacionDesde;
            ReservacionHasta = Reservacion.ReservacionHasta;
            var query = context.Reservacion.Where(r =>
                r.SalaId == Reservacion.SalaId && (
                    (r.ReservacionDesde >= ReservacionDesde && r.ReservacionDesde <= ReservacionHasta)
                    ||
                    (r.ReservacionHasta >= ReservacionDesde && r.ReservacionHasta <= ReservacionHasta)
                    ||
                    (r.ReservacionDesde <= ReservacionDesde && r.ReservacionHasta >= ReservacionHasta)
                )
            );
            if(Reservacion.ID != 0){
                query = query.Where(r => r.ID != Reservacion.ID);
            }
            if(query.Count() > 0){
                return new ValidationResult(GetErrorMessage());
            }
            return ValidationResult.Success;
        }
    }
}