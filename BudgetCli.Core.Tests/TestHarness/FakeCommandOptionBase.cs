using BudgetCli.Core.Models.Options;
using BudgetCli.Data.Enums;

namespace BudgetCli.Core.Tests.TestHarness
{
    public class FakeCommandOptionBase : CommandOptionBase<double>
    {
        public FakeCommandOptionBase(CommandOptionKind optionKind) : base(optionKind)
        {
        }

        public FakeCommandOptionBase(CommandOptionKind optionKind, string rawText) : base(optionKind, rawText)
        {
        }

        public FakeCommandOptionBase(CommandOptionKind optionKind, double data) : base(optionKind, data)
        {
        }

        public override bool TryParseData(string rawText, out double data)
        {
            return double.TryParse(rawText, out data);
        }
    }
}