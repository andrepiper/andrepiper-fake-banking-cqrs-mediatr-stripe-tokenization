using System.ComponentModel.DataAnnotations;

namespace CQRS.Pattern.Infastructure.Models
{
    public class Transaction
    {
        [Key]
        public int Id { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public DateTime Date { get; set; }
        public int AccountId { get; set; } // Foreign key to linked Account
        public bool IsDebit { get; set; } // True for debits, false for credits
    }
}
