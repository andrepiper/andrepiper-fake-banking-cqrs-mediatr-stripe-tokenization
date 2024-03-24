using System.ComponentModel.DataAnnotations;

namespace CQRS.Pattern.Infastructure.Models
{
    public class Payment
    {
        [Key]
        public int Id { get; set; }
        public string Email { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string? Currency { get; set; }
        public string? Description { get; set; }
        public string? ProviderName { get; set; }
        public string? ProviderTransactionId { get; set; }
    }
}
