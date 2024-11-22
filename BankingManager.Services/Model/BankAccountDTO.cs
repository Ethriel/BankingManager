namespace BankingManager.Services.Model
{
    public class BankAccountDTO
    {
        public int Id { get; set; }
        public double Balance { get; set; }
        public string Currency { get; set; }
        public string Number { get; set; }

        public BankAccountDTO() { }
        public BankAccountDTO(double balance, string currency, string number)
        {
            Balance = balance;
            Currency = currency;
            Number = number;
        }
    }
}
