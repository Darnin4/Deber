using AppLogins.Data;
using AppLogins.Models;
using AppLogins.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppLogins.Controllers
{
    public class UsersController : Controller
    {

        private readonly AppDBContext _appDbContext;
        public UsersController(AppDBContext appDBContext)
        {
            _appDbContext = appDBContext;
        }


        //REGISTRO


        [HttpGet]
        public IActionResult RegistroUsers()
        {
            var model = new UsersVM
            {
                Roles = new List<string> { "Administrador", "Cajero" }
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> RegistroUsers(UsersVM modelo)
        {
            if (string.IsNullOrEmpty(modelo.Clave))
            {
                ViewData["Mensaje"] = "La contraseña no puede estar vacía :(";
                modelo.Roles = new List<string> { "Administrador", "Cajero" }; // Recarga la lista de roles
                return View(modelo);
            }

            Users usuario = new Users()
            {
                NombreUsuario = modelo.NombreUsuario,
                Clave = modelo.Clave,
                Rol = modelo.Rol,
                Estado = "activo",
                FechaCreacion = DateTime.Now
            };

            await _appDbContext.Users.AddAsync(usuario);
            await _appDbContext.SaveChangesAsync();

            if (usuario.Id != 0)
            {
                return RedirectToAction("Login", "Acceso");
            }

            ViewData["Mensaje"] = "No se pudo crear el usuario :(";
            modelo.Roles = new List<string> { "Administrador", "Cajero" }; // Recarga la lista de roles
            return View(modelo);
        }




        // Método para obtener los usuarios usando el procedimiento almacenado
        public async Task<IActionResult> MostrarUsers()
        {
            // Obtiene la lista de usuarios directamente desde la base de datos
            var usuarios = await _appDbContext.Users.ToListAsync();

            // Crea el ViewModel con la lista de usuarios obtenida y el título
            var viewModel = new MostrarUsersVM
            {
                Users = usuarios,
                Titulo = "Listado de Usuarios"
            };

            // Retorna la vista con el ViewModel
            return View("~/Views/Users/MostrarUsers.cshtml", viewModel);
        }




        [HttpPost]
        public async Task<IActionResult> EliminarUsers(int id)
        {
            var usuario = await _appDbContext.Users.FindAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }

            _appDbContext.Users.Remove(usuario);
            await _appDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(MostrarUsers));
        }

        [HttpGet]
        public async Task<IActionResult> EditarUsers(int id)
        {
            var usuario = await _appDbContext.Users.FindAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }

            var viewModel = new EditarUsersVM
            {
                Id = usuario.Id,
                NombreUsuario = usuario.NombreUsuario,
                Clave = usuario.Clave,
                Rol = usuario.Rol,
                Estado = usuario.Estado,
            };

            return View("~/Views/Users/EditarUsers.cshtml", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditarUsersPost(EditarUsersVM viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("~/Views/Users/EditarUsers.cshtml", viewModel);
            }

            var usuario = await _appDbContext.Users.FindAsync(viewModel.Id);

            if (usuario == null)
            {
                return NotFound();
            }

            usuario.NombreUsuario = viewModel.NombreUsuario;
            usuario.Clave = viewModel.Clave;
            usuario.Rol = viewModel.Rol;
            usuario.Estado = viewModel.Estado;

            try
            {
                _appDbContext.Update(usuario);
                await _appDbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(usuario.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction(nameof(MostrarUsers));
        }



        private bool UsuarioExists(int id)
        {
            return _appDbContext.Users.Any(e => e.Id == id);
        }


    }
}
