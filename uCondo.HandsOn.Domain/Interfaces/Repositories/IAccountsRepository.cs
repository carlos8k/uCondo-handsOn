using uCondo.HandsOn.Domain.Entities;
using uCondo.HandsOn.Domain.Enums;

namespace uCondo.HandsOn.Domain.Interfaces.Repositories
{
    public interface IAccountsRepository
	{
		ValueTask<IEnumerable<AccountEntity>> GetAsync(string search, AccountType? type, bool? allowEntries);
        ValueTask<AccountEntity> GetAsync(string code);
        ValueTask<AccountEntity> InsertAsync(AccountEntity entity);
		Task DeleteAsync(string code);
        ValueTask<bool> IsDuplicatedAsync(string code);
	}
}