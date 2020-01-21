using System.Net.Mime;
using System.Text.RegularExpressions;
using System;
using System.Linq;
using System.Collections.Generic;
using BudgetCli.Parser.Interfaces;
using BudgetCli.Parser.Models;

namespace BudgetCli.Parser.Parsing
{
    public static class CommandParser
    {
        private const string WHITESPACE = "\\s";

        public static IEnumerable<TokenMatchCollection> GetCurrentUsageToken(ICommandRoot currentRoot, string text)
        {
            return currentRoot.Usages.Select(x => Match(x, text));
        }

        public static TokenMatchCollection Match(ICommandUsage usage, string input)
        {
            string[] inputTokens = Regex.Split(input, WHITESPACE);
            int inputTokenIdx = 0;
            int usageTokensIdx = 0;
            List<int> matchableTokenIndexes = new List<int>();

            TokenMatchCollection match = new TokenMatchCollection(usage);

            while(inputTokenIdx < inputTokens.Length)
            {
                //Take another token from the usage to consider
                if(usageTokensIdx < usage.Tokens.Length)
                {
                    matchableTokenIndexes.Add(usageTokensIdx);
                    usageTokensIdx++;
                }

                bool areAllTokensOptional = true;
                //Find the next token match. Greedy search prefers tokens with lowest index
                int matchLength = -1;
                foreach(var tokenIdx in matchableTokenIndexes)
                {
                    ICommandToken token = usage.Tokens[tokenIdx];
                    areAllTokensOptional &= token.IsOptional;
                    if(token.Matches(inputTokens, inputTokenIdx, out matchLength))
                    {
                        //match!
                        string matchText = string.Join(" ", inputTokens.Skip(inputTokenIdx).Take(matchLength));
                        match.With(new TokenMatch(token, tokenIdx, matchText));
                        matchableTokenIndexes.Remove(tokenIdx);
                        inputTokenIdx += matchLength;

                        if(!token.IsOptional)
                        {
                            //All optional tokens until this required one are no longer viable
                            matchableTokenIndexes.Clear();
                        }

                        break;
                    }
                }

                if(matchLength <= 0)
                {
                    //No match found!
                    if(!areAllTokensOptional || usageTokensIdx == usage.Tokens.Length)
                    {
                        //We couldn't match a required token OR
                        //considering all tokens, no more matches could be found
                        //...so that's the end of the match
                        return match;
                    }
                }
            }

            return match;
        }

        public static ICommandToken GetPreviousToken(ICommandUsage currentUsage, string text)
        {
            int currentIdx = Match(currentUsage, text).Matches.Max(x => x.TokenIdx);
            if(currentIdx > 0 && currentIdx < currentUsage.Tokens.Length+1)
            {
                return currentUsage.Tokens[currentIdx-1];
            }

            return null;
        }

        public static ICommandToken GetCurrentToken(ICommandUsage currentUsage, string text)
        {
            int currentIdx = Match(currentUsage, text).Matches.Max(x => x.TokenIdx);
            if(currentIdx >= 0 && currentIdx < currentUsage.Tokens.Length)
            {
                return currentUsage.Tokens[currentIdx];
            }

            return null;
        }
        
        public static ICommandToken GetNextToken(ICommandUsage currentUsage, string text)
        {
            int currentIdx = Match(currentUsage, text).Matches.Max(x => x.TokenIdx);
            if(currentIdx >= 0 && currentIdx < currentUsage.Tokens.Length-1)
            {
                return currentUsage.Tokens[currentIdx+1];
            }

            return null;
        }
    }
}