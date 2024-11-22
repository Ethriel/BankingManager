using FluentValidation;

namespace BankingManager.Services.Validation
{
    public class AmmountValidator : AbstractValidator<double>
    {
        public AmmountValidator()
        {
            RuleFor(prop => prop)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Ammount can't be lower than zero!");
        }
    }
}
