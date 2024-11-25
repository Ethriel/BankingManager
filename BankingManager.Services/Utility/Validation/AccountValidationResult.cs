using BankingManager.Database.Model;

namespace BankingManager.Services.Utility.Validation
{
    public class AccountValidationResult
    {
        public bool IsValid { get; set; }
        public string Error { get; set; }
        public string[] Errors { get; set; }
        public BankAccount BankAccount { get; set; }
    }
}
