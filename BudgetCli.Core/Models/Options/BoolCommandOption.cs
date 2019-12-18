using BudgetCli.Data.Enums;

namespace BudgetCli.Core.Models.Options
{
    public class BoolCommandOption : CommandOptionBase<bool>
    {
        public BoolCommandOption(CommandOptionKind optionKind) : base(optionKind)
        {
            IsDataValid = false;
        }

        public BoolCommandOption(CommandOptionKind optionKind, string rawText) : base(optionKind, rawText)
        {
        }

        public BoolCommandOption(CommandOptionKind optionKind, bool data) : base(optionKind, data)
        {
        }

        public override bool TryParseData(string rawText, out bool data)
        {
            return bool.TryParse(rawText, out data);
        }
    }
}