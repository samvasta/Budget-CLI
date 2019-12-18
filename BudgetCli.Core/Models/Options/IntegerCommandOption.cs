using BudgetCli.Core.Enums;
using BudgetCli.Data.Enums;

namespace BudgetCli.Core.Models.Options
{
    public class IntegerCommandOption : CommandOptionBase<long>
    {

        public IntegerCommandOption(CommandOptionKind optionKind) : base(optionKind)
        {
            IsDataValid = false;
        }
        
        public IntegerCommandOption(CommandOptionKind optionKind, string rawText) : base(optionKind, rawText)
        {
        }

        public IntegerCommandOption(CommandOptionKind optionKind, long data) : base(optionKind, data)
        {
        }

        public override bool TryParseData(string rawText, out long data)
        {
            return long.TryParse(rawText, out data);
        }
    }
}