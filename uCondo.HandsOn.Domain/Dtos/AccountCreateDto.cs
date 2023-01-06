using System.Text.RegularExpressions;
using uCondo.HandsOn.Domain.Validation;

namespace uCondo.HandsOn.Domain.Dtos
{
    public class AccountCreateDto : AccountDto, IValidationDto
	{
        public string ParentCode { get; set; }

        public ValidationResult IsValid()
        {
            if (string.IsNullOrEmpty(Name))
                return ValidationResult.Fail("O campo 'nome' não pode ser vazio.");

            if (string.IsNullOrEmpty(Code))
                return ValidationResult.Fail("O campo 'código' não pode ser vazio.");

            var codeResult = CheckCodeFormat();

            if (codeResult.Invalid)
                return codeResult;

            return ValidationResult.Success();
        }

        private ValidationResult CheckCodeFormat()
        {
            var codeRegex = new Regex(@"^[0-9][0-9.]*$");
            
            if (!codeRegex.IsMatch(Code))
                return ValidationResult.Fail("O formato do código está inválido. Um código válido é um número, seguido de ponto (.) e então seguido de outro número, com vários níveis.");

            var slices = Code.Split(".");

            foreach (var slice in slices)
            {
                _ = long.TryParse(slice, out var longSlice);

                if (longSlice < 1 || longSlice > 999)
                    return ValidationResult.Fail("O formato do código está inválido. Cada nível do código da conta precisa estar entre 1 e 999 (inclusive).");
            }

            return ValidationResult.Success();
        }
    }
}