#pragma warning disable CA1052 // Static holder types should be static
#pragma warning disable CA1859 // Use concrete types when possible for improved performance
#pragma warning disable IDE0040 // Add accessibility modifiers
#pragma warning disable IDE0161 // Convert to file-scoped namespace

#if XUNIT_NULLABLE
#nullable enable
#else
// In case this is source-imported with global nullable enabled but no XUNIT_NULLABLE
#pragma warning disable CS8625
#endif

using System;
using System.Collections;
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
#if XUNIT_NULLABLE
		static IEqualityComparer<T?> GetEqualityComparer<T>(IEqualityComparer? innerComparer = null) =>
			new AssertEqualityComparer<T?>(innerComparer);
#else
		static IEqualityComparer<T> GetEqualityComparer<T>(IEqualityComparer innerComparer = null) =>
			new AssertEqualityComparer<T>(innerComparer);
#endif

		static IComparer<T> GetRangeComparer<T>()
			where T : IComparable =>
				new AssertRangeComparer<T>();
	}
}
