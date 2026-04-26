using Microsoft.EntityFrameworkCore;
using CoopControlWeb.Modelos;

namespace CoopControlWeb.Modelos
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options)
            : base(options)
        {
        }

        // === ENTIDADES EXISTENTES ===
        public DbSet<Prestamo> Prestamos { get; set; } = default!;
        public DbSet<Aporte> Aportes { get; set; } = default!;
        public DbSet<Socios> Socios { get; set; } = default!;
        public DbSet<User> Users { get; set; } = default!;
        public DbSet<PagoPrestamo> PagoPrestamo { get; set; } = default!;
        public DbSet<Certificado> Certificados { get; set; } = default!;


        // === NUEVA ENTIDAD: DEPOSITOS ===
        public DbSet<Deposito> Depositos { get; set; } = default!;

        // Nuevo DbSet para Ahorros
        public DbSet<Ahorro> Ahorros { get; set; } = default!;


        // Nuevo DbSet para AhorroMovimientos
        public DbSet<AhorroMovimiento> AhorroMovimientos { get; set; } = default!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // === CONFIGURACIÓN DE USER ===
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Email).HasMaxLength(100).IsRequired();
                entity.Property(e => e.PasswordHash).HasMaxLength(255).IsRequired();
                entity.Property(e => e.Role).HasMaxLength(50).IsRequired();
                entity.Property(e => e.Estado).IsRequired();
                entity.Property(e => e.FechaRegistro).HasColumnType("datetime").HasDefaultValueSql("GETDATE()");
            });

            // === CONFIGURACIÓN DE PRESTAMO ===
            modelBuilder.Entity<Prestamo>(entity =>
            {
                entity.ToTable("Prestamo");
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Monto).HasColumnType("decimal(18,2)").IsRequired();
                entity.Property(p => p.TasaInteres).HasColumnType("decimal(5,2)").IsRequired(false);
                entity.Property(p => p.Estado).HasMaxLength(20).IsRequired();
                entity.Property(p => p.Observaciones).HasMaxLength(255);
            });

            // === CONFIGURACIÓN DE APORTE ===
            modelBuilder.Entity<Aporte>(entity =>
            {
                entity.ToTable("Aportes");
                entity.Property(a => a.Monto).HasColumnType("decimal(18,2)").IsRequired();
            });

            // === CONFIGURACIÓN DE PAGO PRESTAMO ===
            modelBuilder.Entity<PagoPrestamo>(entity =>
            {
                entity.ToTable("PagoPrestamo");
                entity.Property(p => p.Monto).HasColumnType("decimal(18,2)").IsRequired();
            });

            // === CONFIGURACIÓN DE DEPOSITO (NUEVO) ===
            modelBuilder.Entity<Deposito>(entity =>
            {
                entity.ToTable("Depositos");
                entity.HasKey(d => d.Id);

                // Relaciones
                entity.HasOne(d => d.Socio)
                      .WithMany()
                      .HasForeignKey(d => d.SocioId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.Aporte)
                      .WithMany()
                      .HasForeignKey(d => d.AporteId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.Prestamo)
                      .WithMany()
                      .HasForeignKey(d => d.PrestamoId)
                      .OnDelete(DeleteBehavior.Restrict);

                // Propiedades
                entity.Property(d => d.Monto).HasColumnType("decimal(18,2)").IsRequired();
                entity.Property(d => d.TipoDeposito).HasMaxLength(50).IsRequired();
                entity.Property(d => d.MetodoPago).HasMaxLength(50);
                entity.Property(d => d.ReferenciaExterna).HasMaxLength(100);
                entity.Property(d => d.Observaciones).HasMaxLength(500);
                entity.Property(d => d.ComprobanteUrl).HasMaxLength(250);
                entity.Property(d => d.UsuarioRegistro).HasMaxLength(100);
                entity.Property(d => d.UsuarioModificacion).HasMaxLength(100);
                entity.Property(d => d.FechaCreacion).HasColumnType("datetime").HasDefaultValueSql("GETDATE()");
                entity.Property(d => d.FechaModificacion).HasColumnType("datetime");
            });

            // Configuración básica para Ahorro
            modelBuilder.Entity<Ahorro>(entity =>
            {
                entity.ToTable("Ahorros");
                entity.HasKey(a => a.Id);
                entity.Property(a => a.Saldo).HasColumnType("decimal(18,2)").IsRequired();
                entity.Property(a => a.TasaInteresAnual).HasColumnType("decimal(5,2)");
            });

            // Si es necesario, añade configuración para AhorroMovimiento aquí
        }
    }
}