using System.Text.RegularExpressions;
using System;
using System.Linq;
using BudgetCli.Parser.Interfaces;
using BudgetCli.Parser.Models;
using BudgetCli.Parser.Models.Tokens;
using BudgetCli.Parser.Parsing;
using Xunit;
using BudgetCli.Util.Models;

namespace BudgetCli.Parser.Tests.Parsing
{
    public class CommandParserTests
    {
        private ICommandRoot BuildCommandRoot(params ICommandUsage[] usages)
        {
            var builder =  new CommandRoot.Builder().WithToken("command")
                                                    .Description("test command");
            
            foreach(var usage in usages)
            {
                builder.WithUsage(usage);
            }

            return builder.Build();
        }

        private bool StringValueParser(string input, out string value)
        {
            value = input;
            return true;
        }

        [Fact]
        public void TestStringSplitRegex()
        {
            string[] tokens = CommandParser.Tokenize("firstToken \"secondToken\" \"third Token\" 'fourth Token' 'fifth \"token\"' \"sixth 'Token'\"");

            string[] expected = new string[]
            {
                "firstToken",
                "secondToken",
                "third Token",
                "fourth Token",
                "fifth \"token\"",
                "sixth 'Token'"
            };

            Assert.Equal(expected, tokens);
        }

        [Theory]
        [InlineData("verb1", null)]                     //No match
        [InlineData("1234", 0)]                         //Partial
        [InlineData("1234 verb1", 1)]                   //Partial
        [InlineData("1234 verb1 verb2", 2)]             //Partial
        [InlineData("1234 verb1 verb2 verb3", 3)]       //Full match
        [InlineData("1234 verb1 ver", 1)]               //Partial token
        [InlineData("1234 verb1 verb2 verb3 verb4", 3)] //Too many tokens
        public void TestMatch(string input, int? expectedMatchIdx)
        {
            var tokens = new ICommandToken[] 
            {
                new ArgumentToken<int>.Builder()
                                        .Name("name")
                                        .Parser(int.TryParse)
                                        .IsOptional(false)
                                        .Build(),
                new VerbToken(new Name("verb1", "alt1")),
                new VerbToken(new Name("verb2", "alt2")),
                new VerbToken(new Name("verb3", "alt3"))
            };
            var builder = new CommandUsage.Builder()
                                    .Description("test usage");
            foreach(var token in tokens)
            {
                builder.WithToken(token);
            }

            ICommandUsage usage = builder.Build();

            TokenMatchCollection matchCollection = CommandParser.Match(usage.Tokens, input);

            Assert.Same(usage.Tokens, matchCollection.MatchableTokens);
            if(expectedMatchIdx.HasValue)
            {
                Assert.NotEmpty(matchCollection.Matches);
                Assert.Same(tokens[expectedMatchIdx.Value], matchCollection.Matches.Last(x => x.MatchOutcome == Enums.MatchOutcome.Full).Token);
                Assert.Equal(expectedMatchIdx, matchCollection.Matches.Where(x => x.MatchOutcome == Enums.MatchOutcome.Full).Max(x => x.TokenIdx));
            }
            else
            {
                Assert.Empty(matchCollection.Matches);
            }
        }
        

