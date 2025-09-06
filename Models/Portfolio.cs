using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace apptrade.Models
{
    [Table("portfolios")]
    public class Portfolio
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        
        [Required]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        public string Owner { get; set; } = string.Empty;
        
        [Required]
        [Column("user_id")]
        public long UserId { get; set; }
    }
}
