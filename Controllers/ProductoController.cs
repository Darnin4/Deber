using AppLogins.Data;
using AppLogins.Models;
using AppLogins.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AppLogins.Controllers
{
    public class ProductoController : Controller
    {
        private readonly AppDBContext _appDbContext;
        public ProductoController(AppDBContext appDBContext)
        {
            _appDbContext = appDBContext;
        }

        // REGISTRO

        [HttpGet]
        public IActionResult RegistroProducto()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> RegistroProducto(ProductoVM modelo)
        {
            if (string.IsNullOrEmpty(modelo.Nombre))
            {
                ViewData["Mensaje"] = "El nombre del producto no puede estar vacío :(";
                return View();
            }

            Producto productos = new Producto()
            {
                Nombre = modelo.Nombre,
                IdTipo = modelo.IdTipo,
                Iva = modelo.Iva,
                CodigoBarras = modelo.CodigoBarras,
                Estado = "activo", // Valor predeterminado
                FechaCreacion = DateTime.Now // Valor predeterminado de la fecha actual
            };

            await _appDbContext.Producto.AddAsync(productos);
            await _appDbContext.SaveChangesAsync();

            if (productos.Id != 0)
            {

                return RedirectToAction("Login", "Acceso");
            }

            ViewData["Mensaje"] = "No se pudo crear el producto :(";
            return View();
        }


        // Mostrar Productos
        public async Task<IActionResult> MostrarProductos()
        {
            var productos = await _appDbContext.Producto.ToListAsync();

            var viewModel = new MostrarProductoVM
            {
                Producto = productos,
                Titulo = "Listado de Productos"
            };

            return View("~/Views/Producto/MostrarProductos.cshtml", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EliminarProducto(int id)
        {
            var productos = await _appDbContext.Producto.FindAsync(id);

            if (productos == null)
            {
                return NotFound();
            }

            _appDbContext.Producto.Remove(productos);
            await _appDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(MostrarProductos));
        }

        [HttpGet]
        public async Task<IActionResult> EditarProducto(int id)
        {
            var productos = await _appDbContext.Producto.FindAsync(id);

            if (productos == null)
            {
                return NotFound();
            }

            var viewModel = new EditarProductoVM
            {
                Id = productos.Id,
                Nombre = productos.Nombre,
                IdTipo = productos.IdTipo,
                Iva = productos.Iva,
                CodigoBarras = productos.CodigoBarras,
                Estado = productos.Estado,
            };

            return View("~/Views/Producto/EditarProducto.cshtml", viewModel);
        }


        [HttpPost]
        public async Task<IActionResult> EditarProductoPost(EditarProductoVM viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("~/Views/Producto/EditarProducto.cshtml", viewModel);
            }

            var productos = await _appDbContext.Producto.FindAsync(viewModel.Id);

            if (productos == null)
            {
                return NotFound();
            }

            productos.Nombre = viewModel.Nombre;
            productos.IdTipo = viewModel.IdTipo;
            productos.Iva = viewModel.Iva;
            productos.CodigoBarras = viewModel.CodigoBarras;
            productos.Estado = viewModel.Estado;

            try
            {
                _appDbContext.Entry(productos).State = EntityState.Modified;
                await _appDbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductoExists(productos.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction(nameof(MostrarProductos));
        }


        // Mostrar Productos
        public async Task<IActionResult> VerProductos()
        {
            var productos = await _appDbContext.Producto.ToListAsync();

            var viewModel = new MostrarProductoVM
            {
                Producto = productos,
                Titulo = "Listado de Productos"
            };

            return View("~/Views/Producto/VerProductos.cshtml", viewModel);
        }



        //TIPO DE PRODUCTO





        private bool ProductoExists(int id)
        {
            return _appDbContext.Producto.Any(e => e.Id == id);
        }
    }
}
