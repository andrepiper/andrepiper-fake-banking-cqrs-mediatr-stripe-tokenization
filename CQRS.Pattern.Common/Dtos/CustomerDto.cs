using CQRS.Pattern.Infastructure.Models;

namespace CQRS.Pattern.Common.Dtos
{
    public class CustomerDto
    {
        public int Id { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public ICollection<Invoice> Invoices { get; set; } // Navigation property for related invoices
    }
}
