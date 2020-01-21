using System;
using System.Linq;
using System.Collections.Generic;

using BudgetCli.Parser.Interfaces;
using BudgetCli.Parser.Models;

namespace BudgetCli.Parser.Parsing
{
    public class CommandLibrary
    {
        private readonly List<ICommandRoot> _commands;
        private readonly IRecognizer<ICommandRoot> _recognizer;

        public CommandLibrary(IRecognizer<ICommandRoot> recognizer)
        {
            _commands = new List<ICommandRoot>();
            _recognizer = recognizer;
        }

        public CommandLibrary Include(ICommandRoot command)
        {
            _commands.Add(command);
            return this;
        }

        public IEnumerable<Name> GetAllCommandNames()
        {
            return _commands.Select(x => x.CommandName);
        }

        public IEnumerable<ICommandRoot> GetAllCommands()
        {
            return _commands;
        }

        public bool TryGetCommand(string text, out ICommandRoot command)
        {
            IOrderedEnumerable<Recognizer<ICommandRoot>.MatchResult> results = _recognizer.Recognize(text);

            if(results.Any())
            {
                command = results.First().Value;
                return true;
            }

            command = null;
            return false;
        }

        public Dictionary<ICommandRoot, float> GetCommandSuggestions(string text)
        {
            Dictionary<ICommandRoot, float> suggestions = new Dictionary<ICommandRoot, float>();
            
            IOrderedEnumerable<Recognizer<ICommandRoot>.MatchResult> results = _recognizer.Recognize(text);

            foreach(var matchResult in results)
            {
                if(!suggestions.ContainsKey(matchResult.Value))
                {
                    suggestions.Add(matchResult.Value, matchResult.Confidence);
                }
                else if(suggestions[matchResult.Value] < matchResult.Confidence)
                {
                    suggestions[matchResult.Value] = matchResult.Confidence;
                }
            }

            return suggestions;
        }
    }
}