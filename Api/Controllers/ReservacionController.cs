using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using reservacion.Areas.Identity.Data;
using reservacion.Models;

namespace reservacion.Api.Controllers{
    
    [Route("api/[controller]")]
    [Authorize(Roles = "Normal", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ReservacionController: Controller
    {
        private readonly reservacion.Data.ReservacionDbContext _context;
        private readonly Microsoft.AspNetCore.Http.IHttpContextAccessor _httpContextAccessor;

        public ReservacionController(
            reservacion.Data.ReservacionDbContext context,
            Microsoft.AspNetCore.Http.IHttpContextAccessor httpContextAccessor
        )
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public async Task<ActionResult<List<Reservacion>>> GetAllAsync(){
            var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            User user = _context.Users.SingleOrDefaultAsync(u => 
                u.UserName == User.FindFirstValue(ClaimTypes.NameIdentifier)
            ).Result;
            var reservaciones = await _context.Reservacion
                .Where(
                    r => r.UserId.Equals(user.Id)
                ).ToListAsync();
            foreach(var reservacion in reservaciones){
                // Enviar los detalles de usuario es inseguro
                reservacion.User = null;
            }
            return reservaciones;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Reservacion>> GetAsync(int id){
            var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            User user = _context.Users.SingleOrDefaultAsync(u => 
                u.UserName == User.FindFirstValue(ClaimTypes.NameIdentifier)
            ).Result;
            var reservacion = await _context.Reservacion
                .SingleOrDefaultAsync(r => 
                    r.ID == id &&
                    r.UserId.Equals(user.Id)
                );
            if(reservacion == null){
                return NotFound();
            }
            // Enviar los detalles de usuario es inseguro
            reservacion.User = null;
            return reservacion;
        }

        [HttpPost]
        public async Task<ActionResult<Reservacion>> PostAsync([FromBody]Reservacion reservacion){
            if(!ModelState.IsValid){
                List<string> errores = new List<string>();
                foreach(var value in ModelState.Values){
                    foreach(var error in value.Errors){
                        errores.Add(error.ErrorMessage);
                    }
                }
                return BadRequest(errores);
            }
            var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            User user = _context.Users.SingleOrDefaultAsync(u => 
                u.UserName == User.FindFirstValue(ClaimTypes.NameIdentifier)
            ).Result;
            reservacion.FechaReservacion = DateTime.Now;
            reservacion.UserId = user.Id;
            _context.Reservacion.Add(reservacion);
            await _context.SaveChangesAsync();
            reservacion.User = null;
            return Ok(reservacion);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<Reservacion>> PutAsync(int id, [FromBody]Reservacion reservacion){
            var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            User user = _context.Users.SingleOrDefaultAsync(u => 
                u.UserName == User.FindFirstValue(ClaimTypes.NameIdentifier)
            ).Result;

            var reservacionOld = await _context.Reservacion
                .SingleOrDefaultAsync(r => 
                    r.ID == id &&
                    r.UserId.Equals(user.Id)
                );
            if(reservacionOld == null){
                return NotFound();
            }
            reservacion.ID = id;
            reservacion.UserId = user.Id;
            Boolean isValid = TryValidateModel(reservacion, nameof(reservacion));
            if(!isValid){
                List<string> errores = new List<string>();
                foreach(var value in ModelState.Values){
                    foreach(var error in value.Errors){
                        errores.Add(error.ErrorMessage);
                    }
                }
                return BadRequest(errores);
            }
            
            reservacionOld.FechaReservacion = DateTime.Now;
            reservacionOld.UserId = user.Id;
            reservacionOld.ReservacionDesde = reservacion.ReservacionDesde;
            reservacionOld.ReservacionHasta = reservacion.ReservacionHasta;
            reservacionOld.SalaId = reservacion.SalaId;


            _context.Attach(reservacionOld).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            reservacionOld.User = null;
            return Ok(reservacionOld);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> PutAsync(int id){
            var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            User user = _context.Users.SingleOrDefaultAsync(u => 
                u.UserName == User.FindFirstValue(ClaimTypes.NameIdentifier)
            ).Result;

            var reservacion = await _context.Reservacion
                .SingleOrDefaultAsync(r => 
                    r.ID == id &&
                    r.UserId.Equals(user.Id)
                );
            if(reservacion == null){
                return NotFound();
            }

            _context.Reservacion.Remove(reservacion);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}