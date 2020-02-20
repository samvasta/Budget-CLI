using System;
using Xunit;
using BudgetCli.Parser.Models.Tokens;
using BudgetCli.Parser.Enums;
using BudgetCli.Parser.Models;
using BudgetCli.Util.Models;

namespace BudgetCli.Parser.Tests.Models.Tokens
{
    public class TokenMatchResultTests
    {
        [Fact]
        public void TestBadConstructorArguments()
        {
            var token = new VerbToken(new Name("abcde"));
            
            Assert.Throws<ArgumentNullException>(() =>
            {
                new TokenMatchResult(token, null, "abcde", MatchOutcome.Full, 5, 1);
            });
            
            Assert.Throws<ArgumentNullException>(() =>
            {
                new TokenMatchResult(token, "abcde", null, MatchOutcome.Full, 5, 1);
            });

            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                new TokenMatchResult(token, "abcde", "abcde", MatchOutcome.Full, -1, 1);
            });

            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                new TokenMatchResult(token, "abcde", "abcde", MatchOutcome.Full, 10, 1);
            });
        }

        [Fact]
        public void TestTryGetValue()
        {
            ArgumentToken<int> token = new ArgumentToken<int>.Builder().Name("arg").Parser(int.TryParse).Build();

            TokenMatchResult result = new TokenMatchResult(token, "123", "123", MatchOutcome.Full, 3, 1);
            result.SetArgValue(token, 123);

            int argValue;
            bool hasValue = result.TryGetArgValue(token, out argValue);

            Assert.True(hasValue);
            Assert.Equal(123, argValue);
        }

        [Fact]
        public void TestEqualAndHashCode()
        {
            ArgumentToken<int> token = new ArgumentToken<int>.Builder().Name("arg").Parser(int.TryParse).Build();

            TokenMatchResult result1 = new TokenMatchResult(token, "123", "123", MatchOutcome.Full, 3, 1);
            
            TokenMatchResult result2 = new TokenMatchResult(token, "123", "123", MatchOutcome.Full, 3, 1);
            
            Assert.Equal(result1, result2);
            Assert.True(result1 == result2);
            Assert.False(result1 != result2);
            Assert.Equal(result1.GetHashCode(), result2.GetHashCode());
        }
    }
}