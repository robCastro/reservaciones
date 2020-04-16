using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using reservacion.Models;

namespace reservacion.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the User class
    public class User : IdentityUser
    {
        [PersonalData]
        public string Nombres{get; set;}

        [PersonalData]
        public String Apellidos{get; set;}

        public List<Reservacion> Reservaciones {get; set;}
    }
}
