namespace CQRS.Pattern.Infastructure.Models
{
    public class Invoice
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public decimal Amount { get; set; }
        public DateTime Dated { get; set; }

        public int CustomerId { get; set; } // Foreign key to linked Customer
        public Customer Customer { get; set; } // Navigation property for linked Customer
    }
}
