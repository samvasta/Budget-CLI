using System.Reflection;
using BudgetCli.Data.Attributes;
using System.Linq;
using System.Collections.Generic;

namespace BudgetCli.Data.Util
{
    public static class AttributeUtil
    {
        #region - Persisted Attribute -
        
        public static string GetPersistedColumnName<T>(string propertyName)
        {
            var memberInfo = typeof(T).GetMember(propertyName).Where(x => x.MemberType == MemberTypes.Property).FirstOrDefault();
            
            return GetPersistedColumnName(memberInfo);
        }

        public static string GetPersistedColumnName(MemberInfo memberInfo)
        {
            
            var attr = memberInfo.GetCustomAttribute<PersistedAttribute>(true);

            if (attr != null)
            {
                return attr.ColumnName;
            }

            return null;
        }

        public static Dictionary<string, string> GetPersistedPropertyToColumnNameMap<T>()
        {
            Dictionary<string, string> propertyToColumnNames = new Dictionary<string, string>();
            foreach(var member in typeof(T).GetMembers())
            {
                string columnName = GetPersistedColumnName(member);
                if(!string.IsNullOrEmpty(columnName))
                {
                    propertyToColumnNames.Add(member.Name, columnName);
                }
            }

            return propertyToColumnNames;
        }

        #endregion - Persisted Attribute -

    }
}