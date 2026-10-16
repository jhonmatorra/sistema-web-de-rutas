using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sistema_web_de_rutas.Data;
using sistema_web_de_rutas.Models;

namespace sistema_web_de_rutas.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class PanelController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PanelController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var modelo = new PanelAdministradorViewModel();

            modelo.TotalClientes = await _context.Clientes.CountAsync();

            modelo.ProductosActivos = await _context.Productos
                .CountAsync(p => p.Activo);

            modelo.PedidosPendientes = await _context.Pedidos
                .CountAsync(p => p.Estado == "Pendiente");

            var clientesActivos = _context.Clientes
                .AsNoTracking()
                .Where(c => c.Activo);

            var totalActivos = await clientesActivos.CountAsync();

            modelo.ClientesMapa = await clientesActivos
                .Where(c =>
                    c.Latitud.HasValue &&
                    c.Longitud.HasValue &&
                    c.Latitud >= -90.0 &&
                    c.Latitud <= 90.0 &&
                    c.Longitud >= -180.0 &&
                    c.Longitud <= 180.0)
                .OrderBy(c => c.Nombre)
                .ToListAsync();

            modelo.ClientesSinUbicacion =
                totalActivos - modelo.ClientesMapa.Count;

            modelo.UltimosPedidos = await _context.Pedidos
                .AsNoTracking()
                .Include(p => p.Cliente)
                .OrderByDescending(p => p.Fecha)
                .ThenByDescending(p => p.Id)
                .Take(6)
                .ToListAsync();

            modelo.Repartidores = await _context.Repartidores
                .AsNoTracking()
                .OrderBy(r => r.Nombre)
                .ToListAsync();

            return View(modelo);
        }
    }
}