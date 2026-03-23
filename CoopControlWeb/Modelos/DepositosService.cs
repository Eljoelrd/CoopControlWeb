using Microsoft.EntityFrameworkCore;
using CoopControlWeb.Modelos;

namespace CoopControlWeb.Services
{
    public class DepositoService
    {
        private readonly IDbContextFactory<AppDBContext> _dbFactory;

        public DepositoService(IDbContextFactory<AppDBContext> dbFactory)
        {
            _dbFactory = dbFactory ?? throw new ArgumentNullException(nameof(dbFactory));
        }

        // === CONSULTAS ===
        public async Task<List<Deposito>> GetAllAsync()
        {
            using var context = _dbFactory.CreateDbContext();
            return await context.Depositos
                .Include(d => d.Socio)
                .Include(d => d.Aporte)
                .Include(d => d.Prestamo)
                .OrderByDescending(d => d.FechaDeposito)
                .ToListAsync();
        }

        public async Task<Deposito?> GetByIdAsync(int id)
        {
            using var context = _dbFactory.CreateDbContext();
            return await context.Depositos
                .Include(d => d.Socio)
                .Include(d => d.Aporte)
                .Include(d => d.Prestamo)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<List<Deposito>> GetBySocioIdAsync(int socioId)
        {
            using var context = _dbFactory.CreateDbContext();
            return await context.Depositos
                .Include(d => d.Socio)
                .Include(d => d.Aporte)
                .Include(d => d.Prestamo)
                .Where(d => d.SocioId == socioId)
                .OrderByDescending(d => d.FechaDeposito)
                .ToListAsync();
        }

        public async Task<decimal> GetTotalBySocioAsync(int socioId)
        {
            using var context = _dbFactory.CreateDbContext();
            return await context.Depositos
                .Where(d => d.SocioId == socioId && d.EstaProcesado)
                .SumAsync(d => d.Monto);
        }

        // === CREACIÓN ===
        public async Task<bool> CreateAsync(Deposito deposito, string usuarioRegistro)
        {
            if (!deposito.IsValid()) return false;

            using var context = _dbFactory.CreateDbContext();

            using var transaction = await context.Database.BeginTransactionAsync();

            try
            {
                deposito.UsuarioRegistro = usuarioRegistro;
                deposito.FechaCreacion = DateTime.Now;
                deposito.EstaProcesado = true;

                // Si es Aporte: crear Aporte nuevo y enlazar
                if (deposito.TipoDeposito == "Aporte")
                {
                    var aporte = new Aporte
                    {
                        SocioId = deposito.SocioId,
                        Fecha = deposito.FechaDeposito,
                        Monto = deposito.Monto,
                        Tipo = "Ordinario",
                        Concepto = deposito.Observaciones
                    };
                    await context.Aportes.AddAsync(aporte);
                    deposito.Aporte = aporte;
                }

                await context.Depositos.AddAsync(deposito);

                // Si es para préstamo, crear PagoPrestamo automático
                if (deposito.EsParaPrestamo && deposito.PrestamoId.HasValue)
                {
                    var pago = new PagoPrestamo
                    {
                        PrestamoId = deposito.PrestamoId.Value,
                        FechaPago = deposito.FechaDeposito,
                        Monto = deposito.Monto,
                        Metodo = deposito.MetodoPago,
                        Observaciones = $"Pago desde depósito",
                        UsuarioRegistro = usuarioRegistro,
                        FechaCreacion = DateTime.Now
                    };
                    await context.PagoPrestamo.AddAsync(pago);
                }

                await context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        // === ACTUALIZACIÓN ===
        public async Task<bool> UpdateAsync(Deposito deposito, string usuarioModificacion)
        {
            if (!deposito.IsValid()) return false;

            using var context = _dbFactory.CreateDbContext();

            var existing = await context.Depositos
                .Include(d => d.Aporte)
                .FirstOrDefaultAsync(d => d.Id == deposito.Id);

            if (existing == null) return false;

            existing.FechaDeposito = deposito.FechaDeposito;
            existing.Monto = deposito.Monto;
            existing.TipoDeposito = deposito.TipoDeposito;
            existing.MetodoPago = deposito.MetodoPago;
            existing.ReferenciaExterna = deposito.ReferenciaExterna;
            existing.Observaciones = deposito.Observaciones;
            existing.PrestamoId = deposito.PrestamoId;
            existing.FechaModificacion = DateTime.Now;
            existing.UsuarioModificacion = usuarioModificacion;

            // Si quedó como Aporte -> crear o actualizar aporte asociado
            if (deposito.TipoDeposito == "Aporte")
            {
                if (existing.AporteId.HasValue)
                {
                    var aporte = await context.Aportes.FindAsync(existing.AporteId.Value);
                    if (aporte != null)
                    {
                        aporte.Monto = deposito.Monto;
                        aporte.Fecha = deposito.FechaDeposito;
                        aporte.Concepto = deposito.Observaciones;
                    }
                }
                else
                {
                    var nuevoAporte = new Aporte
                    {
                        SocioId = deposito.SocioId,
                        Fecha = deposito.FechaDeposito,
                        Monto = deposito.Monto,
                        Tipo = "Ordinario",
                        Concepto = deposito.Observaciones
                    };
                    await context.Aportes.AddAsync(nuevoAporte);
                    existing.Aporte = nuevoAporte;
                }
            }
            else
            {
                existing.AporteId = null;
            }

            await context.SaveChangesAsync();
            return true;
        }

        // === ELIMINACIÓN ===
        public async Task<bool> DeleteAsync(int id)
        {
            using var context = _dbFactory.CreateDbContext();

            var deposito = await context.Depositos.FindAsync(id);
            if (deposito == null) return false;

            context.Depositos.Remove(deposito);
            await context.SaveChangesAsync();
            return true;
        }

        // === ESTADÍSTICAS ===
        public async Task<Dictionary<string, decimal>> GetTotalsByTypeAsync()
        {
            using var context = _dbFactory.CreateDbContext();
            return await context.Depositos
                .Where(d => d.EstaProcesado)
                .GroupBy(d => d.TipoDeposito)
                .ToDictionaryAsync(
                    g => g.Key,
                    g => g.Sum(d => d.Monto)
                );
        }
    }
}