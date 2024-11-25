using BankingManager.Database.Model;
using BankingManager.Services;
using BankingManager.Services.Model;
using BankingManager.Tests.Utility;

namespace BankingManager.Tests.ServicesTests
{
    public class MapperServiceTests
    {
        private readonly IMapperService<BankAccount, BankAccountDTO> mapperService;
        private readonly MockUtility mockUtility;
        private readonly TestDataUtility testDataUtility;

        public MapperServiceTests()
        {
            mockUtility = new MockUtility();
            testDataUtility = mockUtility.TestDataUtility;
            mapperService = mockUtility.MockMapperService();
        }

        #region MapDto
        [Fact]
        public void MapDto_MappsEntityToDTO()
        {
            var bankAccount = testDataUtility.GetNewTestBankAccount();

            var bankAccountDto = mapperService.MapDto(bankAccount);

            Assert.NotNull(bankAccountDto);
            Assert.True(bankAccount.Id == bankAccountDto.Id && bankAccount.Number.ToString() == bankAccountDto.Number);
        }

        [Fact]
        public async Task MapDtoAsync_MappsEntityToDTO()
        {
            var bankAccount = testDataUtility.GetNewTestBankAccount();

            var bankAccountDto = await mapperService.MapDtoAsync(bankAccount);

            Assert.NotNull(bankAccountDto);
            Assert.True(bankAccount.Id == bankAccountDto.Id && bankAccount.Number.ToString() == bankAccountDto.Number);
        }

        [Fact]
        public void MapDto_MappsNull()
        {
            var bankAccountDto = mapperService.MapDto(null);

            Assert.Null(bankAccountDto);
        }

        [Fact]
        public async Task MapDtoAsync_MappsNull()
        {
            var bankAccountDto = await mapperService.MapDtoAsync(null);

            Assert.Null(bankAccountDto);
        }
        #endregion

        #region MapDtos
        [Fact]
        public void MapDtos_MappsEntitiesToDTOs()
        {
            var bankAccounts = testDataUtility.GetTestBankAccounts();
            var bankAccountDtos = mapperService.MapDtos(bankAccounts);
            var firstBankAccount = bankAccounts.FirstOrDefault();
            var firstBankAccountDto = bankAccountDtos.FirstOrDefault();

            Assert.NotNull(bankAccountDtos);
            Assert.NotEmpty(bankAccountDtos);
            Assert.True(firstBankAccount?.Id == firstBankAccountDto?.Id && firstBankAccount?.Number.ToString() == firstBankAccountDto?.Number);
        }

        [Fact]
        public async Task MapDtosAsync_MappsEntitiesToDTOs()
        {
            var bankAccounts = testDataUtility.GetTestBankAccounts();
            var bankAccountDtos = await mapperService.MapDtosAsync(bankAccounts);
            var firstBankAccount = bankAccounts.FirstOrDefault();
            var firstBankAccountDto = bankAccountDtos.FirstOrDefault();

            Assert.NotNull(bankAccountDtos);
            Assert.NotEmpty(bankAccountDtos);
            Assert.True(firstBankAccount?.Id == firstBankAccountDto?.Id && firstBankAccount?.Number.ToString() == firstBankAccountDto?.Number);
        }

        [Fact]
        public void MapDtos_MappsEmptyFromNull()
        {
            var bankAccountDtos = mapperService.MapDtos(null);

            Assert.NotNull(bankAccountDtos);
            Assert.Empty(bankAccountDtos);
        }

        [Fact]
        public async Task MapDtosAsync_MappsEmptyFromNull()
        {
            var bankAccountDtos = await mapperService.MapDtosAsync(null);

            Assert.NotNull(bankAccountDtos);
            Assert.Empty(bankAccountDtos);
        }

        [Fact]
        public void MapDtos_MappsEmptyFromEmpty()
        {
            var bankAccountDtos = mapperService.MapDtos(new List<BankAccount>());

            Assert.NotNull(bankAccountDtos);
            Assert.Empty(bankAccountDtos);
        }

