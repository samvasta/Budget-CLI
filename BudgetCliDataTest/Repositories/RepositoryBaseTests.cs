using System.Collections.Generic;
using System.Linq;
using BudgetCliDataTest.TestHarness;
using BudgetCliUtil.Logging;
using Moq;
using Xunit;

namespace BudgetCliDataTest.Repositories
{
    public class RepositoryBaseTests
    {
        [Fact]
        public void TestRepositoryBaseInsertNew()
        {
            using(var testDbInfo = SetupUtil.CreateFakeDb())
            {
                //Arrange
                Mock<ILog> log = new Mock<ILog>();
                
                Mock<FakeDto> mockDto = new Mock<FakeDto>();
                mockDto.SetupAllProperties();
                mockDto.SetupGet(d => d.Name).Returns("Test Data");

                var repo = new FakeRepositoryBase(testDbInfo.ConnectionString, log.Object);
                
                //Act
                repo.InsertProxy(mockDto.Object);
                
                //Assert

                //Ensure id was set & is not null
                Assert.NotNull(mockDto.Object.Id);
                mockDto.VerifySet(d => d.Id=It.IsNotNull<long?>());
            }
        }

        
        [Fact]
        public void TestRepositoryBaseInsertNew_NullName()
        {
            using(var testDbInfo = SetupUtil.CreateFakeDb())
            {
                //Arrange
                Mock<ILog> log = new Mock<ILog>();
                log.Setup(l => l.WriteLine(It.IsAny<string>(), LogLevel.Error)).Verifiable();

                Mock<FakeDto> mockDto = new Mock<FakeDto>();
                mockDto.SetupAllProperties();
                mockDto.Setup(d => d.Name).Returns((string)null);

                var repo = new FakeRepositoryBase(testDbInfo.ConnectionString, log.Object);

                //Act
                bool success = repo.InsertProxy(mockDto.Object);

                //Assert

                //Ensure update failed
                Assert.False(success);

                //Ensure failure was logged
                log.Verify(l => l.WriteLine(It.IsAny<string>(), LogLevel.Error));

                //Ensure ID was not set
                Assert.Null(mockDto.Object.Id);
            }
        }

        [Fact]
        public void TestRepositoryBaseInsertWithId()
        {
            using(var testDbInfo = SetupUtil.CreateFakeDb())
            {
                //Arrange
                Mock<ILog> log = new Mock<ILog>();
                
                Mock<FakeDto> mockDto = new Mock<FakeDto>();
                mockDto.SetupAllProperties();
                mockDto.SetupGet(d => d.Name).Returns("Test Data");

                var repo = new FakeRepositoryBase(testDbInfo.ConnectionString, log.Object);
                
                //First insert to get a valid id
                repo.InsertProxy(mockDto.Object);
                
                //Act
                //Insert again should fail
                bool success = repo.InsertProxy(mockDto.Object);

                //Assert

                //Ensure update failed
                Assert.False(success);

                //Ensure failure was logged
                log.Verify(l => l.WriteLine(It.IsAny<string>(), LogLevel.Error));
            }
        }

        
        [Fact]
        public void TestRepositoryBaseUpdate()
        {
            using(var testDbInfo = SetupUtil.CreateFakeDb())
            {
                //Arrange
                Mock<ILog> log = new Mock<ILog>();
                
                Mock<FakeDto> mockDto = new Mock<FakeDto>();
                mockDto.SetupAllProperties();
                mockDto.Setup(d => d.Name).Returns("Test Data");

                var repo = new FakeRepositoryBase(testDbInfo.ConnectionString, log.Object);

                repo.InsertProxy(mockDto.Object);

                //Act
                mockDto.Object.Description = "New Description";

                bool successful = repo.UpdateProxy(mockDto.Object);

                //Assert

                //Ensure update was successful
                Assert.True(successful);
                mockDto.VerifySet((a) => a.Description=It.IsNotNull<string>());
            }
        }
        
        [Fact]
        public void TestRepositoryBaseUpdate_NoId()
        {
            using(var testDbInfo = SetupUtil.CreateFakeDb())
            {
                //Arrange
                Mock<ILog> log = new Mock<ILog>();
                
                Mock<FakeDto> mockDto = new Mock<FakeDto>();
                mockDto.SetupAllProperties();
                mockDto.Setup(d => d.Name).Returns("Test Data");

                var repo = new FakeRepositoryBase(testDbInfo.ConnectionString, log.Object);

                //Skip insert so it has no id

                mockDto.Object.Description = "New Description";

                //Act
                bool successful = repo.UpdateProxy(mockDto.Object);

                //Assert update failed
                Assert.False(successful);
                mockDto.VerifySet(d => d.Description=It.IsNotNull<string>());

                //Ensure failure was logged
                log.Verify(l => l.WriteLine(It.IsAny<string>(), LogLevel.Error));
            }
        }

