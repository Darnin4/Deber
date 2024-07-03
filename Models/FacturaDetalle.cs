namespace AppLogins.Models
{
    public class FacturaDetalle
    {

        public int Id { get; set; }
        public int IdFactura { get; set; }
        public int IdProducto { get; set; }
        public double Cantidad { get; set; }
        public double Precio { get; set; }
        public double Descuento { get; set; }
        public double Total { get; set; }
        public string Estado { get; set; }
        public DateTime FechaCreacion { get; set; }

        public Factura Factura { get; set; }
        public Producto Producto { get; set; }
    }
}
