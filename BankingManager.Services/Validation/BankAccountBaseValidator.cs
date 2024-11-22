using BankingManager.Database.Model;
using FluentValidation;

namespace BankingManager.Services.Validation
{
    public class BankAccountBaseValidator<T> : AbstractValidator<T>
    {
        protected Action<string, ValidationContext<T>> validateCurrencyRule;
        protected Action<string, ValidationContext<T>> validateAccountNumberRule;

        public BankAccountBaseValidator()
        {
            validateCurrencyRule = (value, context) =>
            {
                var parseResult = Enum.TryParse(value, out Currency currency);

                if (!parseResult)
                {
                    var currencies = Enum.GetNames(typeof(Currency));
                    context.AddFailure($"Currency type is not valid: |{value}|! Expected: |{string.Join(" ", currencies)}|");
                }
            };

            validateAccountNumberRule = (value, context) =>
            {
                var parseResult = Guid.TryParse(value, out Guid number);

                if (!parseResult)
                {
                    context.AddFailure($"Account number is not valid!");
                }
            };
        }
    }
}
