using CQRS.Pattern.Infastructure.Models;
using CQRS.Pattern.Infastructure.Repositories.Interfaces;
using MediatR;

namespace CQRS.Pattern.Applications.AccountApplication.Queries
{
    public class AccountQuery : IRequest<List<Account>>
    {
        public string Email { get; set; }
    }

    public class AccountQueryHandler : IRequestHandler<AccountQuery, List<Account>>
    {
        private readonly IAccountRepository _accountRepository;

        public AccountQueryHandler(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<List<Account>> Handle(AccountQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return await _accountRepository.GetAccountsAsync(request.Email);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
