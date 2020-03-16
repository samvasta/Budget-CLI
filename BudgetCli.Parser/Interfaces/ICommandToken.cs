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
        /// For Help Documentation. Usages of the token (ex. "-h --help")
        /// </summary>
        string DisplayName { get; }

        /// <summary>
        /// For Help Documentation. Possible values of the token. Empty will be ignored
        /// </summary>
        string[] PossibleValues { get; }

        TokenMatchResult Matches(string[] inputTokens, int startIdx);
    }
}