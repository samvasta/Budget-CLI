using System;
using BudgetCli.Core.Enums;
using BudgetCli.Data.Enums;
using BudgetCli.Util.Models;

namespace BudgetCli.Core.Models.Options
{
    public class DateRangeCommandOption : CommandOptionBase<Range<DateTime>>
    {

        public DateRangeCommandOption(CommandOptionKind optionKind) : base(optionKind)
        {
            IsDataValid = false;
        }

        public DateRangeCommandOption(CommandOptionKind optionKind, string rawText) : base(optionKind, rawText)
        {
        }

        public DateRangeCommandOption(CommandOptionKind optionKind, Range<DateTime> data) : base(optionKind, data)
        {
        }

        public override bool TryParseData(string rawText, out Range<DateTime> data)
        {
            return Range<DateTime>.TryParse(rawText, DateTime.TryParse, out data);
        }
    }
}