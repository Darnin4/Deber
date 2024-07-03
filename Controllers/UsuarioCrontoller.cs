using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AppLogins.Data;
using AppLogins.Models;
using System.Threading.Tasks;
using AppLogins.ViewModels;

namespace AppLogins.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly AppDBContext _context;

        public UsuarioController(AppDBContext context)
        {
            _context = context;
        }



        // Método para obtener los usuarios usando el procedimiento almacenado
        

        public async Task<IActionResult> MostrarUsuarios()
        {
            // Obtiene la lista de usuarios directamente desde la base de datos
            var usuarios = await _context.Usuarios.ToListAsync();

            // Crea el ViewModel con la lista de usuarios obtenida y el título
            var viewModel = new MostrarUsuarioVM
            {
                Usuarios = usuarios,
                Titulo = "Listado de Usuarios"
            };

            // Retorna la vista con el ViewModel
            return View("~/Views/Acceso/MostrarUsuarios.cshtml", viewModel);
        }







        // Método para eliminar un usuario
        [HttpPost]
        public async Task<IActionResult> EliminarUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(MostrarUsuarios));
        }

        ///EDITAR 

        [HttpGet] // Añade este atributo para indicar que es una solicitud GET
        public async Task<IActionResult> EditarUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }

            var viewModel = new EditarUsuarioVM
            {
                IdUsuario = usuario.IdUsuario,
                NombreCompleto = usuario.NombreCompleto,
                Correo = usuario.Correo,
                // Asegúrate de agregar los demás campos del ViewModel según sea necesario
            };

            // Devuelve la vista desde la ubicación correcta
            return View("~/Views/Acceso/Editar.cshtml", viewModel);
        }
        // Acción para procesar la actualización de usuario
        [HttpPost]
        public async Task<IActionResult> EditarUsuarioPost(EditarUsuarioVM viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("~/Views/Acceso/Editar.cshtml", viewModel); // Vuelve a mostrar el formulario con errores de validación
            }

            var usuario = await _context.Usuarios.FindAsync(viewModel.IdUsuario);

            if (usuario == null)
            {
                return NotFound();
            }

            // Actualiza el usuario con los datos del viewModel
            usuario.NombreCompleto = viewModel.NombreCompleto;
            usuario.Correo = viewModel.Correo;
            // Actualiza los demás campos según sea necesario

            try
            {
                _context.Update(usuario);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(usuario.IdUsuario))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction(nameof(MostrarUsuarios)); // Redirige al listado de usuarios después de la edición
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.IdUsuario == id);
        }
    }
}
