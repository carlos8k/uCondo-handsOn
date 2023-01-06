using Moq;
using uCondo.HandsOn.Business.Services;
using uCondo.HandsOn.Domain.Dtos;
using uCondo.HandsOn.Domain.Entities;
using uCondo.HandsOn.Domain.Enums;
using uCondo.HandsOn.Domain.Interfaces.Repositories;
using uCondo.HandsOn.Domain.Interfaces.Services;

namespace uCondo.HandsOn.Business.Tests.Services
{
    public class AccountsServiceTests
    {
        IAccountsService _service;

        #region shared test data

        readonly AccountEntity _incomeRootParent = new AccountEntity
        {
            Code = "1",
            Name = "Incomes",
            Type = AccountType.Income
        };

        readonly AccountEntity _expenseRootParent = new AccountEntity
        {
            Code = "2",
            Name = "Expenses",
            Type = AccountType.Expense
        };

        readonly AccountEntity _incomeChild1 = new AccountEntity
        {
            Code = "1.1",
            ParentCode = "1",
            Name = "Incomes Child",
            Type = AccountType.Income
        };

        readonly AccountEntity _incomeChild2 = new AccountEntity
        {
            Code = "1.2",
            ParentCode = "1",
            Name = "Incomes Child",
            Type = AccountType.Income
        };

        readonly AccountEntity _incomeChild3 = new AccountEntity
        {
            Code = "1.10",
            ParentCode = "1",
            Name = "Incomes Child 10",
            Type = AccountType.Income
        };

        readonly AccountEntity _doesNotAllowChildren = new AccountEntity
        {
            Code = "1",
            AllowEntries = true,
            Name = "Does Not Allow Children"
        };

        #endregion

        [Fact]
        public async Task Get_NotEmpty()
        {
            var mock = new Mock<IAccountsRepository>();

            mock.Setup(x => x.GetAsync(It.IsAny<string>(), It.IsAny<AccountType?>(), It.IsAny<bool?>()))
                .ReturnsAsync(() => new List<AccountEntity> { _incomeRootParent });

            _service = new AccountsService(mock.Object);

            var result = await _service.GetAsync(null, null, null);

            Assert.NotEmpty(result.Data);
        }

        [Fact]
        public async Task Get_Children_NotEmpty()
        {
            var mock = new Mock<IAccountsRepository>();

            mock.Setup(x => x.GetAsync(It.IsAny<string>(), It.IsAny<AccountType?>(), It.IsAny<bool?>()))
                .ReturnsAsync(() => new List<AccountEntity> { _incomeRootParent, _incomeChild1 });

            _service = new AccountsService(mock.Object);

            var result = await _service.GetAsync(null, null, null);

            Assert.NotEmpty(result.Data.First().Children);
        }

        [Fact]
        public async Task Get_FilterName_NotEmpty()
        {
            var mock = new Mock<IAccountsRepository>();

            mock.Setup(x => x.GetAsync(It.IsAny<string>(), It.IsAny<AccountType?>(), It.IsAny<bool?>()))
                .ReturnsAsync(() => new List<AccountEntity> { _incomeRootParent });

            _service = new AccountsService(mock.Object);

            var result = await _service.GetAsync(_incomeRootParent.Name.Substring(2, 4), null, null);

            Assert.NotEmpty(result.Data);
        }

        [Fact]
        public async Task Get_FilterName_Empty()
        {
            var mock = new Mock<IAccountsRepository>();

            mock.Setup(x => x.GetAsync(It.IsAny<string>(), It.IsAny<AccountType?>(), It.IsAny<bool?>()))
                .ReturnsAsync(() => new List<AccountEntity> { _incomeRootParent });

            _service = new AccountsService(mock.Object);

            var result = await _service.GetAsync("invalid_name", null, null);

            Assert.Empty(result.Data);
        }

