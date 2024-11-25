using BankingManager.Database.Model;
using FluentValidation;

namespace BankingManager.Services.Validation
{
    public class CurrencyValidator : AbstractValidator<string>
    {
        public CurrencyValidator()
        {
            var currencies = Enum.GetNames(typeof(Currency));
            RuleFor(prop => prop)
                .MustAsync(async (value, cancellation) => await Task.FromResult(Enum.TryParse(value, out Currency currency)))
                .WithMessage($"Currency type is not valid! Expected: |{string.Join(", ", currencies)}|");
        }
    }
}
