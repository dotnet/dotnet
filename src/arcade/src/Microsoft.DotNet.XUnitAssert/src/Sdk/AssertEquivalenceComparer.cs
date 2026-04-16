#if !XUNIT_AOT

#pragma warning disable IDE0290 // Use primary constructor

#if XUNIT_NULLABLE
#nullable enable
#else
// In case this is source-imported with global nullable enabled but no XUNIT_NULLABLE
#pragma warning disable CS8604
#pragma warning disable CS8767
#endif

using System.Collections;
using System.Collections.Generic;

namespace Xunit
{
	/// <summary>
	/// An implementation of <see cref="IEqualityComparer"/> that uses the same logic
	/// from <see cref="Assert.Equivalent"/>.
	/// </summary>
#if XUNIT_VISIBILITY_INTERNAL
	internal
#else
	public
#endif
	class AssertEquivalenceComparer : IEqualityComparer
	{
		readonly bool strict;

		/// <summary>
		/// Initializes a new instance of the <see cref="AssertEquivalenceComparer"/> class.
		/// </summary>
		/// <param name="strict">A flag indicating whether comparisons should be strict.</param>
		public AssertEquivalenceComparer(bool strict) =>
			this.strict = strict;

		/// <inheritdoc/>
		public new bool Equals(
#if XUNIT_NULLABLE
			object? x,
			object? y)
#else
			object x,
			object y)
#endif
		{
			Assert.Equivalent(x, y, strict);
			return true;
		}

		/// <inheritdoc/>
		public int GetHashCode(object obj) =>
			obj?.GetHashCode() ?? 0;
	}

	/// <summary>
	/// An implementation of <see cref="IEqualityComparer{T}"/> that uses the same logic
	/// from <see cref="Assert.Equivalent"/>.
	/// </summary>
	/// <typeparam name="T">The item type being compared</typeparam>
	/// <remarks>
	/// A generic version of this is provided so that it can be used with
	/// <see cref="Assert.Equal{T}(IEnumerable{T}?, IEnumerable{T}?, IEqualityComparer{T})"/>
	/// to ensure strict ordering of collections while doing equivalence comparisons for
	/// the items inside the collection, per <see href="https://github.com/xunit/xunit/discussions/3186"/>.
	/// </remarks>
#if XUNIT_VISIBILITY_INTERNAL
	internal
#else
	public
#endif
	class AssertEquivalenceComparer<T> : IEqualityComparer<T>
	{
		readonly bool strict;

		/// <summary>
		/// Initializes a new instance of the <see cref="AssertEquivalenceComparer{T}"/> class.
		/// </summary>
		/// <param name="strict">A flag indicating whether comparisons should be strict.</param>
		public AssertEquivalenceComparer(bool strict) =>
			this.strict = strict;

		/// <inheritdoc/>
		public bool Equals(
#if XUNIT_NULLABLE
			T? x,
			T? y)
#else
			T x,
			T y)
#endif
		{
			Assert.Equivalent(x, y, strict);
			return true;
		}

		/// <inheritdoc/>
		public int GetHashCode(T obj) =>
			obj?.GetHashCode() ?? 0;
	}
}

#endif  // !XUNIT_AOT
