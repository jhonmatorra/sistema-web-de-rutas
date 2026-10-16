namespace sistema_web_de_rutas.Models
{
    public class PanelAdministradorViewModel
    {
        public int TotalClientes { get; set; }

        public int ProductosActivos { get; set; }

        public int PedidosPendientes { get; set; }

        public int ClientesSinUbicacion { get; set; }

        public List<Cliente> ClientesMapa { get; set; } = new();

        public List<Pedido> UltimosPedidos { get; set; } = new();

        public List<Repartidor> Repartidores { get; set; } = new();
    }
}