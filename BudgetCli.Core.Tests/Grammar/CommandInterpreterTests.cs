using System;
using System.Linq;
using System.Collections.Generic;
using Xunit;
using BudgetCli.Core.Grammar;
using BudgetCli.Parser.Parsing;
using BudgetCli.Parser.Models;
using BudgetCli.Parser.Interfaces;
using BudgetCli.Parser.Models.Tokens;
using Moq;
using BudgetCli.Data.Repositories;
using BudgetCli.Data.Util;
using BudgetCli.Data.Tests.TestHarness;
using BudgetCli.Util.Logging;
using BudgetCli.Core.Models.Commands;
using BudgetCli.Core.Models.Commands.Accounts;

namespace BudgetCli.Core.Tests.Grammar
{
    public class CommandInterpreterTests
    {

        class FakeCommandUsageMatchData : CommandUsageMatchData
        {
            public override bool IsSuccessful { get { return true; } }

            private Dictionary<ArgumentToken, object> _argValues;
            public FakeCommandUsageMatchData(ICommandRoot command, ICommandUsage usage, Dictionary<ArgumentToken, object> argValues)
                : base(command, usage, null)
            {
                _argValues = argValues;
            }

            public override bool HasToken(ICommandToken token)
            {
                return true;
            }

            public override bool TryGetArgValue<T>(ArgumentToken argument, out T value)
            {
                if(_argValues.ContainsKey(argument))
                {
                    value = (T)_argValues[argument];
                    return true;
                }
                value = default(T);
                return false;
            }

            public override bool TryGetArgValue<T>(string argName, out T value)
            {
                foreach(var kvp in _argValues)
                {
                    if(kvp.Key.ArgumentName.Equals(argName))
                    {
                        value = (T)kvp.Value;
                        return true;
                    }
                }

                value = default(T);
                return false;
            }
        }

        private ICommandToken[] GetMatchableTokens(ICommandRoot commandRoot, ICommandUsage usage)
        {
            return commandRoot.CommonTokens.Concat(usage.Tokens).ToArray();
        }

        [Fact]
        public void AddAccountCommand()
        {
            using(var testDbInfo = SetupUtil.CreateTestDb())
            {
                Mock<ILog> mockLog = new Mock<ILog>();

                ICommandRoot root = BudgetCliCommands.CMD_NEW_ACCOUNT;
                ICommandUsage usage = root.Usages.First(x => !x.IsHelp);
                ICommandToken[] matchableTokens = GetMatchableTokens(root, usage);

                Dictionary<ArgumentToken, object> argValues = new Dictionary<ArgumentToken, object>();
                argValues.Add(BudgetCliCommands.OPT_ACCOUNT_NAME.Arguments[0], "name");
                FakeCommandUsageMatchData matchData = new FakeCommandUsageMatchData(root, usage, argValues);

                CommandInterpreter interpreter = new CommandInterpreter(SetupUtil.CreateMockRepositoryBag(testDbInfo.ConnectionString, mockLog.Object), BudgetCliCommands.BuildCommandLibrary());

                ICommandAction action;
                bool success = interpreter.TryParseCommand("new account \"name\"", out action);

                Assert.True(success);
                Assert.IsType<AddAccountCommand>(action);

                AddAccountCommand addAccount = (AddAccountCommand)action;
                Assert.Equal("name", addAccount.AccountName.GetValue(null));
            }
        }
    }
}