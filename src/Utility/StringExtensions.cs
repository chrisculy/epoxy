using System.Collections.Generic;
using System.Globalization;

namespace Epoxy.Utility
{
	public static class StringExtensions
	{
		public static string FormatInvariant(this string format, params object[] arguments)
		{
			return string.Format(CultureInfo.InvariantCulture, format, arguments);
		}

		public static bool IsNullOrEmpty(this string s)
		{
			return string.IsNullOrEmpty(s);
		}

		public static string Join(this IEnumerable<string> values, string delimiter)
		{
			return string.Join(delimiter, values);
		}
	}
}
