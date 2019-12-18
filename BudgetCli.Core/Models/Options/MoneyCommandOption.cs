using BudgetCli.Data.Enums;
using BudgetCli.Util.Models;

namespace BudgetCli.Core.Models.Options
{
    public class MoneyCommandOption : CommandOptionBase<Money>
    {

        public MoneyCommandOption(CommandOptionKind optionKind) : base(optionKind)
        {
            IsDataValid = false;
        }
        
        public MoneyCommandOption(CommandOptionKind optionKind, string rawText) : base(optionKind, rawText)
        {
        }

        public MoneyCommandOption(CommandOptionKind optionKind, Money data) : base(optionKind, data)
        {
        }

        public override bool TryParseData(string rawText, out Money data)
        {
            return Money.TryParse(rawText, out data);            
        }
    }
}