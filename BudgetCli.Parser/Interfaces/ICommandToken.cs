using System.Collections.Generic;
using BudgetCli.Parser.Enums;
using BudgetCli.Parser.Models;
using BudgetCli.Parser.Models.Tokens;

namespace BudgetCli.Parser.Interfaces
{
    public interface ICommandToken
    {
        TokenKind Kind { get; }

        /// <summary>
        /// If true, this token can be omitted
        /// </summary>
        bool IsOptional { get; }
        
        /// <summary>
        /// 
        /// </summary>
        string Description { get; }

        string[] PossibleValues { get; }

        TokenMatchResult Matches(string[] inputTokens, int startIdx);
    }
}