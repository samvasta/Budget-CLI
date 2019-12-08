using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace BudgetCli.Util.Attributes
{
    public static class HelpInfoUtil
    {
        
        public static string GetPropertyName<TClass, TProp>(this TClass model, Expression<Func<TClass,TProp>> selector)
        {
            var member = selector.Body as MemberExpression;
            return GetPropertyMetadataDisplayName(member.Member);
        }
        public static string GetPropertyDescription<TClass, TProp>(this TClass model, Expression<Func<TClass,TProp>> selector)
        {
            var member = selector.Body as MemberExpression;
            return GetPropertyMetadataDescription(member.Member);
        }

        public static string GetPropertyMetadataDisplayName<T>(string propertyName)
        {
            var memberInfo = typeof(T).GetMember(propertyName).Where(x => x.MemberType == MemberTypes.Property).FirstOrDefault();
            
            return GetPropertyMetadataDisplayName(memberInfo);
        }

        public static string GetPropertyMetadataDisplayName(MemberInfo memberInfo)
        {
            
            var attr = memberInfo.GetCustomAttribute<HelpInfoAttribute>(true);

            if (attr != null)
            {
                return attr.DisplayName;
            }

            return null;
        }
        
        public static string GetPropertyMetadataDescription<T>(string propertyName)
        {
            var memberInfo = typeof(T).GetMember(propertyName).Where(x => x.MemberType == MemberTypes.Property).FirstOrDefault();
            
            return GetPropertyMetadataDescription(memberInfo);
        }

        public static string GetPropertyMetadataDescription(MemberInfo memberInfo)
        {
            
            var attr = memberInfo.GetCustomAttribute<HelpInfoAttribute>(true);

            if (attr != null)
            {
                return attr.Description;
            }

            return null;
        }

        public static Dictionary<string, HelpInfoAttribute> GetPropertyMetadataAttributes<T>()
        {
            Dictionary<string, HelpInfoAttribute> propertyToColumnNames = new Dictionary<string, HelpInfoAttribute>();
            foreach(var member in typeof(T).GetMembers())
            {
                var attr = member.GetCustomAttribute<HelpInfoAttribute>(true);

                if (attr != null)
                {
                    propertyToColumnNames.Add(member.Name, attr);
                }
            }

            return propertyToColumnNames;
        }
    }
}