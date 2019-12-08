using BudgetCli.Data.Models;

namespace BudgetCli.Core.Models
{
    public interface IDataModel<TDto> where TDto : IDbModel
    {
         long? Id { get; set; }

         TDto ToDto();
    }
}