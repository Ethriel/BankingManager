using FluentValidation;

namespace BankingManager.Services.Validation
{
    public class AccountNumberValidator : AbstractValidator<string>
    {
        public AccountNumberValidator()
        {
            RuleFor(prop => prop)
                .MustAsync(async (value, cancellation) => await Task.FromResult(Guid.TryParse(value, out Guid accountNumber)))
                .WithMessage("Account number is not valid!");
        }
    }
}
