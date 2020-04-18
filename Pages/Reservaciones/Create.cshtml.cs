using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using reservacion.Data;
using reservacion.Models;

namespace reservacion.Pages.Reservaciones
{
    [Authorize(Roles = "Normal")]
    public class CreateModel : PageModel
    {
        private readonly reservacion.Data.ReservacionDbContext _context;

        public CreateModel(reservacion.Data.ReservacionDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["SalaId"] = new SelectList(_context.Sala, "ID", "Nombre");
            return Page();
        }
        
        [BindProperty]
        public Reservacion Reservacion { get; set; }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            Reservacion.FechaReservacion = DateTime.Now;
            Reservacion.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Boolean isValid = TryValidateModel(Reservacion, nameof(Reservacion));
            if(!isValid){
                ViewData["SalaId"] = new SelectList(_context.Sala, "ID", "Nombre");
                return Page();
            }
            /* if (!ModelState.IsValid)
            {
                ViewData["SalaId"] = new SelectList(_context.Sala, "ID", "Nombre");
                return Page();
            } */
            _context.Reservacion.Add(Reservacion);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
        
    }
}
