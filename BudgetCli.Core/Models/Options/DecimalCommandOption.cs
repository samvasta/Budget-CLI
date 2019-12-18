using BudgetCli.Core.Enums;
using BudgetCli.Data.Enums;

namespace BudgetCli.Core.Models.Options
{
    public class DecimalCommandOption : CommandOptionBase<decimal>
    {

        public DecimalCommandOption(CommandOptionKind optionKind) : base(optionKind)
        {
            IsDataValid = false;
        }
        
        public DecimalCommandOption(CommandOptionKind optionKind, string rawText) : base(optionKind, rawText)
        {
        }

        public DecimalCommandOption(CommandOptionKind optionKind, decimal data) : base(optionKind, data)
        {
        }

        public override bool TryParseData(string rawText, out decimal data)
        {
            return decimal.TryParse(rawText, out data);
        }
    }
}