using API.Controllers.Accounts.ViewModels;
using API.Controllers.InputModels;
using Application.Commands.Accounts.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Accounts
{
    [ApiController]
    [Route("api/[controller]")]

    public class AccountsController : ControllerBase
    {
        private readonly ICreateAccountCommand _createAccountCommand;
        private readonly IGetAccountsCommand _getAccountsCommand;
        private readonly IGetAccountCommand _getAccountCommand;
        public AccountsController
        (
            ICreateAccountCommand createAccountCommand,
            IGetAccountsCommand getAccountsCommand,
            IGetAccountCommand getAccountCommand
        )
        {
            _getAccountsCommand = getAccountsCommand;
            _createAccountCommand = createAccountCommand;
            _getAccountCommand = getAccountCommand;
        }

        [HttpPost]
        public async Task<ActionResult> CreateAccount([FromBody] CreateAccountInputModel input)
        {
            await _createAccountCommand.ExecuteCommand(input.ToDto());
            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<List<AccountViewModel>>> GetAll()
        {
            var accounts = await _getAccountsCommand.ExecuteCommand();
            return accounts.Select(account => new AccountViewModel(account)).ToList();
        }

        [HttpGet("{id}")]
        public async Task<AccountViewModel> GetAccount([FromRoute] Guid id)
        {
            var account = await _getAccountCommand.ExecuteCommand(id);
            return new AccountViewModel(account);
        }
    }
}