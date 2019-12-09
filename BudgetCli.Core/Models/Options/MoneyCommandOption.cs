using BudgetCli.Util.Models;

namespace BudgetCli.Core.Models.Options
{
    public class MoneyCommandOption : CommandOptionBase<Money>
    {
        public MoneyCommandOption(string rawText) : base(rawText)
        {
        }

        public MoneyCommandOption(Money data) : base(data)
        {
        }

        public override bool TryParseData(string rawText, out Money data)
        {
            return Money.TryParse(rawText, out data);            
        }
    }
}