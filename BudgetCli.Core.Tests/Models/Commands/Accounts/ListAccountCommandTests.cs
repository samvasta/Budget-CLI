using System;
using System.Windows.Input;
using System.Reflection.PortableExecutable;
using System.Collections.Generic;
using System.Linq;
using BudgetCli.Core.Models;
using BudgetCli.Core.Models.CommandResults;
using BudgetCli.Core.Models.Commands.Accounts;
using BudgetCli.Core.Models.Interfaces;
using BudgetCli.Data.Models;
using BudgetCli.Data.Repositories;
using BudgetCli.Data.Tests.TestHarness;
using BudgetCli.Util.Logging;
using BudgetCli.Util.Models;
using Moq;
using Xunit;
using BudgetCli.Data.Enums;
using BudgetCli.Core.Tests.TestHarness;

namespace BudgetCli.Core.Tests.Models.Commands.Accounts
{
    public class ListAccountCommandTests
    {
        private void UpsertAccount(AccountDto dto, RepositoryBag repositories, double initialFunds = 0.0)
        {
            repositories.AccountRepository.Upsert(dto);
            repositories.AccountStateRepository.Upsert(new AccountStateDto(){
                AccountId = dto.Id.Value,
                IsClosed = false,
                Funds = ((Money)initialFunds).InternalValue,
                Timestamp = DateTime.Now
            });
        }

        [Fact]
        public void ListAccountCommandActionKind()
        {
            ListAccountCommand action = new ListAccountCommand("ls accounts", SetupUtil.CreateMockRepositoryBag(string.Empty, null));
            Assert.Equal(CommandActionKind.ListAccount, action.CommandActionKind);
        }

        [Fact]
        public void ListAccountCommand_NullListeners()
        {
            using(var testDbInfo = SetupUtil.CreateTestDb())
            {
                //Arrange
                Mock<ILog> mockLog = new Mock<ILog>();
                AccountRepository repo = new AccountRepository(testDbInfo.ConnectionString, mockLog.Object);
                AccountStateRepository accountStateRepo = new AccountStateRepository(testDbInfo.ConnectionString, mockLog.Object);
                RepositoryBag repositories = SetupUtil.CreateMockRepositoryBag(testDbInfo.ConnectionString, mockLog.Object, repo, accountStateRepo);

                ListAccountCommand action = new ListAccountCommand("ls accounts", repositories);

                //Act
                bool successful = action.TryExecute(mockLog.Object, null);

                //Passes automatically if no exceptions were thrown
            }
        }

        [Fact]
        public void ListAccountCommandExecute()
        {
            using(var testDbInfo = SetupUtil.CreateTestDb())
            {
                //Arrange
                Mock<ILog> mockLog = new Mock<ILog>();
                AccountRepository repo = new AccountRepository(testDbInfo.ConnectionString, mockLog.Object);
                AccountStateRepository accountStateRepo = new AccountStateRepository(testDbInfo.ConnectionString, mockLog.Object);
                RepositoryBag repositories = SetupUtil.CreateMockRepositoryBag(testDbInfo.ConnectionString, mockLog.Object, repo, accountStateRepo);

                AccountDto account1 = new AccountDto(){
                    Id = null,
                    AccountKind = AccountKind.Sink,
                    Name = "account1",
                    Priority = 5,
                    Description = "account1 description",
                    CategoryId = null
                };
                AccountDto account2 = new AccountDto(){
                    Id = null,
                    AccountKind = AccountKind.Sink,
                    Name = "account2",
                    Priority = 5,
                    Description = "account2 description",
                    CategoryId = null
                };

                UpsertAccount(account1, repositories);
                UpsertAccount(account2, repositories);

                ListAccountCommand action = new ListAccountCommand("ls accounts", repositories);

                FakeCommandActionListener listener = new FakeCommandActionListener();

                //Act
                bool successful = action.TryExecute(mockLog.Object, new ICommandActionListener[]{listener});

                IEnumerable<ReadCommandResult<Account>> results = listener.GetResults<ReadCommandResult<Account>>();
                
                IEnumerable<Account> accounts = results.First().FilteredItems;

                //Assert
                Assert.True(successful);
                Assert.True(listener.OnlyHasType<ReadCommandResult<Account>>());
                Assert.Equal(1, results.Count());
                Assert.Equal(2, accounts.Count());

                Assert.Contains(DtoToModelTranslator.FromDto(account1, DateTime.Today, repositories), accounts);
                Assert.Contains(DtoToModelTranslator.FromDto(account2, DateTime.Today, repositories), accounts);
            }
        }
        
