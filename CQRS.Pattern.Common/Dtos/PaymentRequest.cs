namespace CQRS.Pattern.Common.Dtos
{
    public class PaymentRequest
    {
        public string Email { get; set; }
        public string Provider { get; set; }
        public string TokenizedCard { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public string Currency { get; set; }
    }
}
