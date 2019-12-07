using BudgetCliData.Models;

namespace BudgetCliCore.Models
{
    public interface IDataModel<TDto> where TDto : IDbModel
    {
         long? Id { get; set; }

         TDto ToDto();
    }
}