using AutoMapper;
using Azure;
using CQRS.Pattern.Infastructure.Models;
using CQRS.Pattern.Infastructure.Repositories.Interfaces;
using MediatR;

namespace CQRS.Pattern.Applications.AccountApplication.Queries
{
    public class AccountWithTransactionsQuery : IRequest<object>
    {
        public long AccountNo { get; set; }
    }

    public class AccountWithTransactionsQueryHandler : IRequestHandler<AccountWithTransactionsQuery, object>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMapper _mapper;

        public AccountWithTransactionsQueryHandler(IAccountRepository accountRepository, ITransactionRepository transactionRepository, IMapper mapper)
        {
            _accountRepository = accountRepository;
            _transactionRepository = transactionRepository;
            _mapper = mapper;
        }

        public async Task<object> Handle(AccountWithTransactionsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var account = await _accountRepository.GetByIdAsync(request.AccountNo);
                if (account == null)
                {
                    throw new Exception("Error while fetching transaction");
                }
                var balance = await _accountRepository.GetBalanceAsync(account.Id);
                var transactionsData = await _transactionRepository.TransactionsByAccountIdAsync(account.Id);
                if (transactionsData.Any())
                {
                    return new
                    {
                        Transactions = transactionsData,
                        Balance = balance,
                        Dated = DateTime.Now,
                    };
                }
                return new
                {
                    Transactions = new List<object>(),
                    Balance = balance,
                    Dated = DateTime.Now
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
