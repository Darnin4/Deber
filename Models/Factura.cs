using System.ComponentModel.DataAnnotations;

namespace AppLogins.Models
{
    public class Factura
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public int IdCliente { get; set; }
        public double Total { get; set; }           // Total ingresado por el usuario
        public double Iva { get; set; }             // IVA ingresado por el usuario
        public double SubTotal { get; set; }        // Subtotal ingresado por el usuario
        public string Estado { get; set; }
        public DateTime FechaCreacion { get; set; }

        // Propiedades calculadas
        public double SubTotalCalculado { get; set; }   // Subtotal calculado
        public double IvaCalculado { get; set; }        // IVA calculado
        public double TotalCalculado { get; set; }      // Total calculado

        // Método para calcular IVA y Total
        public void CalcularTotales()
        {
            // Calcular IVA (asumiendo un 15%)
            IvaCalculado = SubTotal * 0.15;

            // Calcular Total
            TotalCalculado = SubTotal + IvaCalculado;
        }

        public Cliente Cliente { get; set; }



        public class FacturaDetalle
        {
            public int Id { get; set; }

            [Display(Name = "Producto")]
            public int IdProducto { get; set; }

            public Producto Producto { get; set; }

            [Display(Name = "Cantidad")]
            public decimal Cantidad { get; set; }

            [Display(Name = "Precio")]
            public decimal Precio { get; set; }

            [Display(Name = "Descuento")]
            public decimal Descuento { get; set; }

            [Display(Name = "Total")]
            public decimal Total { get; set; }
        }
    }
}
