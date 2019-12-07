using System;
using System.Runtime.Serialization;

namespace BudgetCliCore.Exceptions
{
    public class BuilderException : Exception
    {
        public BuilderException(Type builderClass, Type constructedClass)
            : this(builderClass, constructedClass, "Invalid builder configuration")
        {
        }
        public BuilderException(Type builderClass, Type constructedClass, string message)
            : base($"Failed to build object of type {constructedClass.FullName} using builder of type {builderClass.FullName}. {message}")
        {
        }
    }
}