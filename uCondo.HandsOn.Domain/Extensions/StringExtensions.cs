using System.Globalization;

namespace uCondo.HandsOn.Domain.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Makes a contains operation without compare accent or case
        /// </summary>
        /// <param name="value">Source value</param>
        /// <param name="subValue">Substring to check</param>
        /// <returns>Returns true if value contains sub value. Otherwise returns false.</returns>
        public static bool ContainsInsensitive(this string value, string subValue)
        {
            return CultureInfo.InvariantCulture.CompareInfo
                .IndexOf(value.ToUpperInvariant(), subValue.ToUpperInvariant(), CompareOptions.IgnoreNonSpace) > -1;
        }
    }
}