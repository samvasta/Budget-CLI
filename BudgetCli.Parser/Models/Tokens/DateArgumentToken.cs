using System;
using System.Collections.Generic;
using System.Net.Mime;
using BudgetCli.Parser.Enums;
using BudgetCli.Parser.Interfaces;
using BudgetCli.Parser.Util;
using BudgetCli.Util.Enums;
using BudgetCli.Util.Utilities;
using Humanizer;

namespace BudgetCli.Parser.Models.Tokens
{
    public class DateArgumentToken : ArgumentToken
    {
        public DateTime CurrentDate { get; }
        public DateArgumentToken(string argumentName, bool isOptional)
            : this(argumentName, isOptional, DateTime.Today)
        {
            //Intentionally blank
        }

        public DateArgumentToken(string argumentName, bool isOptional, DateTime currentDate)
            : base(argumentName, isOptional, new string[0])
        {
            CurrentDate = currentDate;
        }

        public override TokenMatchResult Matches(string[] inputTokens, int startIdx)
        {
            DateMatchUtils.TryMatchDate(this, CurrentDate, inputTokens, startIdx, out TokenMatchResult result);
            return result;
        }
    }
}