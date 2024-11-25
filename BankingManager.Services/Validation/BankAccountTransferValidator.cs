using BankingManager.Services.Model.Actions;
using FluentValidation;

namespace BankingManager.Services.Validation
{
    public class BankAccountTransferValidator : AbstractValidator<TransferAction>
    {
        public BankAccountTransferValidator(AccountNumberValidator accountNumberValidator, AmmountValidator ammountValidator, CurrencyValidator currencyValidator)
        {
            RuleFor(transfer => transfer.Ammount).SetValidator(ammountValidator);
            RuleFor(transfer => transfer.Currency).SetValidator(currencyValidator);
            RuleFor(transfer => transfer.FromAccountNumber).SetValidator(accountNumberValidator);
            RuleFor(transfer => transfer.ToAccountNumber).SetValidator(accountNumberValidator);
        }
    }
}
