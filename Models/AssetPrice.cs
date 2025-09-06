using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace apptrade.Models
{
    [Table("assest_prices")]
    public class AssetPrice
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        
        [Required]
        [Column("asset_id")]
        public long AssetId { get; set; }
        
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        
        [Column("recorded_at")]
        public DateTime RecordedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation property
        public Asset Asset { get; set; } = null!;
    }
}
