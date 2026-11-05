using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VeterinariaApi.Data;
using VeterinariaApi.Models;

namespace VeterinariaApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CitasController(VeterinariaContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Cita>>> GetCitas([FromQuery] DateTime? desde, [FromQuery] int? mascotaId)
    {
        IQueryable<Cita> q = context.Citas.AsNoTracking().Include(c => c.Mascota);
        if (desde.HasValue) q = q.Where(c => c.FechaHora >= desde.Value);
        if (mascotaId.HasValue) q = q.Where(c => c.MascotaId == mascotaId.Value);
        return Ok(await q.OrderBy(c => c.FechaHora).ToListAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Cita>> GetCita(int id)
    {
        var cita = await context.Citas.AsNoTracking().Include(c => c.Mascota).FirstOrDefaultAsync(c => c.CitaId == id);
        return cita is null ? NotFound() : Ok(cita);
    }

    [HttpPost]
    public async Task<ActionResult<Cita>> PostCita(Cita cita)
    {
        if (!await context.Mascotas.AnyAsync(m => m.MascotaId == cita.MascotaId)) return BadRequest("La mascota indicada no existe.");
        if (await context.Citas.AnyAsync(c => c.MascotaId == cita.MascotaId && c.FechaHora == cita.FechaHora)) return Conflict("La mascota ya tiene una cita en esa fecha y hora.");
        context.Citas.Add(cita);
        await context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetCita), new { id = cita.CitaId }, cita);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutCita(int id, Cita cita)
    {
        if (id != cita.CitaId) return BadRequest("El id de la ruta no coincide con el cuerpo.");
        if (!await context.Citas.AnyAsync(c => c.CitaId == id)) return NotFound();
        if (!await context.Mascotas.AnyAsync(m => m.MascotaId == cita.MascotaId)) return BadRequest("La mascota indicada no existe.");
        if (await context.Citas.AnyAsync(c => c.CitaId != id && c.MascotaId == cita.MascotaId && c.FechaHora == cita.FechaHora)) return Conflict("La mascota ya tiene una cita en esa fecha y hora.");
        context.Entry(cita).State = EntityState.Modified;
        await context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCita(int id)
    {
        var cita = await context.Citas.FindAsync(id);
        if (cita is null) return NotFound();
        context.Citas.Remove(cita);
        await context.SaveChangesAsync();
        return NoContent();
    }
}
