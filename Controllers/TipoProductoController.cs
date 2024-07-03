using AppLogins.Data;
using AppLogins.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using AppLogins.ViewModels;


namespace AppLogins.Controllers
{
    public class TipoProductoController : Controller
    {
        private readonly AppDBContext _appDbContext;

        public TipoProductoController(AppDBContext appDBContext)
        {
            _appDbContext = appDBContext;
        }

        // REGISTRO

        [HttpGet]
        public IActionResult RegistroTipoProducto()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegistroTipoProducto(TipoProductoVM modelo)
        {
            if (string.IsNullOrEmpty(modelo.Tipo))
            {
                ViewData["Mensaje"] = "El tipo de producto no puede estar vacío :(";
                return View();
            }

            TipoProducto tipoProducto = new TipoProducto()
            {
                Tipo = modelo.Tipo,
                Estado = "activo", // Valor predeterminado
                FechaCreacion = DateTime.Now // Valor predeterminado de la fecha actual
            };

            await _appDbContext.TipoProducto.AddAsync(tipoProducto);
            await _appDbContext.SaveChangesAsync();

            if (tipoProducto.Id != 0)
            {
                return RedirectToAction("Login", "Acceso");
            }

            ViewData["Mensaje"] = "No se pudo crear el tipo de producto :(";
            return View();
        }






        // Mostrar Tipos de Productos
        public async Task<IActionResult> MostrarTiposProductos()
        {
            var tiposProductos = await _appDbContext.TipoProducto.ToListAsync();

            var viewModel = new MostrarTipoProductoVM
            {
                TipoProducto = tiposProductos,
                Titulo = "Listado de Tipos de Productos"
            };

            return View("~/Views/TipoProducto/MostrarTiposProductos.cshtml", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EliminarTipoProducto(int id)
        {
            var tipoProducto = await _appDbContext.TipoProducto.FindAsync(id);

            if (tipoProducto == null)
            {
                return NotFound();
            }

            _appDbContext.TipoProducto.Remove(tipoProducto);
            await _appDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(MostrarTiposProductos));
        }

        [HttpGet]
        public async Task<IActionResult> EditarTipoProducto(int id)
        {
            var tipoProducto = await _appDbContext.TipoProducto.FindAsync(id);

            if (tipoProducto == null)
            {
                return NotFound();
            }

            var viewModel = new EditarTipoProductoVM 
            {
                Id = tipoProducto.Id,
                Tipo = tipoProducto.Tipo,
                Estado = tipoProducto.Estado,
            };

            return View("~/Views/TipoProducto/EditarTipoProducto.cshtml", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditarTipoProductoPost(EditarTipoProductoVM viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("~/Views/TipoProducto/EditarTipoProducto.cshtml", viewModel);
            }

            var tipoProducto = await _appDbContext.TipoProducto.FindAsync(viewModel.Id);

            if (tipoProducto == null)
            {
                return NotFound();
            }

            tipoProducto.Tipo = viewModel.Tipo;
            tipoProducto.Estado = viewModel.Estado;

            try
            {
                _appDbContext.Update(tipoProducto);
                await _appDbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TipoProductoExists(tipoProducto.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction(nameof(MostrarTiposProductos));
        }

        private bool TipoProductoExists(int id)
        {
            return _appDbContext.TipoProducto.Any(e => e.Id == id);
        }
    }
}
