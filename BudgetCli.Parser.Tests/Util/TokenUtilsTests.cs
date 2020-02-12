using System;
using System.Linq;
using BudgetCli.Parser.Util;
using Xunit;

namespace BudgetCli.Parser.Tests.Util
{
    public class TokenUtilsTests
    {

        [Theory]
        [InlineData(0, 1, "this")]
        [InlineData(1, 1, "is")]
        [InlineData(1, 3, "is a list")]
        [InlineData(3, 100, "list of tokens")]
        public void GetMatchText(int start, int length, string expected)
        {
            string[] tokens = {"this", "is", "a", "list", "of", "tokens"};
            string matchText = TokenUtils.GetMatchText(tokens, start, length);

            Assert.Equal(expected, matchText);
        }        
    }
}