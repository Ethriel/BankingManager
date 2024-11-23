namespace BankingManager.Services.Utility.ErrorMessages
{
    public class ErrorMessages
    {
        private readonly static string IdentifierPlaceholder = "Identfier: |{0}|";

        public readonly static string AccountIdentifierError = "Identifier is not valid!";
        public readonly static string AddErrorBase = "Could not add a new banking account";
        /// <summary>
        /// Use string.Format() and pass an identifier as parameter: either account number or id
        /// </summary>
        public readonly static string AddErrorLogger = $"{AddErrorBase}. {IdentifierPlaceholder}";
        public readonly static string BalanceError = "Balance error: balance: |{0}|, transfer ammount: |{1}|";
        /// <summary>
        /// Use string.Format() and pass an expected and received currencies as parameters
        /// </summary>
        public readonly static string CurrencyMissmatchError = "Invalid account currency. Expected: |{0}|, received: |{1}|";
        public readonly static string DeleteErrorBase = "Could not delete a banking account";
        /// <summary>
        /// Use string.Format() and pass an an identifier as parameter: either account number or id
        /// </summary>
        public readonly static string DoesNotExist = $"{DeleteErrorBase}. Account does not exist! {IdentifierPlaceholder}";
        /// <summary>
        /// Use string.Format() and pass an identifier as parameter: either account number or id
        /// </summary>
        public readonly static string ExistingAccountError = "Account |{0}| already exists!";
        /// <summary>
        /// Use string.Format() and pass an identifier as parameter: either account number or id
        /// </summary>
        public readonly static string InvalidAccountError = "Account |{0}|. Please, see error details";
        public readonly static string MappingError = $"Mapping result is null";
        /// <summary>
        /// Base transfer error message. Use string.Format(), where: |0| - from account number and |1| - to account number
        /// </summary>
        public readonly static string TransferErrorBase = "Could not transfer money from |{0}| to |{1}|";
        /// <summary>
        /// This error message uses base transfer error message.
        /// Also use string.Format(), where additional parameters are: |2| - balance of |0| and |3| - transfer ammount
        /// </summary>
        public readonly static string TransferBalanceError = $"{TransferErrorBase}. |{{0}}| balance: |{{2}}|, transfer ammount: |{{3}}|";
        public readonly static string UpdateErrorBase = "Could not update an account |{0}|";
    }
}
