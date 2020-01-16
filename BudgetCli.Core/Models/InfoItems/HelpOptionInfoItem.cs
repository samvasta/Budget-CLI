using System.Linq;
using System.Text;
using System;
using System.Collections.Generic;

namespace BudgetCli.Core.Models.InfoItems
{
    public class HelpOptionInfoItem
    {
        public string OptionName { get; set; }
        public List<string> OptionAlternatives { get; set; }
        public List<string> ArgumentTypes { get; set; }
        public string Description { get; set; }
        public string DefaultValue { get; set; }
        public List<string> PossibleValues { get; set; }

        public string OptionFormat 
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                
                sb.Append(OptionName);
                
                if(OptionAlternatives != null)
                {
                    foreach(string alternative in OptionAlternatives)
                    {
                        sb.Append(", ").Append(alternative);
                    }
                }

                if(ArgumentTypes != null)
                {
                    foreach(string argType in ArgumentTypes)
                    {
                        sb.Append(' ').Append('<').Append(argType).Append('>');
                    }
                }

                sb.Append("  ");
                
                return sb.ToString();
            }
        }
    }
}