        [Fact]
        public async Task Get_FilterCode_NotEmpty()
        {
            var mock = new Mock<IAccountsRepository>();

            mock.Setup(x => x.GetAsync(It.IsAny<string>(), It.IsAny<AccountType?>(), It.IsAny<bool?>()))
                .ReturnsAsync(() => new List<AccountEntity> { _incomeRootParent });

            _service = new AccountsService(mock.Object);

            var result = await _service.GetAsync(_incomeRootParent.Code, null, null);

            Assert.NotEmpty(result.Data);
        }

        [Fact]
        public async Task Get_FilterCode_Empty()
        {
            var mock = new Mock<IAccountsRepository>();

            mock.Setup(x => x.GetAsync(It.IsAny<string>(), It.IsAny<AccountType?>(), It.IsAny<bool?>()))
                .ReturnsAsync(() => new List<AccountEntity> { _incomeRootParent });

            _service = new AccountsService(mock.Object);

            var result = await _service.GetAsync("999", null, null);

            Assert.Empty(result.Data);
        }

        [Fact]
        public async Task Get_FilterChildName_NotEmpty()
        {
            var mock = new Mock<IAccountsRepository>();

            mock.Setup(x => x.GetAsync(It.IsAny<string>(), It.IsAny<AccountType?>(), It.IsAny<bool?>()))
                .ReturnsAsync(() => new List<AccountEntity> { _incomeRootParent, _incomeChild1 });

            _service = new AccountsService(mock.Object);

            var result = await _service.GetAsync(_incomeChild1.Name, null, null);

            Assert.NotEmpty(result.Data);
        }

        [Fact]
        public async Task Get_FilterChildName_Empty()
        {
            var mock = new Mock<IAccountsRepository>();

            mock.Setup(x => x.GetAsync(It.IsAny<string>(), It.IsAny<AccountType?>(), It.IsAny<bool?>()))
                .ReturnsAsync(() => new List<AccountEntity> { _incomeRootParent, _incomeChild1 });

            _service = new AccountsService(mock.Object);

            var result = await _service.GetAsync("invalid_child_name", null, null);

            Assert.Empty(result.Data);
        }

        [Fact]
        public async Task Get_RootOrder_Equals()
        {
            var mock = new Mock<IAccountsRepository>();

            mock.Setup(x => x.GetAsync(It.IsAny<string>(), It.IsAny<AccountType?>(), It.IsAny<bool?>()))
                .ReturnsAsync(() => new List<AccountEntity> { _expenseRootParent, _incomeRootParent });

            _service = new AccountsService(mock.Object);

            var result = await _service.GetAsync(null, null, null);

            Assert.Equal(result.Data.First().Code, _incomeRootParent.Code);
            Assert.Equal(result.Data.Last().Code, _expenseRootParent.Code);
        }

        [Fact]
        public async Task Get_ChildOrder_Equals()
        {
            var mock = new Mock<IAccountsRepository>();

            mock.Setup(x => x.GetAsync(It.IsAny<string>(), It.IsAny<AccountType?>(), It.IsAny<bool?>()))
                .ReturnsAsync(() => new List<AccountEntity> { _incomeRootParent, _incomeChild1, _incomeChild2, _incomeChild3 });

            _service = new AccountsService(mock.Object);

            var result = await _service.GetAsync(null, null, null);

            var children = result.Data.First().Children.ToList();

            Assert.Equal(children[0].Code, _incomeChild1.Code);
            Assert.Equal(children[1].Code, _incomeChild2.Code);
            Assert.Equal(children[2].Code, _incomeChild3.Code);
        }

