using BankingManager.Database.Model;

namespace BankingManager.Services
{
    public class UtilityService : IUtilityService
    {
        public IEnumerable<string> GetCurrencies()
        {
            return Enum.GetNames(typeof(Currency));
        }

        public async Task<IEnumerable<string>> GetCurrenciesAsync()
        {
            return await Task.FromResult(GetCurrencies());
        }
    }
}
