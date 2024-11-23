using BankingManager.Database.Model;
using BankingManager.Services.Model;
using BankingManager.Services.Model.Actions;
using BankingManager.Services.Utility.ApiResult;
using BankingManager.Services.Utility.ErrorMessages;
using BankingManager.Services.Utility.Validation;
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
            var apiResult = default(IApiResult);

            var existingAccountResult = ValidateExistingAccount(accountNumber: bankAccount.Number);

            if (existingAccountResult.IsValid)
            {
                var existingAccountErrorMessage = string.Format(ErrorMessages.ExistingAccountError, bankAccount.Number);
                logger.LogError(existingAccountErrorMessage);
                apiResult = new ApiErrorResult(ApiResultStatus.Conflict, existingAccountErrorMessage, existingAccountErrorMessage, [existingAccountErrorMessage]);
            }
            else
            {
                var accountFromDto = mapperService.MapEntity(bankAccount);

                if (accountFromDto == null)
                {
                    var loggerErrorMessage = $"{string.Format(ErrorMessages.AddErrorLogger, bankAccount.Number)}. {ErrorMessages.MappingError}";
                    logger.LogError(ErrorMessages.MappingError);
                    apiResult = new ApiErrorResult(loggerErrorMessage: ErrorMessages.MappingError, errorMessage: ErrorMessages.MappingError, errors: [ErrorMessages.MappingError]);
                }
                else
                {
                    //accountFromDto.Number = Guid.NewGuid();
                    var addResult = bankAccountService.Create(accountFromDto);

                    if (!addResult)
                    {
                        var loggerErrorMessage = string.Format(ErrorMessages.AddErrorLogger, accountFromDto.Number);
                        logger.LogError(loggerErrorMessage);
                        apiResult = new ApiErrorResult(loggerErrorMessage: loggerErrorMessage, errorMessage: ErrorMessages.AddErrorBase, errors: [ErrorMessages.AddErrorBase]);
                    }
                    else
                    {
                        logger.LogInformation($"Successfully added a new bank account. Account number: |{accountFromDto.Number}|");
                        apiResult = new ApiOkResult(ApiResultStatus.NoContent);
                    }
                }
            }

            return apiResult;
        }

        public async Task<IApiResult> AddBankAccountAsync(BankAccountDTO bankAccount)
        {
            return await Task.FromResult(AddBankAccount(bankAccount));
        }

        public IApiResult DeleteBankAccount(object id)
        {
            var apiResult = default(IApiResult);

            var existingAccountResult = ValidateExistingAccount(accountId: id);

            if (!existingAccountResult.IsValid)
            {
                var notExistingAccountErrorMessage = string.Format(ErrorMessages.DoesNotExist, id);
                apiResult = new ApiErrorResult(ApiResultStatus.NotFound, existingAccountResult.Error, existingAccountResult.Error, existingAccountResult.Errors);
            }
            else
            {
                var deleteResult = bankAccountService.Delete(id);

                if (!deleteResult)
                {
                    var loggerErrorMessage = string.Format(ErrorMessages.DeleteErrorBase, id);
                    logger.LogError(loggerErrorMessage);
                    apiResult = new ApiErrorResult(loggerErrorMessage: loggerErrorMessage, errorMessage: ErrorMessages.DeleteErrorBase, errors: [ErrorMessages.DeleteErrorBase]);
                }
                else
                {
                    logger.LogInformation($"Successfully deleted a bank account. Account id: |{id}|");
                    apiResult = new ApiOkResult(ApiResultStatus.NoContent);
                }
            }

            return apiResult;
        }

        public async Task<IApiResult> DeleteBankAccountAsync(object id)
        {
            return await Task.FromResult(DeleteBankAccount(id));
        }

        public IApiResult Deposit(BankAccountAction bankAccountAction)
        {
            var apiResult = default(IApiResult);

            var accountCurrencyResult = ValidateAccountCurrency(bankAccountAction.AccountNumber, bankAccountAction.Currency);

            if (!accountCurrencyResult.IsValid)
            {
                apiResult = new ApiErrorResult(ApiResultStatus.BadRequest, accountCurrencyResult.Error, accountCurrencyResult.Error, accountCurrencyResult.Errors);
            }
            else
            {
                accountCurrencyResult.BankAccount.Balance += bankAccountAction.Ammount;
                var accountDto = mapperService.MapDto(accountCurrencyResult.BankAccount);
                apiResult = UpdateBankAccount(accountDto);
            }

            return apiResult;
        }

        public async Task<IApiResult> DepositAsync(BankAccountAction bankAccountAction)
        {
            return await Task.FromResult(Deposit(bankAccountAction));
        }

        public IApiResult GetBankAccountByNumber(string bankAccountNumber)
        {
            var apiResult = default(IApiResult);

            var existingAccountResult = ValidateExistingAccount(accountNumber: bankAccountNumber);

            if (!existingAccountResult.IsValid)
            {
                apiResult = new ApiErrorResult(ApiResultStatus.BadRequest, existingAccountResult.Error, existingAccountResult.Error, existingAccountResult.Errors);
            }
            else
            {
                var existingAccountDto = mapperService.MapDto(existingAccountResult.BankAccount);
                var successMessage = "Success";
                logger.LogInformation($"{successMessage}. Account number: {bankAccountNumber}");
                apiResult = new ApiOkResult(message: successMessage, data: existingAccountDto);
            }

            return apiResult;
        }

        public async Task<IApiResult> GetBankAccountByNumberAsync(string bankAccountNumber)
        {
            return await Task.FromResult(GetBankAccountByNumber(bankAccountNumber));
        }

        public IApiResult GetBankAccounts()
        {
            var apiResult = default(IApiResult);

            var accounts = bankAccountService.Read().ToArray();

            if (accounts == null || accounts.Length == 0)
            {
                var noAccountsMessage = "No accounts to show";
                logger.LogWarning(noAccountsMessage);
                apiResult = new ApiOkResult(ApiResultStatus.NoContent, noAccountsMessage);
            }
            else
            {
                var successMessage = "Success";
                logger.LogInformation($"{successMessage}. Accounts count: {accounts.Length}");
                var accountsDtos = mapperService.MapDtos(accounts);
                apiResult = new ApiOkResult(ApiResultStatus.Ok, successMessage, accountsDtos);
            }

            return apiResult;
        }

        public async Task<IApiResult> GetBankAccountsAsync()
        {
            return await Task.FromResult(GetBankAccounts());
        }

        public IApiResult Transfer(TransferAction transferAction)
        {
            var apiResult = default(IApiResult);

            var existingFromAccountResult = ValidateAccountCurrency(transferAction.FromAccountNumber, transferAction.Currency);
            var existingToAccountResult = ValidateAccountCurrency(transferAction.ToAccountNumber, transferAction.Currency);

            if (!existingFromAccountResult.IsValid)
            {
                apiResult = new ApiErrorResult(ApiResultStatus.BadRequest, existingFromAccountResult.Error, existingFromAccountResult.Error, existingFromAccountResult.Errors);
            }
            else if (!existingToAccountResult.IsValid)
            {
                apiResult = new ApiErrorResult(ApiResultStatus.BadRequest, existingToAccountResult.Error, existingToAccountResult.Error, existingToAccountResult.Errors);
            }
            else if (!existingFromAccountResult.IsValid && !existingToAccountResult.IsValid)
            {
                var combinedErrorMessage = $"Account 1: {existingFromAccountResult.Error}. Account 2: {existingToAccountResult.Error}";
                string[] combinedErrors = [.. existingFromAccountResult.Errors, .. existingToAccountResult.Errors];
                apiResult = new ApiErrorResult(ApiResultStatus.BadRequest, combinedErrorMessage, combinedErrorMessage, combinedErrors);
            }
            else
            {
                var fromAccount = existingFromAccountResult.BankAccount;
                var toAccount = existingToAccountResult.BankAccount;
                var transferErrorMessage = string.Empty;

                var balanceResult = ValidateBalanceManipulation(fromAccount.Balance, transferAction.Ammount, fromAccount.Number.ToString(), toAccount.Number.ToString());

                if (!balanceResult.IsValid)
                {
                    apiResult = new ApiErrorResult(loggerErrorMessage: balanceResult.Error, errorMessage: balanceResult.Error, errors: balanceResult.Errors);
                }
                else
                {
                    fromAccount.Balance -= transferAction.Ammount;
                    toAccount.Balance += transferAction.Ammount;

                    var fromAccountDto = mapperService.MapDto(fromAccount);
                    var toAccountDto = mapperService.MapDto(toAccount);

                    apiResult = UpdateBankAccount(fromAccountDto);
                    apiResult = UpdateBankAccount(toAccountDto);
                }
            }

            return apiResult;
        }

        public async Task<IApiResult> TransferAsync(TransferAction transferAction)
        {
            return await Task.FromResult(Transfer(transferAction));
        }

        public IApiResult UpdateBankAccount(BankAccountDTO bankAccount)
        {
            var apiResult = default(IApiResult);

            var existingAccountResult = ValidateExistingAccount(accountId: bankAccount.Id);

            if (!existingAccountResult.IsValid)
            {
                var notExistingAccountErrorMessage = string.Format(ErrorMessages.DoesNotExist, bankAccount.Id);
                apiResult = new ApiErrorResult(ApiResultStatus.NotFound, existingAccountResult.Error, existingAccountResult.Error, existingAccountResult.Errors);
            }
            else
            {
                var newAccountData = mapperService.MapEntity(bankAccount);
                var updatedAccount = bankAccountService.Update(existingAccountResult.BankAccount, newAccountData);

                if (updatedAccount == null)
                {
                    var updateErrorMessage = string.Format(ErrorMessages.UpdateErrorBase, newAccountData.Number);
                    apiResult = new ApiErrorResult(ApiResultStatus.BadRequest, updateErrorMessage, updateErrorMessage, [updateErrorMessage]);
                }
                else
                {
                    var updatedAccountDto = mapperService.MapDto(updatedAccount);
                    apiResult = new ApiOkResult(ApiResultStatus.Ok, "Contact updated", data: updatedAccountDto);
                }
            }

            return apiResult;
        }

        public async Task<IApiResult> UpdateBankAccountAsync(BankAccountDTO bankAccount)
        {
            return await Task.FromResult(UpdateBankAccount(bankAccount));
        }

        public IApiResult Withdrow(BankAccountAction bankAccountAction)
        {
            var apiResult = default(IApiResult);

            var existingAccountResult = ValidateAccountCurrency(bankAccountAction.AccountNumber, bankAccountAction.Currency);

            if (!existingAccountResult.IsValid)
            {
                apiResult = new ApiErrorResult(ApiResultStatus.BadRequest, existingAccountResult.Error, existingAccountResult.Error, existingAccountResult.Errors);
            }
            else
            {
                var account = existingAccountResult.BankAccount;
                var balanceResult = ValidateBalanceManipulation(account.Balance, bankAccountAction.Ammount, account.Number.ToString());

                if (!balanceResult.IsValid)
                {
                    apiResult = new ApiErrorResult(loggerErrorMessage: balanceResult.Error, errorMessage: balanceResult.Error, errors: balanceResult.Errors);
                }
                else
                {
                    account.Balance -= bankAccountAction.Ammount;
                    var accountDto = mapperService.MapDto(account);
                    apiResult = UpdateBankAccount(accountDto);
                }
            }

            return apiResult;
        }

        public async Task<IApiResult> WithdrowAsync(BankAccountAction bankAccountAction)
        {
            return await Task.FromResult(Withdrow(bankAccountAction));
        }

        private AccountValidationResult ValidateExistingAccount(object accountId = null, string accountNumber = null)
        {
            var accountValidationResult = default(AccountValidationResult);
            var existingAccount = default(BankAccount);
            var identifier = default(object);

            if (accountId == null && accountNumber != null)
            {
                existingAccount = bankAccountService.ReadByCondition(a => a.Number.ToString() == accountNumber);
                identifier = accountNumber;
            }
            else if (accountNumber == null && accountId != null)
            {
                existingAccount = bankAccountService.ReadById(accountId);
                identifier = accountId;
            }
            else
            {
                accountValidationResult = new AccountValidationResult
                {
                    IsValid = false,
                    Error = ErrorMessages.AccountIdentifierError,
                    Errors = [ErrorMessages.AccountIdentifierError]
                };
            }

            if (existingAccount == null)
            {
                var notExistingAccountErrorMessage = string.Format(ErrorMessages.DoesNotExist, identifier);
                logger.LogError(notExistingAccountErrorMessage);
                accountValidationResult = new AccountValidationResult
                {
                    IsValid = false,
                    Error = notExistingAccountErrorMessage,
                    Errors = [notExistingAccountErrorMessage]
                };
            }
            else
            {
                accountValidationResult = new AccountValidationResult
                {
                    IsValid = true,
                    BankAccount = existingAccount
                };
            }

            return accountValidationResult;
        }

        private AccountValidationResult ValidateAccountCurrency(string accountNumber, string currencyName)
        {
            var existingAccountResult = ValidateExistingAccount(accountNumber: accountNumber);
            var accountValidationResult = default(AccountValidationResult);

            if (!existingAccountResult.IsValid)
            {
                return existingAccountResult;
            }
            else
            {
                var existingCurrencyName = Enum.GetName(typeof(Currency), existingAccountResult.BankAccount.Currency);

                if (currencyName != existingCurrencyName)
                {
                    var currencyMissmatchError = string.Format(ErrorMessages.CurrencyMissmatchError, existingCurrencyName, currencyName);
                    accountValidationResult = new AccountValidationResult
                    {
                        IsValid = false,
                        Error = currencyMissmatchError,
                        Errors = [currencyMissmatchError]
                    };
                    logger.LogError(currencyMissmatchError);
                }
                else
                {
                    accountValidationResult = new AccountValidationResult
                    {
                        IsValid = true,
                        BankAccount = existingAccountResult.BankAccount
                    };
                }
            }

            return accountValidationResult;
        }

        private AccountValidationResult ValidateBalanceManipulation(double balance, double ammount, string fromAccountNumber, string toAccountNumber = null)
        {
            var errorMessage = !string.IsNullOrEmpty(toAccountNumber) ?
                string.Format(ErrorMessages.TransferBalanceError, fromAccountNumber, toAccountNumber, balance, ammount) :
                string.Format(ErrorMessages.BalanceError, balance, ammount);

            return balance < ammount ?
                new AccountValidationResult
                {
                    IsValid = false,
                    Error = errorMessage,
                    Errors = [errorMessage]
                } :
                new AccountValidationResult
                {
                    IsValid = true
                };
        }
    }
}
