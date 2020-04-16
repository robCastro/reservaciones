using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using reservacion.Data;
using reservacion.Models;

namespace reservacion.Pages.Salas
{
    public class CreateModel : PageModel
    {
        private readonly reservacion.Data.ReservacionDbContext _context;

        public CreateModel(reservacion.Data.ReservacionDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Sala Sala { get; set; }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Sala.Add(Sala);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
