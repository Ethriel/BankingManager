using BankingManager.Database.Model;
using BankingManager.Services.Model;
using BankingManager.Services.Model.Actions;
using BankingManager.Services.Utility.ApiResult;
using Microsoft.Extensions.Logging;

namespace BankingManager.Services
{
    public class BankingAccountService : IBankingAccountService
    {
        private readonly IEntityExtendedService<BankAccount> bankAccountService;
        private readonly IMapperService<BankAccount, BankAccountDTO> mapperService;
        private readonly ILogger<IBankingAccountService> logger;

        public BankingAccountService
            (IEntityExtendedService<BankAccount> bankAccountService,
            IMapperService<BankAccount, BankAccountDTO> mapperService,
            ILogger<IBankingAccountService> logger)
        {
            this.bankAccountService = bankAccountService;
            this.mapperService = mapperService;
            this.logger = logger;
        }
        public IApiResult AddBankAccount(BankAccountDTO bankAccount)
        {
            throw new NotImplementedException();
        }

        public async Task<IApiResult> AddBankAccountAsync(BankAccountDTO bankAccount)
        {
            return await Task.FromResult(AddBankAccount(bankAccount));
        }

        public IApiResult DeleteBankAccount(object id)
        {
            throw new NotImplementedException();
        }

        public async Task<IApiResult> DeleteBankAccountAsync(object id)
        {
            return await Task.FromResult(DeleteBankAccount(id));
        }

        public IApiResult Deposit(BankAccountAction bankAccountAction)
        {
            throw new NotImplementedException();
        }

        public async Task<IApiResult> DepositAsync(BankAccountAction bankAccountAction)
        {
            return await Task.FromResult(Deposit(bankAccountAction));
        }

        public IApiResult GetBankAccountByNumber(string bankAccountNumber)
        {
            throw new NotImplementedException();
        }

        public async Task<IApiResult> GetBankAccountByNumberAsync(string bankAccountNumber)
        {
            return await Task.FromResult(GetBankAccountByNumber(bankAccountNumber));
        }

        public IApiResult GetBankAccounts()
        {
            throw new NotImplementedException();
        }

        public async Task<IApiResult> GetBankAccountsAsync()
        {
            return await Task.FromResult(GetBankAccounts());
        }

        public IApiResult Transfer(TransferAction transferAction)
        {
            throw new NotImplementedException();
        }

        public async Task<IApiResult> TransferAsync(TransferAction transferAction)
        {
            return await Task.FromResult(Transfer(transferAction));
        }

        public IApiResult UpdateBankAccount(BankAccountDTO bankAccount)
        {
            throw new NotImplementedException();
        }

        public async Task<IApiResult> UpdateBankAccountAsync(BankAccountDTO bankAccount)
        {
            return await Task.FromResult(UpdateBankAccount(bankAccount));
        }

        public IApiResult Withdrow(BankAccountAction bankAccountAction)
        {
            throw new NotImplementedException();
        }

        public async Task<IApiResult> WithdrowAsync(BankAccountAction bankAccountAction)
        {
            return await Task.FromResult(Withdrow(bankAccountAction));
        }
    }
}
