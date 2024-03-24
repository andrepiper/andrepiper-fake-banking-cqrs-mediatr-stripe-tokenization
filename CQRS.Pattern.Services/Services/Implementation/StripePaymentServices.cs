using CQRS.Pattern.Applications.AccountApplication.Queries;
using CQRS.Pattern.Applications.PaymentApplication.Commands;
using CQRS.Pattern.Common.Dtos;
using CQRS.Pattern.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Configuration;
using Stripe;

namespace CQRS.Pattern.Services.Implementation
{
    public class StripePaymentServices : IStripePaymentServices
    {
        protected readonly IMediator _mediator;
        protected readonly IConfiguration _configuration;
        public StripePaymentServices(IMediator mediator, IConfiguration configuration)
        {
            _mediator = mediator;
            StripeConfiguration.ApiKey = configuration["Stripe.Dev.SecretKey"];
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="tokenizedCard"></param>
        /// <param name="description"></param>
        /// <param name="amount"></param>
        /// <param name="currency"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<PaymentDto> InvokePayment(string email, string tokenizedCard, string description, decimal amount, string currency = "USD")
        {
            //check if customer Exist
            var customerWithAcc = await _mediator.Send(new AccountQuery()
            {
                Email = email.ToLower(),
            });
            //throw error and stop transaction if user doesnt exist
            if (customerWithAcc == null)
            {
                throw new Exception("Payment wasnt successful, invalid user.");
            }
            //select payment default account
            var paymentAcc = customerWithAcc.Where(p => p.IsDefault).FirstOrDefault();
            //continue with payment
            var options = new ChargeCreateOptions
            {
                Amount = (long)amount * 100, // Convert to cents as required by Stripe
                Currency = currency,
                Source = tokenizedCard, // Use customer ID or Tokenized Card instead of card details for better security
                Description = description
            };
            //init stripe charge service
            var service = new ChargeService();
            //charge customer
            var charge = await service.CreateAsync(options);
            if (charge == null)
            {
                throw new Exception("Stripe payment failed");
            }
            //invoke command handle for saving payment object
            var commandResponse = await _mediator.Send(new SavePaymentCommand
            {
                Payment = new Infastructure.Models.Payment()
                {
                    Amount = amount,
                    Currency = currency,
                    Date = DateTime.UtcNow,
                    Description = description,
                    ProviderTransactionId = charge.Id,
                    ProviderName = "STRIPE",
                    Email = email
                },
                TokenizedCard = tokenizedCard,
                PaymentAccount = paymentAcc
            });
            //conduct transaction

            return commandResponse;
        }
    }
}
