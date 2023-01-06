using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace uCondo.HandsOn.Infra.Context
{
    public class HandsOnDbContextFactory : IDesignTimeDbContextFactory<HandsOnDbContext>
    {
        public HandsOnDbContext CreateDbContext(string[] args)
        {
            var options = new DbContextOptionsBuilder<HandsOnDbContext>()
                    .UseSqlServer("localhost")
                    .EnableSensitiveDataLogging()
                    .Options;

            return new HandsOnDbContext(options);
        }
    }
}