using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VeterinariaApi.Data;
using VeterinariaApi.Models;

namespace VeterinariaApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MascotasController(VeterinariaContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Mascota>>> GetMascotas([FromQuery] string? especie, [FromQuery] string? nombre)
    {
        IQueryable<Mascota> q = context.Mascotas.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(especie)) q = q.Where(m => m.Especie == especie);
        if (!string.IsNullOrWhiteSpace(nombre)) q = q.Where(m => m.Nombre.Contains(nombre));
        return Ok(await q.OrderBy(m => m.Nombre).ToListAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Mascota>> GetMascota(int id)
    {
        var mascota = await context.Mascotas.AsNoTracking().FirstOrDefaultAsync(m => m.MascotaId == id);
        return mascota is null ? NotFound() : Ok(mascota);
    }

    [HttpGet("{id}/citas")]
    public async Task<ActionResult<IEnumerable<Cita>>> GetCitasDeMascota(int id)
    {
        if (!await context.Mascotas.AnyAsync(m => m.MascotaId == id)) return NotFound();
        return Ok(await context.Citas.AsNoTracking().Where(c => c.MascotaId == id).OrderBy(c => c.FechaHora).ToListAsync());
    }

    [HttpPost]
    public async Task<ActionResult<Mascota>> PostMascota(Mascota mascota)
    {
        if (await context.Mascotas.AnyAsync(m => m.CodigoChip == mascota.CodigoChip)) return Conflict("El código de chip ya está registrado.");
        context.Mascotas.Add(mascota);
        await context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetMascota), new { id = mascota.MascotaId }, mascota);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutMascota(int id, Mascota mascota)
    {
        if (id != mascota.MascotaId) return BadRequest("El id de la ruta no coincide con el cuerpo.");
        if (!await context.Mascotas.AnyAsync(m => m.MascotaId == id)) return NotFound();
        if (await context.Mascotas.AnyAsync(m => m.MascotaId != id && m.CodigoChip == mascota.CodigoChip)) return Conflict("El código de chip ya está registrado.");
        context.Entry(mascota).State = EntityState.Modified;
        await context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMascota(int id)
    {
        var mascota = await context.Mascotas.FindAsync(id);
        if (mascota is null) return NotFound();
        if (await context.Citas.AnyAsync(c => c.MascotaId == id)) return Conflict("No se puede eliminar una mascota que tiene citas registradas.");
        context.Mascotas.Remove(mascota);
        await context.SaveChangesAsync();
        return NoContent();
    }
}
