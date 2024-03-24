using AutoMapper;
using CQRS.Pattern.Infastructure.Repositories.Interfaces;
using MediatR;

namespace CQRS.Pattern.Applications.AccountApplication.Queries
{
    public class DefaultAccountCommand : IRequest<object>
    {
        public string Email { get; set; }
        public long AccountNo { get; set; }
    }

    public class DefaultAccountCommandHandler : IRequestHandler<DefaultAccountCommand, object>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IMapper _mapper;

        public DefaultAccountCommandHandler(IAccountRepository accountRepository, IMapper mapper)
        {
            _accountRepository = accountRepository;
            _mapper = mapper;
        }

        public async Task<object> Handle(DefaultAccountCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _accountRepository.EnforceDefaultAccount(request.Email, request.AccountNo);
                var account = await _accountRepository.GetByIdAsync(request.AccountNo);
                var bal = await _accountRepository.GetBalanceAsync(account.Id);
                return new
                {
                    AccountId = account.Id,
                    Balance = bal,
                    AccountType = account.AccountType,
                    Dated = DateTime.Now,
                    IsDefault = account.IsDefault
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
