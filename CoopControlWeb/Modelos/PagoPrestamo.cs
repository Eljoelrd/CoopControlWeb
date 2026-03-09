using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoopControlWeb.Modelos
{
    [Table("PagoPrestamo")]
    public class PagoPrestamo
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Debe especificar el préstamo.")]
        public int PrestamoId { get; set; }

        [ForeignKey("PrestamoId")]
        public Prestamo Prestamo { get; set; } = null!;

        [Required(ErrorMessage = "Debe indicar la fecha del pago.")]
        [DataType(DataType.Date)]
        public DateTime FechaPago { get; set; }

        [Required(ErrorMessage = "Debe indicar el monto del pago.")]
        [Range(1, double.MaxValue, ErrorMessage = "El monto debe ser mayor que cero.")]
        public decimal Monto { get; set; }

        [StringLength(50)]
        public string? Metodo { get; set; } // Efectivo, transferencia, etc.

        [StringLength(250)]
        public string? Observaciones { get; set; }

        public bool EsAbonoExtraordinario { get; set; } = false;

        [StringLength(100)]
        public string? UsuarioRegistro { get; set; }

        [StringLength(250)]
        public string? ComprobanteUrl { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        [DataType(DataType.DateTime)]
        public DateTime? FechaModificacion { get; set; }
    }
}
