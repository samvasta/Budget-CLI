using System;
using System.Runtime.CompilerServices;

namespace BudgetCli.Data.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class PersistedAttribute : Attribute
    {
        private string _columnName;

        public PersistedAttribute([CallerMemberName] string columnName = null)
        {
            _columnName = columnName;
        }

        public string ColumnName
        {
            get { return _columnName; }
        }
    }
}