using System;

namespace Stock.API.Exceptions
{
    public class StockTradingDomainException : Exception
    {
        public StockTradingDomainException()
        { }

        public StockTradingDomainException(string message)
            : base(message)
        { }

        public StockTradingDomainException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
