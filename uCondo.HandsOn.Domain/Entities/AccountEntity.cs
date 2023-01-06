using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using uCondo.HandsOn.Domain.Enums;

namespace uCondo.HandsOn.Domain.Entities
{
    public class AccountEntity : IComparable<AccountEntity>
    {
        [Key]
        public string Code { get; set; }

        public string ParentCode { get; set; }

        [Required]
        public string Name { get; set; }

        public AccountType Type { get; set; }

        public bool AllowEntries { get; set; }

        public AccountEntity Parent { get; set; }
        public IEnumerable<AccountEntity> Children { get; set; } = new List<AccountEntity>();

        /// <summary>
        /// Compare AccountEntity by code using version number style with 'n' levels.
        /// Inpired by https://github.com/dotnet/dotnet/blob/8d5c52c5dd54417a2071c196663bb29ab1ee2442/src/runtime/src/libraries/System.Private.CoreLib/src/System/Version.cs#L123
        /// </summary>
        /// <param name="value">Object to compare.</param>
        /// <returns>Returns 0 if versions are equal, -1 if the current object is lower and 1 if the current object is major.</returns>
        public int CompareTo(AccountEntity value)
        {
            var currentSlices = Code.Split(".");
            var toCompareSlices = value.Code.Split(".");

            for (int i = 0; i < currentSlices.Length; i++)
            {
                _ = int.TryParse(currentSlices[i], out var currentSliceInt);

                if (toCompareSlices.Length > i)
                {
                    _ = int.TryParse(toCompareSlices[i], out var toCompareSliceInt);

                    if (currentSliceInt > toCompareSliceInt)
                        return 1;
                    if (currentSliceInt < toCompareSliceInt)
                        return -1;
                }
            }

            if (currentSlices.Length > toCompareSlices.Length)
                return 1;
            if (currentSlices.Length < toCompareSlices.Length)
                return -1;

            return 0;
        }
    }
}