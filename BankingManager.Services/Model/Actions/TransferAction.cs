namespace BankingManager.Services.Model.Actions
{
    public class TransferAction
    {
        public string FromAccountNumber { get; set; }
        public string ToAccountNumber { get; set; }
        public string Currency { get; set; }
        public double Ammount { get; set; }
    }
}
