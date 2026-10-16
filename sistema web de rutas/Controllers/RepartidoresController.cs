using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sistema_web_de_rutas.Data;
using sistema_web_de_rutas.Models;

namespace sistema_web_de_rutas.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class RepartidoresController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _usuarios;
        private readonly RoleManager<IdentityRole> _roles;

        public RepartidoresController(
            ApplicationDbContext context,
            UserManager<IdentityUser> usuarios,
            RoleManager<IdentityRole> roles)
        {
            _context = context;
            _usuarios = usuarios;
            _roles = roles;
        }

        public async Task<IActionResult> Index()
        {
            var repartidores = await _context.Repartidores
                .Include(r => r.Usuario)
                .OrderBy(r => r.Nombre)
                .ToListAsync();

            return View(repartidores);
        }

        [HttpGet]
        public IActionResult Crear()
        {
            return View(new CrearRepartidorViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(
            CrearRepartidorViewModel modelo)
        {
            if (!ModelState.IsValid)
            {
                return View(modelo);
            }

            var existente = await _usuarios.FindByNameAsync(
                modelo.Usuario);

            if (existente != null)
            {
                ModelState.AddModelError(nameof(modelo.Usuario),
                    "Ese nombre de usuario ya está registrado.");

                return View(modelo);
            }

            if (!await _roles.RoleExistsAsync("Repartidor"))
            {
                ModelState.AddModelError("",
                    "No existe el rol Repartidor. Revisa la inicialización de roles.");

                return View(modelo);
            }

            // Cuenta, rol y perfil se guardan juntos.
            await using var transaccion =
                await _context.Database.BeginTransactionAsync();

            var usuario = new IdentityUser
            {
                UserName = modelo.Usuario
            };

            var resultado = await _usuarios.CreateAsync(
                usuario, modelo.Clave);

            if (!resultado.Succeeded)
            {
                foreach (var error in resultado.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(modelo);
            }

            var resultadoRol = await _usuarios.AddToRoleAsync(
                usuario, "Repartidor");

            if (!resultadoRol.Succeeded)
            {
                foreach (var error in resultadoRol.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(modelo);
            }

            var repartidor = new Repartidor
            {
                Nombre = modelo.Nombre.Trim(),
                UsuarioId = usuario.Id,
                CapacidadCarga = modelo.CapacidadCarga,
                Disponible = modelo.Disponible
            };

            _context.Repartidores.Add(repartidor);
            await _context.SaveChangesAsync();

            await transaccion.CommitAsync();

            TempData["Mensaje"] = "Repartidor registrado correctamente.";

            return RedirectToAction(nameof(Index));
        }
    }
}