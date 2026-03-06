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

        // Entidades de la cooperativa
        public DbSet<Prestamo> Prestamos { get; set; } = default!;
        public DbSet<Aporte> Aportes { get; set; } = default!;
        public DbSet<Socios> Socios { get; set; } = default!;
        public DbSet<User> Users { get; set; } = default!;
        public DbSet<PagoPrestamo> PagoPrestamo { get; set; } = default!;
        


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuración de User
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(e => e.PasswordHash)
                    .HasMaxLength(255)
                    .IsRequired();

                entity.Property(e => e.Role)
                    .HasMaxLength(50)
                    .IsRequired();

                entity.Property(e => e.Estado)
                    .IsRequired();

                entity.Property(e => e.FechaRegistro)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("GETDATE()");
            });

            // Puedes agregar aquí configuraciones adicionales para otras entidades si lo necesitas

            modelBuilder.Entity<Prestamo>(entity =>
            {
                entity.ToTable("Prestamo");

                entity.HasKey(p => p.Id);

                entity.Property(p => p.Monto).HasColumnType("decimal(18,2)").IsRequired();
                entity.Property(p => p.Estado).HasMaxLength(20).IsRequired();
                entity.Property(p => p.Observaciones).HasMaxLength(255);
            });


        }
    }
}
