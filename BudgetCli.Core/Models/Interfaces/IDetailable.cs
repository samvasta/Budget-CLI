using System.Collections.Generic;
using BudgetCli.Core.Models.ModelInfo;

namespace BudgetCli.Core.Models.Interfaces
{
    public interface IDetailable
    {
        string TypeName { get; }
        string DisplayName { get; }
         IEnumerable<ModelPropertyValue> GetPropertyValues();
    }
}