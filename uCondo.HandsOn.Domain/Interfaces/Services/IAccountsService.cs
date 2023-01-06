using uCondo.HandsOn.Domain.Dtos;
using uCondo.HandsOn.Domain.Enums;
using uCondo.HandsOn.Domain.Validation;

namespace uCondo.HandsOn.Domain.Interfaces.Services
{
    public interface IAccountsService
	{
		ValueTask<ValidationResult<IEnumerable<AccountDto>>> GetAsync(string search, AccountType? type, bool? allowEntries);
		ValueTask<ValidationResult<AccountChildSequenceDto>> GetNextCodeAsync(string accountCode);
		ValueTask<ValidationResult<AccountDto>> CreateAsync(AccountCreateDto dto);
		ValueTask<ValidationResult> DeleteAsync(string code);
	}
}