        [Theory]
        [InlineData("nomatch")]                                         //No match
        [InlineData("verb0", 0)]                                        //one verb
        [InlineData("verb0 verb4", 0, 4)]                               //Verbs only (no options)
        [InlineData("verb0 -o1", 0, 1)]                                 //Verb and one option
        [InlineData("verb0 -o2", 0, 2)]                                 //Verb and one option out of order
        [InlineData("verb0 -o2 -o1", 0, 1, 2)]                          //Verb and two options out of order
        [InlineData("verb0 -o2 -o1 verb4", 0, 1, 2, 4)]                 //Verb and two options out of order and verb
        [InlineData("verb0 -o2 --option3 -o1 verb4", 0, 1, 2, 3, 4)]    //All tokens present
        [InlineData("verb0 verb4 -o1 -o2 -o3", 0, 4)]                   //All tokens present, wrong order
        public void TestMatchWithOptionalTokens(string input, params int[] expectedMatchingIndexes)
        {
            var tokens = new ICommandToken[] 
            {
                new VerbToken(new Name("verb0")),
                new StandAloneOptionToken(new Name("-o1", "--option1")),
                new StandAloneOptionToken(new Name("-o2", "--option2")),
                new StandAloneOptionToken(new Name("-o3", "--option3")),
                new VerbToken(new Name("verb4")),
            };
            var builder = new CommandUsage.Builder()
                                    .Description("test usage");
            foreach(var token in tokens)
            {
                builder.WithToken(token);
            }

            ICommandUsage usage = builder.Build();

            TokenMatchCollection matchCollection = CommandParser.Match(usage.Tokens, input);

            Assert.Same(usage.Tokens, matchCollection.MatchableTokens);
            if(expectedMatchingIndexes.Length > 0)
            {
                Assert.NotEmpty(matchCollection.Matches);

                Assert.Equal(expectedMatchingIndexes.Length, matchCollection.Matches.Count());

                //Ensure all that are expected are there
                foreach(var expectedMatchingIndex in expectedMatchingIndexes)
                {
                    Assert.True(matchCollection.Matches.Any(x => x.TokenIdx == expectedMatchingIndex));
                }
            }
            else
            {
                Assert.Empty(matchCollection.Matches);
            }
        }

        [Theory]
        [InlineData("nomatch")]                                                 //No match
        [InlineData("verb0", 0)]                                                //one verb
        [InlineData("verb0 verb4", 0, 4)]                                       //Verbs only (no options)
        [InlineData("verb0 -o1", 0, 1)]                                         //Verb and one option
        [InlineData("verb0 -o2 1 2.34", 0, 2)]                                  //Verb and one option out of order
        [InlineData("verb0 -o2 1", 0, 2)]                                       //Verb and one option with partial args
        [InlineData("verb0 -o2 1.23 4", 0, 2)]                                  //Verb and one option with args in wrong order
        [InlineData("verb0 -o2 1 2.34 -o1", 0, 1, 2)]                           //Verb and two options out of order
        [InlineData("verb0 -o2 1 2.34 -o1 verb4", 0, 1, 2, 4)]                  //Verb and two options out of order and verb
        [InlineData("verb0 -o2 1 2.34 --option3 -o1 verb4", 0, 1, 2, 3, 4)]     //All tokens present
        [InlineData("verb0 verb4 -o1 -o2 1 2.34 -o3", 0, 4)]                    //All tokens present, wrong order
        public void TestMatchWithOptionalTokensWithArgs(string input, params int[] expectedMatchingIndexes)
        {
            ArgumentToken arg1 = new ArgumentToken<int>.Builder().Name("arg1").IsOptional(false).Parser(int.TryParse).Build();
            ArgumentToken arg2 = new ArgumentToken<double>.Builder().Name("arg2").IsOptional(false).Parser(double.TryParse).Build();
            var tokens = new ICommandToken[] 
            {
                new VerbToken(new Name("verb0")),
                new StandAloneOptionToken(new Name("-o1", "--option1")),
                new OptionWithArgumentToken.Builder().Name("-o2", "--option2").WithArgument(arg1).WithArgument(arg2).Build(),
                new StandAloneOptionToken(new Name("-o3", "--option3")),
                new VerbToken(new Name("verb4")),
            };
            var builder = new CommandUsage.Builder()
                                    .Description("test usage");
            foreach(var token in tokens)
            {
                builder.WithToken(token);
            }

            ICommandUsage usage = builder.Build();

            TokenMatchCollection matchCollection = CommandParser.Match(usage.Tokens, input);

            Assert.Same(usage.Tokens, matchCollection.MatchableTokens);
            if(expectedMatchingIndexes.Length > 0)
            {
                Assert.NotEmpty(matchCollection.Matches);

                Assert.Equal(expectedMatchingIndexes.Length, matchCollection.Matches.Count());

                //Ensure all that are expected are there
                foreach(var expectedMatchingIndex in expectedMatchingIndexes)
                {
                    Assert.True(matchCollection.Matches.Any(x => x.TokenIdx == expectedMatchingIndex));
                }
            }
            else
            {
                Assert.Empty(matchCollection.Matches);
            }
        }

        
        [Theory]
        [InlineData("-o2 1 2.34", true)]  //full match
        [InlineData("-o2 1", false)]      //partial match
        public void TestTokenIsFullMatch(string input, bool expIsFullMatch)
        {
            ArgumentToken arg1 = new ArgumentToken<int>.Builder().Name("arg1").IsOptional(false).Parser(int.TryParse).Build();
            ArgumentToken arg2 = new ArgumentToken<double>.Builder().Name("arg2").IsOptional(false).Parser(double.TryParse).Build();
            var tokens = new ICommandToken[] 
            {
                new OptionWithArgumentToken.Builder().Name("-o2", "--option2").WithArgument(arg1).WithArgument(arg2).Build()
            };
            var builder = new CommandUsage.Builder()
                                    .Description("test usage");
            foreach(var token in tokens)
            {
                builder.WithToken(token);
            }

            ICommandUsage usage = builder.Build();

            TokenMatchCollection matchCollection = CommandParser.Match(usage.Tokens, input);

            Assert.Same(usage.Tokens, matchCollection.MatchableTokens);
            Assert.NotEmpty(matchCollection.Matches);

            Assert.Equal(1, matchCollection.Matches.Count());
            ParserTokenMatch match = matchCollection.Matches.First();
            Assert.Equal(expIsFullMatch, match.IsFullMatch);
        }

        

