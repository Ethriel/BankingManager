using FluentValidation;

namespace BankingManager.Services.Validation
{
    public class AmmountValidator : AbstractValidator<double>
    {
        public AmmountValidator()
        {
            RuleFor(prop => prop)
                .MustAsync(async (value, cancellation) => await Task.FromResult(value >= 0))
                .WithMessage("Ammount can't be lower than zero!");
        }
    }
}
