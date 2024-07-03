using System;
using System.ComponentModel.DataAnnotations;

namespace AppLogins.ViewModels
{
    public class EditarClienteVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no puede tener más de 100 caracteres")]
        public string Nombres { get; set; }

        [Required(ErrorMessage = "Los apellidos son obligatorios")]
        [StringLength(100, ErrorMessage = "Los apellidos no pueden tener más de 100 caracteres")]
        public string Apellidos { get; set; }

        [Required(ErrorMessage = "La cédula es obligatoria")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "La cédula debe ser un número de 10 dígitos")]
        public string Cedula { get; set; }

        [Required(ErrorMessage = "La dirección es obligatoria")]
        [StringLength(200, ErrorMessage = "La dirección no puede tener más de 200 caracteres")]
        public string Direccion { get; set; }

        [Required(ErrorMessage = "El teléfono es obligatorio")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "El teléfono debe ser un número de 10 dígitos")]
        public string Telefono { get; set; }

        [StringLength(50, ErrorMessage = "El estado no puede tener más de 50 caracteres")]
        [Required(ErrorMessage = "El estado es obligatorio")]
        public string Estado { get; set; }

        [Required(ErrorMessage = "La fecha de creación es obligatoria")]
        public DateTime FechaCreacion { get; set; }
    }
}
