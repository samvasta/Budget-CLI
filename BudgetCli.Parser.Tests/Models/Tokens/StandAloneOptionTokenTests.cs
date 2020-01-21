using System;
using Xunit;
using BudgetCli.Parser.Models.Tokens;
using BudgetCli.Parser.Enums;
using BudgetCli.Parser.Models;

namespace BudgetCli.Parser.Tests.Models.Tokens
{
    public class StandAloneOptionTokenTests
    {
        [Fact]
        public void TestStandAloneOptionTokenConstruction()
        {
            Name name = new Name("option", "alt1", "alt2");
            var token = new StandAloneOptionToken(name);

            Assert.Same(name, token.Name);
            Assert.Equal("option", token.Description);
            Assert.True(token.IsOptional);
            Assert.Equal(TokenKind.StandAloneOption, token.Kind);
        }

        [Theory]
        [InlineData(new []{"option", "b", "c", "d"}, 0, true, 1)]       //Simplest case 
        [InlineData(new []{"a", "b", "option", "d"}, 2, true, 1)]       //Start in the middle
        [InlineData(new []{"a", "b", "c", "alt1"}, 3, true, 1)]         //Start at the end
        [InlineData(new []{"a", "b", "alt1", "alt2"}, 2, true, 1)]      //Multiple matches
        [InlineData(new []{"option", "b", "c", "d"}, 2, false, 0)]      //Start after match
        [InlineData(new []{"a", "b", "c", "d"}, 0, false, 0)]           //No match
        public void TestStandAloneOptionMatches(string[] input, int startIdx, bool expMatches, int expMatchLength)
        {
            var token = new StandAloneOptionToken(new Name("option", "alt1", "alt2"));

            int matchLength;
            bool matches = token.Matches(input, startIdx, out matchLength);

            Assert.True(expMatches == matches, $"Failed in case \"{String.Join(", ", input)}\"");
            Assert.Equal(expMatchLength, matchLength);
        }
    }
}