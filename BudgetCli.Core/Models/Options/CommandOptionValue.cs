using BudgetCli.Core.Enums;

namespace BudgetCli.Core.Models.Options
{
    public class CommandOptionValue
    {
        public CommandOptionKind OptionKind { get; }
        public char ShortName { get; set; }
        public string LongName { get; set; }
        public CommandOptionBase OptionDefinition { get; set; }
        
    }
}