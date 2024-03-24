namespace CQRS.Pattern.Common.Dtos
{
    public class TransactionDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }

        public int AccountId { get; set; } // Foreign key to linked Account
        public bool Debit { get; set; } // True for debits, false for credits
    }
}
