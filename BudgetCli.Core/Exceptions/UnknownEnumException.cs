using System;
using System.Runtime.Serialization;

namespace BudgetCli.Core.Exceptions
{
    public class UnknownEnumException<T> : Exception where T : Enum
    {
        public UnknownEnumException(T enumValue) : this(enumValue, String.Empty)
        {
        }
        
        public UnknownEnumException(T enumValue, string additionalMessage) : base($"Unkown {typeof(T).Name} \"{Enum.GetName(typeof(T), enumValue)}\": {additionalMessage}")
        {
        }
    }
}