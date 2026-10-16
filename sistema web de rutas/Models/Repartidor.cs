using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace sistema_web_de_rutas.Models
{
    public class Repartidor
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = "";

        [Required]
        public string UsuarioId { get; set; } = "";

        public IdentityUser Usuario { get; set; } = null!;

        [Range(1, 100000)]
        public int CapacidadCarga { get; set; } = 100;

        public bool Disponible { get; set; } = true;
    }
}