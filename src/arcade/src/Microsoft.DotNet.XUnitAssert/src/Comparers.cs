#if XUNIT_NULLABLE
#nullable enable
#else
// In case this is source-imported with global nullable enabled but no XUNIT_NULLABLE
#pragma warning disable CS8625
#endif

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
		static IEqualityComparer<T?> GetEqualityComparer<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces |
				DynamicallyAccessedMemberTypes.PublicFields |
				DynamicallyAccessedMemberTypes.NonPublicFields |
				DynamicallyAccessedMemberTypes.PublicProperties |
				DynamicallyAccessedMemberTypes.NonPublicProperties |
				DynamicallyAccessedMemberTypes.PublicMethods)] T>(IEqualityComparer? innerComparer = null) =>
			new AssertEqualityComparer<T?>(innerComparer);
#else
		static IEqualityComparer<T> GetEqualityComparer<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces |
				DynamicallyAccessedMemberTypes.PublicFields |
				DynamicallyAccessedMemberTypes.NonPublicFields |
				DynamicallyAccessedMemberTypes.PublicProperties |
				DynamicallyAccessedMemberTypes.NonPublicProperties |
				DynamicallyAccessedMemberTypes.PublicMethods)] T>(IEqualityComparer innerComparer = null) =>
			new AssertEqualityComparer<T>(innerComparer);
#endif

		static IComparer<T> GetRangeComparer<T>()
			where T : IComparable =>
				new AssertRangeComparer<T>();
	}
}
