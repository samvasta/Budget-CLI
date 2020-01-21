using BudgetCli.Core.Enums;

namespace BudgetCli.Core.Models.InfoItems
{
    public class HelpUsageToken
    {
        public HelpUsageTokenKind Kind { get; set; }

        public bool IsOptional { get; set; }
        
        public string Text { get; set; }

        public string[] AlternateTexts { get; set; }

        public HelpUsageToken(string text, HelpUsageTokenKind kind = HelpUsageTokenKind.Option, bool isOptional = true, params string[] alternateTexts)
        {
            Text = text;
            Kind = kind;
            IsOptional = isOptional;
            AlternateTexts = alternateTexts;
        }
    }
}