        [Fact]
        public void ListAccountCommandExecute_NameOption()
        {
            using(var testDbInfo = SetupUtil.CreateTestDb())
            {
                //Arrange
                Mock<ILog> mockLog = new Mock<ILog>();
                AccountRepository repo = new AccountRepository(testDbInfo.ConnectionString, mockLog.Object);
                AccountStateRepository accountStateRepo = new AccountStateRepository(testDbInfo.ConnectionString, mockLog.Object);
                RepositoryBag repositories = SetupUtil.CreateMockRepositoryBag(testDbInfo.ConnectionString, mockLog.Object, repo, accountStateRepo);

                AccountDto account1 = new AccountDto(){
                    Id = null,
                    AccountKind = AccountKind.Sink,
                    Name = "account1",
                    Priority = 5,
                    Description = "account1 description",
                    CategoryId = null
                };
                AccountDto account2 = new AccountDto(){
                    Id = null,
                    AccountKind = AccountKind.Sink,
                    Name = "account2",
                    Priority = 5,
                    Description = "account2 description",
                    CategoryId = null
                };

                UpsertAccount(account1, repositories);
                UpsertAccount(account2, repositories);

                ListAccountCommand action = new ListAccountCommand("ls accounts", repositories);
                action.NameOption.SetData("1");

                FakeCommandActionListener listener = new FakeCommandActionListener();

                //Act
                bool successful = action.TryExecute(mockLog.Object, new ICommandActionListener[]{listener});

                IEnumerable<ReadCommandResult<Account>> results = listener.GetResults<ReadCommandResult<Account>>();
                
                IEnumerable<Account> accounts = results.First().FilteredItems;

                //Assert
                Assert.True(successful);
                Assert.True(listener.OnlyHasType<ReadCommandResult<Account>>());
                Assert.Equal(1, results.Count());
                Assert.Equal(1, accounts.Count());

                Assert.Contains(DtoToModelTranslator.FromDto(account1, DateTime.Today, repositories), accounts);
                Assert.DoesNotContain(DtoToModelTranslator.FromDto(account2, DateTime.Today, repositories), accounts);
            }
        }
        
        [Fact]
        public void ListAccountCommandExecute_DescriptionOption()
        {
            using(var testDbInfo = SetupUtil.CreateTestDb())
            {
                //Arrange
                Mock<ILog> mockLog = new Mock<ILog>();
                AccountRepository repo = new AccountRepository(testDbInfo.ConnectionString, mockLog.Object);
                AccountStateRepository accountStateRepo = new AccountStateRepository(testDbInfo.ConnectionString, mockLog.Object);
                RepositoryBag repositories = SetupUtil.CreateMockRepositoryBag(testDbInfo.ConnectionString, mockLog.Object, repo, accountStateRepo);

                AccountDto account1 = new AccountDto(){
                    Id = null,
                    AccountKind = AccountKind.Sink,
                    Name = "account1",
                    Priority = 5,
                    Description = "account1 description",
                    CategoryId = null
                };
                AccountDto account2 = new AccountDto(){
                    Id = null,
                    AccountKind = AccountKind.Sink,
                    Name = "account2",
                    Priority = 5,
                    Description = "account2 description",
                    CategoryId = null
                };

                UpsertAccount(account1, repositories);
                UpsertAccount(account2, repositories);

                ListAccountCommand action = new ListAccountCommand("ls accounts", repositories);
                action.DescriptionOption.SetData("account1");

                FakeCommandActionListener listener = new FakeCommandActionListener();

                //Act
                bool successful = action.TryExecute(mockLog.Object, new ICommandActionListener[]{listener});

                IEnumerable<ReadCommandResult<Account>> results = listener.GetResults<ReadCommandResult<Account>>();
                
                IEnumerable<Account> accounts = results.First().FilteredItems;

                //Assert
                Assert.True(successful);
                Assert.True(listener.OnlyHasType<ReadCommandResult<Account>>());
                Assert.Equal(1, results.Count());
                Assert.Equal(1, accounts.Count());

                Assert.Contains(DtoToModelTranslator.FromDto(account1, DateTime.Today, repositories), accounts);
                Assert.DoesNotContain(DtoToModelTranslator.FromDto(account2, DateTime.Today, repositories), accounts);
            }
        }
        
