using System;
using System.Linq;
using System.Collections.Generic;

using BudgetCli.Parser.Interfaces;
using BudgetCli.Parser.Models;
using BudgetCli.Util.Models;

namespace BudgetCli.Parser.Parsing
{
    public class CommandLibrary
    {
        private readonly List<ICommandRoot> _commands;

        public CommandLibrary()
        {
            _commands = new List<ICommandRoot>();
        }

        public CommandLibrary AddCommand(ICommandRoot command)
        {
            _commands.Add(command);
            return this;
        }

        public IEnumerable<string> GetAllCommandNames()
        {
            foreach(var command in _commands)
            {
                yield return String.Join(" ", command.CommonTokens.Select(x => x.Name.Preferred));
            }
        }

        public IEnumerable<ICommandRoot> GetAllCommands()
        {
            return _commands;
        }

        public bool TryGetCommand(string text, out ICommandRoot command)
        {
            float bestMatchQuality = 0;
            command = null;

            foreach(var c in _commands)
            {
                TokenMatchCollection match = CommandParser.Match(c.CommonTokens, text);
                if(match.MatchQuality > bestMatchQuality)
                {
                    bestMatchQuality = match.MatchQuality;
                    command = c;
                }
            }

            return bestMatchQuality > 0;
        }

        public Dictionary<ICommandRoot, float> GetCommandSuggestions(string text)
        {
            Dictionary<ICommandRoot, float> suggestions = new Dictionary<ICommandRoot, float>();
            
            foreach(var c in _commands)
            {
                TokenMatchCollection match = CommandParser.Match(c.CommonTokens, text);
                suggestions.Add(c, match.MatchQuality);
            }

            return suggestions;
        }

        public CommandUsageMatchData Parse(string text)
        {
            CommandUsageMatchData bestMatchData = null;
            float bestMatchQuality = float.NegativeInfinity;

            foreach(var command in _commands)
            {
                Dictionary<ICommandUsage, TokenMatchCollection> matchDict = CommandParser.GetUsageMatchResults(command, text);
                foreach(var kvp in matchDict)
                {
                    ICommandUsage usage = kvp.Key;
                    TokenMatchCollection matchCollection = kvp.Value;

                    if(matchCollection.IsFullMatch && matchCollection.MatchQuality > bestMatchQuality)
                    {
                        bestMatchQuality = matchCollection.MatchQuality;
                        bestMatchData = new CommandUsageMatchData(command, usage, matchCollection);
                    }        
                }
            }
            return bestMatchData;
        }
    }
}