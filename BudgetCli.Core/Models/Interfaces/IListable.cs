using System.Collections.Generic;
using BudgetCli.Core.Models.ModelInfo;

namespace BudgetCli.Core.Models.Interfaces
{
    public interface IListable : IDetailable
    {
         IEnumerable<ModelProperty> GetProperties();
    }
}