using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace sistema_web_de_rutas.Models
{
    public class Producto
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = "";

        [StringLength(300)]
        public string? Descripcion { get; set; }

        // TODO: Mostrar como "Carga por pieza" y validar los valores
        // según el espacio y las condiciones de transporte de cada producto.
        [Precision(12, 2)]
        [Range(typeof(decimal), "0.01", "100000",
            ParseLimitsInInvariantCulture = true,
            ErrorMessage = "El precio debe estar entre 0.01 y 100000.")]
        public decimal Precio { get; set; }

        [Range(0, 100000)]
        public int Stock { get; set; }

        [Range(1, 100000)]
        public int UnidadesCarga { get; set; } = 1;

        public bool Activo { get; set; } = true;
    }
}