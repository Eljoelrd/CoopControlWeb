using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoopControlWeb.Modelos
{
    [Table("Certificados")]
    public class Certificado
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Debe seleccionar el socio.")]
        public int SocioId { get; set; }

        [ForeignKey("SocioId")]
        public Socios? Socio { get; set; }

        [Required(ErrorMessage = "Debe indicar el tipo de certificado.")]
        [StringLength(100)]
        public string Tipo { get; set; } = "Plazo Fijo";

        [Required(ErrorMessage = "El monto es obligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor a cero.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Monto { get; set; }

        [Required]
        public DateTime FechaEmision { get; set; } = DateTime.Now;

        [Required]
        public DateTime FechaVencimiento { get; set; }

        [Required]
        [StringLength(50)]
        public string Estado { get; set; } = "Activo"; // Activo, Vencido, Cancelado

        [StringLength(500)]
        public string? Observaciones { get; set; }
    }
}
