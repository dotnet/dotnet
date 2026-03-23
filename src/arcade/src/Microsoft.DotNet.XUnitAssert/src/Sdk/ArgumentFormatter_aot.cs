#if XUNIT_AOT

#pragma warning disable IDE0060 // Remove unused parameter

#if XUNIT_NULLABLE
#nullable enable
#else
// In case this is source-imported with global nullable enabled but no XUNIT_NULLABLE
#pragma warning disable CS8603
#pragma warning disable CS8604
#endif

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

#if XUNIT_ARGUMENTFORMATTER_PRIVATE
namespace Xunit.Internal
#else
namespace Xunit.Sdk
#endif
{
	partial class ArgumentFormatter
	{
		/// <summary>
		/// Formats a value for display.
		/// </summary>
		/// <param name="value">The value to be formatted</param>
		public static string Format<TKey, TValue>(KeyValuePair<TKey, TValue> value) =>
			string.Format(
				CultureInfo.CurrentCulture,
				"[{0}] = {1}",
				Format(value.Key),
				Format(value.Value)
			);

		static string FormatComplexValue(
			object value,
			int depth,
			Type type,
			bool isAnonymousType)
		{
			// For objects which implement a custom ToString method, just call that
			var toString = value.ToString();
			if (toString is string && toString != type.FullName)
				return toString;

			return string.Format(CultureInfo.CurrentCulture, "{0}{{ {1} }}", isAnonymousType ? "" : type.Name + " ", Ellipsis);
		}

		static string FormatValueTypeValue(
			object value,
			Type type) =>
				Convert.ToString(value, CultureInfo.CurrentCulture) ?? "null";

#if XUNIT_NULLABLE
		static string? GetGroupingKeyPrefix(IEnumerable enumerable) =>
#else
		static string GetGroupingKeyPrefix(IEnumerable enumerable) =>
#endif
			null;

#if XUNIT_NULLABLE
		internal static Type? GetSetElementType(object? obj) =>
#else
		internal static Type GetSetElementType(object obj) =>
#endif
			null;

		static bool IsEnumerableOfGrouping(IEnumerable collection) =>
			false;
	}
}

#endif  // XUNIT_AOT
