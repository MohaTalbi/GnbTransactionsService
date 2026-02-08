namespace GnbTransactionsService.Domain.Exceptions
{
    public class CurrencyConversionException : Exception
    {
        /// <summary>
        ///  This exception is thrown when a currency conversion fails.
        /// </summary>
        /// <param name="message"></param>
        public CurrencyConversionException(string message)
        : base(message)
        {}
    }

    public class DataConsistencyException : Exception
    {
        /// <summary>
        /// this exception is thrown when there is an inconsistency in the data.
        /// </summary>
        /// <param name="message"></param>
        public DataConsistencyException(string message) : base(message) 
        { }
    }
}
