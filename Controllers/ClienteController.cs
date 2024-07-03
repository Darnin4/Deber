using AppLogins.Data;
using AppLogins.Models;
using AppLogins.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace AppLogins.Controllers
{
    public class ClienteController : Controller
    {
        private readonly AppDBContext _appDbContext;

        public ClienteController(AppDBContext appDBContext)
        {
            _appDbContext = appDBContext;
        }
        //CREAR CLIENTE



        [HttpGet]
        public IActionResult CrearCliente()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CrearCliente(ClienteVM clienteVM)
        {
            if (ModelState.IsValid)
            {
                // Mapear ClienteVM a la entidad Cliente
                var cliente = new Cliente
                {
                    Nombres = clienteVM.Nombres,
                    Apellidos = clienteVM.Apellidos,
                    Cedula = clienteVM.Cedula,
                    Direccion = clienteVM.Direccion,
                    Telefono = clienteVM.Telefono,
                    Estado = clienteVM.Estado,
                    FechaCreacion = clienteVM.FechaCreacion
                };

                // Guardar en la base de datos
                _appDbContext.Add(cliente);
                await _appDbContext.SaveChangesAsync();

                return RedirectToAction(nameof(MostrarClientes));
            }

            return View(clienteVM);
        }

        // Mostrar Clientes
        public async Task<IActionResult> MostrarClientes()
        {
            var clientes = await _appDbContext.Cliente.ToListAsync();

            var viewModel = new MostrarClienteVM
            {
                Clientes = clientes,
                Titulo = "Listado de Clientes"
            };

            return View("~/Views/Cliente/MostrarClientes.cshtml", viewModel);
        }








        [HttpGet]
        public async Task<IActionResult> GetCliente(string cedula)
        {
            var cliente = await _appDbContext.Cliente.FirstOrDefaultAsync(c => c.Cedula == cedula);
            if (cliente == null)
            {
                return NotFound();
            }

            return Json(new
            {
                Nombre = cliente.Nombres,
                Apellido = cliente.Apellidos,
                Direccion = cliente.Direccion,
                Telefono = cliente.Telefono
            });
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



        [HttpGet]
        public async Task<IActionResult> EditarCliente(int id)
        {
            var cliente = await _appDbContext.Cliente.FindAsync(id);

            if (cliente == null)
            {
                return NotFound();
            }

            var viewModel = new EditarClienteVM
            {
                Id = cliente.Id,
                Nombres = cliente.Nombres,
                Apellidos = cliente.Apellidos,
                Cedula = cliente.Cedula,
                Direccion = cliente.Direccion,
                Telefono = cliente.Telefono,
                Estado = cliente.Estado,
            };

            return View("~/Views/Cliente/EditarCliente.cshtml", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditarClientePost(EditarClienteVM viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("~/Views/Cliente/EditarCliente.cshtml", viewModel);
            }

            var cliente = await _appDbContext.Cliente.FindAsync(viewModel.Id);

            if (cliente == null)
            {
                return NotFound();
            }

            cliente.Nombres = viewModel.Nombres;
            cliente.Apellidos = viewModel.Apellidos;
            cliente.Cedula = viewModel.Cedula;
            cliente.Direccion = viewModel.Direccion;
            cliente.Telefono = viewModel.Telefono;
            cliente.Estado = viewModel.Estado;

            try
            {
                _appDbContext.Entry(cliente).State = EntityState.Modified;
                await _appDbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClienteExists(cliente.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction(nameof(MostrarClientes));
        }


        [HttpPost]
        public async Task<IActionResult> EliminarCliente(int id)
        {
            var cliente = await _appDbContext.Cliente.FindAsync(id);

            if (cliente == null)
            {
                return NotFound();
            }

            _appDbContext.Cliente.Remove(cliente);
            await _appDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(MostrarClientes));
        }

        private bool ClienteExists(int id)
        {
            return _appDbContext.Cliente.Any(e => e.Id == id);
        }
    }
}
