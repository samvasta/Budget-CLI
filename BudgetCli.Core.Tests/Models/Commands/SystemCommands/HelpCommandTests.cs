using System;
using System.Collections.Generic;
using System.Linq;
using BudgetCli.Core.Models;
using BudgetCli.Core.Models.CommandResults;
using BudgetCli.Core.Models.Commands.Accounts;
using BudgetCli.Core.Models.Commands.SystemCommands;
using BudgetCli.Core.Models.Interfaces;
using BudgetCli.Data.Enums;
using BudgetCli.Data.Models;
using BudgetCli.Data.Repositories;
using BudgetCli.Data.Tests.TestHarness;
using BudgetCli.Util.Logging;
using Moq;
using Xunit;

namespace BudgetCli.Core.Tests.Models.Commands.SystemCommands
{
    public class HelpCommandTests
    {
        [Fact]
        public void TestTryDoAction()
        {
            using(var testDbInfo = SetupUtil.CreateTestDb())
            {
                Mock<ILog> mockLog = new Mock<ILog>();
                RepositoryBag repositories = SetupUtil.CreateMockRepositoryBag(testDbInfo.ConnectionString, mockLog.Object);
                
                HelpCommand command = new HelpCommand("new account -h", repositories, CommandActionKind.AddAccount);

                List<ICommandActionListener> listeners = new List<ICommandActionListener>();

                Mock<ICommandActionListener> mockListener = new Mock<ICommandActionListener>();
                mockListener.Setup(x => x.OnCommand(It.IsAny<SystemCommandResult>())).Verifiable();

                listeners.Add(mockListener.Object);

                command.TryExecute(mockLog.Object, listeners);

                mockListener.VerifyAll();
            }
        }        
    }
}