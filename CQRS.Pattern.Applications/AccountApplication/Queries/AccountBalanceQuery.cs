using AutoMapper;
using CQRS.Pattern.Infastructure.Repositories.Interfaces;
using MediatR;

namespace CQRS.Pattern.Applications.AccountApplication.Queries
{
    public class AccountBalanceQuery : IRequest<object>
    {
        public string Email { get; set; }
    }

    public class AccountBalanceQueryHandler : IRequestHandler<AccountBalanceQuery, object>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMapper _mapper;

        public AccountBalanceQueryHandler(IAccountRepository accountRepository, ITransactionRepository transactionRepository, IMapper mapper)
        {
            _accountRepository = accountRepository;
            _transactionRepository = transactionRepository;
            _mapper = mapper;
        }

        public async Task<object> Handle(AccountBalanceQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var accounts = await _accountRepository.GetAccountsAsync(request.Email);
                if (accounts == null)
                {
                    throw new Exception("Error while fetching balance");
                }
                var response = new List<object>();
                foreach (var account in accounts)
                {
                    var bal = await _accountRepository.GetBalanceAsync(account.Id);
                    response.Add(new
                    {
                        AccountId = account.Id,
                        Balance = bal,
                        AccountType = account.AccountType,
                        Currency = account.Currency,
                        Dated = DateTime.Now,
                        IsDefault = account.IsDefault
                    });
                }
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
