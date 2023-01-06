using System;
using uCondo.HandsOn.Domain.Entities;

namespace uCondo.HandsOn.Domain.Tests.Entities
{
    public class AccountEntityTests
    {
        [Fact]
        public void AccountEntity_RootCompareToSort_Equals()
        {
            var entities = new List<AccountEntity>
            {
                new AccountEntity { Code = "5"},
                new AccountEntity { Code = "3"},
                new AccountEntity { Code = "2"},
                new AccountEntity { Code = "6"},
                new AccountEntity { Code = "1"},
                new AccountEntity { Code = "4"},
            };

            var orderedEntities = entities.OrderBy(x => x).ToList();

            Assert.Equal("1", orderedEntities[0].Code);
            Assert.Equal("2", orderedEntities[1].Code);
            Assert.Equal("3", orderedEntities[2].Code);
            Assert.Equal("4", orderedEntities[3].Code);
            Assert.Equal("5", orderedEntities[4].Code);
            Assert.Equal("6", orderedEntities[5].Code);
        }

        [Fact]
        public void AccountEntity_ChildCompareToSort_Equals()
        {
            var entities = new List<AccountEntity>
            {
                new AccountEntity
                {
                    Code = "1",
                    Children = new List<AccountEntity>
                    {
                        new AccountEntity { Code = "1.5"},
                        new AccountEntity { Code = "1.22"},
                        new AccountEntity { Code = "1.2"},
                        new AccountEntity { Code = "1.10"},
                        new AccountEntity { Code = "1.1"},
                        new AccountEntity { Code = "1.4"},
                    }
                }
            };

            var orderedChildren = entities.First().Children.OrderBy(x => x).ToList();

            Assert.Equal("1.1", orderedChildren[0].Code);
            Assert.Equal("1.2", orderedChildren[1].Code);
            Assert.Equal("1.4", orderedChildren[2].Code);
            Assert.Equal("1.5", orderedChildren[3].Code);
            Assert.Equal("1.10", orderedChildren[4].Code);
            Assert.Equal("1.22", orderedChildren[5].Code);
        }
    }
}