using System;
using System.Reflection;
using Xunit;
using Moq;
using BudgetCli.Util.Models;

namespace BudgetCli.Util.Tests.Models
{
    public class NameTests
    {
        [Fact]
        public void EmptyPreferredThrowsArgException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                new Name("", "alt1", "alt2");
            });
        }
        [Fact]
        public void NullPreferredThrowsArgException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                new Name(null, "alt1", "alt2");
            });
        }

        [Fact]
        public void EmptyAlternateThrowsArgException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                new Name("preferred", "alt1", "alt2", "");
            });
        }
        [Fact]
        public void NullAlternateThrowsArgException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                new Name("preferred", "alt1", "alt2", null);
            });
        }
        [Fact]
        public void PreferredWithWhitespaceThrowsArgException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                new Name("preferred with whitespace", "alt1", "alt2");
            });
            Assert.Throws<ArgumentException>(() =>
            {
                new Name("preferred-with\twhitespace", "alt1", "alt2");
            });
            Assert.Throws<ArgumentException>(() =>
            {
                new Name("preferred-with\nwhitespace", "alt1", "alt2");
            });
        }
        [Fact]
        public void AlternateWithWhitespaceThrowsArgException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                new Name("preferred", "alt1", "alt2", "alt3 with whitespace");
            });
            Assert.Throws<ArgumentException>(() =>
            {
                new Name("preferred", "alt1", "alt2", "alt3-with\twhitespace");
            });
            Assert.Throws<ArgumentException>(() =>
            {
                new Name("preferred", "alt1", "alt2", "alt3-with\nwhitespace");
            });
        }

        
        
        [Fact]
        public void TestConstructionWithoutAlternates()
        {
            Name name = new Name("preferred");

            Assert.NotNull(name);
            Assert.Equal("preferred", name.Preferred);
            Assert.Equal(0, name.Alternates.Length);
        }

        [Fact]
        public void TestConstructionWithAlternates()
        {
            Name name = new Name("preferred", "alt1", "alt2", "alt3");

            Assert.NotNull(name);
            Assert.Equal("preferred", name.Preferred);
            Assert.Equal(3, name.Alternates.Length);
            Assert.Contains("alt1", name.Alternates);
            Assert.Contains("alt2", name.Alternates);
            Assert.Contains("alt3", name.Alternates);
        }


        [Fact]
        public void TestNameEqualsIgnoreCase()
        {
            Name name = new Name("preferred");

            Assert.True(name.Equals("PrEfErReD", StringComparison.CurrentCultureIgnoreCase));
        }


        [Fact]
        public void TestNameEqualsWithoutAlts()
        {
            Name name = new Name("preferred");
            Name name2 = new Name("preferred");

            Assert.Equal(name, name2);
        }
        [Fact]
        public void TestNameEqualsWithAlts()
        {
            Name name = new Name("preferred", "alt1", "alt2", "alt3");
            Name name2 = new Name("preferred", "alt1", "alt2", "alt3");

            Assert.Equal(name, name2);
        }
        [Fact]
        public void TestNameEqualsWithAltsInDifferentOrder()
        {
            Name name = new Name("preferred", "alt1", "alt2", "alt3");
            Name name2 = new Name("preferred", "alt2", "alt3", "alt1");

            Assert.Equal(name, name2);
        }
        [Fact]
        public void TestNameNotEqualsWithoutAlts()
        {
            Name name = new Name("Preferred");
            Name name2 = new Name("preferred");

            Assert.NotEqual(name, name2);
        }
        [Fact]
        public void TestNameNotEqualsWithAlts()
        {
            Name name = new Name("Preferred");
            Name name2 = new Name("preferred");

            Assert.NotEqual(name, name2);
        }
        
        [Fact]
        public void TestNameEqualsStringWithoutAlts()
        {
            Name name = new Name("preferred");

            Assert.True(name.Equals("preferred"));
        }
        [Fact]
        public void TestNameEqualsStringWithAlts()
        {
            Name name = new Name("preferred", "alt1", "alt2", "alt3");

            Assert.True(name.Equals("alt3"));
        }
        [Fact]
        public void TestNameNotEqualsStringWithoutAlts()
        {
            Name name = new Name("Preferred");

            Assert.False(name.Equals("preferred"));
        }
        [Fact]
        public void TestNameNotEqualsStringWithAlts()
        {
            Name name = new Name("Preferred", "Alt1", "Alt2", "Alt3");

            Assert.False(name.Equals("alt4"));
        }


        [Fact]
        public void TestNameGetHashCodeWithoutAlts()
        {
            Name name = new Name("preferred");
            Name name2 = new Name("preferred");

            Assert.Equal(name.GetHashCode(), name2.GetHashCode());
        }
        [Fact]
        public void TestNameGetHashCodeWithAlts()
        {
            Name name = new Name("preferred", "alt1", "alt2", "alt3");
            Name name2 = new Name("preferred", "alt1", "alt2", "alt3");

            Assert.Equal(name.GetHashCode(), name2.GetHashCode());
        }
        [Fact]
        public void TestNameNotGetHashCodeWithoutAlts()
        {
            Name name = new Name("Preferred");
            Name name2 = new Name("preferred");

            Assert.NotEqual(name.GetHashCode(), name2.GetHashCode());
        }
        [Fact]
        public void TestNameNotGetHashCodeWithAlts()
        {
            Name name = new Name("Preferred", "Alt1", "Alt2", "Alt3");
            Name name2 = new Name("preferred", "alt1", "alt2", "alt3");

            Assert.NotEqual(name.GetHashCode(), name2.GetHashCode());
        }


        [Theory]
        [InlineData("this is my A1 string", true, 11, 2)]
        [InlineData("A1, this is my string", true, 0, 2)]
        [InlineData("this is my A2 string", true, 11, 2)]
        [InlineData("this is my A5 string", false, -1, 0)]
        public void TestNameIsIn(string input, bool expIsIn, int expIdx, int expLength)
        {
            Name name = new Name("A1", "A2", "A3");

            int index;
            int length;
            bool isIn = name.IsIn(input, out index, out length);
            Assert.Equal(expIsIn, isIn);
            Assert.Equal(expIdx, index);
            Assert.Equal(expLength, length);
        }

        [Theory]
        [InlineData("A1, this is my String", true, 2)]
        [InlineData("A2, this is my String", true, 2)]
        [InlineData("A5, this is my String", false, 0)]
        [InlineData("this is my A1 String", false, 0)]
        [InlineData("this is my A2 String", false, 0)]
        [InlineData("this is my A5 String", false, 0)]
        public void TestNameIsStartOf(string input, bool expIsIn, int expLength)
        {
            Name name = new Name("A1", "A2", "A3");

            int length;
            bool isIn = name.IsStartOf(input, out length);
            Assert.Equal(expIsIn, isIn);
            Assert.Equal(expLength, length);
        }

        [Theory]
        [InlineData("this_is_a_short_test", "this_is_the_third_name", 8)]
        [InlineData("this_name_is_a1", "this_name_is_a1", 15)]
        [InlineData("this_name_is_a", "this_name_is_a1", 14)]
        [InlineData("this", "this_is_the_third_name", 4)]
        public void TestNameLongestMatch(string input, string expLongestMatch, int expLength)
        {
            Name name = new Name("this_name_is_a1", "this_name_is_a2", "this_is_the_third_name");

            int length;
            string longestMatch = name.GetLongestMatch(input, out length);
            Assert.Equal(expLongestMatch, longestMatch);
            Assert.Equal(expLength, length);
        }
    }
}