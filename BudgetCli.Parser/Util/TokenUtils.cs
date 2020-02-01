using System;
using System.Linq;
using BudgetCli.Parser.Models.Tokens;
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

        public static ArgumentToken<int> BuildArgInt(string name, bool isOptional = false)
        {
            return new ArgumentToken<int>.Builder().Name(name).IsOptional(isOptional).Parser(Int32.TryParse).Build();
        }
        
        public static ArgumentToken<string> BuildArgString(string name, bool isOptional = false)
        {
            return new ArgumentToken<string>.Builder().Name(name).IsOptional(isOptional).Parser(TryParseString).Build();
        }
        
        public static ArgumentToken<float> BuildArgFloat(string name, bool isOptional = false)
        {
            return new ArgumentToken<float>.Builder().Name(name).IsOptional(isOptional).Parser(Single.TryParse).Build();
        }
        
        public static ArgumentToken<double> BuildArgDouble(string name, bool isOptional = false)
        {
            return new ArgumentToken<double>.Builder().Name(name).IsOptional(isOptional).Parser(Double.TryParse).Build();
        }
        
        public static DateArgumentToken BuildArgDate(string name, bool isOptional = false)
        {
            return new DateArgumentToken(name, isOptional);
        }
    }
}