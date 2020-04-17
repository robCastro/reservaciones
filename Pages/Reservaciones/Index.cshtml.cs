using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using reservacion.Data;
using reservacion.Models;

namespace reservacion.Pages.Reservaciones
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly reservacion.Data.ReservacionDbContext _context;

        public IndexModel(reservacion.Data.ReservacionDbContext context)
        {
            _context = context;
        }

        public IList<Reservacion> Reservacion { get;set; }

        public async Task OnGetAsync()
        {
            Reservacion = await _context.Reservacion
                .Include(r => r.Sala)
                .Include(r => r.User).Where(
                    r => r.UserId.Equals(User.FindFirstValue(ClaimTypes.NameIdentifier))
                ).ToListAsync();
        }
    }
}
