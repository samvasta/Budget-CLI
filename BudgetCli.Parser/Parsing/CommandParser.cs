using System.IO.Compression;
using System.Net.Mime;
using System.Text.RegularExpressions;
using System;
using System.Linq;
using System.Collections.Generic;
using BudgetCli.Parser.Interfaces;
using BudgetCli.Parser.Models;
using BudgetCli.Parser.Models.Tokens;
using BudgetCli.Parser.Enums;

namespace BudgetCli.Parser.Parsing
{
    public static class CommandParser
    {
        //From https://stackoverflow.com/a/366532
        private const string WHITESPACE_NOT_IN_QUOTES = @"[^\s""']+|""([^""]*)""|'([^']*)'";

        public static string[] Tokenize(string input)
        {
            List<string> tokens = new List<string>();
            MatchCollection matches = Regex.Matches(input, WHITESPACE_NOT_IN_QUOTES);
            foreach(Match match in matches)
            {
                if(match.Groups[2].Success)
                {
                    //single quotes
                    tokens.Add(match.Groups[2].Value);
                }
                else if(match.Groups[1].Success)
                {
                    //double quotes
                    tokens.Add(match.Groups[1].Value);
                }
                else
                {
                    //group 0 (un-quoted tokens) should always be a success, so not worth checking
                    tokens.Add(match.Value);
                }
            }
            return tokens.ToArray();
        }

        public static IEnumerable<TokenMatchCollection> GetCurrentUsageToken(ICommandRoot currentRoot, string text)
        {
            return currentRoot.Usages.Select(x =>
            {
                ICommandToken[] tokens = currentRoot.CommonTokens.Concat(x.Tokens).ToArray();
                return Match(tokens, text);
            });
        }

        public static TokenMatchCollection Match(ICommandToken[] commandTokens, string input)
        {
            string[] inputTokens = Tokenize(input);
            int inputTokenIdx = 0;
            int commandTokensIdx = 0;
            List<int> matchableTokenIndexes = new List<int>();

            TokenMatchCollection matchCollection = new TokenMatchCollection(commandTokens);

            while(inputTokenIdx < inputTokens.Length)
            {
                //Take another token from the usage to consider
                if(commandTokensIdx < commandTokens.Length)
                {
                    matchableTokenIndexes.Add(commandTokensIdx);
                    commandTokensIdx++;
                }

                bool areAllTokensOptional = true;
                //Find the next token match. Greedy search prefers tokens with lowest index
                TokenMatchResult bestMatch = TokenMatchResult.None;
                int bestMatchIdx = -1;
                foreach(var tokenIdx in matchableTokenIndexes)
                {
                    ICommandToken token = commandTokens[tokenIdx];
                    areAllTokensOptional &= token.IsOptional;
                    TokenMatchResult matchResult = token.Matches(inputTokens, inputTokenIdx);
                    int tokenMatchLength = matchResult.TokensMatched;

                    if(matchResult.IsBetterMatchThan(bestMatch))
                    {
                        bestMatch = matchResult;
                        bestMatchIdx = tokenIdx;
                    }

                    if(matchResult.MatchOutcome == MatchOutcome.Full)
                    {
                        //match!
                        string matchText = string.Join(" ", inputTokens.Skip(inputTokenIdx).Take(tokenMatchLength));
                        matchCollection.With(new ParserTokenMatch(tokenIdx, matchResult));
                        matchableTokenIndexes.Remove(tokenIdx);
                        inputTokenIdx += tokenMatchLength;

                        if(!token.IsOptional)
                        {
                            //All optional tokens until this required one are no longer viable
                            matchableTokenIndexes.Clear();
                        }

                        break;
                    }
                }

                if(bestMatch.MatchOutcome < MatchOutcome.Full)
                {                    
                    //No match found!
                    if(!areAllTokensOptional || commandTokensIdx == commandTokens.Length)
                    {
                        //We couldn't match a required token OR
                        //considering all tokens, no more matches could be found
                        //...so that's the end of the match

                        if(bestMatch.MatchOutcome != MatchOutcome.None)
                        {
                            //Add a partial match for whatever last failed to match
                            matchCollection.With(new ParserTokenMatch(bestMatchIdx, bestMatch));
                        }
                        
                        return matchCollection;
                    }
                }
            }

            return matchCollection;
        }

        public static ICommandToken GetPreviousToken(ICommandUsage currentUsage, string text)
        {
            int currentIdx = Match(currentUsage.Tokens, text).Matches.Max(x => x.TokenIdx);
            if(currentIdx > 0 && currentIdx < currentUsage.Tokens.Length+1)
            {
                return currentUsage.Tokens[currentIdx-1];
            }

            return null;
        }

        public static ICommandToken GetCurrentToken(ICommandUsage currentUsage, string text)
        {
            int currentIdx = Match(currentUsage.Tokens, text).Matches.Max(x => x.TokenIdx);
            if(currentIdx >= 0 && currentIdx < currentUsage.Tokens.Length)
            {
                return currentUsage.Tokens[currentIdx];
            }

            return null;
        }
        
        public static ICommandToken GetNextToken(ICommandUsage currentUsage, string text)
        {
            int currentIdx = Match(currentUsage.Tokens, text).Matches.Max(x => x.TokenIdx);
            if(currentIdx >= 0 && currentIdx < currentUsage.Tokens.Length-1)
            {
                return currentUsage.Tokens[currentIdx+1];
            }

            return null;
        }
    }
}