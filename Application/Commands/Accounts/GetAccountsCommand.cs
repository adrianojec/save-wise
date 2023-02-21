using Application.Commands.Accounts.Dtos;
using Application.Commands.Accounts.Interfaces;
using Application.Core;
using Application.UserRepository;

namespace Application.Commands.Accounts
{
    public class GetAccountsCommand : IGetAccountsCommand
    {
        private readonly IAccountRepository _accountRepository;
        public GetAccountsCommand(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }
        public async Task<Result<List<AccountDto>>> ExecuteCommand()
        {
            var accounts = await _accountRepository.GetAll();

            var data = accounts.Where(account => !account.isArchived).Select(account => new AccountDto(account)).ToList();

            return Result<List<AccountDto>>.Success(data);
        }
    }
}