using BudgetCli.Core.Enums;

namespace BudgetCli.Core.Models.Options
{
    public class DecimalCommandOption : CommandOptionBase<decimal>
    {
        public DecimalCommandOption(string rawText) : base(rawText)
        {
        }

        public DecimalCommandOption(decimal data) : base(data)
        {
        }

        public override bool TryParseData(string rawText, out decimal data)
        {
            return decimal.TryParse(rawText, out data);
        }
    }
}