using System;
using System.Runtime.Serialization;

namespace BudgetCli.Core.Exceptions
{
    public class BudgetCliParseException : Exception
    {
        public BudgetCliParseException()
        {
        }

        public BudgetCliParseException(string message) : base(message)
        {
        }

        public BudgetCliParseException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected BudgetCliParseException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}