        [Fact]
        public void TestRepositoryBaseUpsertNoId()
        {
            using(var testDbInfo = SetupUtil.CreateFakeDb())
            {
                //Arrange
                Mock<ILog> log = new Mock<ILog>();
                
                Mock<FakeDto> mockDto = new Mock<FakeDto>();
                mockDto.SetupAllProperties();
                mockDto.Setup(d => d.Name).Returns("Test Data");
                mockDto.Object.Description = "New Description";

                var repo = new FakeRepositoryBase(testDbInfo.ConnectionString, log.Object);

                //Act

                bool successful = repo.Upsert(mockDto.Object);

                //Assert

                //Ensure update was successful
                Assert.True(successful);
                mockDto.VerifySet((a) => a.Description=It.IsNotNull<string>());
            }
        }

        [Fact]
        public void TestRepositoryBaseUpsertWithId()
        {
            using(var testDbInfo = SetupUtil.CreateFakeDb())
            {
                //Arrange
                Mock<ILog> log = new Mock<ILog>();
                
                Mock<FakeDto> mockDto = new Mock<FakeDto>();
                mockDto.SetupAllProperties();
                mockDto.Setup(d => d.Name).Returns("Test Data");

                var repo = new FakeRepositoryBase(testDbInfo.ConnectionString, log.Object);

                repo.InsertProxy(mockDto.Object);

                //Act
                mockDto.Object.Description = "New Description";

                bool successful = repo.Upsert(mockDto.Object);

                //Assert

                //Ensure update was successful
                Assert.True(successful);
                mockDto.VerifySet((a) => a.Description=It.IsNotNull<string>());
            }
        }

        [Fact]
        public void TestRepositoryBaseGetNextId_EmptyTable()
        {
            using(var testDbInfo = SetupUtil.CreateFakeDb())
            {
                //Arrange
                Mock<ILog> log = new Mock<ILog>();
                var repo = new FakeRepositoryBase(testDbInfo.ConnectionString, log.Object);
                
                //Act
                long nextId = repo.GetNextId();
                
                //Assert
                Assert.Equal(1, nextId);
            }
        }

        [Fact]
        public void TestRepositoryBaseGetNextId()
        {
            using(var testDbInfo = SetupUtil.CreateFakeDb())
            {
                //Arrange
                Mock<ILog> log = new Mock<ILog>();
                
                Mock<FakeDto> mockDto = new Mock<FakeDto>();
                mockDto.SetupGet(d => d.Name).Returns("Test Data");

                var repo = new FakeRepositoryBase(testDbInfo.ConnectionString, log.Object);
                repo.InsertProxy(mockDto.Object);
                
                //Act
                long nextId = repo.GetNextId();

                //Assert
                Assert.Equal(2, nextId);
            }
        }

        [Fact]
        public void TestRepositoryBaseGetAll()
        {
            using(var testDbInfo = SetupUtil.CreateFakeDb())
            {
                //Arrange
                Mock<ILog> log = new Mock<ILog>();

                List<Mock<FakeDto>> mockDtos = new List<Mock<FakeDto>>();
                var repo = new FakeRepositoryBase(testDbInfo.ConnectionString, log.Object);
                for(int i = 0; i < 10; i++)
                {
                    Mock<FakeDto> mockDto = new Mock<FakeDto>();
                    mockDto.SetupGet(d => d.Name).Returns("Test Data " + (i+1));
                    mockDto.SetupProperty(d => d.Id);

                    mockDtos.Add(mockDto);

                    repo.InsertProxy(mockDto.Object);
                }
                
                
                //Act
                IEnumerable<FakeDto> allRows = repo.GetAll();

                //Assert

                //Ensure all orignal dto's had their Id set to unique values
                var mockDtoIds = mockDtos.Select(x => x.Object.Id);
                Assert.Equal(mockDtos.Count, mockDtoIds.Distinct().Count());

                Assert.Equal(mockDtos.Count, allRows.Count());
                
                var returnedDtoIds = allRows.Select(x => x.Id);
                Assert.Equal(returnedDtoIds.Count(), returnedDtoIds.Distinct().Count());
            }
        }

        [Fact]
        public void TestRepositoryBaseGetById()
        {
            using(var testDbInfo = SetupUtil.CreateFakeDb())
            {
                //Arrange
                Mock<ILog> log = new Mock<ILog>();

                List<Mock<FakeDto>> mockDtos = new List<Mock<FakeDto>>();
                var repo = new FakeRepositoryBase(testDbInfo.ConnectionString, log.Object);
                for(int i = 0; i < 10; i++)
                {
                    Mock<FakeDto> mockDto = new Mock<FakeDto>();
                    mockDto.SetupGet(d => d.Name).Returns("Test Data " + (i+1));
                    mockDto.SetupProperty(d => d.Id);

                    mockDtos.Add(mockDto);
                    
                    repo.InsertProxy(mockDto.Object);
                }
                
                
                //Act
                FakeDto id6 = repo.GetById(6);

                //Assert
                Assert.Equal(6, id6.Id);
                Assert.Equal("Test Data 6", id6.Name);
            }
        }

