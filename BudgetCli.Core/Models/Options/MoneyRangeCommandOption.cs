using BudgetCli.Data.Enums;
using BudgetCli.Util.Models;

namespace BudgetCli.Core.Models.Options
{
    public class MoneyRangeCommandOption : CommandOptionBase<Range<Money>>
    {

        public MoneyRangeCommandOption(CommandOptionKind optionKind) : base(optionKind)
        {
            IsDataValid = false;
        }
        
        public MoneyRangeCommandOption(CommandOptionKind optionKind, string rawText) : base(optionKind, rawText)
        {
        }

        public MoneyRangeCommandOption(CommandOptionKind optionKind, Range<Money> data) : base(optionKind, data)
        {
        }

        public override bool TryParseData(string rawText, out Range<Money> data)
        {
            return Range<Money>.TryParse(rawText, Money.TryParse, out data);
        }
    }
}