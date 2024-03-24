using CQRS.Pattern.Common.Dtos;
using CQRS.Pattern.Infastructure.Data;
using CQRS.Pattern.Infastructure.Models;
using CQRS.Pattern.Infastructure.Repositories.Interfaces;
using MediatR;

namespace CQRS.Pattern.Applications.PaymentApplication.Commands
{
    public class SavePaymentCommand : IRequest<PaymentDto>
    {
        public Payment Payment { get; set; }
        public string TokenizedCard { get; set; }
        public Account PaymentAccount { get; set; }

    }

    public class SavePaymentCommandCommandHandler : IRequestHandler<SavePaymentCommand, PaymentDto>
    {

        private readonly IGenericRepository<Payment, CQRSDbContext> _paymentRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IAccountRepository _accountRepository;

        public SavePaymentCommandCommandHandler(IGenericRepository<Payment, CQRSDbContext> paymentRepository,
            ITransactionRepository transactionRepository, IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
            _transactionRepository = transactionRepository;
            _paymentRepository = paymentRepository;
        }

        public async Task<PaymentDto> Handle(SavePaymentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                //create payment obj from DTO
                var paymentDto = new PaymentDto
                {
                    Amount = request.Payment.Amount,
                    Email = request.Payment.Email,// local user in your system
                    Date = DateTime.UtcNow,
                    PaymentMethod = new PaymentMethodDto()
                    {
                        Provider = request.Payment.ProviderName,
                        TokenizedCard = request.TokenizedCard

                    },
                    Currency = request.Payment.Currency,
                    Description = request.Payment.Description
                };

                //persist payment model
                await _paymentRepository.AddAsync(request.Payment);
                await _paymentRepository.SaveChangesAsync();

                //persist transaction model to ledger
                await _transactionRepository.AddAsync(new Transaction()
                {
                    AccountId = request.PaymentAccount.Id,
                    Amount = request.Payment.Amount,
                    Date = DateTime.UtcNow,
                    IsDebit = false,
                    Currency = request.Payment.Currency,
                    Description = request.Payment.Description
                });

                //update Id of dto
                paymentDto.PaymentId = $"{request.Payment.Id}";
                paymentDto.ProviderTransactionId = $"{request.Payment.ProviderTransactionId}";
                //updated running balance
                await _accountRepository.LedgerBalance(request.PaymentAccount, request.Payment.Amount, false);

                return paymentDto;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
