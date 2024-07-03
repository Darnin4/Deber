using System;
using System.Collections.Generic;
using AppLogins.Models;

namespace AppLogins.ViewModels
{
    public class MostrarClienteVM
    {
        public string Titulo { get; set; } // Título de la página o sección

        public List<Cliente> Clientes { get; set; } // Lista de clientes a mostrar

        // Constructor para inicialización
     
    }
}
