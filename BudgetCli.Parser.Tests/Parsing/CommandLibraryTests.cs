using System;
using System.Linq;
using BudgetCli.Parser.Interfaces;
using BudgetCli.Parser.Models;
using BudgetCli.Parser.Models.Tokens;
using BudgetCli.Parser.Parsing;
using BudgetCli.Parser.Util;
using BudgetCli.Util.Models;
using Moq;
using Xunit;

namespace BudgetCli.Parser.Tests.Parsing
{
    public class CommandLibraryTests
    {
        [Fact]
        public void TestAddCommand()
        {
            Mock<ICommandRoot> commandRoot = new Mock<ICommandRoot>();
            CommandLibrary library = new CommandLibrary();

            library.AddCommand(commandRoot.Object);

            Assert.Contains(commandRoot.Object, library.GetAllCommands());
        }

        [Fact]
        public void TestGetAllCommands()
        {
            VerbToken token = new VerbToken(new Name("verb", "v"));
            Mock<ICommandRoot> commandRoot1 = new Mock<ICommandRoot>();
            commandRoot1.SetupGet(x => x.CommonTokens).Returns(new []{token});

            CommandLibrary library = new CommandLibrary();
            library.AddCommand(commandRoot1.Object);

            var names = library.GetAllCommands();

            Assert.Equal(1, names.Count());
            Assert.Equal(commandRoot1.Object, names.First());
        }

        [Fact]
        public void TestGetAllCommandNames()
        {
            VerbToken token = new VerbToken(new Name("verb", "v"));
            Mock<ICommandRoot> commandRoot1 = new Mock<ICommandRoot>();
            commandRoot1.SetupGet(x => x.CommonTokens).Returns(new []{token});

            CommandLibrary library = new CommandLibrary();
            library.AddCommand(commandRoot1.Object);

            var names = library.GetAllCommandNames();

            Assert.Equal(1, names.Count());
            Assert.Equal("verb", names.First());
        }

        [Fact]
        public void TestTryGetCommand()
        {
            VerbToken token1 = new VerbToken(new Name("verb1", "a"));
            Mock<ICommandRoot> commandRoot1 = new Mock<ICommandRoot>();

            VerbToken token2 = new VerbToken(new Name("verb2", "b"));
            Mock<ICommandRoot> commandRoot2 = new Mock<ICommandRoot>();
            
            VerbToken token3 = new VerbToken(new Name("verb3", "c"));
            Mock<ICommandRoot> commandRoot3 = new Mock<ICommandRoot>();

            commandRoot1.SetupGet(x => x.CommonTokens).Returns(new []{token1});
            commandRoot2.SetupGet(x => x.CommonTokens).Returns(new []{token2});
            commandRoot3.SetupGet(x => x.CommonTokens).Returns(new []{token3});

            CommandLibrary library = new CommandLibrary();
            library.AddCommand(commandRoot1.Object);
            library.AddCommand(commandRoot2.Object);
            library.AddCommand(commandRoot3.Object);

            ICommandRoot cmd1;
            bool success1 = library.TryGetCommand("verb3", out cmd1);
            ICommandRoot cmd2;
            bool success2 = library.TryGetCommand("b", out cmd2);

            Assert.True(success1);
            Assert.Equal(commandRoot3.Object, cmd1);

            Assert.True(success2);
            Assert.Equal(commandRoot2.Object, cmd2);
        }

        [Fact]
        public void TestGetCommandSuggestions()
        {
            VerbToken token1 = new VerbToken(new Name("verb1", "a"));
            Mock<ICommandRoot> commandRoot1 = new Mock<ICommandRoot>();

            VerbToken token2 = new VerbToken(new Name("verb11", "b"));
            Mock<ICommandRoot> commandRoot2 = new Mock<ICommandRoot>();
            
            VerbToken token3 = new VerbToken(new Name("verb111", "c"));
            Mock<ICommandRoot> commandRoot3 = new Mock<ICommandRoot>();

            commandRoot1.SetupGet(x => x.CommonTokens).Returns(new []{token1});
            commandRoot2.SetupGet(x => x.CommonTokens).Returns(new []{token2});
            commandRoot3.SetupGet(x => x.CommonTokens).Returns(new []{token3});

            CommandLibrary library = new CommandLibrary();
            library.AddCommand(commandRoot1.Object);
            library.AddCommand(commandRoot2.Object);
            library.AddCommand(commandRoot3.Object);

            var suggestions = library.GetCommandSuggestions("verb");

            var orderedCommands = suggestions.OrderByDescending(x => x.Value).Select(x => x.Key);

            Assert.Equal(commandRoot1.Object, orderedCommands.First());
            Assert.Equal(commandRoot2.Object, orderedCommands.Skip(1).First());
            Assert.Equal(commandRoot3.Object, orderedCommands.Skip(2).First());
        }

