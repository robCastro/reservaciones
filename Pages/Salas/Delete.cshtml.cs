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

namespace reservacion.Pages.Salas
{
    
    [Authorize(Roles="Administrador")]
    public class DeleteModel : PageModel
    {
        private readonly reservacion.Data.ReservacionDbContext _context;

        public DeleteModel(reservacion.Data.ReservacionDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Sala Sala { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Sala = await _context.Sala.FirstOrDefaultAsync(m => m.ID == id);

            if (Sala == null)
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

            Sala = await _context.Sala.FindAsync(id);

            if (Sala != null)
            {
                _context.Sala.Remove(Sala);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
