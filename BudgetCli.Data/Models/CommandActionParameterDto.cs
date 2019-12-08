using BudgetCli.Data.Attributes;

namespace BudgetCli.Data.Models
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