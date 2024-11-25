using BankingManager.Database.Model;
using BankingManager.Services;
using BankingManager.Tests.Utility;
using System.Reflection;

namespace BankingManager.Tests.ServicesTests
{
    public class EntityServiceTests
    {
        private readonly IEntityService<BankAccount> entityService;
        private readonly MockUtility mockUtility;
        private readonly TestDataUtility testDataUtility;

        public EntityServiceTests()
        {
            mockUtility = new MockUtility();
            testDataUtility = mockUtility.TestDataUtility;
            entityService = mockUtility.MockBankAccountEntityService();
        }

        #region Read
        [Fact]
        public void Read_ShouldGetBankAccountsFromDb()
        {
            var bankAccounts = entityService.Read();

            Assert.NotNull(bankAccounts);
            Assert.NotEmpty(bankAccounts);
        }

        [Fact]
        public async Task ReadAsync_ShouldGetBankAccountsFromDb()
        {
            var bankAccounts = await entityService.ReadAsync();

            Assert.NotNull(bankAccounts);
            Assert.NotEmpty(bankAccounts);
        }

        [Fact]
        public void Read_ShouldReturnAnEmptyResult()
        {
            // Simulate an empty db
            var localEntityService = mockUtility.MockBankAccountEntityServiceEmptyDb();

            var bankAccounts = localEntityService.Read();

            Assert.NotNull(bankAccounts);
            Assert.Empty(bankAccounts);
        }

        [Fact]
        public async Task ReadAsync_ShouldReturnAnEmptyResult()
        {
            // Simulate an empty db
            var localEntityService = mockUtility.MockBankAccountEntityServiceEmptyDb();

            var bankAccounts = await localEntityService.ReadAsync();

            Assert.NotNull(bankAccounts);
            Assert.Empty(bankAccounts);
        }
        #endregion

        #region Create
        [Fact]
        public void Create_ShoudAddBanckAccount()
        {
            var newBankAccount = testDataUtility.GetNewTestBankAccount();

            var createResult = entityService.Create(newBankAccount);
            var bankAccounts = entityService.Read();

            Assert.True(createResult);
            Assert.Equal(4, bankAccounts.Count());
        }

        [Fact]
        public async Task CreateAsync_ShoudAddBankAccount()
        {
            var newBankAccount = testDataUtility.GetNewTestBankAccount();
            var createResult = await entityService.CreateAsync(newBankAccount);

            var bankAccounts = await entityService.ReadAsync();

            Assert.True(createResult);
            Assert.Equal(4, bankAccounts.Count());
        }

        [Fact]
        public void Create_ShouldFailToAddNull()
        {
            var res = entityService.Create(null);
            Assert.False(res);
        }

        [Fact]
        public async Task CreateAsync_ShouldFailToAddNull()
        {
            var res = await entityService.CreateAsync(null);
            Assert.False(res);
        }
        #endregion

        #region Delete
        [Fact]
        public void Delete_ShouldRemoveBankAccount()
        {
            var bankAccounts = entityService.Read();
            var oldCount = bankAccounts.Count();
            var removeResult = entityService.Delete(1);

            Assert.True(removeResult);
            Assert.Equal(oldCount - 1, bankAccounts.Count());
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveBankAccount()
        {
            var bankAccounts = await entityService.ReadAsync();
            var oldCount = bankAccounts.Count();
            var removeResult = await entityService.DeleteAsync(1);

            Assert.True(removeResult);
            Assert.Equal(oldCount - 1, bankAccounts.Count());
        }

        [Fact]
        public void Delete_ShouldReturnFalseIfBankAccountNotFound()
        {
            var bankAccounts = entityService.Read();
            var oldCount = bankAccounts.Count();
            var removeResult = entityService.Delete(4);

            Assert.False(removeResult);
            Assert.Equal(oldCount, bankAccounts.Count());
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnFalseIfBankAccountNotFound()
        {
            var bankAccounts = await entityService.ReadAsync();
            var oldCount = bankAccounts.Count();
            var removeResult = await entityService.DeleteAsync(4);

            Assert.False(removeResult);
            Assert.Equal(3, bankAccounts.Count());
        }
        #endregion

        #region Update
        [Fact]
        public void Update_ShouldUpdateBankAccount()
        {
            var oldBankAccount = entityService.Read().FirstOrDefault(bankAccount => bankAccount.Id == 1);
            var newBankAccount = testDataUtility.GetNewTestBankAccount(1, 300, Currency.Euro);

            var updatedBankAccount = entityService.Update(oldBankAccount, newBankAccount);

            Assert.NotNull(updatedBankAccount);
            Assert.Equal(updatedBankAccount.Balance, newBankAccount.Balance);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateBankAccount()
        {
            var bankAccounts = await entityService.ReadAsync();
            var oldBankAccount = bankAccounts.FirstOrDefault(bankAccount => bankAccount.Id == 1);
            var newBankAccount = testDataUtility.GetNewTestBankAccount(1, 400, Currency.Dollar);

            var updatedBankAccount = await entityService.UpdateAsync(oldBankAccount, newBankAccount);

            Assert.NotNull(updatedBankAccount);
            Assert.Equal(updatedBankAccount.Balance, newBankAccount.Balance);
        }

        [Fact]
        public void Update_ThrowsTargetExceptionIfOldBankAccountIsNull()
        {
            var newBankAccount = testDataUtility.GetNewTestBankAccount(1, 400, Currency.Dollar);

            Assert.Throws<TargetException>(() => entityService.Update(null, newBankAccount));
        }

        [Fact]
        public void UpdateAsync_ThrowsTargetExceptionIfOldBankAccountIsNull()
        {
            var newBankAccount = testDataUtility.GetNewTestBankAccount(1, 400, Currency.Dollar);

            Assert.ThrowsAsync<TargetException>(async () => await entityService.UpdateAsync(null, newBankAccount));
        }

        [Fact]
        public void Update_ThrowsNullReferenceExceptionIfNewBankAccountIsNull()
        {
            var oldBankAccount = entityService.Read().FirstOrDefault(bankAccount => bankAccount.Id == 1);

            Assert.Throws<NullReferenceException>(() => entityService.Update(oldBankAccount, null));
        }

        [Fact]
        public void UpdateAsync_ThrowsNullReferenceExceptionIfNewBankAccountIsNull()
        {
            var oldBankAccount = entityService.Read().FirstOrDefault(bankAccount => bankAccount.Id == 1);

            Assert.ThrowsAsync<NullReferenceException>(async () => await entityService.UpdateAsync(oldBankAccount, null));
        }

        [Fact]
        public void Update_ThrowsNullReferenceExceptionIfBothAreNull()
        {
            Assert.Throws<NullReferenceException>(() => entityService.Update(null, null));
        }

        [Fact]
        public void UpdateAsync_ThrowsNullReferenceExceptionIfBothAreNull()
        {
            Assert.ThrowsAsync<NullReferenceException>(async () => await entityService.UpdateAsync(null, null));
        }
        #endregion
    }
}
