using CQRS.Pattern.Common.Dtos;
using CQRS.Pattern.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CQRS.Pattern.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IStripePaymentServices _stripePaymentServices;
        public PaymentsController(IStripePaymentServices stripePaymentServices)
        {
            _stripePaymentServices = stripePaymentServices;
        }

        [HttpPost("payment")]
        public async Task<IActionResult> AddPayment([FromBody] PaymentRequest PaymentRequest)
        {
            var paymentServicesRequest = await _stripePaymentServices.InvokePayment(
                PaymentRequest.Email,
                PaymentRequest.TokenizedCard,
                PaymentRequest.Description,
                PaymentRequest.Amount,
                PaymentRequest.Currency);
            if (string.IsNullOrEmpty(paymentServicesRequest.ProviderTransactionId) &&
                string.IsNullOrEmpty(paymentServicesRequest.PaymentId))
            {
                //!payment completed on stripe rails and !persisted 
                throw new Exception($"Payment failed");
            }
            return Ok(new
            {
                Succes = paymentServicesRequest != null ? true : false,
                Message = paymentServicesRequest != null ? $"Transaction emal will be sent to Account : {PaymentRequest.Email}" : "Transaction failed!",
                PaymentProviderTransactionId = paymentServicesRequest.ProviderTransactionId,
                ServiceTransactionId = paymentServicesRequest.PaymentId
            });
        }
    }
}