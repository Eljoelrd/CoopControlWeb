using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto_Final.Modelos
{
    [Table("tablNacionalidad")]
    public class Nacionalidad
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Nombre No puede estar en blanco.")]
        [StringLength(30, ErrorMessage = "Max 50 caracteres.")]
        public string Nombre { get; set; }

    
    }
}