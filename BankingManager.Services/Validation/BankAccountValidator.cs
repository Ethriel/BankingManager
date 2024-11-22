using BankingManager.Services.Model;
using FluentValidation;

namespace BankingManager.Services.Validation
{
    public class BankAccountValidator : AbstractValidator<BankAccountDTO>
    {
        public BankAccountValidator(AccountNumberValidator accountNumberValidator, AmmountValidator ammountValidator, CurrencyValidator currencyValidator)
        {
            RuleFor(account => account.Balance).SetValidator(ammountValidator);

            RuleFor(account => account.Currency).SetValidator(currencyValidator);

            RuleFor(account => account.Number).SetValidator(accountNumberValidator);
        }
    }
}
