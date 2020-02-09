using System;
using System.Collections.Generic;
using BudgetCli.Parser.Models.Tokens;

namespace BudgetCli.Parser.Models
{
    public class ValueBag<T>
    {
        private readonly Dictionary<T, object> _values;

        public ValueBag()
        {
            _values = new Dictionary<T, object>();
        }
        
        // although this method doesn't require the generic parameter, I kept it here
        // to ensure users cannot add something they won't be able to get out
        public void SetValue<E>(T token, E value) where E : class
        {
            if(token.Equals(default(T)))
            {
                throw new ArgumentNullException(nameof(token));
            }

            _values.Add(token, value);
        }

        public bool TryGetValue<E>(T token, out E value)
        {
            if(_values.ContainsKey(token))
            {
                value = (E)_values[token];
                return true;
            }

            value = default(E);
            return false;
        }
    }
}