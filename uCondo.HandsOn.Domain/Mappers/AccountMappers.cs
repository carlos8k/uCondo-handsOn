using uCondo.HandsOn.Domain.Dtos;
using uCondo.HandsOn.Domain.Entities;

namespace uCondo.HandsOn.Domain.Mappers
{
    public static class AccountMappers
    {
        public static AccountDto ToDto(this AccountEntity entity)
        {
            var children = new List<AccountDto>();

            foreach (var child in entity.Children ?? Enumerable.Empty<AccountEntity>())
                children.Add(child.ToDto());

            return new AccountDto
            {
                Code = entity.Code,
                Name = entity.Name,
                Type = entity.Type,
                Children = children,
                AllowEntries = entity.AllowEntries,
            };
        }
    }
}