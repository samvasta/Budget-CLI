using System.Text.RegularExpressions;
using System;
using BudgetCli.Core.Enums;
using BudgetCli.Data.Enums;

namespace BudgetCli.Core.Models.Options
{
    public abstract class CommandOptionBase
    {
        public CommandOptionKind OptionKind { get; }
        public virtual bool IsDataValid { get; protected set; }
        protected object Data { get; set; }

        protected CommandOptionBase(CommandOptionKind optionKind)
        {
            OptionKind = optionKind;
        }

        /// <summary>
        /// Returns Data if available, otherwise it returns the default value provided
        /// </summary>
        public virtual object GetValue(object defaultValue)
        {
            if(IsDataValid)
            {
                return Data;
            }
            return defaultValue;
        }

        public abstract bool SetData(string rawText);
    }

    public abstract class CommandOptionBase<T> : CommandOptionBase
    {
        protected new T Data { get; set; }

        public CommandOptionBase(CommandOptionKind optionKind) : base(optionKind)
        {
            IsDataValid = false;
        }

        public CommandOptionBase(CommandOptionKind optionKind, string rawText) : base(optionKind)
        {
            SetData(rawText);
        }

        public CommandOptionBase(CommandOptionKind optionKind, T data) : base(optionKind)
        {
            //Assume data is valid if it is set directly
            IsDataValid = true;
            Data = data;
        }

        public override bool SetData(string rawText)
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
    }
}