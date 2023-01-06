using uCondo.HandsOn.Domain.Entities;
using uCondo.HandsOn.Domain.Enums;
using uCondo.HandsOn.Domain.Interfaces.Repositories;
using uCondo.HandsOn.Infra.Repositories;

namespace uCondo.HandsOn.Infra.Tests.Repositories
{
    public class AccountsRepositoryTests : IClassFixture<HandsOnDbContextDataFixture>
	{
        readonly IAccountsRepository _repository;

        public AccountsRepositoryTests(HandsOnDbContextDataFixture fixture)
        {
            _repository = new AccountsRepository(fixture.Context);
        }

        [Fact]
        public async Task Get_NotEmpty()
        {
            var entities = await _repository.GetAsync(null, null, null);

            Assert.NotEmpty(entities);
        }

        [Fact]
        public async Task Get_FilterExpense_AllEqual()
        {
            var entities = await _repository.GetAsync(null, AccountType.Expense, null);

            Assert.All(entities, x => Assert.Equal(AccountType.Expense, x.Type));
        }

        [Fact]
        public async Task Get_FilterIncome_AllEqual()
        {
            var entities = await _repository.GetAsync(null, AccountType.Income, null);

            Assert.All(entities, x => Assert.Equal(AccountType.Income, x.Type));
        }

        [Fact]
        public async Task Get_AllowEntries_AllTrue()
        {
            var entities = await _repository.GetAsync(null, null, true);

            Assert.All(entities, x => Assert.True(x.AllowEntries));
        }

        [Fact]
        public async Task Get_DoesNotAllowEntries_AllFalse()
        {
            var entities = await _repository.GetAsync(null, null, false);

            Assert.All(entities, x => Assert.False(x.AllowEntries));
        }

        [Fact]
        public async Task Get_ByCode_NotNull()
        {
            var entity = await _repository.GetAsync("1");

            Assert.NotNull(entity);
        }

        [Fact]
        public async Task Get_ByCode_Null()
        {
            var entity = await _repository.GetAsync("999");

            Assert.Null(entity);
        }

        [Fact]
        public async Task Insert_Get_NotNull()
        {
            _ = await _repository.InsertAsync(new AccountEntity
            {
                Code = "888",
                Name = "Inserted"
            });

            var entity = await _repository.GetAsync("888");

            Assert.NotNull(entity);
        }

        [Fact]
        public async Task Insert_DeleteGet_Null()
        {
            _ = await _repository.InsertAsync(new AccountEntity
            {
                Code = "777",
                Name = "To delete"
            });

            await _repository.DeleteAsync("777");

            var entity = await _repository.GetAsync("777");

            Assert.Null(entity);
        }

        [Fact]
        public async Task IsDuplicated_True()
        {
            var result = await _repository.IsDuplicatedAsync("1");

            Assert.True(result);
        }

        [Fact]
        public async Task IsDuplicated_False()
        {
            var result = await _repository.IsDuplicatedAsync("666");

            Assert.False(result);
        }
    }
}