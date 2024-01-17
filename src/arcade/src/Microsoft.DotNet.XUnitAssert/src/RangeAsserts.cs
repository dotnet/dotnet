#if XUNIT_NULLABLE
#nullable enable
#else
// In case this is source-imported with global nullable enabled but no XUNIT_NULLABLE
#pragma warning disable CS8604
#endif

using System;
using System.Collections.Generic;
using Xunit.Sdk;

namespace Xunit
{
#if XUNIT_VISIBILITY_INTERNAL
	internal
#else
	public
#endif
	partial class Assert
	{
		/// <summary>
		/// Verifies that a value is within a given range.
		/// </summary>
		/// <typeparam name="T">The type of the value to be compared</typeparam>
		/// <param name="actual">The actual value to be evaluated</param>
		/// <param name="low">The (inclusive) low value of the range</param>
		/// <param name="high">The (inclusive) high value of the range</param>
		/// <exception cref="InRangeException">Thrown when the value is not in the given range</exception>
		public static void InRange<T>(
			T actual,
			T low,
			T high)
				where T : IComparable =>
					InRange(actual, low, high, GetRangeComparer<T>());

		/// <summary>
		/// Verifies that a value is within a given range, using a comparer.
		/// </summary>
		/// <typeparam name="T">The type of the value to be compared</typeparam>
		/// <param name="actual">The actual value to be evaluated</param>
		/// <param name="low">The (inclusive) low value of the range</param>
		/// <param name="high">The (inclusive) high value of the range</param>
		/// <param name="comparer">The comparer used to evaluate the value's range</param>
		/// <exception cref="InRangeException">Thrown when the value is not in the given range</exception>
		public static void InRange<T>(
			T actual,
			T low,
			T high,
			IComparer<T> comparer)
		{
			GuardArgumentNotNull(nameof(actual), actual);
			GuardArgumentNotNull(nameof(low), low);
			GuardArgumentNotNull(nameof(high), high);
			GuardArgumentNotNull(nameof(comparer), comparer);

			if (comparer.Compare(low, actual) > 0 || comparer.Compare(actual, high) > 0)
				throw InRangeException.ForValueNotInRange(actual, low, high);
		}

		/// <summary>
		/// Verifies that a value is not within a given range, using the default comparer.
		/// </summary>
		/// <typeparam name="T">The type of the value to be compared</typeparam>
		/// <param name="actual">The actual value to be evaluated</param>
		/// <param name="low">The (inclusive) low value of the range</param>
		/// <param name="high">The (inclusive) high value of the range</param>
		/// <exception cref="NotInRangeException">Thrown when the value is in the given range</exception>
		public static void NotInRange<T>(
			T actual,
			T low,
			T high)
				where T : IComparable =>
					NotInRange(actual, low, high, GetRangeComparer<T>());

		/// <summary>
		/// Verifies that a value is not within a given range, using a comparer.
		/// </summary>
		/// <typeparam name="T">The type of the value to be compared</typeparam>
		/// <param name="actual">The actual value to be evaluated</param>
		/// <param name="low">The (inclusive) low value of the range</param>
		/// <param name="high">The (inclusive) high value of the range</param>
		/// <param name="comparer">The comparer used to evaluate the value's range</param>
		/// <exception cref="NotInRangeException">Thrown when the value is in the given range</exception>
		public static void NotInRange<T>(
			T actual,
			T low,
			T high,
			IComparer<T> comparer)
		{
			GuardArgumentNotNull(nameof(actual), actual);
			GuardArgumentNotNull(nameof(low), low);
			GuardArgumentNotNull(nameof(high), high);
			GuardArgumentNotNull(nameof(comparer), comparer);

			if (comparer.Compare(low, actual) <= 0 && comparer.Compare(actual, high) <= 0)
				throw NotInRangeException.ForValueInRange(actual, low, high);
		}
	}
}
