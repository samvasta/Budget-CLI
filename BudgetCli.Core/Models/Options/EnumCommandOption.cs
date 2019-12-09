using System;

namespace BudgetCli.Core.Models.Options
{
    public class EnumCommandOption<T> : CommandOptionBase<T> where T : struct, IConvertible
    {
        public EnumCommandOption(string rawText) : base(rawText)
        {
        }

        public EnumCommandOption(T data) : base(data)
        {
        }

        public override bool TryParseData(string rawText, out T data)
        {
            return Enum.TryParse<T>(rawText, out data);
        }
    }
}