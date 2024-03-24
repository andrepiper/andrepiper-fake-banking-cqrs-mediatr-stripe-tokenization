namespace CQRS.Pattern.Infastructure.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public ICollection<Invoice> Invoices { get; set; } // Navigation property for related invoices
    }
}
