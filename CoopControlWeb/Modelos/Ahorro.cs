using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoopControlWeb.Modelos
{
    public enum TipoAhorro
    {
        Normal,
        Especial,
        Navideno,
        Escolar,
        Infantil,
        ClubViajes,
        InversionCapital
    }

    [Table("Ahorros")]
    public class Ahorro
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int SocioId { get; set; }

        [ForeignKey("SocioId")]
        public Socios Socio { get; set; } = null!;

        [Required]
        public TipoAhorro Tipo { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Saldo { get; set; } = 0m;

        // Tasa anual expresada en porcentaje (ej. 6 => 6%)
        [Column(TypeName = "decimal(5,2)")]
        public decimal TasaInteresAnual { get; set; } = 0m;

        // Para plazos o vencimientos (si aplica)
        public DateTime? FechaCreacion { get; set; } = DateTime.Now;
        public DateTime? FechaVencimiento { get; set; }
        public string? Notas { get; set; }

        // Retiro o reglas especiales - no mapeadas
        [NotMapped]
        public decimal InteresEstimadoAnual => Math.Round(Saldo * (TasaInteresAnual / 100m), 2);

        [NotMapped]
        public decimal DisponibleConInteres => Saldo + InteresEstimadoAnual;

        // Helper para obtener la tasa por defecto según el tipo de ahorro
        public static decimal GetDefaultTasaInteres(TipoAhorro tipo)
        {
            return tipo switch
            {
                TipoAhorro.Normal => 0m,            // Ahorro capital (distribución según asamblea)
                TipoAhorro.Especial => 6m,         // 6% anual
                TipoAhorro.Navideno => 7m,         // 7% anual
                TipoAhorro.Escolar => 5m,          // 5% anual
                TipoAhorro.Infantil => 5m,         // 5% anual
                TipoAhorro.ClubViajes => 0m,       // Sin interés por defecto
                TipoAhorro.InversionCapital => 8m, // 8% anual por defecto para inversiones
                _ => 0m,
            };
        }
    }
}