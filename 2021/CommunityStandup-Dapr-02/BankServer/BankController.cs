using System;
using System.Threading.Tasks;
using Dapr.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BankServer
{
    [ApiController]
    public class BankController : ControllerBase
    {
        const string StoreName = "statestore";
        const string PubSubName = "pubsub";

        private readonly ILogger logger;
        private readonly DaprClient daprClient;

        public BankController(ILogger<BankController> logger, DaprClient daprClient)
        {
            this.logger = logger;
            this.daprClient = daprClient;
        }

        [HttpGet("accounts/{username}")]
        public async Task<ActionResult<Account>> Get([FromRoute] string username)
        {
            var state = await this.daprClient.GetStateEntryAsync<Account>(StoreName, username);
            if (state.Value is null)
            {
                return new Account() { Username = username, };
            }

            return state.Value;
        }
        
        [HttpPost("accounts/deposit")]
        public async Task<ActionResult<Account>> Deposit(Transaction transaction)
        {
            var state = await this.daprClient.GetStateEntryAsync<Account>(StoreName, transaction.Username);
            state.Value ??= new Account() { Username = transaction.Username, };

            // We create a record of the transaction history for auditing.
            //
            // TODO: do something with this
            var history = new TransactionHistory()
            {
                Username = transaction.Username,
                StartingBalance = state.Value.Balance,
                Time = DateTime.UtcNow,
            };

            state.Value.Balance += transaction.Amount;
            history.EndingBalance = state.Value.Balance;
            await state.SaveAsync();

            await this.daprClient.PublishEventAsync(PubSubName, "history", history);

            return state.Value;
        }

        [HttpPost("accounts/withdraw")]
        public async Task<ActionResult<Account>> Withdraw(Transaction transaction)
        {
            var state = await this.daprClient.GetStateEntryAsync<Account>(StoreName, transaction.Username);
            if (state.Value == null)
            {
                return this.NotFound();
            }

            // We create a record of the transaction history for auditing.
            //
            // TODO: do something with this
            var history = new TransactionHistory()
            {
                Username = transaction.Username,
                StartingBalance = state.Value.Balance,
                Time = DateTime.UtcNow,
            };

            state.Value.Balance -= transaction.Amount;
            history.EndingBalance = state.Value.Balance;
            await state.SaveAsync();

            await this.daprClient.PublishEventAsync(PubSubName, "history", history);

            return state.Value;
        }
    }
}
