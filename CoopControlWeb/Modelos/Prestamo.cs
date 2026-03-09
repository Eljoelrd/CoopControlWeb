using CoopControlWeb.Modelos;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Prestamo
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Debe seleccionar un socio.")]
    public int SocioId { get; set; }

    public Socios Socio { get; set; } = null!;

    [Required(ErrorMessage = "El monto es obligatorio.")]
    [Range(1, double.MaxValue, ErrorMessage = "El monto debe ser mayor que cero.")]
    public decimal Monto { get; set; }

    [Range(0, 100, ErrorMessage = "La tasa de interés debe estar entre 0 y 100%.")]
    public decimal? TasaInteres { get; set; }

    [Range(1, 360, ErrorMessage = "El plazo debe estar entre 1 y 360 meses.")]
    public int? PlazoMeses { get; set; }

    [Required(ErrorMessage = "Debe indicar la fecha del préstamo.")]
    [DataType(DataType.Date)]
    public DateTime FechaPrestamo { get; set; }

    [DataType(DataType.Date)]
    public DateTime? FechaDevolucion { get; set; }

    [NotMapped]
    public DateTime? FechaVencimiento => PlazoMeses.HasValue ? FechaPrestamo.AddMonths(PlazoMeses.Value) : null;

    [Required(ErrorMessage = "Debe indicar el estado del préstamo.")]
    public string Estado { get; set; } = "Activo";

    [StringLength(50)]
    public string? TipoPrestamo { get; set; } // Personal, Comercial, Vivienda, etc.

    [StringLength(50)]
    public string? MetodoPagoPreferido { get; set; } // Efectivo, Transferencia, etc.

    public string? Observaciones { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime FechaCreacion { get; set; } = DateTime.Now;

    [DataType(DataType.DateTime)]
    public DateTime? FechaModificacion { get; set; }

    [NotMapped]
    public decimal? TotalConInteres => TasaInteres.HasValue
        ? Monto + (Monto * TasaInteres.Value / 100)
        : Monto;

    [NotMapped]
    public decimal? CuotaMensual => (TotalConInteres.HasValue && PlazoMeses.HasValue && PlazoMeses.Value > 0)
        ? Math.Round(TotalConInteres.Value / PlazoMeses.Value, 2)
        : null;

    public ICollection<PagoPrestamo>? PrestamoPagos { get; set; }

    [NotMapped]
    public decimal MontoPagado => PrestamoPagos?.Sum(p => p.Monto) ?? 0;

    [NotMapped]
    public decimal SaldoPendiente => TotalConInteres.HasValue ? TotalConInteres.Value - MontoPagado : 0;
}
