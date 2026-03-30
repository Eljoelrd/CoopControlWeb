using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoopControlWeb.Modelos
{
    [Table("Depositos")]
    public class Deposito
    {
        [Key]
        public int Id { get; set; }

        // === RELACIÓN CON SOCIO (OBLIGATORIA) ===
        [Required(ErrorMessage = "Debe seleccionar el socio.")]
        public int SocioId { get; set; }

        [ForeignKey("SocioId")]
        public Socios Socio { get; set; } = null!;

        // === RELACIONES OPCIONALES ===
        public int? AporteId { get; set; }

        [ForeignKey("AporteId")]
        public Aporte? Aporte { get; set; }

        public int? PrestamoId { get; set; }

        [ForeignKey("PrestamoId")]
        public Prestamo? Prestamo { get; set; }

        // Nuevo: tipo de ahorro asociado cuando TipoDeposito == "AhorroLibre"
        [NotMapped]
        [StringLength(50)]
        public string? AhorroTipo { get; set; }

        // === DATOS DEL DEPÓSITO ===
        [Required(ErrorMessage = "Debe indicar la fecha del depósito.")]
        [DataType(DataType.Date)]
        public DateTime FechaDeposito { get; set; }

        [Required(ErrorMessage = "Debe indicar el monto del depósito.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor que cero.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Monto { get; set; }

        [Required(ErrorMessage = "Debe seleccionar el tipo de depósito.")]
        [StringLength(50)]
        public string TipoDeposito { get; set; } = string.Empty;
        // Valores: "Aporte", "PagoPrestamo", "AhorroLibre", "Reembolso", etc.

        [StringLength(50)]
        public string? MetodoPago { get; set; }

        [StringLength(100)]
        public string? ReferenciaExterna { get; set; }

        [StringLength(500)]
        public string? Observaciones { get; set; }

        [StringLength(250)]
        public string? ComprobanteUrl { get; set; }

        // === ESTADO Y AUDITORÍA ===
        public bool EstaProcesado { get; set; } = true;

        [StringLength(100)]
        public string? UsuarioRegistro { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        [DataType(DataType.DateTime)]
        public DateTime? FechaModificacion { get; set; }

        [StringLength(100)]
        public string? UsuarioModificacion { get; set; }

        // === PROPIEDADES CALCULADAS ===
        [NotMapped]
        public bool EsParaAporte => TipoDeposito == "Aporte" && AporteId.HasValue;

        [NotMapped]
        public bool EsParaPrestamo => TipoDeposito == "PagoPrestamo" && PrestamoId.HasValue;

        [NotMapped]
        public bool EsAhorroLibre => TipoDeposito == "AhorroLibre";

        // === VALIDACIÓN DE REGLA DE NEGOCIO ===
        public bool IsValid()
        {
            // Para depósito tipo Aporte permitimos crear un aporte nuevo (no se exige AporteId).
            // Si es para préstamo, debe tener PrestamoId
            if (TipoDeposito == "PagoPrestamo" && !PrestamoId.HasValue) return false;
            // No puede tener ambos a la vez
            if (AporteId.HasValue && PrestamoId.HasValue) return false;
            return true;
        }
    }
}