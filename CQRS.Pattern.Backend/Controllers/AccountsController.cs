using CQRS.Pattern.Applications.AccountApplication.Commands;
using CQRS.Pattern.Applications.AccountApplication.Queries;
using CQRS.Pattern.Common.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CQRS.Pattern.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AccountController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("balance/customer/{email}")]
        public async Task<IActionResult> AccountsBalance(string email)
        {
            var accountsWithBalances = await _mediator.Send(new AccountBalanceQuery
            {
                Email = email
            });
            return Ok(accountsWithBalances);
        }

        [HttpGet("/transactions/{account}")]
        public async Task<IActionResult> AccountTransactions(long account)
        {
            var accountWithTransactions = await _mediator.Send(new AccountWithTransactionsQuery
            {
                AccountNo = account
            });
            return Ok(accountWithTransactions);
        }

        [HttpGet("/set-default-account/{email}/{account}")]
        public async Task<IActionResult> SetDefaultAccount(string email, long account)
        {
            var accountsWithBalances = await _mediator.Send(new DefaultAccountCommand
            {
                Email = email,
                AccountNo = account
            });
            return Ok(accountsWithBalances);
        }

        [HttpPost("create-account")]
        public async Task<AccountDto> AddAccountAsync([FromBody] AccountDto accountDto)
        {
            var account = await _mediator.Send(new CreateAccountCommand
            {
                accountDto = accountDto
            });
            return account;
        }
    }
}