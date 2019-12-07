using System;
using System.Reflection;
using BudgetCliData.Attributes;
using Xunit;
using System.Linq;
using BudgetCliData.Util;

namespace BudgetCliDataTest.Attributes
{
    public class PersistedAttributeTests
    {

        [Fact]
        public void TestPersistedAttributeDefaultName()
        {
            string columnName = AttributeUtil.GetPersistedColumnName<PersistedObject>(nameof(PersistedObject.ThisIsPersisted));

            Assert.Equal(nameof(PersistedObject.ThisIsPersisted), columnName);
        }

        [Fact]
        public void TestPersistedAttributeOverrideName()
        {
            string columnName = AttributeUtil.GetPersistedColumnName<PersistedObject>(nameof(PersistedObject.ThisIsPersistedWithCustomName));

            Assert.Equal("Test Name", columnName);
        }


        [Fact]
        public void TestPersistedAttributeMissing()
        {
            string columnName = AttributeUtil.GetPersistedColumnName<PersistedObject>(nameof(PersistedObject.ThisIsNotPersisted));

            Assert.Null(columnName);
        }

        [Fact]
        public void TestPersistedPropertyToColumnNameDict()
        {
            var dictionary = AttributeUtil.GetPersistedPropertyToColumnNameMap<PersistedObject>();
            
            Assert.True(dictionary.ContainsKey(nameof(PersistedObject.ThisIsPersisted)));
            Assert.True(dictionary.ContainsKey(nameof(PersistedObject.ThisIsPersistedWithCustomName)));
            Assert.False(dictionary.ContainsKey(nameof(PersistedObject.ThisIsNotPersisted)));

            Assert.Equal(nameof(PersistedObject.ThisIsPersisted), dictionary[nameof(PersistedObject.ThisIsPersisted)]);
            Assert.Equal("Test Name", dictionary[nameof(PersistedObject.ThisIsPersistedWithCustomName)]);
        }


        private class PersistedObject
        {
            [Persisted]
            public int ThisIsPersisted { get; set; }

            [Persisted("Test Name")]
            public int ThisIsPersistedWithCustomName { get; set; }

            public int ThisIsNotPersisted { get; set; }
        }

    }
}