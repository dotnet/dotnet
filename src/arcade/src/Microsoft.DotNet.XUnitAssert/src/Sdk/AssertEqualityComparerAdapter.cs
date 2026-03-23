#pragma warning disable CA1031 // Do not catch general exception types
#pragma warning disable IDE0290 // Use primary constructor

#if XUNIT_NULLABLE
#nullable enable
#else
// In case this is source-imported with global nullable enabled but no XUNIT_NULLABLE
#pragma warning disable CS8604
#pragma warning disable CS8767
#endif

using System;
using System.Collections;
using System.Collections.Generic;

namespace Xunit.Sdk
{
	/// <summary>
	/// A class that wraps <see cref="IEqualityComparer{T}"/> to add <see cref="IEqualityComparer"/>.
	/// </summary>
	/// <typeparam name="T">The type that is being compared.</typeparam>
	sealed class AssertEqualityComparerAdapter<T> : IEqualityComparer, IAssertEqualityComparer<T>
	{
		readonly IEqualityComparer<T> innerComparer;

		/// <summary>
		/// Initializes a new instance of the <see cref="AssertEqualityComparerAdapter{T}"/> class.
		/// </summary>
		/// <param name="innerComparer">The comparer that is being adapted.</param>
		public AssertEqualityComparerAdapter(IEqualityComparer<T> innerComparer) =>
			this.innerComparer = innerComparer ?? throw new ArgumentNullException(nameof(innerComparer));

		/// <inheritdoc/>
		public new bool Equals(
#if XUNIT_NULLABLE
			object? x,
			object? y) =>
				innerComparer.Equals((T?)x, (T?)y);
#else
			object x,
			object y) =>
				innerComparer.Equals((T)x, (T)y);
#endif

		/// <inheritdoc/>
		public bool Equals(
#if XUNIT_NULLABLE
			T? x,
			T? y) =>
#else
			T x,
			T y) =>
#endif
				innerComparer.Equals(x, y);

		public AssertEqualityResult Equals(
#if XUNIT_NULLABLE
			T? x,
			CollectionTracker? xTracker,
			T? y,
			CollectionTracker? yTracker)
#else
			T x,
			CollectionTracker xTracker,
			T y,
			CollectionTracker yTracker)
#endif
		{
			if (innerComparer is IAssertEqualityComparer<T> innerAssertEqualityComparer)
				return innerAssertEqualityComparer.Equals(x, xTracker, y, yTracker);

			return AssertEqualityResult.ForResult(Equals(x, y), x, y);
		}

		/// <inheritdoc/>
		public int GetHashCode(object obj) =>
			innerComparer.GetHashCode((T)obj);

		/// <inheritdoc/>
		public int GetHashCode(T obj) =>
			// This warning disable is here because sometimes IEqualityComparer<T>.GetHashCode marks the obj parameter
			// with [DisallowNull] and sometimes it doesn't, and we need to be able to support both scenarios when
			// someone brings in the assertion library via source.
#pragma warning disable CS8607
			innerComparer.GetHashCode(obj);
#pragma warning restore CS8607
	}
}
