using System.ComponentModel.DataAnnotations;

namespace CQRS.Pattern.Infastructure.Models
{
    public class Account
    {
        [Key]
        public int Id { get; set; }
        public string Email { get; set; }
        public string AccountType { get; set; }
        public string Currency { get; set; }
        public decimal Balance { get; set; }
        public bool IsDefault { get; set; } // Default account to handle payments
        public Account()
        {
        }
    }
}
