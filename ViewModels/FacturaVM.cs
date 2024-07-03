using AppLogins.Models;
using System;
using System.Collections.Generic;

namespace AppLogins.ViewModels
{
    public class FacturaVM
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

        // Cliente seleccionado
        public Cliente Cliente { get; set; }

        // Listas para selección en la vista
        public List<Cliente> Clientes { get; set; }
        public List<Producto> Productos { get; set; }

        // Detalles de la factura
        public List<FacturaDetalleVM> Detalles { get; set; }

        // Método para calcular IVA y Total
        public void CalcularTotales()
        {
            // Calcular IVA (asumiendo un 15%)
            IvaCalculado = SubTotal * 0.15;

            // Calcular Total
            TotalCalculado = SubTotal + IvaCalculado;
        }
    }

    public class FacturaDetalleVM
    {
        public int Id { get; set; }
        public int IdProducto { get; set; }
        public int Cantidad { get; set; }
        public double Precio { get; set; }
        public double Descuento { get; set; }
        public double Total { get; set; }
        public string Estado { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