        [Fact]
        public void ListAccountCommandExecute_PriorityOption()
        {
            using(var testDbInfo = SetupUtil.CreateTestDb())
            {
                //Arrange
                Mock<ILog> mockLog = new Mock<ILog>();
                AccountRepository repo = new AccountRepository(testDbInfo.ConnectionString, mockLog.Object);
                AccountStateRepository accountStateRepo = new AccountStateRepository(testDbInfo.ConnectionString, mockLog.Object);
                RepositoryBag repositories = SetupUtil.CreateMockRepositoryBag(testDbInfo.ConnectionString, mockLog.Object, repo, accountStateRepo);

                AccountDto account1 = new AccountDto(){
                    Id = null,
                    AccountKind = AccountKind.Sink,
                    Name = "account1",
                    Priority = 5,
                    Description = "account1 description",
                    CategoryId = null
                };
                AccountDto account2 = new AccountDto(){
                    Id = null,
                    AccountKind = AccountKind.Sink,
                    Name = "account2",
                    Priority = 10,
                    Description = "account2 description",
                    CategoryId = null
                };

                UpsertAccount(account1, repositories);
                UpsertAccount(account2, repositories);

                ListAccountCommand action = new ListAccountCommand("ls accounts", repositories);
                action.PriorityOption.SetData(new Range<long>(4, 6));

                FakeCommandActionListener listener = new FakeCommandActionListener();

                //Act
                bool successful = action.TryExecute(mockLog.Object, new ICommandActionListener[]{listener});

                IEnumerable<ReadCommandResult<Account>> results = listener.GetResults<ReadCommandResult<Account>>();
                
                IEnumerable<Account> accounts = results.First().FilteredItems;

                //Assert
                Assert.True(successful);
                Assert.True(listener.OnlyHasType<ReadCommandResult<Account>>());
                Assert.Equal(1, results.Count());
                Assert.Equal(1, accounts.Count());

                Assert.Contains(DtoToModelTranslator.FromDto(account1, DateTime.Today, repositories), accounts);
                Assert.DoesNotContain(DtoToModelTranslator.FromDto(account2, DateTime.Today, repositories), accounts);
            }
        }
        
