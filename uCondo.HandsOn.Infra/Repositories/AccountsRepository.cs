using Microsoft.EntityFrameworkCore;
using uCondo.HandsOn.Domain.Entities;
using uCondo.HandsOn.Domain.Enums;
using uCondo.HandsOn.Domain.Interfaces.Repositories;
using uCondo.HandsOn.Infra.Context;

namespace uCondo.HandsOn.Infra.Repositories
{
    public class AccountsRepository : IAccountsRepository
    {
        readonly HandsOnDbContext _context;

        public AccountsRepository(HandsOnDbContext context)
        {
            _context = context;
        }

        public async ValueTask<IEnumerable<AccountEntity>> GetAsync(string search, AccountType? type, bool? allowEntries)
        {
            var query = _context.Accounts.AsQueryable();

            if (type.HasValue)
                query = query.Where(x => x.Type == type.Value);

            if (allowEntries.HasValue)
                query = query.Where(x => x.AllowEntries == allowEntries.Value);

            return await query
                .AsNoTracking()
                .ToListAsync();
        }

        public async ValueTask<AccountEntity> GetAsync(string code)
        {
            return await _context.Accounts
                .Include(x => x.Children)
                .FirstOrDefaultAsync(x => x.Code == code);
        }

        public async ValueTask<AccountEntity> InsertAsync(AccountEntity entity)
        {
            _context.Entry(entity).State = EntityState.Added;
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task DeleteAsync(string code)
        {
            var entity = await GetAsync(code);

            if (entity != null)
            {
                _context.Entry(entity).State = EntityState.Deleted;
                await _context.SaveChangesAsync();
            }
        }

        public async ValueTask<bool> IsDuplicatedAsync(string code)
        {
            return await _context.Accounts.AnyAsync(x => x.Code == code);
        }
    }
}