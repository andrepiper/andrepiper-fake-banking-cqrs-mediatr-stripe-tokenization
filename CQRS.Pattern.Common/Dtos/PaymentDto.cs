namespace CQRS.Pattern.Common.Dtos
{
    public class PaymentDto
    {
        public string Email { get; set; }
        public PaymentMethodDto PaymentMethod { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string? PaymentId { get; set; }
        public string? Description { get; set; }
        public string? Currency { get; set; }
        public string? ProviderTransactionId { get; set; }
    }
}