        [Fact]
        public async Task GetNextCode_RootAccount_Equals()
        {
            var mock = new Mock<IAccountsRepository>();

            mock.Setup(x => x.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(() => _incomeRootParent);

            _service = new AccountsService(mock.Object);

            var result = await _service.GetNextCodeAsync(_incomeRootParent.Code);

            Assert.Equal($"{_incomeRootParent.Code}.1", result.Data.NextCode);
            Assert.Equal(_incomeRootParent.Code, result.Data.NextParentCode);
        }

        [Fact]
        public async Task GetNextCode_ChildAccount_Equals()
        {
            var incomeRootParentWithChildren = new AccountEntity
            {
                Code = "1",
                Name = "Incomes With Children",
                Type = AccountType.Income,
                Children = new List<AccountEntity> { _incomeChild1, _incomeChild2, _incomeChild3 }
            };

            var mock = new Mock<IAccountsRepository>();

            mock.Setup(x => x.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(() => incomeRootParentWithChildren);

            _service = new AccountsService(mock.Object);

            var result = await _service.GetNextCodeAsync(incomeRootParentWithChildren.Code);

            Assert.Equal($"{incomeRootParentWithChildren.Code}.11", result.Data.NextCode);
            Assert.Equal(incomeRootParentWithChildren.Code, result.Data.NextParentCode);
        }

        [Fact]
        public async Task GetNextCode_LevelExceeded_Equals()
        {
            var incomeLevel999 = new AccountEntity
            {
                Code = $"1.1.999",
                Name = "999 child",
                ParentCode = "1.1",
            };

            var incomeChild = new AccountEntity
            {
                Code = $"1.1",
                Name = "Child",
                ParentCode = "1",
                Children = new List<AccountEntity> { incomeLevel999 }
            };

            var incomeRootParentWithChildren = new AccountEntity
            {
                Code = "1",
                Name = "Root",
                Type = AccountType.Income,
                Children = new List<AccountEntity> { _incomeChild1 }
            };

            var mock = new Mock<IAccountsRepository>();

            mock.SetupSequence(x => x.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(() => incomeChild)
                .ReturnsAsync(() => incomeRootParentWithChildren)
                .ReturnsAsync(() => incomeRootParentWithChildren);

            _service = new AccountsService(mock.Object);

            var result = await _service.GetNextCodeAsync(_incomeChild1.Code);

            Assert.Equal($"{incomeRootParentWithChildren.Code}.2", result.Data.NextCode);
            Assert.Equal(incomeRootParentWithChildren.Code, result.Data.NextParentCode);
        }

        [Fact]
        public async Task GetNextCode_LevelExceededWithMoreChildren_Equals()
        {
            var incomeLevel999 = new AccountEntity
            {
                Code = $"1.1.999",
                Name = "999 child",
                ParentCode = "1.1",
            };

            var incomeChild = new AccountEntity
            {
                Code = $"1.1",
                Name = "Child",
                ParentCode = "1",
                Children = new List<AccountEntity> { incomeLevel999 }
            };

            var incomeChild2 = new AccountEntity
            {
                Code = $"1.2",
                Name = "Child",
                ParentCode = "1",
                Children = new List<AccountEntity> { incomeLevel999 }
            };

            var incomeRootParentWithChildren = new AccountEntity
            {
                Code = "1",
                Name = "Root",
                Type = AccountType.Income,
                Children = new List<AccountEntity> { _incomeChild1, incomeChild2 }
            };

            var mock = new Mock<IAccountsRepository>();

            mock.SetupSequence(x => x.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(() => incomeChild)
                .ReturnsAsync(() => incomeRootParentWithChildren)
                .ReturnsAsync(() => incomeRootParentWithChildren);

            _service = new AccountsService(mock.Object);

            var result = await _service.GetNextCodeAsync(_incomeChild1.Code);

            Assert.Equal($"{incomeRootParentWithChildren.Code}.3", result.Data.NextCode);
            Assert.Equal(incomeRootParentWithChildren.Code, result.Data.NextParentCode);
        }

        [Fact]
        public async Task Create_Valid()
        {
            var mock = new Mock<IAccountsRepository>();

            mock.Setup(x => x.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(() => _incomeRootParent);
            mock.Setup(x => x.InsertAsync(It.IsAny<AccountEntity>()))
                .ReturnsAsync((AccountEntity entity) => entity);

            _service = new AccountsService(mock.Object);

            var result = await _service.CreateAsync(new AccountCreateDto
            {
                Type = _incomeRootParent.Type,
                ParentCode = _incomeRootParent.Code,
                Code = $"{_incomeRootParent.Code}.1",
                Name = $"Child {_incomeRootParent.Code}.1",
            });

            Assert.True(result.Valid);
        }

        [Fact]
        public async Task Create_ParentNotFound_Invalid()
        {
            var mock = new Mock<IAccountsRepository>();

            mock.Setup(x => x.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(() => default);

            _service = new AccountsService(mock.Object);

            var result = await _service.CreateAsync(new AccountCreateDto
            {
                Type = _incomeRootParent.Type,
                ParentCode = _incomeRootParent.Code,
                Code = $"{_incomeRootParent.Code}.1",
                Name = $"Child {_incomeRootParent.Code}.1",
            });

            Assert.True(result.Invalid);
        }

        [Fact]
        public async Task Create_DoesNotAllowChildren_Invalid()
        {
            var mock = new Mock<IAccountsRepository>();

            mock.Setup(x => x.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(() => _doesNotAllowChildren);

            _service = new AccountsService(mock.Object);

            var result = await _service.CreateAsync(new AccountCreateDto
            {
                Type = _doesNotAllowChildren.Type,
                ParentCode = _doesNotAllowChildren.Code,
                Code = $"{_doesNotAllowChildren.Code}.1",
                Name = $"Child {_doesNotAllowChildren.Code}.1",
            });

            Assert.True(result.Invalid);
        }

        [Fact]
        public async Task Create_WrongLevel_Invalid()
        {
            var mock = new Mock<IAccountsRepository>();

            mock.Setup(x => x.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(() => _incomeRootParent);

            _service = new AccountsService(mock.Object);

            var result = await _service.CreateAsync(new AccountCreateDto
            {
                Type = _incomeRootParent.Type,
                ParentCode = _incomeRootParent.Code,
                Code = $"{_incomeRootParent.Code}.1.1.1.1",
                Name = $"Child {_incomeRootParent.Code}.1.1.1.1",
            });

            Assert.True(result.Invalid);
        }

        [Fact]
        public async Task Create_WrongType_Invalid()
        {
            var mock = new Mock<IAccountsRepository>();

            mock.Setup(x => x.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(() => _incomeRootParent);

            _service = new AccountsService(mock.Object);

            var result = await _service.CreateAsync(new AccountCreateDto
            {
                ParentCode = _incomeRootParent.Code,
                Code = $"{_incomeRootParent.Code}.1",
                Name = $"Child {_incomeRootParent.Code}.1",
                Type = _incomeRootParent.Type == AccountType.Expense ?
                    AccountType.Income : AccountType.Expense
            });

            Assert.True(result.Invalid);
        }

        [Fact]
        public async Task Create_DuplicatedCode_Invalid()
        {
            var mock = new Mock<IAccountsRepository>();

            mock.Setup(x => x.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(() => _incomeRootParent);
            mock.Setup(x => x.IsDuplicatedAsync(It.IsAny<string>()))
                .ReturnsAsync(() => true);

            _service = new AccountsService(mock.Object);

            var result = await _service.CreateAsync(new AccountCreateDto
            {
                Type = _incomeRootParent.Type,
                ParentCode = _incomeRootParent.Code,
                Code = $"{_incomeRootParent.Code}.1",
                Name = $"Child {_incomeRootParent.Code}.1",
            });

            Assert.True(result.Invalid);
        }

        [Fact]
        public async Task Delete_Valid()
        {
            var mock = new Mock<IAccountsRepository>();

            _service = new AccountsService(mock.Object);

            var result = await _service.DeleteAsync(_incomeRootParent.Code);

            Assert.True(result.Valid);
        }
    }
}