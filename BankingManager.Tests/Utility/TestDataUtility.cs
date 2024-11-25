using BankingManager.Database.Model;
using BankingManager.Services.Model;

namespace BankingManager.Tests.Utility
{
    public class TestDataUtility
    {
        public List<BankAccount> TestBankAccounts { get; private set; }
        public List<BankAccountDTO> TestBankAccountDtos { get; private set; }
        public BankAccount TestBankAccount { get; private set; }
        public BankAccountDTO TestBankAccountDto { get; private set; }

        public TestDataUtility()
        {
            TestBankAccounts = GetTestBankAccounts();
            TestBankAccountDtos = GetTestBankAccountDTOs();
            TestBankAccount = GetNewTestBankAccount();
            TestBankAccountDto = GetNewTestBankAccountDto();
        }

        public List<BankAccount> GetTestBankAccounts()
        {
            // Reassign value each time this method is called to ensure that the DTO matches it
            TestBankAccounts = new List<BankAccount>()
            {
                new BankAccount
                {
                    Id = 1,
                    Balance = 100,
                    Currency = Currency.Dollar,
                    Number = Guid.NewGuid()
                },
                new BankAccount
                {
                    Id = 2,
                    Balance = 200,
                    Currency = Currency.Euro,
                    Number = Guid.NewGuid()
                },
                new BankAccount
                {
                    Id = 3,
                    Balance = 300,
                    Currency = Currency.Dollar,
                    Number = Guid.NewGuid()
                }
            };

            return TestBankAccounts;
        }

        public List<BankAccountDTO> GetTestBankAccountDTOs()
        {
            // Assign new value each time this method is called if it was not setted up yet to ensure that the DTO matches it
            TestBankAccounts ??= GetTestBankAccounts();
            TestBankAccountDtos = TestBankAccounts.Select(x => new BankAccountDTO
            {
                Id = x.Id,
                Balance = x.Balance,
                Currency = Enum.GetName(typeof(Currency), x.Currency),
                Number = x.Number.ToString()
            }).ToList();

            return TestBankAccountDtos;
        }

        public BankAccount GetNewTestBankAccount(int id = 4, double balance = 100, Currency currency = Currency.Dollar)
        {
            // Reassign value each time this method is called to ensure that the DTO matches it
            TestBankAccount = new BankAccount
            {
                Id = id,
                Balance = balance,
                Currency = currency,
                Number = Guid.NewGuid()
            };

            return TestBankAccount;
        }

        public BankAccountDTO GetNewTestBankAccountDto()
        {
            // Assign new value each time this method is called if it was not setted up yet to ensure that the DTO matches it
            TestBankAccount ??= GetNewTestBankAccount();
            return new BankAccountDTO
            {
                Id = TestBankAccount.Id,
                Balance = TestBankAccount.Balance,
                Currency = Enum.GetName(typeof(Currency), TestBankAccount.Currency),
                Number = TestBankAccount.Number.ToString()
            };
        }

        public BankAccountDTO GetNewTestBankAccountDto(int id = 4, double balance = 100, string currency = "Dollar")
        {
            return new BankAccountDTO
            {
                Id = id,
                Balance = balance,
                Currency = currency,
                Number = Guid.NewGuid().ToString()
            };
        }
    }
}
