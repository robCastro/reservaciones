using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using reservacion.Data;
using reservacion.Models;

namespace reservacion.Pages.Reservaciones
{
    [Authorize(Roles = "Normal")]
    public class DetailsModel : PageModel
    {
        private readonly reservacion.Data.ReservacionDbContext _context;

        public DetailsModel(reservacion.Data.ReservacionDbContext context)
        {
            _context = context;
        }

        public Reservacion Reservacion { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Reservacion = await _context.Reservacion
                .Include(r => r.Sala)
                .Include(r => r.User).FirstOrDefaultAsync(m => m.ID == id);

            if (Reservacion == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
