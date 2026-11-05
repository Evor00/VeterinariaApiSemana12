using Microsoft.EntityFrameworkCore;
using VeterinariaApi.Models;

namespace VeterinariaApi.Data;

public partial class VeterinariaContext(DbContextOptions<VeterinariaContext> options) : DbContext(options)
{
    public virtual DbSet<Mascota> Mascotas { get; set; }
    public virtual DbSet<Cita> Citas { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Mascota>(entity => { entity.ToTable("Mascota"); entity.HasIndex(e => e.CodigoChip).IsUnique(); });
        modelBuilder.Entity<Cita>(entity =>
        {
            entity.ToTable("Cita");
            entity.Property(e => e.FechaHora).HasColumnType("timestamp without time zone");
            entity.HasIndex(e => new { e.MascotaId, e.FechaHora }).IsUnique();
            entity.HasOne(d => d.Mascota).WithMany(p => p.Citas).HasForeignKey(d => d.MascotaId).OnDelete(DeleteBehavior.Restrict);
        });
    }
}
