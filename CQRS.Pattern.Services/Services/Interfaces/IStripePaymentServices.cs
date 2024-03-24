using CQRS.Pattern.Common.Dtos;

namespace CQRS.Pattern.Services.Interfaces
{
    public interface IStripePaymentServices
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="tokenizedCard"></param>
        /// <param name="description"></param>
        /// <param name="amount"></param>
        /// <param name="currency"></param>
        /// <returns></returns>
        public Task<PaymentDto> InvokePayment(string email, string tokenizedCard, string description, decimal amount, string currency);
    }
}
