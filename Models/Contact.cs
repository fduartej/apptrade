using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace apptrade.Models
{
    [Table("t_contact")]
    public class Contact
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required(ErrorMessage = "Nombre es obligatorios.")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Email es obligatorios.")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Mensaje es obligatorios.")]
        public string? Message { get; set; }
        public bool IsPositive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}