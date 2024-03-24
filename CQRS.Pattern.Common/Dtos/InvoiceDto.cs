namespace CQRS.Pattern.Common.Dtos
{
    public class InvoiceDto
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public decimal Amount { get; set; }
        public DateTime DueDate { get; set; }

        public int CustomerId { get; set; } // Foreign key to linked Customer
        public CustomerDto Customer { get; set; } // Navigation property for linked Customer
    }
}
