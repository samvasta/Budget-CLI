using System;
using BudgetCli.Core.Enums;

namespace BudgetCli.Core.Models.Options
{
    public class DateCommandOption : CommandOptionBase<DateTime>
    {
        public DateCommandOption(string rawText) : base(rawText)
        {
        }

        public DateCommandOption(DateTime data) : base(data)
        {
        }

        public override bool TryParseData(string rawText, out DateTime data)
        {
            return DateTime.TryParse(rawText, out data);
        }
    }
}