using AppLogins.Data;
using AppLogins.Models;
using AppLogins.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AppLogins.Controllers
{
    public class FacturaController : Controller
    {
        private readonly AppDBContext _appDbContext;

        public FacturaController(AppDBContext appDBContext)
        {
            _appDbContext = appDBContext;
        }

        // Mostrar listado de facturas
        public async Task<IActionResult> Index()
        {
            var facturas = await _appDbContext.Factura.ToListAsync();

            return View(facturas);
        }

        // Detalles de una factura
        public async Task<IActionResult> DetallesFactura(int id)
        {
            var factura = await _appDbContext.Factura.FindAsync(id);

            if (factura == null)
            {
                return NotFound();
            }

            return View(factura);
        }
        [HttpGet]
        public async Task<IActionResult> CrearFactura()
        {
            var clientes = await _appDbContext.Cliente.ToListAsync();
            var productos = await _appDbContext.Producto.ToListAsync();

            var facturaVM = new FacturaVM
            {
                Clientes = clientes,
                Productos = productos,
                Fecha = DateTime.Now
            };

            return View(facturaVM);
        }


        [HttpPost]
public async Task<IActionResult> CrearFactura(FacturaVM facturaVM)
{
    if (ModelState.IsValid)
    {
        using (var transaction = _appDbContext.Database.BeginTransaction())
        {
            try
            {
                // Crear una instancia de Factura y asignarle los valores del ViewModel
                var factura = new Factura
                {
                    Fecha = facturaVM.Fecha,
                    IdCliente = facturaVM.IdCliente,
                    Total = facturaVM.TotalCalculado,
                    Iva = facturaVM.IvaCalculado,
                    SubTotal = facturaVM.SubTotalCalculado,
                    Estado = facturaVM.Estado,
                    FechaCreacion = DateTime.Now
                };

                // Agregar factura a la base de datos
                _appDbContext.Add(factura);
                await _appDbContext.SaveChangesAsync();

                // Guardar detalles de la factura (factura_detalle)
                foreach (var detalleVM in facturaVM.Detalles)
                {
                    var detalle = new FacturaDetalle
                    {
                        IdFactura = factura.Id, // Asignar el Id de la factura creada
                        IdProducto = detalleVM.IdProducto,
                        Cantidad = detalleVM.Cantidad,
                        Precio = detalleVM.Precio,
                        Descuento = detalleVM.Descuento,
                        Total = detalleVM.Cantidad * detalleVM.Precio - detalleVM.Descuento, // Calcular el total del detalle
                        Estado = detalleVM.Estado,
                        FechaCreacion = DateTime.Now
                    };

                    // Agregar detalle a la base de datos
                    _appDbContext.Add(detalle);
                }

                await _appDbContext.SaveChangesAsync();

                // Confirmar transacción
                transaction.Commit();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Manejar cualquier excepción que pueda ocurrir durante el guardado
                ModelState.AddModelError("", "Error al intentar guardar la factura. Por favor, intenta de nuevo más tarde.");
                transaction.Rollback();
            }
        }
    }

    // Si el ModelState no es válido, regresar a la vista con el mismo modelo para mostrar errores
    facturaVM.Clientes = await _appDbContext.Cliente.ToListAsync(); // Cargar clientes nuevamente
    facturaVM.Productos = await _appDbContext.Producto.ToListAsync(); // Cargar productos nuevamente

    return View(facturaVM);
}


        // Editar factura (GET)
        [HttpGet]
        public async Task<IActionResult> EditarFactura(int id)
        {
            var factura = await _appDbContext.Factura.FindAsync(id);

            if (factura == null)
            {
                return NotFound();
            }

            return View(factura);
        }

        // Editar factura (POST)
        [HttpPost]
        public async Task<IActionResult> EditarFacturaPost(Factura factura)
        {
            if (ModelState.IsValid)
            {
                _appDbContext.Entry(factura).State = EntityState.Modified;
                await _appDbContext.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(factura);
        }

        // Eliminar factura (POST)
        [HttpPost]
        public async Task<IActionResult> EliminarFactura(int id)
        {
            var factura = await _appDbContext.Factura.FindAsync(id);

            if (factura == null)
            {
                return NotFound();
            }

            _appDbContext.Factura.Remove(factura);
            await _appDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public IActionResult ObtenerDatosCliente(int clienteId)
        {
            var cliente = _appDbContext.Cliente.FirstOrDefault(c => c.Id == clienteId);

            if (cliente == null)
            {
                return NotFound();
            }

            var datosCliente = new ClienteVM
            {
                Id = cliente.Id,
                Nombres = cliente.Nombres,
                Apellidos = cliente.Apellidos,
                Cedula = cliente.Cedula,
                Direccion = cliente.Direccion,
                Telefono = cliente.Telefono,
                Estado = cliente.Estado,
                FechaCreacion = cliente.FechaCreacion
            };

            return Json(datosCliente);
        }

    }
}
