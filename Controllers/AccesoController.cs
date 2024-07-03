using AppLogins.Data;
using AppLogins.Models;
using AppLogins.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;


namespace AppLogins.Controllers
{
    public class AccesoController : Controller
    {
        private readonly AppDBContext _appDbContext;
        public AccesoController(AppDBContext appDBContext)
        {
            _appDbContext = appDBContext;
        }

        [HttpGet]

        public IActionResult Registrarse()
        {
            return View();
        }
        [HttpPost]

        public async Task<IActionResult> Registrarse(UsuarioVMS modelo)
        {
            if (modelo.Clave != modelo.ConfirmarClave)
            {
                ViewData["Mensaje"] = "Las contraseñas no coinciden :(";
                return View();

            }

            Usuario usuario = new Usuario()
            {
                NombreCompleto = modelo.NombreCompleto,
                Correo = modelo.Correo,
                Clave = modelo.Clave,
            };

            await _appDbContext.Usuarios.AddAsync(usuario);
            await _appDbContext.SaveChangesAsync();

            if (usuario.IdUsuario != 0) return RedirectToAction("Login", "Acceso");

            ViewData["Mensaje"] = "No se pudo crear el usuario :(";

            return View();
        }


        [HttpGet]

        public IActionResult Login()
        {
            if (User.Identity!.IsAuthenticated) return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Login(LoginVM modelo)
        {
            Usuario? usuario_encontrado = await _appDbContext.Usuarios
                                        .Where(u =>
                                         u.Correo == modelo.Correo &&
                                         u.Clave == modelo.Clave
                                         ).FirstOrDefaultAsync();

            if (usuario_encontrado == null)
            {
                ViewData["Mensaje"] = "No se encontraron coincidencias   :(";

                return View();
            }


            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, usuario_encontrado.NombreCompleto)
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            AuthenticationProperties properties = new AuthenticationProperties()
            {
                AllowRefresh = true,
            };

            await HttpContext.SignInAsync(
           CookieAuthenticationDefaults.AuthenticationScheme,
           new ClaimsPrincipal(claimsIdentity),
           properties
           );
            return RedirectToAction("Index", "Home");

        }

        ///PROYECTO

        [HttpGet]
        public IActionResult RegistroUsers()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RegistroUsers(UsersVM modelo)
        {

            // Verifica que la contraseña no sea nula o vacía
            if (string.IsNullOrEmpty(modelo.Clave))
            {
                ViewData["Mensaje"] = "La contraseña no puede estar vacía :(";
                return View();
            }

            // Crea una nueva instancia de Users con los datos del modelo
            Users usuario = new Users()
            {
                NombreUsuario = modelo.NombreUsuario,
                Clave = modelo.Clave,
                Rol = modelo.Rol,
                Estado = "activo", // Valor predeterminado
                FechaCreacion = DateTime.Now // Valor predeterminado de la fecha actual
            };

            // Añade el nuevo usuario a la base de datos de forma asíncrona
            await _appDbContext.Users.AddAsync(usuario);
            await _appDbContext.SaveChangesAsync();

            // Verifica que el usuario se haya guardado correctamente
            if (usuario.Id != 0)
            {
                return RedirectToAction("Login", "Acceso");
            }

            // Si no se pudo crear el usuario, muestra un mensaje de error
            ViewData["Mensaje"] = "No se pudo crear el usuario :(";
            return View();
        }



        ////////// LOGIN USERS /////////

        [HttpGet]
        public IActionResult LoginUsers()
        {
            if (User.Identity!.IsAuthenticated) return RedirectToAction("Index", "Home");

            var model = new LoginUsersVM
            {
                Roles = new List<string> { "Cajero", "Administrador" }
            };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> LoginUsers(LoginUsersVM modelo)
        {
            Users? usuario_encontrado = await _appDbContext.Users
                                         .Where(u =>
                                            u.NombreUsuario == modelo.NombreUsuario &&
                                            u.Clave == modelo.Clave &&
                                            u.Rol == modelo.Rol
                                         ).FirstOrDefaultAsync();

            if (usuario_encontrado == null)
            {
                ViewData["Mensaje"] = "No se encontraron coincidencias :(";
                return View(modelo);
            }

            List<Claim> claims = new List<Claim>()
    {
        new Claim(ClaimTypes.Name, usuario_encontrado.NombreUsuario),
        new Claim(ClaimTypes.Role, usuario_encontrado.Rol)
    };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            AuthenticationProperties properties = new AuthenticationProperties()
            {
                AllowRefresh = true,
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                properties
            );

            if (usuario_encontrado.Rol == "Administrador")
            {
                return RedirectToAction("VistaAdministrador", "Home");
            }
            else if (usuario_encontrado.Rol == "Cajero")
            {
                return RedirectToAction("VistaCajero", "Home");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }



    }
}

