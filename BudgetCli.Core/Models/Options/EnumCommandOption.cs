using System;
using BudgetCli.Data.Enums;

namespace BudgetCli.Core.Models.Options
{
    public class EnumCommandOption<T> : CommandOptionBase<T> where T : struct, IConvertible
    {

        public EnumCommandOption(CommandOptionKind optionKind) : base(optionKind)
        {
            IsDataValid = false;
        }
        
        public EnumCommandOption(CommandOptionKind optionKind, string rawText) : base(optionKind, rawText)
        {
        }

        public EnumCommandOption(CommandOptionKind optionKind, T data) : base(optionKind, data)
        {
        }

        public override bool TryParseData(string rawText, out T data)
        {
            return Enum.TryParse<T>(rawText, out data);
        }
    }
}