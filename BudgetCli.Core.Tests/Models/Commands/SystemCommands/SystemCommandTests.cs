using System.Collections.Generic;
using BudgetCli.Core.Enums;
using BudgetCli.Core.Grammar;
using BudgetCli.Core.Models.CommandResults;
using BudgetCli.Core.Models.Commands.SystemCommands;
using BudgetCli.Core.Models.Interfaces;
using BudgetCli.Data.Repositories;
using BudgetCli.Data.Tests.TestHarness;
using BudgetCli.Util.Logging;
using Moq;
using Xunit;

namespace BudgetCli.Core.Tests.Models.Commands.SystemCommands
{
    public class SystemCommandTests
    {
        [Fact]
        public void TestTryDoAction()
        {
            using(var testDbInfo = SetupUtil.CreateTestDb())
            {
                Mock<ILog> mockLog = new Mock<ILog>();
                RepositoryBag repositories = SetupUtil.CreateMockRepositoryBag(testDbInfo.ConnectionString, mockLog.Object);
                
                SystemCommand command = new SystemCommand("version", CommandKind.Version);

                List<ICommandActionListener> listeners = new List<ICommandActionListener>();

                Mock<ICommandActionListener> mockListener = new Mock<ICommandActionListener>();
                mockListener.Setup(x => x.OnCommand(It.Is<SystemCommandResult>(x => x.CommandKind == CommandKind.Version && x.IsSuccessful && x.CommandAction == command))).Verifiable();

                listeners.Add(mockListener.Object);

                command.TryExecute(mockLog.Object, listeners);

                mockListener.VerifyAll();
            }
        }
        
    }
}