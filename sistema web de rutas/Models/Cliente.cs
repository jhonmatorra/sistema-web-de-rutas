using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using sistema_web_de_rutas.ModelBinders;

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

        // TODO: Convertir Tipo en desplegable cuando se definan las categorías.
        [Required]
        public string Tipo { get; set; } = "Individual";

        [ModelBinder(BinderType = typeof(CoordenadaModelBinder))]
        [Range(-90.0, 90.0,
             ErrorMessage = "La latitud debe estar entre -90 y 90.")]
        public double? Latitud { get; set; }

        [ModelBinder(BinderType = typeof(CoordenadaModelBinder))]
        [Range(-180.0, 180.0,
            ErrorMessage = "La longitud debe estar entre -180 y 180.")]
        public double? Longitud { get; set; }

        public bool Activo { get; set; } = true;
    }
}