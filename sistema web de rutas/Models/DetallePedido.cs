using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace sistema_web_de_rutas.Models
{
    public class DetallePedido
    {
        public int Id { get; set; }

        public int PedidoId { get; set; }

        public Pedido Pedido { get; set; } = null!;

        public int ProductoId { get; set; }

        public Producto Producto { get; set; } = null!;

        [Range(1, 10000)]
        public int Cantidad { get; set; }

        // Conserva el precio aplicado al registrar el pedido.
        [Precision(12, 2)]
        public decimal PrecioUnitario { get; set; }

        // Conserva la carga por pieza aplicada al registrar el pedido.
        [Range(1, 100000)]
        public int CargaUnitaria { get; set; } = 1;

        [NotMapped]
        public decimal Subtotal => Cantidad * PrecioUnitario;
    }
}