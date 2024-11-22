using BankingManager.Services.Model;
using BankingManager.Services.Model.Actions;
using BankingManager.Services.Utility.ApiResult;

namespace BankingManager.Services
{
    public interface IBankingAccountService
    {
        IApiResult AddBankAccount(BankAccountDTO bankAccount);
        Task<IApiResult> AddBankAccountAsync(BankAccountDTO bankAccount);
        IApiResult DeleteBankAccount(object id);
        Task<IApiResult> DeleteBankAccountAsync(object id);
        IApiResult Deposit(BankAccountAction bankAccountAction);
        Task<IApiResult> DepositAsync(BankAccountAction bankAccountAction);
        IApiResult GetBankAccounts();
        Task<IApiResult> GetBankAccountsAsync();
        IApiResult GetBankAccountByNumber(string bankAccountNumber);
        Task<IApiResult> GetBankAccountByNumberAsync(string bankAccountNumber);
        IApiResult Transfer(TransferAction transferAction);
        Task<IApiResult> TransferAsync(TransferAction transferAction);
        IApiResult UpdateBankAccount(BankAccountDTO bankAccount);
        Task<IApiResult> UpdateBankAccountAsync(BankAccountDTO bankAccount);
        IApiResult Withdrow(BankAccountAction bankAccountAction);
        Task<IApiResult> WithdrowAsync(BankAccountAction bankAccountAction);
    }
}
