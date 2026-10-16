using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using sistema_web_de_rutas.Models;

namespace sistema_web_de_rutas.Controllers
{
    [Authorize]
    public class AccesoController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccesoController(
            SignInManager<IdentityUser> signInManager)
        {
            _signInManager = signInManager;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Ingresar(string? returnUrl = null)
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction(nameof(Inicio));
            }

            return View(new LoginViewModel
            {
                ReturnUrl = returnUrl
            });
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Ingresar(
            LoginViewModel modelo)
        {
            if (!ModelState.IsValid)
            {
                return View(modelo);
            }

            var resultado = await _signInManager.PasswordSignInAsync(
                modelo.Usuario.Trim(),
                modelo.Clave,
                isPersistent: false,
                lockoutOnFailure: true);

            if (resultado.Succeeded)
            {
                if (Url.IsLocalUrl(modelo.ReturnUrl))
                {
                    return LocalRedirect(modelo.ReturnUrl!);
                }

                return RedirectToAction(nameof(Inicio));
            }

            ModelState.AddModelError("",
                "Usuario o contraseña incorrectos, o acceso bloqueado temporalmente.");

            return View(modelo);
        }

        public IActionResult Inicio()
        {
            if (User.IsInRole("Administrador"))
            {
                return RedirectToAction("Index", "Panel");
            }

            return View("PanelRepartidor");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Salir()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction(nameof(Ingresar));
        }

        public IActionResult Denegado()
        {
            return StatusCode(403,
                "Tu cuenta no tiene permiso para acceder a esta sección.");
        }
    }
}