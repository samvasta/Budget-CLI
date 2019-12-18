using BudgetCli.Core.Enums;
using BudgetCli.Data.Enums;

namespace BudgetCli.Core.Models.Options
{
    public class StringCommandOption : CommandOptionBase<string>
    {

        public StringCommandOption(CommandOptionKind optionKind) : base(optionKind)
        {
            IsDataValid = false;
        }
        
        public StringCommandOption(CommandOptionKind optionKind, string rawText) : base(optionKind, rawText)
        {
        }

        public override bool TryParseData(string rawText, out string data)
        {
            data = rawText;
            return true;
        }
    }
}