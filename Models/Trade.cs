using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace apptrade.Models
{
    [Table("trades")]
    public class Trade
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        
        [Required]
        [Column("portfolio_id")]
        public long PortfolioId { get; set; }
        
        [Required]
        [Column("asset_id")]
        public long AssetId { get; set; }
        
        [Required]
        [Column("trade_type")]
        public string TradeType { get; set; } = string.Empty; // 'buy', 'sell'
        
        [Required]
        public int Quantity { get; set; }
        
        [Required]
        [Column("price_per_unit", TypeName = "decimal(18,2)")]
        public decimal PricePerUnit { get; set; }
        
        [Column("trade_date")]
        public DateTime TradeDate { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        public Portfolio Portfolio { get; set; } = null!;
        public Asset Asset { get; set; } = null!;
    }
}
