using BudgetCli.Data.Attributes;
using BudgetCli.Data.Models;

namespace BudgetCli.Data.Tests.TestHarness
{
    public class FakeDto : IDbModel
    {
        [Persisted]
        public virtual long? Id { get; set; }

        [Persisted]
        public virtual string Name { get; set; }

        [Persisted]
        public virtual string Description { get; set; }
    }
}