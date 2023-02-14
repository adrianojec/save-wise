using API.Controllers.Transactions.InputModels;
using API.Controllers.Transactions.ViewModels;
using Application.Commands.Transactions.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Transactions
{
    [ApiController]
    [Route("api/accounts/{accountId}/[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly ICreateTransactionCommand _createTransactionCommand;
        private readonly IGetTransactionsCommand _getTransactionsCommand;
        private readonly IGetTransactionCommand _getTransactionCommand;
        public TransactionsController(
            ICreateTransactionCommand createTransactionCommand,
            IGetTransactionsCommand getTransactionsCommand,
            IGetTransactionCommand getTransactionCommand
            )
        {
            _createTransactionCommand = createTransactionCommand;
            _getTransactionsCommand = getTransactionsCommand;
            _getTransactionCommand = getTransactionCommand;
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromRoute] Guid accountId, [FromBody] CreateTransactionInputModel input)
        {
            await _createTransactionCommand.ExecuteCommand(accountId, input.ToCreateTransactionDto());
            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<List<TransactionViewModel>>> GetAll()
        {
            var transactions = await _getTransactionsCommand.ExecuteCommand();
            return transactions.Select(transaction => new TransactionViewModel(transaction)).ToList();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TransactionViewModel>> Get([FromRoute] Guid accountId, [FromRoute] Guid id)
        {
            var transaction = await _getTransactionCommand.ExecuteCommand(accountId, id);
            return new TransactionViewModel(transaction);
        }
    }
}