        [Theory]
        [InlineData("option firstArg \"secondArg\" \"Third Arg\"")]
        [InlineData("option firstArg \"secondArg\" \"Third 'Arg'\"")]
        [InlineData("option firstArg \"secondArg\" 'Third \"Arg\"'")]
        [InlineData("option firstArg \"secondArg\" 'Third Arg")]
        public void TestOptionWithQuotedString(string input)
        {
            ICommandArgumentToken<string>.ValueParser trivialStringParser = (string i, out string v) => { v=i; return true; };

            ArgumentToken arg1 = new ArgumentToken<string>.Builder().Name("arg1").Parser(trivialStringParser).Build();
            ArgumentToken arg2 = new ArgumentToken<string>.Builder().Name("arg2").Parser(trivialStringParser).Build();
            ArgumentToken arg3 = new ArgumentToken<string>.Builder().Name("arg3").Parser(trivialStringParser).Build();
            var token = new OptionWithArgumentToken.Builder()
                                .Name("option", "alt1", "alt2")
                                .WithArgument(arg1)
                                .WithArgument(arg2)
                                .WithArgument(arg3)
                                .Build();

            ICommandUsage usage = new CommandUsage.Builder().WithToken(token).Build();

            TokenMatchCollection matchCollection = CommandParser.Match(usage.Tokens, input);

            Assert.True(matchCollection.Matches.Count() == 1);
            ParserTokenMatch match = matchCollection.Matches.First();
            Assert.True(match.IsFullMatch);
        }

