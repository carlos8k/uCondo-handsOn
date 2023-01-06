using Microsoft.EntityFrameworkCore;
using uCondo.HandsOn.Domain.Entities;

namespace uCondo.HandsOn.Infra.Context
{
    public class HandsOnDbContext : DbContext
    {
        public HandsOnDbContext(DbContextOptions<HandsOnDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccountEntity>()
                        .HasOne(x => x.Parent)
                        .WithMany(x => x.Children)
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .HasForeignKey(x => x.ParentCode);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<AccountEntity> Accounts { get; set; }
    }
}