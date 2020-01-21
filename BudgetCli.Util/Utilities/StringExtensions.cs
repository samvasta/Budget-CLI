using System.Text.RegularExpressions;
namespace BudgetCli.Util.Utilities
{
    public static class StringExtensions
    {
        private const string WHITESPACE = "\\s";

        public static bool ContainsWhitespace(this string str)
        {
            return Regex.IsMatch(str, WHITESPACE);
        }        
    }
}