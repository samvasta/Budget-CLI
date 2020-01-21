using BudgetCli.Parser.Enums;
using BudgetCli.Parser.Interfaces;

namespace BudgetCli.Parser.Models.Tokens
{
    public class StandAloneOptionToken : ICommandToken
    {
        public TokenKind Kind { get { return TokenKind.StandAloneOption; } }

        public bool IsOptional { get { return true; } }

        public Name Name { get; }

        public string Description { get { return Name.Preferred; } }

        public StandAloneOptionToken(Name name)
        {
            Name = name;
        }

        public bool Matches(string[] inputTokens, int startIdx, out int matchLength)
        {
            if(startIdx >= inputTokens.Length || startIdx < 0)
            {
                matchLength = 0;
                return false;
            }

            if(Name.Equals(inputTokens[startIdx]))
            {
                matchLength = 1;
                return true;
            }
            matchLength = 0;
            return false;
        }
    }
}