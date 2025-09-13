using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace apptrade.Models
{

    [Table("t_wallet")]
    public class Wallet
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [Required(ErrorMessage = "Usuario es obligatorios.")]
        public Customer? customer { get; set; }
        public decimal Balance { get; set; } = 0; // Saldo inicial por defecto

    }
}