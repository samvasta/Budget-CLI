using BudgetCli.Core.Enums;
using BudgetCli.Util.Models;

namespace BudgetCli.Core.Models.Options
{
    public class DecimalRangeCommandOption : CommandOptionBase<Range<decimal>>
    {
        public DecimalRangeCommandOption(string rawText) : base(rawText)
        {
        }

        public DecimalRangeCommandOption(Range<decimal> data) : base(data)
        {
        }

        public override bool TryParseData(string rawText, out Range<decimal> data)
        {
            return Range<decimal>.TryParse(rawText, decimal.TryParse, out data);
        }
    }
}