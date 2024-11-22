using System.ComponentModel.DataAnnotations;

namespace BankingManager.Database.Model
{
    public enum Currency
    {
        Dollar,
        Euro
    }

    public class BankAccount
    {
        public int Id { get; set; }
        [Required]
        public double Balance { get; set; }
        [Required]
        public Currency Currency { get; set; }
        public Guid Number { get; set; }

        public BankAccount() { }
        public BankAccount(double balance, Currency currency, Guid number)
        {
            Balance = balance;
            Currency = currency;
            Number = number;
        }
    }
}