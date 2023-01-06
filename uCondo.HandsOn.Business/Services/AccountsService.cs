using uCondo.HandsOn.Domain.Dtos;
using uCondo.HandsOn.Domain.Entities;
using uCondo.HandsOn.Domain.Enums;
using uCondo.HandsOn.Domain.Extensions;
using uCondo.HandsOn.Domain.Interfaces.Repositories;
using uCondo.HandsOn.Domain.Interfaces.Services;
using uCondo.HandsOn.Domain.Mappers;
using uCondo.HandsOn.Domain.Validation;

namespace uCondo.HandsOn.Business.Services
{
    public class AccountsService : IAccountsService
    {
        readonly IAccountsRepository _repository;

        public AccountsService(IAccountsRepository repository)
        {
            _repository = repository;
        }

        public async ValueTask<ValidationResult<IEnumerable<AccountDto>>> GetAsync(string search, AccountType? type, bool? allowEntries)
        {
            var entities = await _repository.GetAsync(search, type, allowEntries);

            entities = FilterEntities(search, entities);
            entities = LoadChildren(entities);

            return ValidationResult<IEnumerable<AccountDto>>.Success(entities.Select(x => x.ToDto()));
        }

        public async ValueTask<ValidationResult<AccountChildSequenceDto>> GetNextCodeAsync(string accountCode)
        {
            var parentAccount = await _repository.GetAsync(accountCode);

            var checkAllowChildrenResult = CheckAllowChildren(parentAccount);

            if (checkAllowChildrenResult.Invalid)
                return ValidationResult<AccountChildSequenceDto>.Fail(checkAllowChildrenResult.Message);

            var lastChild = parentAccount.Children
                .OrderBy(x => x)
                .LastOrDefault();

            var lastChildCode = lastChild?.Code ?? $"{parentAccount}.0";

            var lastChildSlices = lastChildCode.Split(".");
            var lastLevelCode = lastChildSlices.Last();
            var intLastLevelCode = int.Parse(lastLevelCode);

            if (intLastLevelCode == 999)
            {
                var firstLevelCode = lastChildSlices.First();

                return await GetNextCodeAsync(firstLevelCode);
            }
            else
            {
                return ValidationResult<AccountChildSequenceDto>.Success(new AccountChildSequenceDto
                {
                    NextParentCode = parentAccount.Code,
                    NextCode = $"{parentAccount.Code}.{intLastLevelCode + 1}"
                });
            }
        }

        public async ValueTask<ValidationResult<AccountDto>> CreateAsync(AccountCreateDto dto)
        {
            var parentAccount = await _repository.GetAsync(dto.ParentCode);

            if (!string.IsNullOrEmpty(dto.ParentCode) && parentAccount == null)
                return ValidationResult<AccountDto>.Fail($"A conta pai informada não existe.");

            if (parentAccount != null)
            {
                var checkAllowChildrenResult = CheckAllowChildren(parentAccount);

                if (checkAllowChildrenResult.Invalid)
                    return ValidationResult<AccountDto>.Fail(checkAllowChildrenResult.Message);

                var checkAllowLevelResult = CheckAllowLevel(parentAccount.Code, dto.Code);

                if (checkAllowLevelResult.Invalid)
                    return ValidationResult<AccountDto>.Fail(checkAllowLevelResult.Message);

                if (parentAccount.Type != dto.Type)
                    return ValidationResult<AccountDto>.Fail($"O tipo da conta filha não corresponde ao tipo da conta pai.");
            }

            if (await _repository.IsDuplicatedAsync(dto.Code))
                return ValidationResult<AccountDto>.Fail($"O código {dto.Code} já pertence a outra conta.");

            var entity = await _repository.InsertAsync(new AccountEntity
            {
                Code = dto.Code,
                Name = dto.Name,
                Type = dto.Type,
                ParentCode = dto.ParentCode,
                AllowEntries = dto.AllowEntries
            });

            return ValidationResult<AccountDto>.Success(entity.ToDto());
        }

        public async ValueTask<ValidationResult> DeleteAsync(string code)
        {
            await _repository.DeleteAsync(code);

            return ValidationResult.Success();
        }

        #region static methods

        private static IEnumerable<AccountEntity> FilterEntities(string search, IEnumerable<AccountEntity> entities)
        {
            if (string.IsNullOrEmpty(search))
                return entities;

            var parentsToAdd = new List<AccountEntity>();

            var filteredEntities = entities
                .Where(x => x.Name.ContainsInsensitive(search) || x.Code.StartsWith(search)).ToList();

            foreach (var filteredEntity in filteredEntities)
            {
                var parentToAdd = entities.FirstOrDefault(x => x.Code == filteredEntity.ParentCode);

                if (parentToAdd != null)
                {
                    if (!parentsToAdd.Contains(parentToAdd) && !filteredEntities.Contains(parentToAdd))
                        parentsToAdd.Add(parentToAdd);

                    parentToAdd = entities.FirstOrDefault(x => x.Code == parentToAdd.ParentCode);

                    while (parentToAdd != null)
                    {
                        if (parentToAdd != null)
                            if (!parentsToAdd.Contains(parentToAdd) && !filteredEntities.Contains(parentToAdd))
                                parentsToAdd.Add(parentToAdd);

                        parentToAdd = entities.FirstOrDefault(x => x.Code == parentToAdd.ParentCode);
                    }
                }
            }

            filteredEntities.AddRange(parentsToAdd);

            return filteredEntities;
        }

        private static IEnumerable<AccountEntity> LoadChildren(IEnumerable<AccountEntity> entities)
        {
            foreach (var filteredEntity in entities)
                filteredEntity.Children = new List<AccountEntity>(entities
                    .Where(x => x.ParentCode == filteredEntity.Code)
                    .OrderBy(x => x));

            return entities
                .OrderBy(x => x)
                .Where(x => x.ParentCode == null);
        }

        private static ValidationResult CheckAllowChildren(AccountEntity entity)
        {
            if (entity.AllowEntries)
                return ValidationResult.Fail($"A conta pai '{entity.Code} - {entity.Name}' aceita lançamentos, portanto não pode ter contas filhas.");

            return ValidationResult.Success();
        }

        private static ValidationResult CheckAllowLevel(string parentCode, string childCode)
        {
            var parentLevels = parentCode.Count(x => x == '.') + 1;
            var childLevels = childCode.Count(x => x == '.') + 1;

            if (parentLevels + 1 != childLevels)
                return ValidationResult.Fail($"A conta filha '{childCode}' deve ter um nível a mais que a conta pai '{parentCode}'.");

            return ValidationResult.Success();
        }

        #endregion
    }
}