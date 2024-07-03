

using System.ComponentModel.DataAnnotations;

namespace AppLogins.ViewModels
{
    public class UsuarioVMS
    {
        public int IdUsuario { get; set; }

        [RegularExpression(@"^[A-Z\s]{1,50}$", ErrorMessage = "Caracteres no permitidos")]
        [StringLength(50, ErrorMessage = "El nombre completo no puede tener más de 50 caracteres")]
        [Required(ErrorMessage = "El nombre completo es obligatorio")]
        public string NombreCompleto { get; set; }
        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "El formato del correo electrónico es inválido")]
        public string Correo { get; set; }

        public string Clave { get; set; }
        public string ConfirmarClave { get; set; }
    }
}
