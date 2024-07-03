namespace AppLogins.ViewModels
{
    public class EditarUsersVM
    {

        public int Id { get; set; }
        public string Clave { get; set; }
        public string NombreUsuario { get; set; }
        public string Rol { get; set; }
        public string Estado { get; set; }
        public List<string> Roles { get; set; } = new List<string> { "Administrador", "Cajero" };
        public List<string> Estados { get; set; } = new List<string> { "Activo", "Inactivo" };

    }
}
