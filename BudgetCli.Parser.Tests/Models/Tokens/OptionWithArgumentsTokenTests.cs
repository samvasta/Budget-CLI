using System;
using Xunit;
using BudgetCli.Parser.Models.Tokens;
using BudgetCli.Parser.Enums;
using BudgetCli.Parser.Models;
using BudgetCli.Parser.Interfaces;
using BudgetCli.Util.Models;

namespace BudgetCli.Parser.Tests.Models.Tokens
{
    public class OptionWithArgumentsTokenTests
    {
        [Fact]
        public void TestOptionWithArgumentsTokenConstruction()
        {
            ArgumentToken arg1 = new ArgumentToken<int>.Builder().Name("arg1").Parser(int.TryParse).Build();

            Name name = new Name("option", "alt1", "alt2");
            var token = new OptionWithArgumentToken.Builder()
                                .Name("option", "alt1", "alt2")
                                .WithArgument(arg1)
                                .Build();

            Assert.Equal(name, token.Name);
            Assert.Equal("option <arg1>", token.Description);
            Assert.True(token.IsOptional);
            Assert.Equal(TokenKind.OptionWithArgument, token.Kind);
            Assert.Equal(1, token.Arguments.Length);
            Assert.Same(arg1, token.Arguments[0]);
        }

        [Theory]
        [InlineData(new []{"option", "123", "c", "d"}, 0, MatchOutcome.Full, 2)]     //Simplest case 
        [InlineData(new []{"a", "option", "123", "d"}, 1, MatchOutcome.Full, 2)]     //Start in the middle
        [InlineData(new []{"a", "b", "option", "123"}, 2, MatchOutcome.Full, 2)]     //Start in the end
        [InlineData(new []{"a", "b", "c", "alt1"}, 3, MatchOutcome.Partial, 1)]      //Start at the end with partial match
        [InlineData(new []{"a", "alt2", "alt1", "123"}, 1, MatchOutcome.Partial, 1)] //Multiple matches (partial)
        [InlineData(new []{"option", "123", "123", "d"}, 0, MatchOutcome.Full, 2)]   //Start at match with extra args
        [InlineData(new []{"a", "b", "c", "d"}, 0, MatchOutcome.Partial, 0)]            //No match
        [InlineData(new string[]{}, 0, MatchOutcome.None, 0)]                        //empty
        [InlineData(new []{"a"}, 0, MatchOutcome.Partial, 0)]                           //not enough
        public void TestOptionWithArgumentsMatches(string[] input, int startIdx, MatchOutcome expMatchLength, int expNumMatchTokens)
        {
            ArgumentToken arg1 = new ArgumentToken<int>.Builder().Name("arg1").Parser(int.TryParse).Build();
            var token = new OptionWithArgumentToken.Builder()
                                .Name("option", "alt1", "alt2")
                                .WithArgument(arg1)
                                .Build();

            TokenMatchResult matchResult = token.Matches(input, startIdx);

            Assert.True(expMatchLength == matchResult.MatchOutcome, $"Failed in case \"{input}\"");
            Assert.Equal(expNumMatchTokens, matchResult.TokensMatched);
        }

        [Theory]
        [InlineData(new []{"option", "123", "456", "789", "e", "f", "g"}, 0, MatchOutcome.Full, 4)]     //Simplest case 
        [InlineData(new []{"a", "option", "123", "456", "789", "f", "g"}, 1, MatchOutcome.Full, 4)]     //Start in the middle
        [InlineData(new []{"a", "b", "c", "alt2", "123", "456", "789"}, 3, MatchOutcome.Full, 4)]       //Start in the end
        [InlineData(new []{"a", "b", "c", "d", "alt1", "123", "456"}, 4, MatchOutcome.Partial, 3)]      //Start at the end with partial match
        [InlineData(new []{"a", "alt2", "alt1", "123", "456", "789", "g"}, 1, MatchOutcome.Partial, 1)] //Multiple matches (partial)
        [InlineData(new []{"option", "123", "456", "789", "012", "345", "g"}, 0, MatchOutcome.Full, 4)] //Start after match with extra args
        [InlineData(new []{"a", "b", "c", "d", "e", "f", "g"}, 0, MatchOutcome.Partial, 0)]                //No match
        public void TestOptionWithArgumentsMatches_MultipleArgs(string[] input, int startIdx, MatchOutcome expMatchLength, int expNumMatchTokens)
        {
            ArgumentToken arg1 = new ArgumentToken<int>.Builder().Name("arg1").Parser(int.TryParse).Build();
            ArgumentToken arg2 = new ArgumentToken<int>.Builder().Name("arg2").Parser(int.TryParse).Build();
            ArgumentToken arg3 = new ArgumentToken<int>.Builder().Name("arg3").Parser(int.TryParse).Build();
            var token = new OptionWithArgumentToken.Builder()
                                .Name("option", "alt1", "alt2")
                                .WithArgument(arg1)
                                .WithArgument(arg2)
                                .WithArgument(arg3)
                                .Build();

            TokenMatchResult matchResult = token.Matches(input, startIdx);

            Assert.True(expMatchLength == matchResult.MatchOutcome, $"Failed in case \"{input}\"");
            Assert.Equal(expNumMatchTokens, matchResult.TokensMatched);
        }
    }
}