        [Theory]
        [InlineData("option")]
        [InlineData("option firstArg")]
        [InlineData("option firstArg \"secondArg\"")]
        public void TestOptionWithQuotedStringNotFullMatch(string input)
        {
            ICommandArgumentToken<string>.ValueParser trivialStringParser = (string i, out string v) => { v=i; return true; };

            ArgumentToken arg1 = new ArgumentToken<string>.Builder().Name("arg1").Parser(trivialStringParser).Build();
            ArgumentToken arg2 = new ArgumentToken<string>.Builder().Name("arg2").Parser(trivialStringParser).Build();
            ArgumentToken arg3 = new ArgumentToken<string>.Builder().Name("arg3").Parser(trivialStringParser).Build();
            var token = new OptionWithArgumentToken.Builder()
                                .Name("option", "alt1", "alt2")
                                .WithArgument(arg1)
                                .WithArgument(arg2)
                                .WithArgument(arg3)
                                .Build();

            ICommandUsage usage = new CommandUsage.Builder().WithToken(token).Build();

            TokenMatchCollection matchCollection = CommandParser.Match(usage.Tokens, input);

            Assert.True(matchCollection.Matches.Count() == 1);
            ParserTokenMatch match = matchCollection.Matches.First();
            Assert.False(match.IsFullMatch, input);
        }

        [Fact]
        public void TestMatchArgValuesWithOptionalTokens()
        {
            ArgumentToken arg1 = new ArgumentToken<int>.Builder().Name("arg1").IsOptional(false).Parser(int.TryParse).Build();
            ArgumentToken arg2 = new ArgumentToken<double>.Builder().Name("arg2").IsOptional(false).Parser(double.TryParse).Build();
            var tokens = new ICommandToken[] 
            {
                new VerbToken(new Name("verb0")),
                new OptionWithArgumentToken.Builder().Name("-o2", "--option2").WithArgument(arg1).WithArgument(arg2).Build(),
            };
            
            var builder = new CommandUsage.Builder().Description("test usage");

            foreach(var token in tokens)
            {
                builder.WithToken(token);
            }

            ICommandUsage usage = builder.Build();

            TokenMatchCollection matchCollection = CommandParser.Match(usage.Tokens, "verb0 -o2 1 2.34");

            int arg1Value;
            bool arg1Exists = matchCollection.TryGetArgValue(arg1, out arg1Value);

            double arg2Value;
            bool arg2Exists = matchCollection.TryGetArgValue(arg2, out arg2Value);

            Assert.True(arg1Exists);
            Assert.True(arg2Exists);
            Assert.Equal(1, arg1Value);
            Assert.Equal(2.34, arg2Value);
        }

        [Theory]
        [InlineData("5 verb1 verb2", 1, 2, 3)]
        [InlineData("5", null, 0, 1)]
        [InlineData("5 verb1 verb2 verb3", 2, 3, null
        )]
        public void TestAdjacentTokens(string input, int? prevIdx, int? currentIdx, int? nextIdx)
        {
            var tokens = new ICommandToken[] 
            {
                new ArgumentToken<int>.Builder()
                                        .Name("name")
                                        .Parser(int.TryParse)
                                        .IsOptional(false)
                                        .Build(),
                new VerbToken(new Name("verb1", "alt1")),
                new VerbToken(new Name("verb2", "alt2")),
                new VerbToken(new Name("verb3", "alt3"))
            };
            var builder = new CommandUsage.Builder()
                                    .Description("test usage");
            foreach(var token in tokens)
            {
                builder.WithToken(token);
            }

            ICommandUsage usage = builder.Build();

            ICommandToken currentToken = CommandParser.GetCurrentToken(usage, input);
            ICommandToken prevToken = CommandParser.GetPreviousToken(usage, input);
            ICommandToken nextToken = CommandParser.GetNextToken(usage, input);

            if(prevIdx.HasValue)
            {
                Assert.Same(tokens[prevIdx.Value], prevToken);
            }
            else
            {
                Assert.Null(prevToken);
            }
            
            if(currentIdx.HasValue)
            {
                Assert.Same(tokens[currentIdx.Value], currentToken);
            }
            else
            {
                Assert.Null(currentToken);
            }
            
            if(nextIdx.HasValue)
            {
                Assert.Same(tokens[nextIdx.Value], nextToken);
            }
            else
            {
                Assert.Null(nextToken);
            }
        }
    }
}