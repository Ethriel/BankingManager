namespace BankingManager.Services.Model.Actions
{
    public class TransferAction
    {
        public BankAccountAction FromAccount { get; set; }
        public BankAccountAction ToAccount { get; set; }
    }
}