        [Fact]
        public void ListAccountCommandExecute_CategoryOption()
        {
            using(var testDbInfo = SetupUtil.CreateTestDb())
            {
                //Arrange
                Mock<ILog> mockLog = new Mock<ILog>();
                AccountRepository repo = new AccountRepository(testDbInfo.ConnectionString, mockLog.Object);
                AccountStateRepository accountStateRepo = new AccountStateRepository(testDbInfo.ConnectionString, mockLog.Object);
                RepositoryBag repositories = SetupUtil.CreateMockRepositoryBag(testDbInfo.ConnectionString, mockLog.Object, repo, accountStateRepo);

                AccountDto account1 = new AccountDto(){
                    Id = null,
                    AccountKind = AccountKind.Sink,
                    Name = "account1",
                    Priority = 5,
                    Description = "account1 description",
                    CategoryId = null
                };
                UpsertAccount(account1, repositories);
                AccountDto account2 = new AccountDto(){
                    Id = null,
                    AccountKind = AccountKind.Sink,
                    Name = "account2",
                    Priority = 5,
                    Description = "account2 description",
                    CategoryId = account1.Id
                };
                UpsertAccount(account2, repositories);

                ListAccountCommand action = new ListAccountCommand("ls accounts", repositories);
                action.CategoryIdOption.SetData(account1.Id.Value);

                FakeCommandActionListener listener = new FakeCommandActionListener();

                //Act
                bool successful = action.TryExecute(mockLog.Object, new ICommandActionListener[]{listener});

                IEnumerable<ReadCommandResult<Account>> results = listener.GetResults<ReadCommandResult<Account>>();
                
                IEnumerable<Account> accounts = results.First().FilteredItems;

                //Assert
                Assert.True(successful);
                Assert.True(listener.OnlyHasType<ReadCommandResult<Account>>());
                Assert.Equal(1, results.Count());
                Assert.Equal(1, accounts.Count());

                Assert.DoesNotContain(DtoToModelTranslator.FromDto(account1, DateTime.Today, repositories), accounts);
                Assert.Contains(DtoToModelTranslator.FromDto(account2, DateTime.Today, repositories), accounts);
            }
        }
        
        [Fact]
        public void ListAccountCommandExecute_IdOption()
        {
            using(var testDbInfo = SetupUtil.CreateTestDb())
            {
                //Arrange
                Mock<ILog> mockLog = new Mock<ILog>();
                AccountRepository repo = new AccountRepository(testDbInfo.ConnectionString, mockLog.Object);
                AccountStateRepository accountStateRepo = new AccountStateRepository(testDbInfo.ConnectionString, mockLog.Object);
                RepositoryBag repositories = SetupUtil.CreateMockRepositoryBag(testDbInfo.ConnectionString, mockLog.Object, repo, accountStateRepo);

                AccountDto account1 = new AccountDto(){
                    Id = null,
                    AccountKind = AccountKind.Sink,
                    Name = "account1",
                    Priority = 5,
                    Description = "account1 description",
                    CategoryId = null
                };
                UpsertAccount(account1, repositories);
                AccountDto account2 = new AccountDto(){
                    Id = null,
                    AccountKind = AccountKind.Sink,
                    Name = "account2",
                    Priority = 5,
                    Description = "account2 description",
                    CategoryId = null
                };
                UpsertAccount(account2, repositories);

                ListAccountCommand action = new ListAccountCommand("ls accounts", repositories);
                action.IdOption.SetData(account1.Id.Value);

                FakeCommandActionListener listener = new FakeCommandActionListener();

                //Act
                bool successful = action.TryExecute(mockLog.Object, new ICommandActionListener[]{listener});

                IEnumerable<ReadCommandResult<Account>> results = listener.GetResults<ReadCommandResult<Account>>();
                
                IEnumerable<Account> accounts = results.First().FilteredItems;

                //Assert
                Assert.True(successful);
                Assert.True(listener.OnlyHasType<ReadCommandResult<Account>>());
                Assert.Equal(1, results.Count());
                Assert.Equal(1, accounts.Count());

                Assert.Contains(DtoToModelTranslator.FromDto(account1, DateTime.Today, repositories), accounts);
                Assert.DoesNotContain(DtoToModelTranslator.FromDto(account2, DateTime.Today, repositories), accounts);
            }
        }
        
