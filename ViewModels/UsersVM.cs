using System.ComponentModel.DataAnnotations;

namespace AppLogins.ViewModels
{
    public class UsersVM
    {
        public int Id { get; set; }

        [RegularExpression(@"^[A-Z\s]{1,50}$", ErrorMessage = "Caracteres no permitidos")]
        [StringLength(50, ErrorMessage = "El nombre de usuario no puede tener más de 50 caracteres")]
        [Required(ErrorMessage = "El nombre de usuario es obligatorio")]
        public string NombreUsuario { get; set; }

        [Required(ErrorMessage = "La clave es obligatoria")]
        public string Clave { get; set; }

        [StringLength(50, ErrorMessage = "El rol no puede tener más de 50 caracteres")]
        [Required(ErrorMessage = "El rol es obligatorio")]
        public string Rol { get; set; }


        [StringLength(50, ErrorMessage = "El estado no puede tener más de 50 caracteres")]
        [Required(ErrorMessage = "El estado es obligatorio")]
        public string Estado { get; set; } = "ACTIVO";

        [Required(ErrorMessage = "La fecha de creación es obligatoria")]
        public DateTime FechaCreacion { get; set; }

        public List<string> Roles { get; set; } = new List<string> { "Administrador", "Cajero" };





    }
}
