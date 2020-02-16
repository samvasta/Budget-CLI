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
    public class DateArgumentToken : ArgumentToken
    {
        public DateArgumentToken(string argumentName, bool isOptional)
            : base(argumentName, isOptional)
        {
        }

        private bool TryMatchExplicitDate(string[] inputTokens, int startIdx, out TokenMatchResult result)
        {
            DateTime output;
            //Explicit date
            if(DateTime.TryParse(inputTokens[startIdx], out output))
            {
                result = new TokenMatchResult(this, inputTokens[startIdx], inputTokens[startIdx], MatchOutcome.Full, inputTokens[startIdx].Length, 1);
                result.SetArgValue(this, output);
                return true;
            }

            result = TokenMatchResult.None;
            return false;
        }

        private bool TryMatchYesterday(string[] inputTokens, int startIdx, out TokenMatchResult result)
        {
            //Yesterday
            if(inputTokens[startIdx].Equals("yesterday", StringComparison.CurrentCultureIgnoreCase))
            {
                DateTime output = DateTime.Today.Subtract(new TimeSpan(1, 0, 0, 0));
                result = new TokenMatchResult(this, inputTokens[startIdx], inputTokens[startIdx], MatchOutcome.Full, inputTokens[startIdx].Length, 1);
                result.SetArgValue(this, output);
                return true;
            }

            result = TokenMatchResult.None;
            return false;
        }

        private bool TryMatchLastDayOfWeek(string[] inputTokens, int startIdx, out TokenMatchResult result)
        {
            //last <day-of-week>
            if(inputTokens.Length > startIdx+1)
            {
                DayOfWeek dayOfWeek;
                if(DateParser.TryParseDayOfWeek(inputTokens[startIdx+1], out dayOfWeek))
                {
                    DateTime output = DateUtil.GetRelativeDateDayOfWeek(dayOfWeek);
                    string matchText = TokenUtils.GetMatchText(inputTokens, startIdx, 2);
                    result = new TokenMatchResult(this, matchText, matchText, MatchOutcome.Full, matchText.Length, 2);
                    result.SetArgValue(this, output);
                    return true;
                }
            }

            result = TokenMatchResult.None;
            return false;
        }

        private bool TryMatchLastDayOfMonth(string[] inputTokens, int startIdx, out TokenMatchResult result)
        {
            //last <month> <day-of-month>
            if(inputTokens.Length > startIdx+2)
            {
                int month;
                int day;
                if(DateParser.TryParseMonth(inputTokens[startIdx+1], out month) &&
                    DateParser.TryParseDayOfMonth(inputTokens[startIdx+2], DateTime.Today.Year-1, month, out day))
                {
                    DateTime output = new DateTime(DateTime.Today.Year-1, month, day);
                    string matchText = TokenUtils.GetMatchText(inputTokens, startIdx, 3);
                    result = new TokenMatchResult(this, matchText, matchText, MatchOutcome.Full, matchText.Length, 3);
                    result.SetArgValue(this, output);
                    return true;
                }
            }

            result = TokenMatchResult.None;
            return false;
        }

        private bool TryMatchRelativeDate(string[] inputTokens, int startIdx, out TokenMatchResult result)
        {
            //<number> <time-unit> ago
            if(inputTokens.Length > startIdx+2 &&
               inputTokens[startIdx+2].Equals("ago", StringComparison.CurrentCultureIgnoreCase))
            {
                int number;
                TimeUnit unit;
                if(int.TryParse(inputTokens[startIdx], out number) &&
                   DateParser.TryParseTimeUnit(inputTokens[startIdx+1], out unit))
                {
                    DateTime output = DateUtil.GetRelativeDate(DateTime.Today, -number, unit);
                    string matchText = TokenUtils.GetMatchText(inputTokens, startIdx, 3);
                    result = new TokenMatchResult(this, matchText, matchText, MatchOutcome.Full, matchText.Length, 3);
                    result.SetArgValue(this, output);
                    return true;
                }
            }

            result = TokenMatchResult.None;
            return false;
        }


        public override TokenMatchResult Matches(string[] inputTokens, int startIdx)
        {
            TokenMatchResult result;
            if(TryMatchExplicitDate(inputTokens, startIdx, out result))
            {
                return result;
            }
            else if(TryMatchYesterday(inputTokens, startIdx, out result))
            {
                return result;
            }
            else if(startIdx == inputTokens.Length-1)
            {
                //End of tokens, cannot match anything else
                return TokenMatchResult.None;
            }
            else if(inputTokens[startIdx].Equals("last", StringComparison.CurrentCultureIgnoreCase))
            {
                if(TryMatchLastDayOfWeek(inputTokens, startIdx, out result))
                {
                    return result;
                }
                else if(TryMatchLastDayOfMonth(inputTokens, startIdx, out result))
                {
                    return result;
                }

                return TokenMatchResult.None;
            }
            else if(TryMatchRelativeDate(inputTokens, startIdx, out result))
            {
                return result;
            }
            
            return TokenMatchResult.None;
        }
    }
}