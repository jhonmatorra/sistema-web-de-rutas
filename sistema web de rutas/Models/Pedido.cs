using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace sistema_web_de_rutas.Models
{
    public class Pedido
    {
        public int Id { get; set; }

        public int ClienteId { get; set; }

        public Cliente Cliente { get; set; } = null!;

        // Se guarda en UTC; en pantalla mostraremos la hora de Bolivia.
        public DateTime Fecha { get; set; } = DateTime.UtcNow;

        [Required]
        public string Estado { get; set; } = "Pendiente";

        [Required]
        public string EstadoPago { get; set; } = "Pendiente";

        [Required]
        public string MetodoPago { get; set; } = "Efectivo";

        [StringLength(500)]
        public string? Observaciones { get; set; }

        // El servidor calculará este importe a partir del detalle.
        [Precision(12, 2)]
        public decimal Total { get; set; }

        public List<DetallePedido> Detalles { get; set; } = new();
    }
}