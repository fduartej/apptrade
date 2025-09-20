using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace apptrade.Models
{
    [Table("t_portfolio")]
    public class Portfolio
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public string Risk { get; set; } = string.Empty;

        public string Owner { get; set; } = string.Empty;

        public Customer? customer { get; set; }
    }
}