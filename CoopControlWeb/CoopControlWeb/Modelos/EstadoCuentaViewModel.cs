using CoopControlWeb.Modelos;
using System.Collections.Generic;

namespace CoopControlWeb.Modelos
{
    public class EstadoCuentaViewModel
    {
        public string NombreCompleto { get; set; } = "";
        public string Cedula { get; set; } = "";
        public decimal TotalAportes { get; set; }
        public decimal TotalPrestamos { get; set; }
        public List<Aporte> Aportes { get; set; } = new();
        public List<Prestamo> Prestamos { get; set; } = new();
    }
}
