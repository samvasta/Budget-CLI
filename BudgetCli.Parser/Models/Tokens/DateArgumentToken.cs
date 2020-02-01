using System;
using System.Net.Mime;
using BudgetCli.Parser.Enums;
using BudgetCli.Parser.Interfaces;
using BudgetCli.Parser.Util;
using BudgetCli.Util.Enums;
using BudgetCli.Util.Utilities;
using Humanizer;

namespace BudgetCli.Parser.Models.Tokens
{
    public class DateArgumentToken : ICommandToken
    {
        public TokenKind Kind { get { return TokenKind.Argument; } }

        public string ArgumentName { get; }
        public bool IsOptional { get; }

        public string Description { get; }

        public string[] PossibleValues { get; }

        public DateArgumentToken(string argumentName, bool isOptional)
        {
            ArgumentName = argumentName;
            Description = $"<{argumentName.Kebaberize()}>";
            IsOptional = isOptional;
            PossibleValues = new string[0];
        }


        public TokenMatchResult Matches(string[] inputTokens, int startIdx)
        {
            DateTime output;
            //Explicit date
            if(DateTime.TryParse(inputTokens[startIdx], out output))
            {
                return new TokenMatchResult(this, inputTokens[startIdx], MatchOutcome.Full, inputTokens[startIdx].Length, 1);
            }
            
            //Yesterday
            if(inputTokens[startIdx].Equals("yesterday", StringComparison.CurrentCultureIgnoreCase))
            {
                output = DateTime.Today.Subtract(new TimeSpan(1, 0, 0, 0));
                return new TokenMatchResult(this, inputTokens[startIdx], MatchOutcome.Full, inputTokens[startIdx].Length, 1);
            }
            if(startIdx == inputTokens.Length-1)
            {
                //End of tokens, cannot match anything else
                output = DateTime.Today;
                return new TokenMatchResult(this, String.Empty, MatchOutcome.None, 0, 0);
            }
            
            if(inputTokens[startIdx].Equals("last", StringComparison.CurrentCultureIgnoreCase))
            {
                //last <day-of-week>
                if(inputTokens.Length > startIdx+1)
                {
                    DayOfWeek dayOfWeek;
                    if(DateParser.TryParseDayOfWeek(inputTokens[startIdx+1], out dayOfWeek))
                    {
                        output = DateUtil.GetRelativeDateDayOfWeek(dayOfWeek);
                        string matchText = TokenUtils.GetMatchText(inputTokens, startIdx, 2);
                        return new TokenMatchResult(this, matchText, MatchOutcome.Full, matchText.Length, 2);
                    }
                }

                //last <month> <day-of-month>
                if(inputTokens.Length > startIdx+2)
                {
                    int month;
                    int day;
                    if(DateParser.TryParseMonth(inputTokens[startIdx+1], out month) &&
                       DateParser.TryParseDayOfMonth(inputTokens[startIdx+2], DateTime.Today.Year-1, month, out day))
                    {
                        output = new DateTime(DateTime.Today.Year-1, month, day);
                        string matchText = TokenUtils.GetMatchText(inputTokens, startIdx, 3);
                        return new TokenMatchResult(this, matchText, MatchOutcome.Full, matchText.Length, 3);
                    }
                }

                output = DateTime.Today;
                return new TokenMatchResult(this, String.Empty, MatchOutcome.None, 0, 0);
            }

            //<number> <time-unit> ago
            if(inputTokens.Length > startIdx+2 &&
               inputTokens[startIdx+2].Equals("ago", StringComparison.CurrentCultureIgnoreCase))
            {
                int number;
                TimeUnit unit;
                if(int.TryParse(inputTokens[startIdx], out number) &&
                   DateParser.TryParseTimeUnit(inputTokens[startIdx+1], out unit))
                {
                    output = DateUtil.GetRelativeDate(DateTime.Today, -number, unit);
                        string matchText = TokenUtils.GetMatchText(inputTokens, startIdx, 3);
                        return new TokenMatchResult(this, matchText, MatchOutcome.Full, matchText.Length, 3);
                }
            }


            output = DateTime.Today;
            return new TokenMatchResult(this, String.Empty, MatchOutcome.None, 0, 0);
        }
    }
}