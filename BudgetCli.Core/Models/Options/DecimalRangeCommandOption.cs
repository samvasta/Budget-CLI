using BudgetCli.Core.Enums;
using BudgetCli.Data.Enums;
using BudgetCli.Util.Models;

namespace BudgetCli.Core.Models.Options
{
    public class DecimalRangeCommandOption : CommandOptionBase<Range<decimal>>
    {

        public DecimalRangeCommandOption(CommandOptionKind optionKind) : base(optionKind)
        {
            IsDataValid = false;
        }
        
        public DecimalRangeCommandOption(CommandOptionKind optionKind, string rawText) : base(optionKind, rawText)
        {
        }

        public DecimalRangeCommandOption(CommandOptionKind optionKind, Range<decimal> data) : base(optionKind, data)
        {
        }

        public override bool TryParseData(string rawText, out Range<decimal> data)
        {
            return Range<decimal>.TryParse(rawText, decimal.TryParse, out data);
        }
    }
}