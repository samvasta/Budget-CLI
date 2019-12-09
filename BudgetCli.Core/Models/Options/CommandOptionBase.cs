using System.Text.RegularExpressions;
using System;
using BudgetCli.Core.Enums;

namespace BudgetCli.Core.Models.Options
{
    public abstract class CommandOptionBase
    {
        public bool IsDataValid { get; protected set; }
        public object Data { get; set; }
    }

    public abstract class CommandOptionBase<T> : CommandOptionBase
    {
        public new T Data { get; set; }

        public CommandOptionBase(string rawText)
        {
            T data;
            if(TryParseData(rawText, out data))
            {
                Data = data;
                IsDataValid = true;
            }
            else
            {
                IsDataValid = false;
            }
        }

        public CommandOptionBase(T data)
        {
            //Assume data is valid if it is set directly
            IsDataValid = true;
            Data = data;
        }

        public abstract bool TryParseData(string rawText, out T data);
    }
}