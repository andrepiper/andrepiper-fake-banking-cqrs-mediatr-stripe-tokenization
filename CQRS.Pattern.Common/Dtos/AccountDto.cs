namespace CQRS.Pattern.Common.Dtos
{
    public class AccountDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string AccountType { get; set; }
        public string Currency { get; set; }
        public decimal Balance { get; set; }
        public IEnumerable<TransactionDto> Transactions { get; set; } // Navigation property for related transactions
        public AccountDto()
        {
            //default to USD
            Currency = "USD";
        }
    }
}
