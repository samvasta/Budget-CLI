using BudgetCli.Core.Enums;

namespace BudgetCli.Core.Models.Options
{
    public class IntegerCommandOption : CommandOptionBase<long>
    {
        public IntegerCommandOption(string rawText) : base(rawText)
        {
        }

        public IntegerCommandOption(long data) : base(data)
        {
        }

        public override bool TryParseData(string rawText, out long data)
        {
            return long.TryParse(rawText, out data);
        }
    }
}