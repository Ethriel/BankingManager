using AutoMapper;
using BankingManager.Database;
using BankingManager.Database.Model;
using BankingManager.Services;
using BankingManager.Services.Model;
using BankingManager.Services.Model.Actions;
using BankingManager.Services.Model.Mapper;
using BankingManager.Services.Validation;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;

namespace BankingManager.Tests.Utility
{
    public class MockUtility
    {
        public TestDataUtility TestDataUtility { get; set; }
        public IEntityService<BankAccount> EntityService { get; set; }
        public IEntityExtendedService<BankAccount> EntityExtendedService { get; set; }
        public IMapperService<BankAccount, BankAccountDTO> MapperService { get; set; }
        public MockUtility()
        {
            TestDataUtility = new TestDataUtility();
        }
        public IEntityService<BankAccount> MockBankAccountEntityService()
        {
            var dbContext = GetMockedDbContext(TestDataUtility.GetTestBankAccounts(), context => context.BankAccounts);
            return BuildBankAccountEntityService(dbContext);
        }

        public IEntityService<BankAccount> MockBankAccountEntityServiceEmptyDb()
        {
            var dbContext = GetMockedDbContext(new List<BankAccount>(), context => context.BankAccounts);
            return BuildBankAccountEntityService(dbContext);
        }

        public IEntityExtendedService<BankAccount> MockBankEntityExtendedService()
        {
            var dbContext = GetMockedDbContext(TestDataUtility.GetTestBankAccounts(), context => context.BankAccounts);
            return BuildBankExtendedService(dbContext);
        }

        public IEntityExtendedService<BankAccount> MockBankEntityExtendedServiceEmptyDb()
        {
            var dbContext = GetMockedDbContext(new List<BankAccount>(), context => context.BankAccounts);
            return BuildBankExtendedService(dbContext);
        }

        public IMapperService<BankAccount, BankAccountDTO> MockMapperService()
        {
            var serviceProvider = GetServiceProvider();
            var mapper = serviceProvider.GetRequiredService<IMapper>();
            MapperService = new Mock<MapperService<BankAccount, BankAccountDTO>>(mapper).Object;

            return MapperService;
        }

        public IBankAccountService MockBankAccountService()
        {
            var dbContext = GetMockedDbContext(TestDataUtility.GetTestBankAccounts(), context => context.BankAccounts);

            return BuildBankAccountService(dbContext);
        }

        public IBankAccountService MockBankAccountServiceEmptyDb()
        {
            var dbContext = GetMockedDbContext(new List<BankAccount>(), context => context.BankAccounts);

            return BuildBankAccountService(dbContext);
        }

        private DbContext GetMockedDbContext(List<BankAccount> inputData,
            Expression<Func<BankingManagerDbContext, DbSet<BankAccount>>> dbSetSelectionExpression)
        {
            var inputDataQueryable = inputData.AsQueryable();
            var dbSetMock = new Mock<DbSet<BankAccount>>();
            var dbMockContext = new Mock<BankingManagerDbContext>();

            dbSetMock.As<IQueryable<BankAccount>>().Setup(s => s.Provider).Returns(inputDataQueryable.Provider);
            dbSetMock.As<IQueryable<BankAccount>>().Setup(s => s.Expression).Returns(inputDataQueryable.Expression);
            dbSetMock.As<IQueryable<BankAccount>>().Setup(s => s.ElementType).Returns(inputDataQueryable.ElementType);
            dbSetMock.As<IQueryable<BankAccount>>().Setup(s => s.GetEnumerator()).Returns(() => inputDataQueryable.GetEnumerator());

            dbSetMock.Setup(set => set.Find(It.IsAny<object[]>())).Returns<object[]>(id =>
                inputData.FirstOrDefault(dog => dog.Id == (int)id.First()));

            dbSetMock.Setup(set => set.AsQueryable()).Returns(inputDataQueryable);
            dbSetMock.Setup(set => set.Add(It.IsAny<BankAccount>())).Callback<BankAccount>(inputData.Add);
            dbSetMock.Setup(set => set.AddRange(It.IsAny<IEnumerable<BankAccount>>())).Callback<IEnumerable<BankAccount>>(inputData.AddRange);
            dbSetMock.Setup(set => set.Remove(It.IsAny<BankAccount>())).Callback<BankAccount>(record => inputData.Remove(record));

            dbSetMock.Setup(set => set.RemoveRange(It.IsAny<IEnumerable<BankAccount>>())).Callback<IEnumerable<BankAccount>>(data =>
            {
                foreach (var record in data) { inputData.Remove(record); }
            });

            dbMockContext.Setup(dbSetSelectionExpression).Returns(dbSetMock.Object);
            dbMockContext.Setup(d => d.Set<BankAccount>()).Returns(dbSetMock.Object);
            dbMockContext.Setup(d => d.Model).Returns(MockContextModel().Object);
            dbMockContext.Setup(d => d.SaveChanges()).Returns(1);

            return dbMockContext.Object;
        }

        private static Mock<IModel> MockContextModel()
        {
            var mockModel = new Mock<IModel>();

            var mockDogEntityType = new Mock<IEntityType>();
            mockDogEntityType.Setup(m => m.ClrType).Returns(typeof(BankAccount));

            mockModel.Setup(m => m.GetEntityTypes()).Returns(new List<IEntityType> { mockDogEntityType.Object });

            return mockModel;
        }

        private IEntityService<BankAccount> BuildBankAccountEntityService(DbContext dbContext)
        {
            EntityService = new Mock<EntityService<BankAccount>>(dbContext).Object;

            return EntityService;
        }

        private IEntityExtendedService<BankAccount> BuildBankExtendedService(DbContext dbContext)
        {
            EntityExtendedService = new Mock<EntityExtendedService<BankAccount>>(dbContext).Object;

            return EntityExtendedService;
        }

        private IBankAccountService BuildBankAccountService(DbContext dbContext)
        {
            var entityExtendedService = BuildBankExtendedService(dbContext);

            var mapperService = MockMapperService();
            var serviceProvider = GetServiceProvider();
            var logger = serviceProvider.GetRequiredService<ILogger<IBankAccountService>>();

            return new Mock<BankAccountService>(entityExtendedService, mapperService, logger).Object;
        }

        private ServiceProvider GetServiceProvider()
        {
            var inMemorySettings = new Dictionary<string, string>
            {
                {"Logging:LogLevel:Default", "Information"},
                {"Logging:LogLevel:Microsoft.AspNetCore", "Warning"},
                {"AllowedHosts", "*"},
                {"ConnectionStrings:Default", "connectionString"},
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            var serviceCollection = new ServiceCollection();

            serviceCollection.AddAutoMapper(typeof(BankAccountMapperProfile))
                             .AddLogging()
                             .AddScoped<AccountNumberValidator>()
                             .AddScoped<AmmountValidator>()
                             .AddScoped<CurrencyValidator>()
                             .AddSingleton<IConfiguration>(provider => configuration)
                             .AddScoped<IValidator<BankAccountAction>, BankAccountActionValidator>()
                             .AddScoped<IValidator<TransferAction>, BankAccountTransferValidator>()
                             .AddScoped<IValidator<BankAccountDTO>, BankAccountValidator>();

            return serviceCollection.BuildServiceProvider();
        }
    }
}