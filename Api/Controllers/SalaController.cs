using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using reservacion.Models;

namespace reservacion.Api.Controllers{
    
    [Route("api/[controller]")]
    [Authorize(Roles = "Administrador", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class SalaController: Controller
    {
        private readonly reservacion.Data.ReservacionDbContext _context;

        public SalaController(reservacion.Data.ReservacionDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Sala>>> GetAllAsync(){
            return await _context.Sala.ToListAsync();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Sala>> GetAsync(int id){
            var sala = await _context.Sala.FindAsync(id);
            if(sala != null){
                return sala;
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<Sala>> PostAsync([FromBody] Sala sala){
            Boolean isValid = TryValidateModel(sala);
            if(!isValid){
                return BadRequest();
            }
            _context.Sala.Add(sala);
            await _context.SaveChangesAsync();
            return sala;
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<Sala>> PutAsync(int id,[FromBody] Sala sala){
            var salaOld = await _context.Sala.FindAsync(id);
            if(salaOld == null){
                return NotFound();
            }
            Boolean isValid = TryValidateModel(sala);
            if(!isValid){
                return BadRequest();
            }
            salaOld.Nombre = sala.Nombre;
            salaOld.DescripcionUbicacion = sala.DescripcionUbicacion;
            salaOld.Reservaciones = sala.Reservaciones;
            _context.Attach(salaOld).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return salaOld;
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> PutAsync(int id){
            Sala sala = await _context.Sala.FindAsync(id);
            if(sala == null){
                return NotFound();
            }
            else{
                _context.Sala.Remove(sala);
                await _context.SaveChangesAsync();
                return Ok();
            }
        }
    }
}