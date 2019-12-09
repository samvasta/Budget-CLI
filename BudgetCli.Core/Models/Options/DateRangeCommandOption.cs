using System;
using BudgetCli.Core.Enums;
using BudgetCli.Util.Models;

namespace BudgetCli.Core.Models.Options
{
    public class DateRangeCommandOption : CommandOptionBase<Range<DateTime>>
    {
        public DateRangeCommandOption(string rawText) : base(rawText)
        {
        }

        public DateRangeCommandOption(Range<DateTime> data) : base(data)
        {
        }

        public override bool TryParseData(string rawText, out Range<DateTime> data)
        {
            return Range<DateTime>.TryParse(rawText, DateTime.TryParse, out data);
        }
    }
}