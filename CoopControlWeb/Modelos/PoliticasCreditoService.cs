using Microsoft.EntityFrameworkCore;
using CoopControlWeb.Modelos;

namespace CoopControlWeb.Services
{
    public class PoliticasCreditoService
    {
        private readonly IDbContextFactory<AppDBContext> _dbFactory;

        public PoliticasCreditoService(IDbContextFactory<AppDBContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }

        /// <summary>
        /// Calcula el monto máximo que un socio puede pedir prestado basado en sus aportes.
        /// </summary>
        public async Task<decimal> CalcularCapacidadPrestamoAsync(int socioId)
        {
            using var context = _dbFactory.CreateDbContext();

            // 1. Obtener total de aportes
            var totalAportes = await context.Aportes
                .Where(a => a.SocioId == socioId)
                .SumAsync(a => a.Monto);

            // 2. Obtener multiplicador de la configuración (por defecto 3)
            decimal multiplicador = await ObtenerMultiplicadorAsync(context);

            // 3. Obtener deuda actual (capital pendiente de préstamos activos)
            var deudaActual = await context.Prestamos
                .Where(p => p.SocioId == socioId && p.Estado == "Activo")
                .SumAsync(p => p.Monto); // Aquí idealmente sería el saldo insoluto

            decimal capacidadTotal = totalAportes * multiplicador;
            decimal disponible = capacidadTotal - deudaActual;

            return disponible > 0 ? disponible : 0;
        }

        /// <summary>
        /// Verifica si un socio es elegible para préstamos antes de ingresar un monto.
        /// </summary>
        public async Task<(bool EsElegible, string Mensaje, decimal LimiteMaximo)> VerificarElegibilidadSocioAsync(int socioId)
        {
            using var context = _dbFactory.CreateDbContext();

            // 1. Verificar Mora
            var tieneMora = await context.Prestamos
                .AnyAsync(p => p.SocioId == socioId && p.Estado == "Mora");

            if (tieneMora)
            {
                return (false, "Socio NO ELEGIBLE: Posee préstamos en mora actualmente.", 0);
            }

            // 2. Calcular Límite
            var limite = await CalcularCapacidadPrestamoAsync(socioId);
            if (limite <= 0)
            {
                return (false, "Socio NO ELEGIBLE: No posee aportes suficientes para garantizar un préstamo.", 0);
            }

            return (true, $"Socio Elegible. Límite máximo disponible: RD$ {limite:N2}", limite);
        }

        /// <summary>
        /// Realiza una validación completa antes de aprobar un nuevo préstamo.
        /// </summary>
        public async Task<(bool EsValido, string Mensaje)> ValidarNuevaSolicitudPrestamoAsync(int socioId, decimal montoSolicitado)
        {
            using var context = _dbFactory.CreateDbContext();

            // 1. Validar Capacidad por Aportes (Multiplicador)
            var capacidadDisponible = await CalcularCapacidadPrestamoAsync(socioId);
            
            if (montoSolicitado > capacidadDisponible)
            {
                return (false, $"Monto excedido. Según sus aportes, su capacidad máxima disponible es RD$ {capacidadDisponible:N2}.");
            }

            // 2. Validar si el socio tiene préstamos en mora
            // (Asumiendo que el estado 'Mora' existe en tu flujo de negocio)
            var tieneMora = await context.Prestamos
                .AnyAsync(p => p.SocioId == socioId && p.Estado == "Mora");

            if (tieneMora)
            {
                return (false, "El socio posee préstamos en mora. Debe regularizar su situación antes de solicitar uno nuevo.");
            }

            return (true, "Solicitud cumple con las políticas de crédito.");
        }

        /// <summary>
        /// Valida si un socio puede retirar una cantidad de aportes.
        /// </summary>
        public async Task<(bool PuedeRetirar, string Mensaje)> ValidarRetiroAportesAsync(int socioId, decimal montoARetirar)
        {
            using var context = _dbFactory.CreateDbContext();

            var totalAportes = await context.Aportes
                .Where(a => a.SocioId == socioId)
                .SumAsync(a => a.Monto);

            var tienePrestamosActivos = await context.Prestamos
                .AnyAsync(p => p.SocioId == socioId && p.Estado == "Activo");

            if (tienePrestamosActivos)
            {
                var saldoDeuda = await context.Prestamos
                    .Where(p => p.SocioId == socioId && p.Estado == "Activo")
                    .SumAsync(p => p.Monto); 

                // Usamos el multiplicador configurado para saber cuánto capital debe quedar retenido
                decimal multiplicador = await ObtenerMultiplicadorAsync(context);

                // Si el socio debe 90k y el multiplicador es 3, debe tener al menos 30k en aportes.
                decimal garantiaRequerida = saldoDeuda / multiplicador;
                if ((totalAportes - montoARetirar) < garantiaRequerida)
                {
                    return (false, $"No puede retirar esa cantidad: Sus aportes garantizan un préstamo activo. Debe mantener al menos RD$ {garantiaRequerida:N2} en su cuenta.");
                }
            }

            if (montoARetirar > totalAportes)
            {
                return (false, "Saldo de aportes insuficiente.");
            }

            return (true, "Retiro autorizado.");
        }

        private async Task<decimal> ObtenerMultiplicadorAsync(AppDBContext context)
        {
            decimal multiplicador = 3.0m;
            try
            {
                var config = await context.Configuraciones.FirstOrDefaultAsync(c => c.Clave == "MultiplicadorPrestamo");
                if (config != null && decimal.TryParse(config.Valor, out var val))
                    multiplicador = val;
            }
            catch
            {
                // Si falla, retornamos el valor por defecto
            }
            return multiplicador;
        }
    }
}