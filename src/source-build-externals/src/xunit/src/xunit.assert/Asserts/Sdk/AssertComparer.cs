#if XUNIT_NULLABLE
#nullable enable
using System.Diagnostics.CodeAnalysis;
#endif

using System;
using System.Collections.Generic;

namespace Xunit.Sdk
{
	/// <summary>
	/// Default implementation of <see cref="IComparer{T}"/> used by the xUnit.net range assertions.
	/// </summary>
	/// <typeparam name="T">The type that is being compared.</typeparam>
	class AssertComparer<T> : IComparer<T>
		where T : IComparable
	{
		/// <inheritdoc/>
#if XUNIT_NULLABLE
		public int Compare([AllowNull] T x, [AllowNull] T y)
#else
		public int Compare(T x, T y)
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
			var comparable1 = x as IComparable<T>;
			if (comparable1 != null)
				return comparable1.CompareTo(y);

			// Implements IComparable
			return x.CompareTo(y);
		}
	}
}
