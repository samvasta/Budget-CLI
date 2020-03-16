using System;
using System.Collections.Generic;
using System.Linq;
using BudgetCli.Parser.Interfaces;
using Humanizer;

namespace BudgetCli.Parser.Models.Tokens
{
    public class EnumArgumentToken<T> : ArgumentToken<T> where T : Enum
    {
        protected EnumArgumentToken(string argumentName, bool isOptional)
            : base(argumentName, isOptional, TryParse, GetPossibleValues<T>())
        {
            //Nothing to do
        }

        private static string[] GetPossibleValues<E>() where E : Enum
        {
            return ((E[])Enum.GetValues(typeof(E))).Select(x => x.Humanize().Transform(To.TitleCase)).ToArray();
        }

        public static bool TryParse(string enumStr, out T enumValue)
        {
            try
            {
                enumValue = (T)enumStr.DehumanizeTo(typeof(T));
            }
            catch (Exception)
            {
                enumValue = ((T[])Enum.GetValues(typeof(T)))[0];
                return false;
            }

            return true;
        }
        
        public new class Builder
        {
            protected string _name;
            protected bool _isOptional;

            public Builder Name(string name)
            {
                _name = name;
                return this;
            }

            public Builder IsOptional(bool isOptional)
            {
                _isOptional = isOptional;
                return this;
            }

            public EnumArgumentToken<T> Build()
            {
                return new EnumArgumentToken<T>(_name, _isOptional);
            }
        }
    }
}