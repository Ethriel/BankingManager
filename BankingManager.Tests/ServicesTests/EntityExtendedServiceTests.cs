using BankingManager.Database.Model;
using BankingManager.Services;
using BankingManager.Tests.Utility;

namespace BankingManager.Tests.ServicesTests
{
    public class EntityExtendedServiceTests
    {
        private readonly IEntityExtendedService<BankAccount> entityExtendedService;
        private readonly MockUtility mockUtility;
        private readonly TestDataUtility testDataUtility;
        public EntityExtendedServiceTests()
        {
            mockUtility = new MockUtility();
            testDataUtility = mockUtility.TestDataUtility;
            entityExtendedService = mockUtility.MockBankEntityExtendedService();
        }

        #region ReadByCondition
        [Fact]
        public void ReadByCondition_ShouldReturnBankAccount()
        {
            var number = entityExtendedService.Read().FirstOrDefault()?.Number;
            var bankAccount = entityExtendedService.ReadByCondition(bankAccount => bankAccount.Number == number);

            Assert.NotNull(bankAccount);
            Assert.Equal(number, bankAccount.Number);
        }

        [Fact]
        public async Task ReadByConditionAsync_ShouldReturnBankAccount()
        {
            var number = entityExtendedService.Read().FirstOrDefault()?.Number;
            var bankAccount = await entityExtendedService.ReadByConditionAsync(bankAccount => bankAccount.Number == number);

            Assert.NotNull(bankAccount);
            Assert.Equal(number, bankAccount.Number);
        }

        [Fact]
        public void ReadByCondition_ShouldReturnNull()
        {
            var number = Guid.NewGuid();
            var bankAccount = entityExtendedService.ReadByCondition(bankAccount => bankAccount.Number == number);

            Assert.Null(bankAccount);
        }

        [Fact]
        public async Task ReadByConditionAsync_ShouldReturnNull()
        {
            var number = Guid.NewGuid();
            var bankAccount = await entityExtendedService.ReadByConditionAsync(bankAccount => bankAccount.Number == number);

            Assert.Null(bankAccount);
        }

        [Fact]
        public void ReadByCondition_ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => entityExtendedService.ReadByCondition(null));
        }

        [Fact]
        public void ReadByConditionAsync_ShouldThrowArgumentNullException()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () => await entityExtendedService.ReadByConditionAsync(null));
        }
        #endregion

        #region ReadById

        [Fact]
        public void ReadById_ShouldReturnBankAccount()
        {
            var id = 1;
            var bankAccount = entityExtendedService.ReadById(id);

            Assert.NotNull(bankAccount);
            Assert.Equal(id, bankAccount.Id);
        }

        [Fact]
        public async Task ReadByIdAsync_ShouldReturnBankAccount()
        {
            var id = 1;
            var bankAccount = await entityExtendedService.ReadByIdAsync(id);

            Assert.NotNull(bankAccount);
            Assert.Equal(id, bankAccount.Id);
        }

        [Fact]
        public void ReadById_ShouldReturnNull()
        {
            var bankAccount = entityExtendedService.ReadById(4);

            Assert.Null(bankAccount);
        }

        [Fact]
        public async Task ReadByIdAsync_ShouldReturnNull()
        {
            var bankAccount = await entityExtendedService.ReadByIdAsync(4);

            Assert.Null(bankAccount);
        }

        [Fact]
        public void ReadById_ShouldThrowNullReferenceException()
        {
            Assert.Throws<NullReferenceException>(() => entityExtendedService.ReadById(null));
        }

        [Fact]
        public void ReadByIdAsync_ShouldThrowNullReferenceException()
        {
            Assert.ThrowsAsync<NullReferenceException>(async () => await entityExtendedService.ReadByIdAsync(null));
        }
        #endregion

        #region ReadEntitiesByCondition
        [Fact]
        public void ReadEntitiesByCondition_ShouldReturnBankAccounts()
        {
            var bankAccounts = entityExtendedService.ReadEntitiesByCondition(bankAccount => bankAccount.Balance >= 100);

            Assert.NotNull(bankAccounts);
            Assert.NotEmpty(bankAccounts);
        }

        [Fact]
        public async Task ReadEntitiesByConditionAsync_ShouldReturnBankAccounts()
        {
            var bankAccounts = await entityExtendedService.ReadEntitiesByConditionAsync(bankAccount => bankAccount.Balance >= 100);

            Assert.NotNull(bankAccounts);
            Assert.NotEmpty(bankAccounts);
        }

        [Fact]
        public void ReadEntitiesByCondition_ShouldBeEmptyResult()
        {
            var bankAccounts = entityExtendedService.ReadEntitiesByCondition(bankAccount => bankAccount.Balance < 100);

            Assert.Empty(bankAccounts);
        }

        [Fact]
        public async Task ReadEntitiesByConditionAsync_ShouldBeEmptyResult()
        {
            var bankAccounts = await entityExtendedService.ReadEntitiesByConditionAsync(bankAccount => bankAccount.Balance < 100);

            Assert.Empty(bankAccounts);
        }

        [Fact]
        public void ReadEntitiesByCondition_ShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => entityExtendedService.ReadEntitiesByCondition(null));
        }

        [Fact]
        public void ReadEntitiesByConditionAsync_ShouldThrowArgumentNullException()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () => await entityExtendedService.ReadEntitiesByConditionAsync(null));
        }
        #endregion

        #region ReadPortion
        [Fact]
        public void ReadPortion_ShouldReturnAPortonOfBankAccounts()
        {
            var allBankAccounts = entityExtendedService.Read();

            var portionOfBankAccounts = entityExtendedService.ReadPortion(1, 2);

            Assert.NotNull(portionOfBankAccounts);
            Assert.NotEmpty(portionOfBankAccounts);
            Assert.NotEqual(allBankAccounts.Count(), portionOfBankAccounts.Count());
        }

        [Fact]
        public async Task ReadPortionAsync_ShouldReturnAPortonOfBankAccounts()
        {
            var allBankAccounts = await entityExtendedService.ReadAsync();

            var portionOfBankAccounts = await entityExtendedService.ReadPortionAsync(1, 2);

            Assert.NotNull(portionOfBankAccounts);
            Assert.NotEmpty(portionOfBankAccounts);
            Assert.NotEqual(allBankAccounts.Count(), portionOfBankAccounts.Count());
        }

        [Fact]
        public void ReadPortion_ShouldReturnEmptyResult()
        {
            var portionOfBankAccounts = entityExtendedService.ReadPortion(10, 10);

            Assert.NotNull(portionOfBankAccounts);
            Assert.Empty(portionOfBankAccounts);
        }

        [Fact]
        public async Task ReadPortionAsync_ShouldReturnEmptyResult()
        {
            var portionOfBankAccounts = await entityExtendedService.ReadPortionAsync(10, 10);

            Assert.NotNull(portionOfBankAccounts);
            Assert.Empty(portionOfBankAccounts);
        }
        #endregion
    }
}
