using System;
using System.Linq;
using BudgetCli.Parser.Interfaces;
using BudgetCli.Parser.Models;
using BudgetCli.Parser.Models.Tokens;
using BudgetCli.Parser.Parsing;
using Xunit;

namespace BudgetCli.Parser.Tests.Parsing
{
    public class CommandParserTests
    {
        private ICommandRoot BuildCommandRoot(params ICommandUsage[] usages)
        {
            var builder =  new CommandRoot.Builder().Name("command")
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

            TokenMatchCollection matchCollection = CommandParser.Match(usage, input);

            Assert.Same(usage, matchCollection.Usage);
            if(expectedMatchIdx.HasValue)
            {
                Assert.NotEmpty(matchCollection.Matches);
                Assert.Same(tokens[expectedMatchIdx.Value], matchCollection.Matches.Last().Token);
                Assert.Equal(expectedMatchIdx, matchCollection.Matches.Max(x => x.TokenIdx));
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

            TokenMatchCollection matchCollection = CommandParser.Match(usage, input);


            Assert.Same(usage, matchCollection.Usage);
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
    }
}