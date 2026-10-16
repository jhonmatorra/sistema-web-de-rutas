using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace sistema_web_de_rutas.Models
{
    public class CrearPedidoViewModel
    {
        [Range(1, int.MaxValue,
            ErrorMessage = "Selecciona un cliente.")]
        public int ClienteId { get; set; }

        [Required]
        [RegularExpression("^(Pendiente|Pagado)$")]
        public string EstadoPago { get; set; } = "Pendiente";

        [Required]
        [RegularExpression("^(Efectivo|QR|Transferencia)$")]
        public string MetodoPago { get; set; } = "Efectivo";

        [StringLength(500)]
        public string? Observaciones { get; set; }

        public List<LineaPedidoViewModel> Lineas { get; set; } = new();

        [ValidateNever]
        public List<SelectListItem> Clientes { get; set; } = new();
    }

    public class LineaPedidoViewModel
    {
        [Range(1, int.MaxValue)]
        public int ProductoId { get; set; }

        [Range(0, 10000,
            ErrorMessage = "La cantidad debe estar entre 0 y 10000.")]
        public int Cantidad { get; set; }

        // Solo para mostrar información en la pantalla.
        [ValidateNever]
        public string Nombre { get; set; } = "";

        [ValidateNever]
        public decimal PrecioReferencia { get; set; }
    }
}