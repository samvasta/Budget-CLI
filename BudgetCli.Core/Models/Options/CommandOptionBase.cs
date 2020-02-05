using System.Text.RegularExpressions;
using System;
using BudgetCli.Core.Enums;
using BudgetCli.Data.Enums;

namespace BudgetCli.Core.Models.Options
{
    public abstract class CommandOptionBase<T>
    {
        public virtual CommandOptionKind OptionKind { get; }
        public virtual bool IsDataValid { get; protected set; }
        protected virtual T Data { get; set; }

        protected CommandOptionBase(CommandOptionKind optionKind)
        {
            OptionKind = optionKind;
            IsDataValid = false;
        }

        protected CommandOptionBase(CommandOptionKind optionKind, string rawText)
        {
            OptionKind = optionKind;
            SetData(rawText);
        }

        protected CommandOptionBase(CommandOptionKind optionKind, T data)
        {
            OptionKind = optionKind;
            //Assume data is valid if it is set directly
            IsDataValid = true;
            Data = data;
        }

        public bool SetData(string rawText)
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
            return IsDataValid;
        }

        public bool SetData(T newData)
        {
            Data = newData;
            IsDataValid = true;
            return true;
        }

        public abstract bool TryParseData(string rawText, out T data);

        /// <summary>
        /// Returns Data if available, otherwise it returns the default value provided
        /// </summary>
        public virtual T GetValue(T defaultValue)
        {
            if(IsDataValid)
            {
                return Data;
            }
            return defaultValue;
        }

        public virtual object GetValue(object defaultValue)
        {
            if(IsDataValid)
            {
                return Data;
            }
            return defaultValue;
        }
    }
}