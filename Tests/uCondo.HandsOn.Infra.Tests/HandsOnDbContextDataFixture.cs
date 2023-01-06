using Microsoft.EntityFrameworkCore;
using uCondo.HandsOn.Domain.Entities;
using uCondo.HandsOn.Domain.Enums;
using uCondo.HandsOn.Infra.Context;

namespace uCondo.HandsOn.Infra.Tests
{
    public sealed class HandsOnDbContextDataFixture : IDisposable
    {
        private static bool _created = false;
        public static readonly object _lock = new();

        public HandsOnDbContext Context { get; private set; }

        public HandsOnDbContextDataFixture()
        {
            Context = CreateContext();

            lock (_lock)
            {
                if (_created) return;

                _created = true;

                var localContext = CreateContext();

                Initialize(localContext);
            }
        }

        public void Dispose()
        {
            Context.Dispose();
        }

        private static HandsOnDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<HandsOnDbContext>()
                    .UseInMemoryDatabase(databaseName: "memorydb")
                    .Options;

            return new HandsOnDbContext(options);
        }

        private static void Initialize(HandsOnDbContext context)
        {
            context.Database.EnsureCreated();

            context.Entry(new AccountEntity
            {
                Code = "1",
                Name = "Expense",
                AllowEntries = false,
                Type = AccountType.Expense,
            }).State = EntityState.Added;

            context.Entry(new AccountEntity
            {
                Code = "2",
                Name = "Income",
                AllowEntries = true,
                Type = AccountType.Income
            }).State = EntityState.Added;

            context.SaveChanges();
            context.Dispose();
        }
    }
}