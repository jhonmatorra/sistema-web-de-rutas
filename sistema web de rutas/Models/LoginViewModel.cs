using System.ComponentModel.DataAnnotations;

namespace sistema_web_de_rutas.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Escribe tu usuario.")]
        public string Usuario { get; set; } = "";

        [Required(ErrorMessage = "Escribe tu contraseña.")]
        [DataType(DataType.Password)]
        public string Clave { get; set; } = "";

        public string? ReturnUrl { get; set; }
    }
}