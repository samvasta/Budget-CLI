using System;
using Xunit;
using BudgetCli.Parser.Models.Tokens;
using BudgetCli.Parser.Enums;

namespace BudgetCli.Parser.Tests.Models.Tokens
{
    public class ArgumentTokenTests
    {
        [Fact]
        public void TestNameHasWhitespace()
        {
            Assert.Throws<ArgumentException>(() => 
            {
                new ArgumentToken<int>.Builder().Name("this has whitespace").Parser(int.TryParse).Build();
            });
            Assert.Throws<ArgumentException>(() => 
            {
                new ArgumentToken<int>.Builder().Name("this\thas-whitespace").Parser(int.TryParse).Build();
            });
            Assert.Throws<ArgumentException>(() => 
            {
                new ArgumentToken<int>.Builder().Name("this\nhas-whitespace").Parser(int.TryParse).Build();
            });
        }

        [Fact]
        public void TestArgumentTokenConstruction()
        {
            var token = new ArgumentToken<int>.Builder().Name("arg1").IsOptional(false).Parser(int.TryParse).Build();

            Assert.Equal("arg1", token.ArgumentName);
            Assert.Equal("<arg1>", token.Description);
            Assert.False(token.IsOptional);
            Assert.Equal(TokenKind.Argument, token.Kind);
        }

        [Theory]
        [InlineData(new []{"1234"}, 0, true, 1)]                        //simplest case
        [InlineData(new []{"12.34"}, 0, false, 0)]                      //double, invalid parse
        [InlineData(new []{"12 34"}, 0, false, 0)]                      //whitespace, invalid parse
        [InlineData(new []{"-1234"}, 0, true, 1)]                       //Negative integer
        [InlineData(new []{"1e9"}, 0, false, 0)]                        //Invalid parse
        [InlineData(new []{"1,000,000"}, 0, false, 0)]                  //Invalid parse
        [InlineData(new []{"a", "b", "1234", "d"}, 2, true, 1)]         //Start = middle
        [InlineData(new []{"a", "b", "c", "1234"}, 3, true, 1)]         //Start = last
        [InlineData(new []{"1234", "b", "c", "d"}, 2, false, 0)]        //Match is before the start token
        public void TestArgumentMatches(string[] input, int startIdx, bool expMatches, int expMatchLength)
        {
            var token = new ArgumentToken<int>.Builder().Name("arg1").IsOptional(false).Parser(int.TryParse).Build();

            int matchLength;
            bool matches = token.Matches(input, startIdx, out matchLength);

            Assert.True(expMatches == matches, $"Failed in case \"{input}\"");
            Assert.Equal(expMatchLength, matchLength);
        }
    }
}