using BankingManager.Services.Model.Actions;
using FluentValidation;

namespace BankingManager.Services.Validation
{
    public class BankAccountActionValidator : AbstractValidator<BankAccountAction>
    {
        public BankAccountActionValidator(AccountNumberValidator accountNumberValidator, AmmountValidator ammountValidator, CurrencyValidator currencyValidator)
        {
            RuleFor(action => action.AccountNumber).SetValidator(accountNumberValidator);

            RuleFor(action => action.Ammount).SetValidator(ammountValidator);

            RuleFor(action => action.Currency).SetValidator(currencyValidator);
        }
    }
}
