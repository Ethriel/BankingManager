using BankingManager.Database.Model;
using BankingManager.Services;
using BankingManager.Services.Model;
using BankingManager.Services.Model.Actions;
using BankingManager.Services.Utility.ApiResult;
using BankingManager.Tests.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingManager.Tests.ServicesTests
{
    public class BankAccountServiceTests
    {
        private readonly MockUtility mockUtility;
        private readonly TestDataUtility testDataUtility;
        private readonly IBankAccountService bankAccountService;
        public BankAccountServiceTests()
        {
            mockUtility = new MockUtility();
            testDataUtility = mockUtility.TestDataUtility;
            bankAccountService = mockUtility.MockBankAccountService();
        }

        #region AddBankAccount
        [Fact]
        public void AddBankAccount_ShouldCreateAndAddBankAccount()
        {
            var bankAccountDto = testDataUtility.GetNewTestBankAccountDto();
            var result = bankAccountService.AddBankAccount(bankAccountDto);

            Assert.NotNull(result);
            Assert.Equal(typeof(ApiOkResult), result.GetType());
        }

        [Fact]
        public async Task AddBankAccountAsync_ShouldCreateAndAddBankAccount()
        {
            var bankAccountDto = testDataUtility.GetNewTestBankAccountDto();
            var result = await bankAccountService.AddBankAccountAsync(bankAccountDto);

            Assert.NotNull(result);
            Assert.Equal(typeof(ApiOkResult), result.GetType());
        }

        [Fact]
        public void AddBankAccount_ShouldBeConflict()
        {
            var bankAccount = mockUtility.EntityExtendedService.Read().FirstOrDefault();
            var bankAccountDto = mockUtility.MapperService.MapDto(bankAccount);
            var result = bankAccountService.AddBankAccount(bankAccountDto);

            Assert.NotNull(result);
            Assert.Equal(typeof(ApiErrorResult), result.GetType());
            Assert.Equal(ApiResultStatus.Conflict, result.ApiResultStatus);
        }

        [Fact]
        public async Task AddBankAccountAsync_ShouldBeConflict()
        {
            var bankAccount = mockUtility.EntityExtendedService.Read().FirstOrDefault();
            var bankAccountDto = mockUtility.MapperService.MapDto(bankAccount);
            var result = await bankAccountService.AddBankAccountAsync(bankAccountDto);

            Assert.NotNull(result);
            Assert.Equal(typeof(ApiErrorResult), result.GetType());
            Assert.Equal(ApiResultStatus.Conflict, result.ApiResultStatus);
        }

        [Fact]
        public void AddBankAccount_ThrowsArgumentNullException()
        {
            Assert.Throws<NullReferenceException>(() => bankAccountService.AddBankAccount(null));
        }

        [Fact]
        public void AddBankAccountAsync_ThrowsArgumentNullException()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () => await bankAccountService.AddBankAccountAsync(null));
        }
        #endregion

        #region DeleteBankAccount
        [Fact]
        public void DeleteBankAccount_ShouldRemoveBankAccount()
        {
            var result = bankAccountService.DeleteBankAccount(1);

            Assert.NotNull(result);
            Assert.Equal(typeof(ApiOkResult), result.GetType());
        }

        [Fact]
        public async Task DeleteBankAccountASync_ShouldRemoveBankAccount()
        {
            var result = await bankAccountService.DeleteBankAccountAsync(1);

            Assert.NotNull(result);
            Assert.Equal(typeof(ApiOkResult), result.GetType());
        }

        [Fact]
        public void DeleteBankAccount_BankAccountNotFound()
        {
            var result = bankAccountService.DeleteBankAccount(5);

            Assert.NotNull(result);
            Assert.Equal(typeof(ApiErrorResult), result.GetType());
            Assert.Equal(ApiResultStatus.NotFound, result.ApiResultStatus);
        }

        [Fact]
        public async Task DeleteBankAccountASync_BankAccountNotFound()
        {
            var result = await bankAccountService.DeleteBankAccountAsync(5);

            Assert.NotNull(result);
            Assert.Equal(typeof(ApiErrorResult), result.GetType());
            Assert.Equal(ApiResultStatus.NotFound, result.ApiResultStatus);
        }

        [Fact]
        public void DeleteBankAccount_BankAccountNotFoundIfIdIsNull()
        {
            var result = bankAccountService.DeleteBankAccount(null);

            Assert.NotNull(result);
            Assert.Equal(typeof(ApiErrorResult), result.GetType());
            Assert.Equal(ApiResultStatus.NotFound, result.ApiResultStatus);
        }

        [Fact]
        public void DeleteBankAccountAsync_ThrowsNullReferenceException()
        {
            Assert.ThrowsAsync<NullReferenceException>(async () => await bankAccountService.DeleteBankAccountAsync(null));
        }
        #endregion

        #region GetBankAccounts
        [Fact]
        public void GetBankAccounts_ShouldListBankAccounts()
        {
            var result = bankAccountService.GetBankAccounts();

            Assert.NotNull(result);
            Assert.IsType<ApiOkResult>(result);
            Assert.Equal(ApiResultStatus.Ok, result.ApiResultStatus);
            Assert.NotEmpty(((IApiOkResult)result).Data as IEnumerable<BankAccountDTO>);
        }

        [Fact]
        public async Task GetBankAccountsAsync_ShouldListBankAccounts()
        {
            var result = await bankAccountService.GetBankAccountsAsync();

            Assert.NotNull(result);
            Assert.IsType<ApiOkResult>(result);
            Assert.Equal(ApiResultStatus.Ok, result.ApiResultStatus);
            Assert.NotEmpty(((IApiOkResult)result).Data as IEnumerable<BankAccountDTO>);
        }

        [Fact]
        public void GetBankAccounts_ShouldReturnNoContent()
        {
            // Simulate an empty db
            var localBankAccountService = mockUtility.MockBankAccountServiceEmptyDb();

            var result = localBankAccountService.GetBankAccounts();

            Assert.NotNull(result);
            Assert.IsType<ApiOkResult>(result);
            Assert.Equal(ApiResultStatus.NoContent, result.ApiResultStatus);
        }

        [Fact]
        public async Task GetBankAccountsAsync_ShouldReturnNoContent()
        {
            // Simulate an empty db
            var localBankAccountService = mockUtility.MockBankAccountServiceEmptyDb();

            var result = await localBankAccountService.GetBankAccountsAsync();

            Assert.NotNull(result);
            Assert.IsType<ApiOkResult>(result);
            Assert.Equal(ApiResultStatus.NoContent, result.ApiResultStatus);
        }
        #endregion

        #region UpdateBankAccount
        [Fact]
        public void UpdateBankAccount_ShouldUpdateBankAccount()
        {
            var oldBankAccount = mockUtility.EntityExtendedService.Read().FirstOrDefault(bankAccount => bankAccount.Id == 1);
            var newBankAccount = new BankAccount { Id = oldBankAccount.Id, Balance = 300, Currency = Currency.Euro, Number = oldBankAccount.Number };
            var newBankAccountDto = mockUtility.MapperService.MapDto(newBankAccount);

            var result = bankAccountService.UpdateBankAccount(newBankAccountDto);

            Assert.NotNull(result);
            Assert.IsType<ApiOkResult>(result);
            Assert.Equal(ApiResultStatus.Ok, result.ApiResultStatus);
            Assert.Equal(newBankAccountDto.Balance, (((IApiOkResult)result).Data as BankAccountDTO).Balance);
        }

        [Fact]
        public async Task UpdateBankAccountAsync_ShouldUpdateBankAccount()
        {
            var oldBankAccount = mockUtility.EntityExtendedService.Read().FirstOrDefault(bankAccount => bankAccount.Id == 1);
            var newBankAccount = new BankAccount { Id = oldBankAccount.Id, Balance = 300, Currency = Currency.Euro, Number = oldBankAccount.Number };
            var newBankAccountDto = mockUtility.MapperService.MapDto(newBankAccount);

            var result = await bankAccountService.UpdateBankAccountAsync(newBankAccountDto);

            Assert.NotNull(result);
            Assert.IsType<ApiOkResult>(result);
            Assert.Equal(ApiResultStatus.Ok, result.ApiResultStatus);
            Assert.Equal(newBankAccountDto.Balance, (((IApiOkResult)result).Data as BankAccountDTO).Balance);
        }

        [Fact]
        public void UpdateBankAccount_ShouldReturnNotFound()
        {
            var newBankAccountDto = testDataUtility.GetNewTestBankAccountDto(5);

            var result = bankAccountService.UpdateBankAccount(newBankAccountDto);

            Assert.NotNull(result);
            Assert.IsType<ApiErrorResult>(result);
            Assert.Equal(ApiResultStatus.NotFound, result.ApiResultStatus);
        }

        [Fact]
        public async Task UpdateBankAccountAsync_ShouldReturnNotFound()
        {
            var newBankAccountDto = testDataUtility.GetNewTestBankAccountDto(5);

            var result = await bankAccountService.UpdateBankAccountAsync(newBankAccountDto);

            Assert.NotNull(result);
            Assert.IsType<ApiErrorResult>(result);
            Assert.Equal(ApiResultStatus.NotFound, result.ApiResultStatus);
        }

        [Fact]
        public void UpdateBankAccount_ThrowsArgumentNullException()
        {
            Assert.Throws<NullReferenceException>(() => bankAccountService.UpdateBankAccount(null));
        }

        [Fact]
        public void UpdateBankAccountAsync_ThrowsArgumentNullException()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () => await bankAccountService.UpdateBankAccountAsync(null));
        }
        #endregion

        #region Deposit
        [Fact]
        public void Deposit_ShouldAddMoney()
        {
            var account = mockUtility.EntityExtendedService.Read().FirstOrDefault();
            var currency = Enum.GetName(typeof(Currency), account?.Currency);
            var ammount = 100;
            var action = new BankAccountAction { AccountNumber = account?.Number.ToString(), Ammount = ammount, Currency = currency };

            var result = bankAccountService.Deposit(action);

            Assert.NotNull(result);
            Assert.Equal(typeof(ApiOkResult), result.GetType());
        }

        [Fact]
        public async Task DepositAsync_ShouldAddMoney()
        {
            var account = mockUtility.EntityExtendedService.Read().FirstOrDefault();
            var currency = Enum.GetName(typeof(Currency), account?.Currency);
            var ammount = 100;
            var action = new BankAccountAction { AccountNumber = account?.Number.ToString(), Ammount = ammount, Currency = currency };

            var result = await bankAccountService.DepositAsync(action);

            Assert.NotNull(result);
            Assert.Equal(typeof(ApiOkResult), result.GetType());
        }

        [Fact]
        public void Deposit_ShouldBeErrorIfCurrencyIsInvalid()
        {
            var account = mockUtility.EntityExtendedService.Read().FirstOrDefault();
            var currency = "Yen";
            var ammount = 100;
            var action = new BankAccountAction { AccountNumber = account?.Number.ToString(), Ammount = ammount, Currency = currency };

            var result = bankAccountService.Deposit(action);

            Assert.NotNull(result);
            Assert.Equal(typeof(ApiErrorResult), result.GetType());
        }

        [Fact]
        public async Task DepositAsync_ShouldBeErrorIfCurrencyIsInvalid()
        {
            var account = mockUtility.EntityExtendedService.Read().FirstOrDefault();
            var currency = "Yen";
            var ammount = 100;
            var action = new BankAccountAction { AccountNumber = account?.Number.ToString(), Ammount = ammount, Currency = currency };

            var result = await bankAccountService.DepositAsync(action);

            Assert.NotNull(result);
            Assert.Equal(typeof(ApiErrorResult), result.GetType());
        }

        [Fact]
        public void Deposit_ShouldBeErrorIfAccountNumberIsInvalid()
        {
            var number = Guid.NewGuid().ToString();
            var currency = "Dollar";
            var ammount = 100;
            var action = new BankAccountAction { AccountNumber = number, Ammount = ammount, Currency = currency };

            var result = bankAccountService.Deposit(action);

            Assert.NotNull(result);
            Assert.Equal(typeof(ApiErrorResult), result.GetType());
        }

        [Fact]
        public async Task DepositAsync_ShouldBeErrorIfAccountNumberIsInvalid()
        {
            var number = Guid.NewGuid().ToString();
            var currency = "Dollar";
            var ammount = 100;
            var action = new BankAccountAction { AccountNumber = number, Ammount = ammount, Currency = currency };

            var result = await bankAccountService.DepositAsync(action);

            Assert.NotNull(result);
            Assert.Equal(typeof(ApiErrorResult), result.GetType());
        }
        #endregion

        #region Withdrow
        [Fact]
        public void Withdrow_ShouldWithdrowMoney()
        {
            var account = mockUtility.EntityExtendedService.Read().FirstOrDefault();
            var currency = Enum.GetName(typeof(Currency), account?.Currency);
            var ammount = 100;
            var action = new BankAccountAction { AccountNumber = account?.Number.ToString(), Ammount = ammount, Currency = currency };

            var result = bankAccountService.Withdrow(action);

            Assert.NotNull(result);
            Assert.Equal(typeof(ApiOkResult), result.GetType());
        }

        [Fact]
        public async Task WithdrowAsync_ShouldWithdrowMoney()
        {
            var account = mockUtility.EntityExtendedService.Read().FirstOrDefault();
            var currency = Enum.GetName(typeof(Currency), account?.Currency);
            var ammount = 100;
            var action = new BankAccountAction { AccountNumber = account?.Number.ToString(), Ammount = ammount, Currency = currency };

            var result = await bankAccountService.WithdrowAsync(action);

            Assert.NotNull(result);
            Assert.Equal(typeof(ApiOkResult), result.GetType());
        }

        [Fact]
        public void Withdrow_ShouldBeErrorIfCurrencyIsInvalid()
        {
            var account = mockUtility.EntityExtendedService.Read().FirstOrDefault();
            var currency = "Yen";
            var ammount = 100;
            var action = new BankAccountAction { AccountNumber = account?.Number.ToString(), Ammount = ammount, Currency = currency };

            var result = bankAccountService.Withdrow(action);

            Assert.NotNull(result);
            Assert.Equal(typeof(ApiErrorResult), result.GetType());
        }

        [Fact]
        public async Task WithdrowAsync_ShouldBeErrorIfCurrencyIsInvalid()
        {
            var account = mockUtility.EntityExtendedService.Read().FirstOrDefault();
            var currency = "Yen";
            var ammount = 100;
            var action = new BankAccountAction { AccountNumber = account?.Number.ToString(), Ammount = ammount, Currency = currency };

            var result = await bankAccountService.DepositAsync(action);

            Assert.NotNull(result);
            Assert.Equal(typeof(ApiErrorResult), result.GetType());
        }

        [Fact]
        public void Withdrow_ShouldBeErrorIfAccountNumberIsInvalid()
        {
            var number = Guid.NewGuid().ToString();
            var currency = "Dollar";
            var ammount = 100;
            var action = new BankAccountAction { AccountNumber = number, Ammount = ammount, Currency = currency };

            var result = bankAccountService.Withdrow(action);

            Assert.NotNull(result);
            Assert.Equal(typeof(ApiErrorResult), result.GetType());
        }

        [Fact]
        public async Task WithdrowAsync_ShouldBeErrorIfAccountNumberIsInvalid()
        {
            var number = Guid.NewGuid().ToString();
            var currency = "Dollar";
            var ammount = 100;
            var action = new BankAccountAction { AccountNumber = number, Ammount = ammount, Currency = currency };

            var result = await bankAccountService.WithdrowAsync(action);

            Assert.NotNull(result);
            Assert.Equal(typeof(ApiErrorResult), result.GetType());
        }

        [Fact]
        public void Withdrow_ShouldBeErrorIfAmmountIsInvalid()
        {
            var account = mockUtility.EntityExtendedService.Read().FirstOrDefault();
            var currency = Enum.GetName(typeof(Currency), account?.Currency);
            var ammount = account.Balance + 100;
            var action = new BankAccountAction { AccountNumber = account?.Number.ToString(), Ammount = ammount, Currency = currency };

            var result = bankAccountService.Withdrow(action);

            Assert.NotNull(result);
            Assert.Equal(typeof(ApiErrorResult), result.GetType());
        }

        [Fact]
        public async Task WithdrowAsync_ShouldBeErrorIfAmmountIsInvalid()
        {
            var account = mockUtility.EntityExtendedService.Read().FirstOrDefault();
            var currency = Enum.GetName(typeof(Currency), account?.Currency);
            var ammount = account.Balance + 100;
            var action = new BankAccountAction { AccountNumber = account?.Number.ToString(), Ammount = ammount, Currency = currency };

            var result = await bankAccountService.WithdrowAsync(action);

            Assert.NotNull(result);
            Assert.Equal(typeof(ApiErrorResult), result.GetType());
        }
        #endregion

        #region GetBankAccountByNumber
        [Fact]
        public void GetBankAccountByNumber_ShouldReturnAccount()
        {
            var number = mockUtility.EntityExtendedService.Read().FirstOrDefault().Number.ToString();
            var result = bankAccountService.GetBankAccountByNumber(number);

            Assert.NotNull(result);
            Assert.Equal(typeof(ApiOkResult), result.GetType());
        }

        [Fact]
        public async Task GetBankAccountByNumberAsync_ShouldReturnAccount()
        {
            var number = mockUtility.EntityExtendedService.Read().FirstOrDefault().Number.ToString();
            var result = await bankAccountService.GetBankAccountByNumberAsync(number);

            Assert.NotNull(result);
            Assert.Equal(typeof(ApiOkResult), result.GetType());
        }

        [Fact]
        public void GetBankAccountByNumber_ShouldBeErrorIfAccountNumberIsInvalid()
        {
            var number = Guid.NewGuid().ToString();
            var result = bankAccountService.GetBankAccountByNumber(number);

            Assert.NotNull(result);
            Assert.Equal(typeof(ApiErrorResult), result.GetType());
        }

        [Fact]
        public async Task GetBankAccountByNumberAsync_ShouldBeErrorIfAccountNumberIsInvalid()
        {
            var number = Guid.NewGuid().ToString();
            var result = await bankAccountService.GetBankAccountByNumberAsync(number);

            Assert.NotNull(result);
            Assert.Equal(typeof(ApiErrorResult), result.GetType());
        }
        #endregion

        #region Transfer
        [Fact]
        public void Transfer_ShouldTransferMoney()
        {
            var accountFrom = mockUtility.EntityExtendedService.Read().FirstOrDefault();
            var accountTo = mockUtility.EntityExtendedService.Read().FirstOrDefault();
            var currency = Enum.GetName(typeof(Currency), accountFrom?.Currency);
            var ammount = 100;
            var action = new TransferAction { FromAccountNumber = accountFrom.Number.ToString(), ToAccountNumber = accountTo.Number.ToString(), Ammount = ammount, Currency = currency };

            var result = bankAccountService.Transfer(action);

            Assert.NotNull(result);
            Assert.Equal(typeof(ApiOkResult), result.GetType());
        }

        [Fact]
        public async Task TransferAsync_ShouldTransferMoney()
        {
            var accountFrom = mockUtility.EntityExtendedService.Read().FirstOrDefault();
            var accountTo = mockUtility.EntityExtendedService.Read().FirstOrDefault();
            var currency = Enum.GetName(typeof(Currency), accountFrom?.Currency);
            var ammount = 100;
            var action = new TransferAction { FromAccountNumber = accountFrom.Number.ToString(), ToAccountNumber = accountTo.Number.ToString(), Ammount = ammount, Currency = currency };

            var result = await bankAccountService.TransferAsync(action);

            Assert.NotNull(result);
            Assert.Equal(typeof(ApiOkResult), result.GetType());
        }

        [Fact]
        public void Transfer_ShouldBeErrorIfCurrencyIsInvalid()
        {
            var accountFrom = mockUtility.EntityExtendedService.Read().FirstOrDefault();
            var accountTo = mockUtility.EntityExtendedService.Read().FirstOrDefault();
            var currency = "Yen";
            var ammount = 100;
            var action = new TransferAction { FromAccountNumber = accountFrom.Number.ToString(), ToAccountNumber = accountTo.Number.ToString(), Ammount = ammount, Currency = currency };

            var result = bankAccountService.Transfer(action);

            Assert.NotNull(result);
            Assert.Equal(typeof(ApiErrorResult), result.GetType());
        }

        [Fact]
        public async Task TransferAsync_ShouldBeErrorIfCurrencyIsInvalid()
        {
            var accountFrom = mockUtility.EntityExtendedService.Read().FirstOrDefault();
            var accountTo = mockUtility.EntityExtendedService.Read().FirstOrDefault();
            var currency = "Yen";
            var ammount = 100;
            var action = new TransferAction { FromAccountNumber = accountFrom.Number.ToString(), ToAccountNumber = accountTo.Number.ToString(), Ammount = ammount, Currency = currency };

            var result = await bankAccountService.TransferAsync(action);

            Assert.NotNull(result);
            Assert.Equal(typeof(ApiErrorResult), result.GetType());
        }

        [Fact]
        public void Transfer_ShouldBeErrorIfFromAccountIsInvalid()
        {
            var accountFromNumber = Guid.NewGuid().ToString();
            var accountTo = mockUtility.EntityExtendedService.Read().FirstOrDefault();
            var currency = Enum.GetName(typeof(Currency), accountTo?.Currency);
            var ammount = 100;
            var action = new TransferAction { FromAccountNumber = accountFromNumber, ToAccountNumber = accountTo.Number.ToString(), Ammount = ammount, Currency = currency };

            var result = bankAccountService.Transfer(action);

            Assert.NotNull(result);
            Assert.Equal(typeof(ApiErrorResult), result.GetType());
        }

        [Fact]
        public async Task TransferAsync_ShouldBeErrorIfFromAccountIsInvalid()
        {
            var accountFromNumber = Guid.NewGuid().ToString();
            var accountTo = mockUtility.EntityExtendedService.Read().FirstOrDefault();
            var currency = Enum.GetName(typeof(Currency), accountTo?.Currency);
            var ammount = 100;
            var action = new TransferAction { FromAccountNumber = accountFromNumber, ToAccountNumber = accountTo.Number.ToString(), Ammount = ammount, Currency = currency };

            var result = await bankAccountService.TransferAsync(action);

            Assert.NotNull(result);
            Assert.Equal(typeof(ApiErrorResult), result.GetType());
        }

        [Fact]
        public void Transfer_ShouldBeErrorIfToAccountIsInvalid()
        {
            var accountFrom = mockUtility.EntityExtendedService.Read().FirstOrDefault();
            var accountToNumber = Guid.NewGuid().ToString();
            var currency = Enum.GetName(typeof(Currency), accountFrom?.Currency);
            var ammount = 100;
            var action = new TransferAction { FromAccountNumber = accountFrom.Number.ToString(), ToAccountNumber = accountToNumber, Ammount = ammount, Currency = currency };

            var result = bankAccountService.Transfer(action);

            Assert.NotNull(result);
            Assert.Equal(typeof(ApiErrorResult), result.GetType());
        }

        [Fact]
        public async Task TransferAsync_ShouldBeErrorIfToAccountIsInvalid()
        {
            var accountFrom = mockUtility.EntityExtendedService.Read().FirstOrDefault();
            var accountToNumber = Guid.NewGuid().ToString();
            var currency = Enum.GetName(typeof(Currency), accountFrom?.Currency);
            var ammount = 100;
            var action = new TransferAction { FromAccountNumber = accountFrom.Number.ToString(), ToAccountNumber = accountToNumber, Ammount = ammount, Currency = currency };

            var result = await bankAccountService.TransferAsync(action);

            Assert.NotNull(result);
            Assert.Equal(typeof(ApiErrorResult), result.GetType());
        }

        [Fact]
        public void Transfer_ShouldBeErrorIfBothAccountsAreInvalid()
        {
            var accountFromNumber = Guid.NewGuid().ToString();
            var accountToNumber = Guid.NewGuid().ToString();
            var currency = Enum.GetName(typeof(Currency), Currency.Dollar);
            var ammount = 100;
            var action = new TransferAction { FromAccountNumber = accountFromNumber, ToAccountNumber = accountToNumber, Ammount = ammount, Currency = currency };

            var result = bankAccountService.Transfer(action);

            Assert.NotNull(result);
            Assert.Equal(typeof(ApiErrorResult), result.GetType());
        }

        [Fact]
        public async Task TransferAsync_ShouldBeErrorIfBothAccountsAreInvalid()
        {
            var accountFromNumber = Guid.NewGuid().ToString();
            var accountToNumber = Guid.NewGuid().ToString();
            var currency = Enum.GetName(typeof(Currency), Currency.Dollar);
            var ammount = 100;
            var action = new TransferAction { FromAccountNumber = accountFromNumber, ToAccountNumber = accountToNumber, Ammount = ammount, Currency = currency };

            var result = await bankAccountService.TransferAsync(action);

            Assert.NotNull(result);
            Assert.Equal(typeof(ApiErrorResult), result.GetType());
        }
        #endregion
    }
}
