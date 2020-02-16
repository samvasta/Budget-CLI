using BudgetCli.Parser.Models.Tokens;

namespace BudgetCli.Parser.Interfaces
{
    public interface IArgValueBag
    {
        void SetArgValue(ArgumentToken argument, object value);

        bool TryGetArgValue<T>(ArgumentToken argument, out T value);

        bool TryGetArgValue<T>(string argName, out T value);
    }
}