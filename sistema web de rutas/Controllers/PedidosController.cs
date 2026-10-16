using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using sistema_web_de_rutas.Data;
using sistema_web_de_rutas.Models;

namespace sistema_web_de_rutas.Controllers
{
    public class PedidosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PedidosController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var pedidos = await _context.Pedidos
                .Include(p => p.Cliente)
                .OrderByDescending(p => p.Id)
                .ToListAsync();

            return View(pedidos);
        }

        private async Task CargarOpciones(CrearPedidoViewModel modelo)
        {
            var clientes = await _context.Clientes
                .Where(c => c.Activo)
                .OrderBy(c => c.Nombre)
                .ToListAsync();

            modelo.Clientes = clientes
                .Select(c => new SelectListItem(
                    c.Nombre, c.Id.ToString()))
                .ToList();

            var productos = await _context.Productos
                .ToDictionaryAsync(p => p.Id);

            foreach (var linea in modelo.Lineas)
            {
                if (productos.TryGetValue(linea.ProductoId, out var producto))
                {
                    linea.Nombre = producto.Nombre;
                    linea.PrecioReferencia = producto.Precio;
                }
                else
                {
                    linea.Nombre = "Producto no disponible";
                }
            }
        }

        [HttpGet]
        public async Task<IActionResult> Crear()
        {
            var productos = await _context.Productos
                .Where(p => p.Activo)
                .OrderBy(p => p.Nombre)
                .ToListAsync();

            var modelo = new CrearPedidoViewModel
            {
                Lineas = productos.Select(p => new LineaPedidoViewModel
                {
                    ProductoId = p.Id
                }).ToList()
            };

            await CargarOpciones(modelo);
            return View(modelo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(
            CrearPedidoViewModel modelo)
        {
            var seleccionadas = modelo.Lineas
                .Where(l => l.Cantidad > 0)
                .ToList();

            if (!seleccionadas.Any())
            {
                ModelState.AddModelError("",
                    "Indica una cantidad mayor a cero en algún producto.");
            }

            if (seleccionadas.Select(l => l.ProductoId)
                .Distinct().Count() != seleccionadas.Count)
            {
                ModelState.AddModelError("",
                    "No se permite repetir un producto.");
            }

            var clienteValido = await _context.Clientes
                .AnyAsync(c => c.Id == modelo.ClienteId && c.Activo);

            if (!clienteValido)
            {
                ModelState.AddModelError("",
                    "Selecciona un cliente activo.");
            }

            var ids = seleccionadas.Select(l => l.ProductoId).ToList();

            var productos = await _context.Productos
                .Where(p => ids.Contains(p.Id) && p.Activo)
                .ToDictionaryAsync(p => p.Id);

            if (seleccionadas.Any(l =>
                !productos.ContainsKey(l.ProductoId)))
            {
                ModelState.AddModelError("",
                    "Uno de los productos ya no está disponible.");
            }

            if (!ModelState.IsValid)
            {
                await CargarOpciones(modelo);
                return View(modelo);
            }

            var pedido = new Pedido
            {
                ClienteId = modelo.ClienteId,
                EstadoPago = modelo.EstadoPago,
                MetodoPago = modelo.MetodoPago,
                Observaciones = modelo.Observaciones
            };

            foreach (var linea in seleccionadas)
            {
                var producto = productos[linea.ProductoId];

                pedido.Detalles.Add(new DetallePedido
                {
                    ProductoId = producto.Id,
                    Cantidad = linea.Cantidad,
                    PrecioUnitario = producto.Precio,
                    CargaUnitaria = producto.UnidadesCarga
                });

                pedido.Total += linea.Cantidad * producto.Precio;
            }

            if (pedido.Total > 9999999999.99m)
            {
                ModelState.AddModelError("",
                    "El importe supera el máximo permitido.");

                await CargarOpciones(modelo);
                return View(modelo);
            }

            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Detalle),
                new { id = pedido.Id });
        }

        public async Task<IActionResult> Detalle(int id)
        {
            var pedido = await _context.Pedidos
                .Include(p => p.Cliente)
                .Include(p => p.Detalles)
                    .ThenInclude(d => d.Producto)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pedido == null)
            {
                return NotFound();
            }

            return View(pedido);
        }
    }
}