        [Fact]
        public void TestRepositoryBaseRemove()
        {
            using(var testDbInfo = SetupUtil.CreateFakeDb())
            {
                //Arrange
                Mock<ILog> log = new Mock<ILog>();

                List<Mock<FakeDto>> mockDtos = new List<Mock<FakeDto>>();
                var repo = new FakeRepositoryBase(testDbInfo.ConnectionString, log.Object);
                for(int i = 0; i < 10; i++)
                {
                    Mock<FakeDto> mockDto = new Mock<FakeDto>();
                    mockDto.SetupGet(d => d.Name).Returns("Test Data " + (i+1));
                    mockDto.SetupProperty(d => d.Id);

                    mockDtos.Add(mockDto);
                    
                    repo.InsertProxy(mockDto.Object);
                }
                
                
                //Act
                Mock<FakeDto> toRemove = mockDtos[5];
                bool success = repo.Remove(toRemove.Object);
                FakeDto removedDto = repo.GetById(toRemove.Object.Id.Value);

                //Assert

                //Ensure row was actually removed
                Assert.True(success);
                Assert.Null(removedDto);

                //Ensure get removed dto failure was logged
                log.Verify(l => l.WriteLine(It.IsAny<string>(), LogLevel.Error));
            }
        }

        [Fact]
        public void TestRepositoryBaseRemove_Null()
        {
            using(var testDbInfo = SetupUtil.CreateFakeDb())
            {
                //Arrange
                Mock<ILog> log = new Mock<ILog>();
                var repo = new FakeRepositoryBase(testDbInfo.ConnectionString, log.Object);
                

                //Act
                bool success = repo.Remove(null);

                //Assert
                Assert.False(success);

                //Ensure get removed dto failure was logged
                log.Verify(l => l.WriteLine(It.IsAny<string>(), LogLevel.Error));
            }
        }

        [Fact]
        public void TestRepositoryBaseRemoveById_NotExists()
        {
            using(var testDbInfo = SetupUtil.CreateFakeDb())
            {
                //Arrange
                Mock<ILog> log = new Mock<ILog>();
                var repo = new FakeRepositoryBase(testDbInfo.ConnectionString, log.Object);
                

                //Act
                bool success = repo.RemoveById(100);

                //Assert
                Assert.False(success);

                //Ensure get removed dto failure was logged
                log.Verify(l => l.WriteLine(It.IsAny<string>(), LogLevel.Error));
            }
        }
        
        [Fact]
        public void TestRepositoryBaseRemove_NoId()
        {
            using(var testDbInfo = SetupUtil.CreateFakeDb())
            {
                //Arrange
                Mock<ILog> log = new Mock<ILog>();
                
                Mock<FakeDto> mockDto = new Mock<FakeDto>();
                mockDto.SetupAllProperties();
                mockDto.Setup(d => d.Name).Returns("Test Data");

                var repo = new FakeRepositoryBase(testDbInfo.ConnectionString, log.Object);

                //Skip insert so it has no id

                //Act
                bool successful = repo.Remove(mockDto.Object);

                //Assert update failed
                Assert.False(successful);

                //Ensure failure was logged
                log.Verify(l => l.WriteLine(It.IsAny<string>(), LogLevel.Error));
            }
        }

        [Fact]
        public void TestRepositoryBaseRemoveById()
        {
            using(var testDbInfo = SetupUtil.CreateFakeDb())
            {
                //Arrange
                Mock<ILog> log = new Mock<ILog>();

                List<Mock<FakeDto>> mockDtos = new List<Mock<FakeDto>>();
                var repo = new FakeRepositoryBase(testDbInfo.ConnectionString, log.Object);
                for(int i = 0; i < 10; i++)
                {
                    Mock<FakeDto> mockDto = new Mock<FakeDto>();
                    mockDto.SetupGet(d => d.Name).Returns("Test Data " + (i+1));
                    mockDto.SetupProperty(d => d.Id);

                    mockDtos.Add(mockDto);
                    
                    repo.InsertProxy(mockDto.Object);
                }
                
                
                //Act
                Mock<FakeDto> toRemove = mockDtos[5];
                bool success = repo.RemoveById(toRemove.Object.Id.Value);
                FakeDto removedDto = repo.GetById(toRemove.Object.Id.Value);

                //Assert

                //Ensure row was actually removed
                Assert.True(success);
                Assert.Null(removedDto);

                //Ensure get removed dto failure was logged
                log.Verify(l => l.WriteLine(It.IsAny<string>(), LogLevel.Error));
            }
        }
    }
}