using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apptrade.Models
{
    [Table("t_watchlist")]
    public class Watchlist
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [Required(ErrorMessage = "Usuario es obligatorios.")]
        public string? UserName { get; set; }
        public Assest? Assest { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}