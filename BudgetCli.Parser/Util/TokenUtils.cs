using System;
using System.Linq;
using System.Text;
using BudgetCli.Parser.Models.Tokens;
using BudgetCli.Util.Models;
using BudgetCli.Util.Utilities;

namespace BudgetCli.Parser.Util
{
    public static class TokenUtils
    {
        public static string GetMatchText(string[] tokens, int start, int length)
        {
            return String.Join(" ", tokens.Skip(start).Take(length));
        }

        private static bool TryParseString(string input, out string output)
        {
            output = input;
            return true;
        }

        internal static string ToDisplayName(this Name name)
        {
            if(name.Alternates.Any())
            {
                return $"({name.Preferred}|{string.Join("|", name.Alternates)})";
            }
            else
            {
                return name.Preferred;
            }
        }

        public static ArgumentToken<int> BuildArgInt(string name, bool isOptional = false)
        {
            return new ArgumentToken<int>.Builder().Name(name)
                                                   .IsOptional(isOptional)
                                                   .Parser(Int32.TryParse)
                                                   .Build();
        }
        
        public static ArgumentToken<string> BuildArgString(string name, bool isOptional = false)
        {
            return new ArgumentToken<string>.Builder().Name(name)
                                                      .IsOptional(isOptional)
                                                      .Parser(TryParseString)
                                                      .Build();
        }
        
        public static ArgumentToken<Money> BuildArgMoney(string name, bool isOptional = false)
        {
            return new ArgumentToken<Money>.Builder().Name(name)
                                                     .IsOptional(isOptional)
                                                     .Parser(Money.TryParse)
                                                     .Build();
        }
        
        public static EnumArgumentToken<TEnum> BuildArgEnum<TEnum>(string name, bool isOptional = false) where TEnum : Enum
        {
            return new EnumArgumentToken<TEnum>.Builder().Name(name)
                                                         .IsOptional(isOptional)
                                                         .Build();
        }
        
        public static ArgumentToken<float> BuildArgFloat(string name, bool isOptional = false)
        {
            return new ArgumentToken<float>.Builder().Name(name)
                                                     .IsOptional(isOptional)
                                                     .Parser(Single.TryParse)
                                                     .Build();
        }
        
        public static ArgumentToken<double> BuildArgDouble(string name, bool isOptional = false)
        {
            return new ArgumentToken<double>.Builder().Name(name)
                                                      .IsOptional(isOptional)
                                                      .Parser(Double.TryParse)
                                                      .Build();
        }
        
        public static DateArgumentToken BuildArgDate(string name, bool isOptional = false)
        {
            return new DateArgumentToken(name, isOptional);
        }

        public static RangeArgumentToken<Money> BuildArgMoneyRange(string name, bool isOptional = false)
        {
            return new RangeArgumentToken<Money>(name, isOptional, Money.TryParse);
        }

        public static RangeArgumentToken<long> BuildArgLongRange(string name, bool isOptional = false)
        {
            return new RangeArgumentToken<long>(name, isOptional, Int64.TryParse);
        }

        public static RangeArgumentToken<DateTime> BuildArgDateRange(string name, bool isOptional = false)
        {
            return new RangeArgumentToken<DateTime>(name, isOptional, DateMatchUtils.TryMatchDate);
        }
    }
}