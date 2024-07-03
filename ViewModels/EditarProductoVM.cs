using AppLogins.Models;
using System.ComponentModel.DataAnnotations;

namespace AppLogins.ViewModels
{
    public class EditarProductoVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El tipo de producto es obligatorio")]
        public int IdTipo { get; set; }

        [Required(ErrorMessage = "El IVA es obligatorio")]
        public string Iva { get; set; }

        [Required(ErrorMessage = "El código de barras es obligatorio")]
        public string CodigoBarras { get; set; }

        [Required(ErrorMessage = "El estado es obligatorio")]
        public string Estado { get; set; }
    }
}
