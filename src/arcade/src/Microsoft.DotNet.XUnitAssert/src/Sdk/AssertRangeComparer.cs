#if XUNIT_NULLABLE
#nullable enable
#else
// In case this is source-imported with global nullable enabled but no XUNIT_NULLABLE
#pragma warning disable CS8767
#endif

using System;
using System.Collections.Generic;

#if XUNIT_NULLABLE
using System.Diagnostics.CodeAnalysis;
#endif

namespace Xunit.Sdk
{
	/// <summary>
	/// Default implementation of <see cref="IComparer{T}"/> used by the xUnit.net range assertions.
	/// </summary>
	/// <typeparam name="T">The type that is being compared.</typeparam>
	sealed class AssertRangeComparer<T> : IComparer<T>
		where T : IComparable
	{
		/// <inheritdoc/>
		public int Compare(
#if XUNIT_NULLABLE
			[AllowNull] T x,
			[AllowNull] T y)
#else
			T x,
			T y)
#endif
		{
			// Null?
			if (x == null && y == null)
				return 0;
			if (x == null)
				return -1;
			if (y == null)
				return 1;

			// Same type?
			if (x.GetType() != y.GetType())
				return -1;

			// Implements IComparable<T>?
			if (x is IComparable<T> comparable1)
				return comparable1.CompareTo(y);

			// Implements IComparable
			return x.CompareTo(y);
		}
	}
}
