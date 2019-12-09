using BudgetCli.Core.Enums;
using BudgetCli.Util.Models;

namespace BudgetCli.Core.Models.Options
{
    public class IntegerRangeCommandOption : CommandOptionBase<Range<long>>
    {
        public IntegerRangeCommandOption(string rawText) : base(rawText)
        {
        }

        public IntegerRangeCommandOption(Range<long> data) : base(data)
        {
        }

        public override bool TryParseData(string rawText, out Range<long> data)
        {
            return Range<long>.TryParse(rawText, long.TryParse, out data);
        }
    }
}