using System;
using System.Linq;
using System.Collections.Generic;

namespace BudgetCli.Core.Models.CommandResults.InfoModels
{
    /// <summary>
    /// A summary of search/filter criteria used in a query.
    /// This data is meant for display purposes only.
    /// </summary>
    public class FilterCriteria
    {
        private Dictionary<string, string> _fields;
        /// <summary>
        /// Dictionary of &lt;Field Name, Requirement Description&gt;
        /// <br/><br/>
        /// for more info, see parameters of <see cref="FilterCriteria.AddField(string, string)"/>
        /// </summary>
        public IReadOnlyDictionary<string, string> Fields
        {
            get
            {
                return _fields;
            }
        }

        public FilterCriteria()
        {
            _fields = new Dictionary<string, string>();
        }

        /// <summary>
        /// Add a filter criterion
        /// </summary>
        /// <param name="fieldName">Name of the field that had a filter criterion (e.g. "Date Created")</param>
        /// <param name="requirements">A brief description of the filter requirements for the field. (e.g. "between Jan. 1 2020 and Feb. 5 2020")</param>
        public void AddField(string fieldName, string requirements)
        {
            _fields.Add(fieldName, requirements);
        }

    }
}