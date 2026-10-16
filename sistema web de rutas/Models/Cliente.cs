using System.ComponentModel.DataAnnotations;

namespace sistema_web_de_rutas.Models
{
    public class Cliente
    {
        public int Id { get; set; }

        [Required]
        [StringLength(120)]
        public string Nombre { get; set; } = "";

        [StringLength(30)]
        public string? Telefono { get; set; }

        [Required]
        [StringLength(240)]
        public string Direccion { get; set; } = "";

        [Required]
        public string Tipo { get; set; } = "Individual";

        [Range(-90, 90)]
        public double? Latitud { get; set; }

        [Range(-180, 180)]
        public double? Longitud { get; set; }

        public bool Activo { get; set; } = true;
    }
}