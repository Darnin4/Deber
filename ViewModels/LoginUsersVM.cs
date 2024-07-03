using System.ComponentModel.DataAnnotations;

namespace AppLogins.ViewModels
{
    public class LoginUsersVM
    {

        public string NombreUsuario { get; set; }

        public string Clave { get; set; }

        public string Rol { get; set; }
        public List<string> Roles { get; set; } = new List<string> { "Administrador", "Cajero" };


    }
}
