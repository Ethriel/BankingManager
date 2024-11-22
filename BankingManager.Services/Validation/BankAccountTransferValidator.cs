using BankingManager.Services.Model.Actions;
using FluentValidation;

namespace BankingManager.Services.Validation
{
    public class BankAccountTransferValidator : AbstractValidator<TransferAction>
    {
        public BankAccountTransferValidator(BankAccountActionValidator bankAccountActionValidator)
        {
            RuleFor(transfer => transfer.FromAccount).NotNull()
                                                     .SetValidator(bankAccountActionValidator);
            RuleFor(transfer => transfer.ToAccount).NotNull()
                                                   .SetValidator(bankAccountActionValidator);
        }
    }
}
