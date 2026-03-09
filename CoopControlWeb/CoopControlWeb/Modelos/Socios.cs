using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoopControlWeb.Modelos
{
    [Table("Socios")]
    public class Socios
    {
        [Key]
        public int Id { get; set; }

        [StringLength(11, ErrorMessage = "11 caracteres máximo.")]
        public string Cedula { get; set; }

        public DateTime FechaNacimiento { get; set; }
        public DateTime FechaIngreso { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "30 caracteres máximo.")]
        public string Nombre { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "50 caracteres máximo.")]
        public string Apellido { get; set; }

        [Required]
        [StringLength(10, ErrorMessage = "10 caracteres máximo.")]
        public string Sexo { get; set; }

        [Required]
        [StringLength(15, ErrorMessage = "15 caracteres máximo.")]
        public string EstadoCivil { get; set; }

        [StringLength(5, ErrorMessage = "5 caracteres máximo.")]
        public string TipoSangre { get; set; }

        [StringLength(50, ErrorMessage = "50 caracteres máximo.")]
        public string Nacionalidad { get; set; }

        [StringLength(20, ErrorMessage = "20 caracteres máximo.")]
        public string Telefono { get; set; }

        [StringLength(50, ErrorMessage = "50 caracteres máximo.")]
        public string Email { get; set; }

        [StringLength(100, ErrorMessage = "100 caracteres máximo.")]
        public string Direccion { get; set; }

    }
}