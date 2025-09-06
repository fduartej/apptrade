using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace apptrade.Models
{
    [Table("watchlist")]
    public class Watchlist
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        
        [Required]
        [Column("user_id")]
        public long UserId { get; set; }
        
        [Required]
        [Column("asset_id")]
        public long AssetId { get; set; }
        
        [Column("added_at")]
        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation property
        public Asset Asset { get; set; } = null!;
    }
}
