namespace BankingManager.Services
{
    public interface IUtilityService
    {
        IEnumerable<string> GetCurrencies();
        Task<IEnumerable<string>> GetCurrenciesAsync();
    }
}
