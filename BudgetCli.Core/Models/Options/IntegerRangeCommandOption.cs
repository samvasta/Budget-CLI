using BudgetCli.Core.Enums;
using BudgetCli.Data.Enums;
using BudgetCli.Util.Models;

namespace BudgetCli.Core.Models.Options
{
    public class IntegerRangeCommandOption : CommandOptionBase<Range<long>>
    {

        public IntegerRangeCommandOption(CommandOptionKind optionKind) : base(optionKind)
        {
            IsDataValid = false;
        }
        
        public IntegerRangeCommandOption(CommandOptionKind optionKind, string rawText) : base(optionKind, rawText)
        {
        }

        public IntegerRangeCommandOption(CommandOptionKind optionKind, Range<long> data) : base(optionKind, data)
        {
        }

        public override bool TryParseData(string rawText, out Range<long> data)
        {
            return Range<long>.TryParse(rawText, long.TryParse, out data);
        }
    }
}