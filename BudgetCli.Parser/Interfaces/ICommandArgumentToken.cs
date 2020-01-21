using System;

namespace BudgetCli.Parser.Interfaces
{
    public interface ICommandArgumentToken : ICommandToken
    {
    }

    public interface ICommandArgumentToken<T> : ICommandArgumentToken
    {
        public delegate bool ValueParser(string text, out T value); 

        ValueParser Parser { get; }
    }

}