using AutoMapper;
using CQRS.Pattern.Common.Dtos;
using CQRS.Pattern.Infastructure.Models;
using CQRS.Pattern.Infastructure.Repositories.Interfaces;
using MediatR;

namespace CQRS.Pattern.Applications.AccountApplication.Commands
{
    public class CreateAccountCommand : IRequest<AccountDto>
    {
        public AccountDto accountDto { get; set; }
    }

    public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, AccountDto>
    {

        private readonly IAccountRepository _accountRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMapper _mapper;

        public CreateAccountCommandHandler(IAccountRepository accountRepository, ITransactionRepository transactionRepository, IMapper mapper)
        {
            _accountRepository = accountRepository;
            _transactionRepository = transactionRepository;
            _mapper = mapper;
        }

        public async Task<AccountDto> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var account = _mapper.Map<Account>(request.accountDto);
                await _accountRepository.AddAsync(account);
                var transactions = _mapper.Map<IEnumerable<Transaction>>(request.accountDto.Transactions);
                transactions.ToList().ForEach(t => t.AccountId = account.Id);
                await _transactionRepository.AddRangeAsync(transactions);
                // TODO
                /***
				 * 1. Need to include transactions
				 * 2. Update account model to include all the transactions associated
				 */
                var response = _mapper.Map<AccountDto>(account);
                response.Transactions = _mapper.Map<IEnumerable<TransactionDto>>(transactions);
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
