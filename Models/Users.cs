namespace AppLogins.Models
{
    public class Users
    {
        public int Id { get; set; }
        public string Clave { get; set; }
        public string NombreUsuario { get; set; }
        public string Rol { get; set; }
        public string Estado { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
