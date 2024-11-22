using FluentValidation;

namespace BankingManager.Services.Validation
{
    public class AccountNumberValidator : AbstractValidator<string>
    {
        public AccountNumberValidator()
        {
            RuleFor(prop => prop)
                .Must(value => Guid.TryParse(value, out Guid accountNumber))
                .WithMessage("Account number is not valid!");
        }
    }
}
