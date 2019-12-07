using BudgetCliData.Attributes;

namespace BudgetCliData.Models
{
    public class CommandActionParameterDto : IDbModel
    {
        [Persisted]
        public virtual long? Id { get; set; }

        [Persisted]
        public virtual long CommandActionDtoId { get; set; }

        [Persisted]
        public virtual string Data { get; set; }
    }
}