using BankingManager.Server.Extensions;
using BankingManager.Services;
using BankingManager.Services.Model;
using BankingManager.Services.Model.Actions;
using Microsoft.AspNetCore.Mvc;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Attributes;

namespace BankingManager.Server.Controllers
{
    [ApiController]
    [Route("bank-account")]
    [AutoValidation]
    public class BankAccountController : Controller
    {
        private readonly IBankAccountService bankAccountService;
        private readonly IUtilityService utilityService;
        private readonly ILogger<BankAccountController> logger;

        public BankAccountController(IBankAccountService bankAccountService, IUtilityService utilityService, ILogger<BankAccountController> logger)
        {
            this.bankAccountService = bankAccountService;
            this.utilityService = utilityService;
            this.logger = logger;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] BankAccountDTO bankAccount)
        {
            var result = await bankAccountService.AddBankAccountAsync(bankAccount);

            return this.ActionResultByApiResult(result, logger);
        }
        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromBody] int id)
        {
            var result = await bankAccountService.DeleteBankAccountAsync(id);

            return this.ActionResultByApiResult(result, logger);
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] BankAccountDTO bankAccount)
        {
            var result = await bankAccountService.UpdateBankAccountAsync(bankAccount);

            return this.ActionResultByApiResult(result, logger);
        }

        [HttpGet("list")]
        public async Task<IActionResult> Get()
        {
            var result = await bankAccountService.GetBankAccountsAsync();

            return this.ActionResultByApiResult(result, logger);
        }

        [HttpGet("account-details")]
        public async Task<IActionResult> GetAccountDetails([FromQuery] string accountNumber)
        {
            var result = await bankAccountService.GetBankAccountByNumberAsync(accountNumber);

            return this.ActionResultByApiResult(result, logger);
        }

        [HttpPost("deposit")]
        public async Task<IActionResult> Deposit([FromBody] BankAccountAction accountAction)
        {
            var result = await bankAccountService.DepositAsync(accountAction);

            return this.ActionResultByApiResult(result, logger);
        }

        [HttpPost("withdrow")]
        public async Task<IActionResult> Withdrow([FromBody] BankAccountAction accountAction)
        {
            var result = await bankAccountService.WithdrowAsync(accountAction);

            return this.ActionResultByApiResult(result, logger);
        }

        [HttpPost("transfer")]
        public async Task<IActionResult> Transfer([FromBody] TransferAction transferAction)
        {
            var result = await bankAccountService.TransferAsync(transferAction);

            return this.ActionResultByApiResult(result, logger);
        }

        [HttpGet("get-currencies")]
        public async Task<IActionResult> GetCurrencies()
        {
            var currencies = await utilityService.GetCurrenciesAsync();

            return Ok(currencies);
        }
    }
}
