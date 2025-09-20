using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace apptrade.Models
{
    [Table("t_portfolio_movement")]
    public class PortfolioMovement
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        [Column("portfolio_id")]
        public long PortfolioId { get; set; }
        
        [Required]
        [Column("movement_type")]
        public string MovementType { get; set; } = string.Empty; // 'buy', 'sell'
        
        [Required]
        public int Quantity { get; set; }
        
        [Column("movement_date")]
        public DateTime MovementDate { get; set; } = DateTime.UtcNow;
        
        public Portfolio Portfolio { get; set; } = null!;
        public Assest Assest { get; set; } = null!;
    }
}