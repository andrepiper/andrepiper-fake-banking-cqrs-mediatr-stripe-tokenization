namespace CQRS.Pattern.Common.Dtos
{
    public class PaymentMethodDto
    {
        public string? Provider { get; set; }

        public string? TokenizedCard { get; set; }
    }
}
