using CQRS.Pattern.Infastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace CQRS.Pattern.Infastructure.Data
{
    public class CQRSDbContext : DbContext
    {
        public CQRSDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions) { }

        protected CQRSDbContext() { }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Payment> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var customer = new Customer
            {
                Id = 1,
                Email = "fake.user@gmail.com",
                Firstname = "Fake",
                Lastname = "User",
                Password = BCrypt.Net.BCrypt.HashPassword("password")
            };
            //creating seed data for customer
            modelBuilder.Entity<Customer>().HasData(
                customer
            );
            //creating seed data for account
            var chqAcc = new Account
            {
                Id = 1,
                Email = customer.Email,
                Balance = 0.0m,
                AccountType = "CHQ",
                IsDefault = true,
                Currency = "USD"
            };
            var savAcc = new Account
            {

                Id = 2,
                Email = customer.Email,
                Balance = 0.0m,
                AccountType = "SAV",
                IsDefault = false,
                Currency = "CAD"
            };
            //each account needs an opening balance set to opening accs (500 X currency for SAV and 100 X currency for CHQ)
            var savAccTran = new Transaction()
            {
                Id = 1,
                AccountId = savAcc.Id,
                Amount = 500.00m,
                Currency = savAcc.Currency,
                Date = DateTime.Now,
                Description = "ACC Opening Tran.",
                IsDebit = false,
            };
            var chqAccTran = new Transaction()
            {
                Id = 2,
                AccountId = chqAcc.Id,
                Amount = 100.00m,
                Currency = chqAcc.Currency,
                Date = DateTime.Now,
                Description = "ACC Opening Tran.",
                IsDebit = false,
            };
            modelBuilder.Entity<Transaction>().HasData(
                savAccTran
            );
            modelBuilder.Entity<Transaction>().HasData(
                chqAccTran
            );
            //data for payment flow and tranaction to SAV acc: payment -> transact to acc -> generate invoice
            var paymentStripe = new Payment()
            {
                Id = 1,
                Email = customer.Email,
                Amount = 1200.00m,
                Currency = "USD",
                Date = DateTime.Now,
                Description = "SaaS Sub. 1yr",
                ProviderName = "Stripe",
                ProviderTransactionId = $"{Guid.NewGuid()}"
            };
            modelBuilder.Entity<Payment>().HasData(
                paymentStripe
            );
            //make a new transaction to destination acc SAV
            var paymentTransaction = new Transaction()
            {

                Id = 3,
                AccountId = savAcc.Id,
                Amount = paymentStripe.Amount,
                Currency = paymentStripe.Currency,
                Date = DateTime.Now,
                Description = paymentStripe.Description,
                IsDebit = false,
            };
            modelBuilder.Entity<Transaction>().HasData(
                paymentTransaction
            );
            //gen. invoice
            var stripeInvoice = new Invoice()
            {
                Id = 1,
                CustomerId = customer.Id,
                Amount = paymentStripe.Amount,
                Number = $"{Guid.NewGuid()}",
                Dated = DateTime.Now
            };
            modelBuilder.Entity<Invoice>().HasData(
                stripeInvoice
            );
            //update acc(s) with fake ledger operations
            var newBal = savAcc.Balance + savAccTran.Amount + paymentStripe.Amount;
            savAcc.Balance = newBal;

            newBal = chqAcc.Balance + chqAccTran.Amount;
            chqAcc.Balance = newBal;

            //finally save the account data with balances
            modelBuilder.Entity<Account>().HasData(
                chqAcc
            );
            modelBuilder.Entity<Account>().HasData(
                savAcc
            );
        }
    }
}
