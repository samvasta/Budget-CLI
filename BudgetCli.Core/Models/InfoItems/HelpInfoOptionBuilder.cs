using System;
using System.Collections.Generic;

namespace BudgetCli.Core.Models.InfoItems
{
    public class HelpInfoOptionBuilder
    {
        protected string _optionName;
        protected List<string> _optionAlternatives;
        protected string _description;
        protected List<string> _argumentTypes;
        protected string _defaultValue;
        protected List<string> _possibleValues;

        public HelpInfoOptionBuilder()
        {
            _optionAlternatives = new List<string>();
            _possibleValues = new List<string>();
            _argumentTypes = new List<string>();
        }

        public HelpInfoOptionBuilder OptionName(string name)
        {
            _optionName = name;
            return this;
        }

        public HelpInfoOptionBuilder WithAltName(string name)
        {
            _optionAlternatives.Add(name);
            return this;
        }

        public HelpInfoOptionBuilder Description(string description)
        {
            _description = description;
            return this;
        }

        public HelpInfoOptionBuilder DefaultValue(string defaultValue)
        {
            _defaultValue = defaultValue;
            return this;
        }

        public HelpInfoOptionBuilder WithPossibleValue(string possibleValue)
        {
            _possibleValues.Add(possibleValue);
            return this;
        }

        public HelpInfoOptionBuilder WithPossibleValues(params string[] possibleValues)
        {
            _possibleValues.AddRange(possibleValues);
            return this;
        }

        public HelpInfoOptionBuilder WithPossibleValues(IEnumerable<string> possibleValues)
        {
            _possibleValues.AddRange(possibleValues);
            return this;
        }

        public HelpInfoOptionBuilder WithArgTypes(params string[] argTypes)
        {
            _argumentTypes.AddRange(argTypes);
            return this;
        }

        public HelpInfoOptionBuilder WithArgTypes(IEnumerable<string> argTypes)
        {
            _argumentTypes.AddRange(argTypes);
            return this;
        }



        public HelpOptionInfoItem Build()
        {
            return new HelpOptionInfoItem()
            {
                OptionName = _optionName,
                OptionAlternatives = _optionAlternatives,
                Description = _description,
                DefaultValue = _defaultValue,
                PossibleValues = _possibleValues,
                ArgumentTypes = _argumentTypes
            };
        }

    }
}