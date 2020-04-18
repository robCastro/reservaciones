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
    public class IndexModel : PageModel
    {
        private readonly reservacion.Data.ReservacionDbContext _context;

        public IndexModel(reservacion.Data.ReservacionDbContext context)
        {
            _context = context;
        }

        public IList<Sala> Sala { get;set; }

        public async Task OnGetAsync()
        {
            Sala = await _context.Sala.ToListAsync();
        }
    }
}
