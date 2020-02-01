using BudgetCli.Parser.Models;
using BudgetCli.Util.Models;

namespace BudgetCli.Parser.Interfaces
{
    public interface INamedCommandToken : ICommandToken
    {
        Name Name { get; }         
    }
}