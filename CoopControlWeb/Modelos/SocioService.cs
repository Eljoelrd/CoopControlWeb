using CoopControlWeb.Modelos;

namespace CoopControlWeb.Modelos
{
    public class SocioService
    {
        private readonly AppDBContext context;

        public SocioService(AppDBContext context)
        {
            this.context = context;
        }

        public EstadoCuentaViewModel? GetEstadoCuenta(int socioId)
        {
            var socio = context.Socios.Find(socioId);
            if (socio == null) return null;

            var aportes = context.Aportes
                .Where(a => a.SocioId == socioId)
                .ToList();

            var prestamos = context.Prestamos
                .Where(p => p.SocioId == socioId)
                .ToList();

            return new EstadoCuentaViewModel
            {
                NombreCompleto = $"{socio.Nombre} {socio.Apellido}",
                Cedula = socio.Cedula,
                TotalAportes = aportes.Sum(a => a.Monto),
                TotalPrestamos = prestamos.Sum(p => p.Monto),
                Aportes = aportes,
                Prestamos = prestamos
            };
        }
    }
}