        [Fact]
        public async Task MapDtosAsync_MappsEmptyFromEmpty()
        {
            var bankAccountDtos = await mapperService.MapDtosAsync(new List<BankAccount>());

            Assert.NotNull(bankAccountDtos);
            Assert.Empty(bankAccountDtos);
        }
        #endregion

        #region MapEntity
        [Fact]
        public void MapEntity_MappsEntityFromDTO()
        {
            var bankAccountDto = testDataUtility.GetNewTestBankAccountDto();
            var bankAccount = mapperService.MapEntity(bankAccountDto);

            Assert.NotNull(bankAccount);
            Assert.True(bankAccount.Id == bankAccountDto.Id && bankAccount.Number.ToString() == bankAccountDto.Number);
        }

        [Fact]
        public async Task MapEntityAsync_MappsEntityFromDTO()
        {
            var bankAccountDto = testDataUtility.GetNewTestBankAccountDto();
            var bankAccount = await mapperService.MapEntityAsync(bankAccountDto);

            Assert.NotNull(bankAccount);
            Assert.True(bankAccount.Id == bankAccountDto.Id && bankAccount.Number.ToString() == bankAccountDto.Number);
        }

        [Fact]
        public void MapEntity_MappsNull()
        {
            var bankAccount = mapperService.MapEntity(null);

            Assert.Null(bankAccount);
        }

        [Fact]
        public async Task MapEntityAsync_MappsNull()
        {
            var bankAccount = await mapperService.MapEntityAsync(null);

            Assert.Null(bankAccount);
        }
        #endregion

        #region MapEntities
        [Fact]
        public void MapEntities_MappsEntitiesFromDTOs()
        {
            var bankAccountDtos = testDataUtility.GetTestBankAccountDTOs();
            var bankAccounts = mapperService.MapEntities(bankAccountDtos);
            var firstBankAccount = bankAccounts.FirstOrDefault();
            var firstBankAccountDto = bankAccountDtos.FirstOrDefault();

            Assert.NotNull(bankAccountDtos);
            Assert.NotEmpty(bankAccountDtos);
            Assert.True(firstBankAccount?.Id == firstBankAccountDto?.Id && firstBankAccount?.Number.ToString() == firstBankAccountDto?.Number);
        }

        [Fact]
        public async Task MapEntitiesAsync_MappsEntitiesFromDTOs()
        {
            var bankAccountDtos = testDataUtility.GetTestBankAccountDTOs();
            var bankAccounts = await mapperService.MapEntitiesAsync(bankAccountDtos);
            var firstBankAccount = bankAccounts.FirstOrDefault();
            var firstBankAccountDto = bankAccountDtos.FirstOrDefault();

            Assert.NotNull(bankAccountDtos);
            Assert.NotEmpty(bankAccountDtos);
            Assert.True(firstBankAccount?.Id == firstBankAccountDto?.Id && firstBankAccount?.Number.ToString() == firstBankAccountDto?.Number);
        }

        [Fact]
        public void MapEntities_MappsEmptyFromNull()
        {
            var bankAccounts = mapperService.MapEntities(null);

            Assert.NotNull(bankAccounts);
            Assert.Empty(bankAccounts);
        }

        [Fact]
        public async Task MapEntitiesAsync_MappsEmptyFromNull()
        {
            var bankAccounts = await mapperService.MapEntitiesAsync(null);

            Assert.NotNull(bankAccounts);
            Assert.Empty(bankAccounts);
        }

        [Fact]
        public void MapEntities_MappsEmptyFromEmpty()
        {
            var bankAccounts = mapperService.MapEntities(new List<BankAccountDTO>());

            Assert.NotNull(bankAccounts);
            Assert.Empty(bankAccounts);
        }

        [Fact]
        public async Task MapEntitiesAsync_MappsEmptyFromEmpty()
        {
            var bankAccounts = await mapperService.MapEntitiesAsync(new List<BankAccountDTO>());

            Assert.NotNull(bankAccounts);
            Assert.Empty(bankAccounts);
        }
        #endregion
    }
}
