using BudgetCli.Parser.Models;

namespace BudgetCli.Parser.Interfaces
{
    public interface INamedCommandToken : ICommandToken
    {
        Name Name { get; }         
    }
}