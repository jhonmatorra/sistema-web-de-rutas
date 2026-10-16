using Microsoft.AspNetCore.Identity;

namespace sistema_web_de_rutas.Data
{
    public static class InicializadorIdentity
    {
        public static async Task InicializarAsync(
            IServiceProvider servicios,
            IConfiguration configuracion)
        {
            var roles = servicios
                .GetRequiredService<RoleManager<IdentityRole>>();

            var usuarios = servicios
                .GetRequiredService<UserManager<IdentityUser>>();

            var contexto = servicios
                .GetRequiredService<ApplicationDbContext>();

            foreach (var nombre in new[] { "Administrador", "Repartidor" })
            {
                if (!await roles.RoleExistsAsync(nombre))
                {
                    Comprobar(await roles.CreateAsync(
                        new IdentityRole(nombre)));
                }
            }

            // Si ya existe, no cambia su contraseña ni sus permisos.
            var existente = await usuarios.FindByNameAsync("admin");

            if (existente != null)
            {
                return;
            }

            var clave = configuracion["CuentaInicial:Clave"];

            if (string.IsNullOrWhiteSpace(clave))
            {
                throw new InvalidOperationException(
                    "Configura CuentaInicial:Clave en los secretos de usuario.");
            }

            // Guarda la cuenta y su rol como una sola operación.
            await using var transaccion =
                await contexto.Database.BeginTransactionAsync();

            var administrador = new IdentityUser
            {
                UserName = "admin"
            };

            Comprobar(await usuarios.CreateAsync(administrador, clave));

            Comprobar(await usuarios.AddToRoleAsync(
                administrador, "Administrador"));

            await transaccion.CommitAsync();
        }

        private static void Comprobar(IdentityResult resultado)
        {
            if (!resultado.Succeeded)
            {
                throw new InvalidOperationException(
                    string.Join("; ",
                        resultado.Errors.Select(e => e.Description)));
            }
        }
    }
}