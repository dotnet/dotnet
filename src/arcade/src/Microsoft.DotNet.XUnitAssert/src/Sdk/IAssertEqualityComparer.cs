#pragma warning disable CA1510 // Use ArgumentNullException throw helper
#pragma warning disable IDE0063 // Use simple 'using' statement

#if XUNIT_NULLABLE
#nullable enable
#else
// In case this is source-imported with global nullable enabled but no XUNIT_NULLABLE
#pragma warning disable CS8604
#endif

using System;
using System.Collections.Generic;

namespace Xunit.Sdk
{
	/// <summary>
	/// Represents a specialized version of <see cref="IEqualityComparer{T}"/> that returns information useful
	/// when formatting results for assertion failures.
	/// </summary>
	/// <typeparam name="T">The type of the objects being compared.</typeparam>
#if XUNIT_VISIBILITY_INTERNAL
	internal
#else
	public
#endif
	interface IAssertEqualityComparer<T> : IEqualityComparer<T>
	{
		/// <summary>
		/// Compares two values and determines if they are equal.
		/// </summary>
		/// <param name="x">The first value</param>
		/// <param name="xTracker">The first value as a <see cref="CollectionTracker"/> (if it's a collection)</param>
		/// <param name="y">The second value</param>
		/// <param name="yTracker">The second value as a <see cref="CollectionTracker"/> (if it's a collection)</param>
		/// <returns>Success or failure information</returns>
		AssertEqualityResult Equals(
#if XUNIT_NULLABLE
			T? x,
			CollectionTracker? xTracker,
			T? y,
			CollectionTracker? yTracker);
#else
			T x,
			CollectionTracker xTracker,
			T y,
			CollectionTracker yTracker);
#endif
	}

	/// <summary>
	/// Extension methods for <see cref="IAssertEqualityComparer{T}"/>
	/// </summary>
#if XUNIT_VISIBILITY_INTERNAL
	internal
#else
	public
#endif
	static class IAssertEqualityComparerExtensions
	{
		/// <summary>
		/// Compares two values and determines if they are equal.
		/// </summary>
		/// <param name="comparer">The comparer</param>
		/// <param name="x">The first value</param>
		/// <param name="y">The second value</param>
		/// <returns>Success or failure information</returns>
		public static AssertEqualityResult Equals<T>(
			this IAssertEqualityComparer<T> comparer,
#if XUNIT_NULLABLE
			T? x,
			T? y)
#else
			T x,
			T y)
#endif
		{
			if (comparer is null)
				throw new ArgumentNullException(nameof(comparer));

			using (var xTracker = x.AsNonStringTracker())
			using (var yTracker = y.AsNonStringTracker())
				return comparer.Equals(x, xTracker, y, yTracker);
		}
	}
}
