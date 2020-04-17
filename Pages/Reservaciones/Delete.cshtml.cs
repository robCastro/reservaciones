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
    [Authorize]
    public class DeleteModel : PageModel
    {
        private readonly reservacion.Data.ReservacionDbContext _context;

        public DeleteModel(reservacion.Data.ReservacionDbContext context)
        {
            _context = context;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Reservacion = await _context.Reservacion.FindAsync(id);

            if (Reservacion != null)
            {
                _context.Reservacion.Remove(Reservacion);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
