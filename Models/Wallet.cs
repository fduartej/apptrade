using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace apptrade.Models
{
    [Table("wallets")]
    public class Wallet
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        
        [Required]
        [Column("user_id")]
        public long UserId { get; set; }
        
        [Column(TypeName = "decimal(12,2)")]
        public decimal Balance { get; set; } = 0.00m;
    }
}
