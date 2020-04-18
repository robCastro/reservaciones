using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using reservacion.Data;
using reservacion.Models;

namespace reservacion.Pages.Reservaciones
{
    [Authorize(Roles = "Normal")]
    public class IndexModel : PageModel
    {
        private const string query = @"
                SELECT * FROM Sala WHERE ID NOT IN (
                    SELECT DISTINCT SALAID FROM Reservacion
                    WHERE (
                        (ReservacionDesde >= @startDate AND ReservacionDesde <= @endDate)
                        OR
                        (ReservacionHasta >= @startDate AND ReservacionHasta <= @endDate)
                        OR
                        (ReservacionDesde <= @startDate AND ReservacionHasta >= @endDate)
                    )
                )
            ";
        private readonly reservacion.Data.ReservacionDbContext _context;

        public IndexModel(reservacion.Data.ReservacionDbContext context)
        {
            _context = context;
        }

        public IList<Reservacion> Reservacion { get;set; }
        public IList<Sala> Sala { get;set; }
        
        [BindProperty]
        public DateTime startDate {get; set;}
        
        [BindProperty]
        public DateTime endDate {get; set;}
        public int salaId{get; set;}

        public async Task OnGetAsync()
        {
            DateTime helper = DateTime.Now;
            startDate = new DateTime(helper.Year, helper.Month, helper.Day, helper.Hour, helper.Minute, 0);
            endDate = startDate.AddHours(2);
            await consultarSalas();
            Reservacion = await _context.Reservacion
                .Include(r => r.Sala)
                .Include(r => r.User).Where(
                    r => r.UserId.Equals(User.FindFirstValue(ClaimTypes.NameIdentifier))
                ).ToListAsync();
            
        }

        public async Task<IActionResult> OnPostAsync(){
            await consultarSalas();
            Reservacion = await _context.Reservacion
                .Include(r => r.Sala)
                .Include(r => r.User).Where(
                    r => r.UserId.Equals(User.FindFirstValue(ClaimTypes.NameIdentifier))
                ).ToListAsync();
            return Page();
        }

        private async Task consultarSalas(){
            Sala = await _context.Sala
                .FromSqlRaw(
                    query, 
                    new SqlParameter("@startDate", startDate), 
                    new SqlParameter("@endDate", endDate)
                ).ToListAsync();
        }
    }
}
