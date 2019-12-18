using BudgetCli.Core.Models;
using BudgetCli.Data.Models;
using BudgetCli.Data.Repositories;
using BudgetCli.Data.Tests.TestHarness;
using BudgetCli.Util.Logging;
using BudgetCli.Util.Models;
using Moq;
using Xunit;

namespace BudgetCli.Core.Tests.Models
{
    public class DtoToModelTranslatorTests
    {
        [Fact]
        public void FromDto_Account()
        {
            using(var testDbInfo = SetupUtil.CreateTestDb())
            {
                //Arrange
                Mock<ILog> mockLog = new Mock<ILog>();
                AccountDto accountDto = new AccountDto()
                {
                    Id = 1,
                    Name = "Test Account",
                    AccountKind = Data.Enums.AccountKind.Source,
                    CategoryId = null,
                    Description = "",
                    Priority = 7
                };

                RepositoryBag repositories = SetupUtil.CreateMockRepositoryBag(testDbInfo.ConnectionString, mockLog.Object);
                
                //Act
                Account account = DtoToModelTranslator.FromDto(accountDto, repositories);
                
                //Assert
                Assert.Equal("Test Account", account.Name);
                Assert.Null(account.CategoryId);
                Assert.Equal(7, account.Priority);
                Assert.Equal(Data.Enums.AccountKind.Source, account.AccountKind);
            }
        }
    }
}