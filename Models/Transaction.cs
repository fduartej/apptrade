using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace apptrade.Models
{
    [Table("transactions")]
    public class Transaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        
        [Required]
        [Column("user_id")]
        public long UserId { get; set; }
        
        [Required]
        [Column("transaction_type")]
        public string TransactionType { get; set; } = string.Empty; // 'credit', 'debit'
        
        [Required]
        [Column(TypeName = "decimal(12,2)")]
        public decimal Amount { get; set; }
        
        [Column("transaction_date")]
        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;
        
        public string? Description { get; set; }
    }
}
