using BudgetCli.Core.Models.Interfaces;
using BudgetCli.Data.Models;

namespace BudgetCli.Core.Models.Interfaces
{
    public interface IDataModel<TDto> : IListable where TDto : IDbModel
    {
         long? Id { get; }

         TDto ToDto();
    }
}