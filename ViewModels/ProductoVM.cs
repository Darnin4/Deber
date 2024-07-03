using AppLogins.Models;

namespace AppLogins.ViewModels
{
    public class ProductoVM
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int IdTipo { get; set; }
        public string Iva { get; set; }
        public string CodigoBarras { get; set; }
        public string Estado { get; set; }
        public DateTime FechaCreacion { get; set; }

        public TipoProducto TipoProducto { get; set; }
    }
}
