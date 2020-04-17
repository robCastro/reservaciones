using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using reservacion.Data;
using reservacion.Models;

namespace reservacion.Pages.Reservaciones
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly reservacion.Data.ReservacionDbContext _context;

        public EditModel(reservacion.Data.ReservacionDbContext context)
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
           ViewData["SalaId"] = new SelectList(_context.Sala, "ID", "Nombre");
           // ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ViewData["SalaId"] = new SelectList(_context.Sala, "ID", "Nombre");
                return Page();
            }
            Reservacion.FechaReservacion = DateTime.Now;
            Reservacion.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _context.Attach(Reservacion).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReservacionExists(Reservacion.ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool ReservacionExists(int id)
        {
            return _context.Reservacion.Any(e => e.ID == id);
        }
    }
}
