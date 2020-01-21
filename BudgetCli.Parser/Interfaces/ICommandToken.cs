using BudgetCli.Parser.Enums;
using BudgetCli.Parser.Models;

namespace BudgetCli.Parser.Interfaces
{
    public interface ICommandToken
    {
        TokenKind Kind { get; }

        bool IsOptional { get; }
        
        string Description { get; }

        bool Matches(string[] inputTokens, int startIdx, out int length);
    }
}