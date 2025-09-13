using System.ComponentModel.DataAnnotations;

namespace apptrade.Dto
{
    public class WalletTransactionViewModel
    {
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Monto debe ser positivo.")]
        public decimal Amount { get; set; }
        [Required]
        public string TransactionType { get; set; }

    }
}