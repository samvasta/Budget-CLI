using System;
using BudgetCli.Parser.Enums;
using BudgetCli.Parser.Models.Tokens;
using BudgetCli.Parser.Parsing;
using BudgetCli.Util.Enums;
using BudgetCli.Util.Utilities;

namespace BudgetCli.Parser.Util
{
    public static class DateMatchUtils
    {
        public static bool TryMatchDate(string dateStr, DateTime currentDate, out DateTime date)
        {
            string[] inputTokens = CommandParser.Tokenize(dateStr);
            int startIdx = 0;
            ArgumentToken token = new DateArgumentToken("arg", false);
            
            if(TryMatchDate(token, currentDate, inputTokens, startIdx, out TokenMatchResult result))
            {
                result.TryGetArgValue(token, out date);
                return true;
            }

            date = currentDate;
            return false;
        }

        public static bool TryMatchDate(string dateStr, out DateTime date)
        {
            string[] inputTokens = CommandParser.Tokenize(dateStr);
            int startIdx = 0;
            ArgumentToken token = new DateArgumentToken("arg", false);
            
            if(TryMatchDate(token, DateTime.Today, inputTokens, startIdx, out TokenMatchResult result))
            {
                result.TryGetArgValue(token, out date);
                return true;
            }

            date = DateTime.Today;
            return false;
        }

        public static bool TryMatchDate(ArgumentToken token, DateTime currentDate, string[] inputTokens, int startIdx, out TokenMatchResult result)
        {
            if(TryMatchExplicitDate(token, inputTokens, startIdx, out result))
            {
                return true;
            }
            else if(TryMatchYesterday(token, currentDate, inputTokens, startIdx, out result))
            {
                return true;
            }
            else if(startIdx == inputTokens.Length-1)
            {
                //End of tokens, cannot match anything else
                result = TokenMatchResult.None;
                return false;
            }
            else if(inputTokens[startIdx].Equals("last", StringComparison.CurrentCultureIgnoreCase))
            {
                if(TryMatchLastDayOfWeek(token, currentDate, inputTokens, startIdx, out result))
                {
                    return true;
                }
                else if(TryMatchLastDayOfMonth(token, currentDate, inputTokens, startIdx, out result))
                {
                    return true;
                }

                result = TokenMatchResult.None;
                return false;
            }
            else if(TryMatchRelativeDate(token, currentDate, inputTokens, startIdx, out result))
            {
                return true;
            }
            
            result = TokenMatchResult.None;
            return false;
        }
        
        public static bool TryMatchExplicitDate(ArgumentToken token, string[] inputTokens, int startIdx, out TokenMatchResult result)
        {
            DateTime output;
            //Explicit date
            if(DateTime.TryParse(inputTokens[startIdx], out output))
            {
                result = new TokenMatchResult(token, inputTokens[startIdx], inputTokens[startIdx], MatchOutcome.Full, inputTokens[startIdx].Length, 1);
                result.SetArgValue(token, output);
                return true;
            }

            result = TokenMatchResult.None;
            return false;
        }

        public static bool TryMatchYesterday(ArgumentToken token, DateTime currentDate, string[] inputTokens, int startIdx, out TokenMatchResult result)
        {
            //Yesterday
            if(inputTokens[startIdx].Equals("yesterday", StringComparison.CurrentCultureIgnoreCase))
            {
                DateTime output = currentDate.Subtract(new TimeSpan(1, 0, 0, 0));
                result = new TokenMatchResult(token, inputTokens[startIdx], inputTokens[startIdx], MatchOutcome.Full, inputTokens[startIdx].Length, 1);
                result.SetArgValue(token, output);
                return true;
            }

            result = TokenMatchResult.None;
            return false;
        }

        public static bool TryMatchLastDayOfWeek(ArgumentToken token, DateTime currentDate, string[] inputTokens, int startIdx, out TokenMatchResult result)
        {
            //last <day-of-week>
            if(inputTokens.Length > startIdx+1)
            {
                DayOfWeek dayOfWeek;
                if(DateParser.TryParseDayOfWeek(inputTokens[startIdx+1], out dayOfWeek))
                {
                    DateTime output = DateUtil.GetRelativeDateDayOfWeek(currentDate, dayOfWeek);
                    string matchText = TokenUtils.GetMatchText(inputTokens, startIdx, 2);
                    result = new TokenMatchResult(token, matchText, matchText, MatchOutcome.Full, matchText.Length, 2);
                    result.SetArgValue(token, output);
                    return true;
                }
            }

            result = TokenMatchResult.None;
            return false;
        }

        public static bool TryMatchLastDayOfMonth(ArgumentToken token, DateTime currentDate, string[] inputTokens, int startIdx, out TokenMatchResult result)
        {
            //last <month> <day-of-month>
            if(inputTokens.Length > startIdx+2)
            {
                int month;
                int day;
                if(DateParser.TryParseMonth(inputTokens[startIdx+1], out month) &&
                    DateParser.TryParseDayOfMonth(inputTokens[startIdx+2], currentDate.Year-1, month, out day))
                {
                    DateTime output = new DateTime(currentDate.Year-1, month, day);
                    string matchText = TokenUtils.GetMatchText(inputTokens, startIdx, 3);
                    result = new TokenMatchResult(token, matchText, matchText, MatchOutcome.Full, matchText.Length, 3);
                    result.SetArgValue(token, output);
                    return true;
                }
            }

            result = TokenMatchResult.None;
            return false;
        }

        public static bool TryMatchRelativeDate(ArgumentToken token, DateTime currentDate, string[] inputTokens, int startIdx, out TokenMatchResult result)
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
                    DateTime output = DateUtil.GetRelativeDate(currentDate, -number, unit);
                    string matchText = TokenUtils.GetMatchText(inputTokens, startIdx, 3);
                    result = new TokenMatchResult(token, matchText, matchText, MatchOutcome.Full, matchText.Length, 3);
                    result.SetArgValue(token, output);
                    return true;
                }
            }

            result = TokenMatchResult.None;
            return false;
        }
    }
}