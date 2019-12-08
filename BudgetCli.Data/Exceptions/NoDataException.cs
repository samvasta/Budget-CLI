using System;
using System.Runtime.Serialization;
using BudgetCli.Data.Models;

namespace BudgetCli.Data.Exceptions
{
    public class NoDataException<T> : Exception where T : IDbModel
    {
        public NoDataException()
        {
        }

        public NoDataException(string message) : base(message)
        {
        }

        public NoDataException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NoDataException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}