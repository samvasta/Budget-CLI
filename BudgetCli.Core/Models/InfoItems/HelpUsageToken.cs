using BudgetCli.Core.Enums;

namespace BudgetCli.Core.Models.InfoItems
{
    public class HelpUsageToken
    {
        public HelpUsageTokenKind Kind { get; set; }

        public bool IsOptional { get; set; }
        
        public string Text { get; set; }

        public HelpUsageToken(string text, HelpUsageTokenKind kind = HelpUsageTokenKind.Option, bool isOptional = true)
        {
            Text = text;
            Kind = kind;
            IsOptional = isOptional;
        }
    }
}