        [Fact]
        public void TestParse()
        {
            VerbToken token1 = new VerbToken(new Name("verb1", "a"));

            ArgumentToken token2 = new ArgumentToken<int>.Builder().Name("arg").Parser(int.TryParse).IsOptional(false).Build();
            
            CommandRoot command1 = new CommandRoot.Builder()
                .Id(0)
                .Description("command")
                .WithToken(token1)
                .WithUsage(new CommandUsage.Builder()
                    .Description("usage1")
                    .WithToken(token2)
                    .Build())
                .Build();

            CommandLibrary library = new CommandLibrary();
            library.AddCommand(command1);

            CommandUsageMatchData matchData = library.Parse("verb1 123");
            
            Assert.True(matchData.IsSuccessful);
            Assert.True(matchData.HasToken(token1));
            Assert.True(matchData.HasToken(token2));

            bool hasArgVal = matchData.TryGetArgValue(token2, out int argVal);
            Assert.True(hasArgVal);
            Assert.Equal(123, argVal);
        }

        [Fact]
        public void TestParseWithOptionalArg()
        {
            VerbToken token1 = new VerbToken(new Name("verb1", "a"));

            ArgumentToken token2 = new ArgumentToken<int>.Builder().Name("arg").Parser(int.TryParse).IsOptional(false).Build();
            ArgumentToken token3 = new ArgumentToken<int>.Builder().Name("arg2").Parser(int.TryParse).IsOptional(true).Build();
            
            CommandRoot command1 = new CommandRoot.Builder()
                .Id(0)
                .Description("command")
                .WithToken(token1)
                .WithUsage(new CommandUsage.Builder()
                    .Description("usage1")
                    .WithToken(token2)
                    .WithToken(token3)
                    .Build())
                .Build();

            CommandLibrary library = new CommandLibrary();
            library.AddCommand(command1);

            CommandUsageMatchData matchData = library.Parse("verb1 123");
            
            Assert.True(matchData.IsSuccessful);
            Assert.True(matchData.HasToken(token1));
            Assert.True(matchData.HasToken(token2));

            bool hasArg1Val = matchData.TryGetArgValue(token2, out int arg1Val);
            Assert.True(hasArg1Val);
            Assert.Equal(123, arg1Val);

            bool hasArg2Val = matchData.TryGetArgValue(token3, out int arg2Val);
            Assert.False(hasArg2Val);
        }

        [Fact]
        public void TestParseWithStringArg()
        {
            VerbToken token1 = new VerbToken(new Name("verb1", "a"));

            ArgumentToken token2 = TokenUtils.BuildArgString("arg1");
            
            CommandRoot command1 = new CommandRoot.Builder()
                .Id(0)
                .Description("command")
                .WithToken(token1)
                .WithUsage(new CommandUsage.Builder()
                    .Description("usage1")
                    .WithToken(token2)
                    .Build())
                .Build();

            CommandLibrary library = new CommandLibrary();
            library.AddCommand(command1);

            CommandUsageMatchData matchData = library.Parse("verb1 \"the entire string\"");
            
            Assert.True(matchData.IsSuccessful);
            Assert.True(matchData.HasToken(token1));
            Assert.True(matchData.HasToken(token2));

            bool hasArg1Val = matchData.TryGetArgValue(token2, out string arg1Val);
            Assert.True(hasArg1Val);
            Assert.Equal("the entire string", arg1Val);
        }
    }
}