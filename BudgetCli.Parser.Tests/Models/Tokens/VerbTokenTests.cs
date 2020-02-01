using System;
using Xunit;
using BudgetCli.Parser.Models.Tokens;
using BudgetCli.Parser.Enums;
using BudgetCli.Parser.Models;
using BudgetCli.Util.Models;

namespace BudgetCli.Parser.Tests.Models.Tokens
{
    public class VerbTokenTests
    {
        [Fact]
        public void TestVerbTokenConstruction()
        {
            Name name = new Name("verb", "alt1", "alt2");
            var token = new VerbToken(name);

            Assert.Same(name, token.Name);
            Assert.Equal("verb", token.Description);
            Assert.False(token.IsOptional);
            Assert.Equal(TokenKind.Verb, token.Kind);
        }

        [Theory]
        [InlineData(new []{"verb", "b", "c", "d"}, 0, true, 1)]     //Simplest case 
        [InlineData(new []{"a", "b", "verb", "d"}, 2, true, 1)]     //Start in the middle
        [InlineData(new []{"a", "b", "c", "alt1"}, 3, true, 1)]     //Start at the end
        [InlineData(new []{"a", "b", "alt1", "alt2"}, 2, true, 1)]  //Multiple matches
        [InlineData(new []{"verb", "b", "c", "d"}, 2, false, 0)]    //Start after match
        [InlineData(new []{"a", "b", "c", "d"}, 0, false, 0)]       //No match
        public void TestVerbMatches(string[] input, int startIdx, bool expMatches, int expMatchLength)
        {
            var token = new VerbToken(new Name("verb", "alt1", "alt2"));

            TokenMatchResult matchResult = token.Matches(input, startIdx);

            if(expMatches)
            {
                Assert.True(matchResult.MatchOutcome == MatchOutcome.Full, $"Failed in case \"{input}\"");
            }
            else
            {
                Assert.False(matchResult.MatchOutcome == MatchOutcome.Full, $"Failed in case \"{input}\"");
            }
            Assert.Equal(expMatchLength, matchResult.TokensMatched);
        }
    }
}