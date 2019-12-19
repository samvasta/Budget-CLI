using System;
using System.Linq;
using BudgetCli.Core.Models;
using BudgetCli.Core.Models.Commands.Accounts;
using BudgetCli.Data.Models;
using BudgetCli.Data.Repositories;
using BudgetCli.Data.Tests.TestHarness;
using BudgetCli.Util.Logging;
using Moq;
using Xunit;

namespace BudgetCli.Core.Tests.Models.Commands.Accounts
{
    public class DeleteAccountCommandTests
    {
        [Fact]
        public void TestDeleteAccountActionExecute()
        {
            using(var testDbInfo = SetupUtil.CreateTestDb())
            {
                //Arrange
                Mock<ILog> mockLog = new Mock<ILog>();
                AccountRepository repo = new AccountRepository(testDbInfo.ConnectionString, mockLog.Object);
                AccountStateRepository accountStateRepo = new AccountStateRepository(testDbInfo.ConnectionString, mockLog.Object);
                RepositoryBag repositories = SetupUtil.CreateMockRepositoryBag(testDbInfo.ConnectionString, mockLog.Object, repo, accountStateRepo);

                AccountDto accountDto = new AccountDto()
                {
                    AccountKind = Data.Enums.AccountKind.Sink,
                    CategoryId = null,
                    Description = "test account",
                    Name = "Test Account",
                    Priority = 5
                };
                bool isInsertSuccessful = repo.Upsert(accountDto);
                long accountId = accountDto.Id.Value;

                int accountCountBeforeDelete = repo.GetAll().Count();
                AccountStateDto stateDto = new AccountStateDto()
                {
                    AccountId = accountId,
                    Funds = 0,
                    Timestamp = DateTime.Now,
                    IsClosed = false
                };
                isInsertSuccessful &= accountStateRepo.Upsert(stateDto);
                int stateCountBeforeDelete = accountStateRepo.GetAll().Count();


                DeleteAccountCommand action = new DeleteAccountCommand("rm account \"Test Account\"", repositories);
                action.AccountName.SetData("Test Account");
                action.IsRecursiveOption.SetData(false);
                
                //Act
                bool successful = action.TryExecute(mockLog.Object);

                int accountCountAfterDelete = repo.GetAll().Count();
                int stateCountAfterDelete = accountStateRepo.GetAll().Count();

                bool isClosed = accountStateRepo.GetLatestByAccountId(accountId).IsClosed;
                
                //Assert
                Assert.True(isInsertSuccessful);
                Assert.True(successful);
                Assert.Equal(1, accountCountBeforeDelete);
                Assert.Equal(1, accountCountAfterDelete);
                Assert.Equal(1, stateCountBeforeDelete);
                Assert.Equal(2, stateCountAfterDelete);
                Assert.True(isClosed);
            }
        }
    }
}