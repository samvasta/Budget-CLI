using System;
using System.Linq;
using BudgetCli.Parser.Models;
using BudgetCli.Parser.Models.Tokens;
using Xunit;

namespace BudgetCli.Parser.Tests.Models
{
    public class TokenMatchTests
    {
        [Fact]
        public void TestNullUsage()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new TokenMatch(null, -1, null);
            });
        }

        [Fact]
        public void TestNullTokenWithBadIndex()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                new TokenMatch(new VerbToken(new Name("verb")), -1, String.Empty);
            });
        }

        [Fact]
        public void TestValidConstruction()
        {
            var token = new VerbToken(new Name("verb"));

            var match1 = new TokenMatch(token, 0, "verb");

            Assert.Same(token, match1.Token);
            Assert.Equal(0, match1.TokenIdx);
            Assert.Equal("verb", match1.MatchText);
        }
    }
}