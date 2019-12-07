using BudgetCliData.Attributes;
using BudgetCliData.Models;

namespace BudgetCliDataTest.TestHarness
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