        [Fact]
        public void ListAccountCommandExecute_FundsOption()
        {
            using(var testDbInfo = SetupUtil.CreateTestDb())
            {
                //Arrange
                Mock<ILog> mockLog = new Mock<ILog>();
                AccountRepository repo = new AccountRepository(testDbInfo.ConnectionString, mockLog.Object);
                AccountStateRepository accountStateRepo = new AccountStateRepository(testDbInfo.ConnectionString, mockLog.Object);
                RepositoryBag repositories = SetupUtil.CreateMockRepositoryBag(testDbInfo.ConnectionString, mockLog.Object, repo, accountStateRepo);

                AccountDto account1 = new AccountDto(){
                    Id = null,
                    AccountKind = AccountKind.Sink,
                    Name = "account1",
                    Priority = 5,
                    Description = "account1 description",
                    CategoryId = null
                };
                UpsertAccount(account1, repositories, 123.45);
                AccountDto account2 = new AccountDto(){
                    Id = null,
                    AccountKind = AccountKind.Sink,
                    Name = "account2",
                    Priority = 5,
                    Description = "account2 description",
                    CategoryId = null
                };
                UpsertAccount(account2, repositories);

                ListAccountCommand action = new ListAccountCommand("ls accounts", repositories);
                action.FundsOption.SetData(new Range<Money>(100.0, 200.0));

                FakeCommandActionListener listener = new FakeCommandActionListener();

                //Act
                bool successful = action.TryExecute(mockLog.Object, new ICommandActionListener[]{listener});

                IEnumerable<ReadCommandResult<Account>> results = listener.GetResults<ReadCommandResult<Account>>();
                
                IEnumerable<Account> accounts = results.First().FilteredItems;

                //Assert
                Assert.True(successful);
                Assert.True(listener.OnlyHasType<ReadCommandResult<Account>>());
                Assert.Equal(1, results.Count());
                Assert.Equal(1, accounts.Count());

                Assert.Contains(DtoToModelTranslator.FromDto(account1, DateTime.Today, repositories), accounts);
                Assert.DoesNotContain(DtoToModelTranslator.FromDto(account2, DateTime.Today, repositories), accounts);
            }
        }
        
        [Fact]
        public void ListAccountCommandExecute_AccountKindOption()
        {
            using(var testDbInfo = SetupUtil.CreateTestDb())
            {
                //Arrange
                Mock<ILog> mockLog = new Mock<ILog>();
                AccountRepository repo = new AccountRepository(testDbInfo.ConnectionString, mockLog.Object);
                AccountStateRepository accountStateRepo = new AccountStateRepository(testDbInfo.ConnectionString, mockLog.Object);
                RepositoryBag repositories = SetupUtil.CreateMockRepositoryBag(testDbInfo.ConnectionString, mockLog.Object, repo, accountStateRepo);

                AccountDto account1 = new AccountDto(){
                    Id = null,
                    AccountKind = AccountKind.Sink,
                    Name = "account1",
                    Priority = 5,
                    Description = "account1 description",
                    CategoryId = null
                };
                UpsertAccount(account1, repositories);
                AccountDto account2 = new AccountDto(){
                    Id = null,
                    AccountKind = AccountKind.Source,
                    Name = "account2",
                    Priority = 5,
                    Description = "account2 description",
                    CategoryId = null
                };
                UpsertAccount(account2, repositories);

                ListAccountCommand action = new ListAccountCommand("ls accounts", repositories);
                action.AccountTypeOption.SetData(AccountKind.Sink);

                FakeCommandActionListener listener = new FakeCommandActionListener();

                //Act
                bool successful = action.TryExecute(mockLog.Object, new ICommandActionListener[]{listener});

                IEnumerable<ReadCommandResult<Account>> results = listener.GetResults<ReadCommandResult<Account>>();
                
                IEnumerable<Account> accounts = results.First().FilteredItems;

                //Assert
                Assert.True(successful);
                Assert.True(listener.OnlyHasType<ReadCommandResult<Account>>());
                Assert.Equal(1, results.Count());
                Assert.Equal(1, accounts.Count());

                Assert.Contains(DtoToModelTranslator.FromDto(account1, DateTime.Today, repositories), accounts);
                Assert.DoesNotContain(DtoToModelTranslator.FromDto(account2, DateTime.Today, repositories), accounts);
            }
        }
    }
}