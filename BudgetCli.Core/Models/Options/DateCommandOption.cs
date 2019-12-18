using System;
using BudgetCli.Core.Enums;
using BudgetCli.Data.Enums;

namespace BudgetCli.Core.Models.Options
{
    public class DateCommandOption : CommandOptionBase<DateTime>
    {

        public DateCommandOption(CommandOptionKind optionKind) : base(optionKind)
        {
            IsDataValid = false;
        }
        
        public DateCommandOption(CommandOptionKind optionKind, string rawText) : base(optionKind, rawText)
        {
        }

        public DateCommandOption(CommandOptionKind optionKind, DateTime data) : base(optionKind, data)
        {
        }

        public override bool TryParseData(string rawText, out DateTime data)
        {
            return DateTime.TryParse(rawText, out data);
        }
    }
}