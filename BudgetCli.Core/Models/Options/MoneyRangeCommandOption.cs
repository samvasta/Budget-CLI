using BudgetCli.Util.Models;

namespace BudgetCli.Core.Models.Options
{
    public class MoneyRangeCommandOption : CommandOptionBase<Range<Money>>
    {
        public MoneyRangeCommandOption(string rawText) : base(rawText)
        {
        }

        public MoneyRangeCommandOption(Range<Money> data) : base(data)
        {
        }

        public override bool TryParseData(string rawText, out Range<Money> data)
        {
            return Range<Money>.TryParse(rawText, Money.TryParse, out data);
        }
    }
}