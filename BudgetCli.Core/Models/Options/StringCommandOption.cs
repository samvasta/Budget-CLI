using BudgetCli.Core.Enums;

namespace BudgetCli.Core.Models.Options
{
    public class StringCommandOption : CommandOptionBase<string>
    {
        public StringCommandOption(string rawText) : base(rawText)
        {
        }

        public override bool TryParseData(string rawText, out string data)
        {
            data = rawText;
            return true;
        }
    }
}