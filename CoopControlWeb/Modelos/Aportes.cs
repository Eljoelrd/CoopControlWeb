using Proyecto_Final.Modelos;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoopControlWeb.Modelos
{
    [Table("Aportes")]
    public class Aporte
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int SocioId { get; set; } // Relación con Socios

        [ForeignKey("SocioId")]
        public Socios Socio { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; }

        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "El monto debe ser mayor a cero.")]
        public decimal Monto { get; set; }

        [StringLength(100)]
        public string Concepto { get; set; }

        [StringLength(30)]
        public string Tipo { get; set; } // Ej: "Ordinario", "Extraordinario"


    }
}
