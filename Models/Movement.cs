using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace apptrade.Models
{
    [Table("movements")]
    public class Movement
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
        [Column("movement_type")]
        public string MovementType { get; set; } = string.Empty; // 'buy', 'sell'
        
        [Required]
        public int Quantity { get; set; }
        
        [Column("movement_date")]
        public DateTime MovementDate { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        public Portfolio Portfolio { get; set; } = null!;
        public Asset Asset { get; set; } = null!;
    }
}
