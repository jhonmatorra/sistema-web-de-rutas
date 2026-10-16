using System.ComponentModel.DataAnnotations;

namespace sistema_web_de_rutas.Models
{
    public class CrearRepartidorViewModel
    {
        [Required(ErrorMessage = "Escribe el nombre del repartidor.")]
        [StringLength(100)]
        public string Nombre { get; set; } = "";

        [Required]
        [StringLength(50)]
        [RegularExpression(@"^[a-zA-Z0-9._-]+$",
            ErrorMessage = "Usa letras, números, punto, guion o guion bajo.")]
        public string Usuario { get; set; } = "";

        [Required]
        [MinLength(10,
            ErrorMessage = "La contraseña requiere al menos 10 caracteres.")]
        [DataType(DataType.Password)]
        public string Clave { get; set; } = "";

        [Required]
        [Compare(nameof(Clave),
            ErrorMessage = "Las contraseñas no coinciden.")]
        [DataType(DataType.Password)]
        public string ConfirmarClave { get; set; } = "";

        [Range(1, 100000)]
        public int CapacidadCarga { get; set; } = 100;

        public bool Disponible { get; set; } = true;
    }
}