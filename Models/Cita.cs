using System.ComponentModel.DataAnnotations;
namespace VeterinariaApi.Models;
public partial class Cita
{
    public int CitaId { get; set; }
    [Required] public DateTime FechaHora { get; set; }
    [Required, StringLength(150)] public string Motivo { get; set; } = null!;
    [Required, StringLength(80)] public string Veterinario { get; set; } = null!;
    [Range(1, int.MaxValue)] public int MascotaId { get; set; }
    public virtual Mascota? Mascota { get; set; }
}
