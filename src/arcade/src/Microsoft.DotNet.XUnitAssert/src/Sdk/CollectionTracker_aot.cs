#if XUNIT_AOT
#if XUNIT_NULLABLE
#nullable enable
#else
// In case this is source-imported with global nullable enabled but no XUNIT_NULLABLE
#pragma warning disable CS8603
#pragma warning disable CS8619
#endif

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Xunit.Sdk
{
	partial class CollectionTracker
	{
#if XUNIT_NULLABLE
		static AssertEqualityResult? CheckIfSetsAreEqual(
			CollectionTracker? x,
			CollectionTracker? y,
			IEqualityComparer? itemComparer)
#else
		static AssertEqualityResult CheckIfSetsAreEqual(
			CollectionTracker x,
			CollectionTracker y,
			IEqualityComparer itemComparer)
#endif
		{
			if (x == null || y == null)
				return null;

			var elementTypeX = ArgumentFormatter.GetSetElementType(x.InnerEnumerable);
			var elementTypeY = ArgumentFormatter.GetSetElementType(y.InnerEnumerable);

			if (elementTypeX == null || elementTypeY == null)
				return null;

			if (elementTypeX != elementTypeY)
				return AssertEqualityResult.ForResult(false, x.InnerEnumerable, y.InnerEnumerable);

			if (itemComparer == null)
				return null;

			return AssertEqualityResult.ForResult(
				CompareTypedSets(
					(ISet<object>)x.InnerEnumerable,
					(ISet<object>)y.InnerEnumerable,
					(IEqualityComparer<object>)itemComparer
				),
				x.InnerEnumerable,
				y.InnerEnumerable
			);
		}

#if XUNIT_NULLABLE
		static (Type?, MethodInfo?) GetAssertEqualityComparerMetadata(IEqualityComparer itemComparer) =>
#else
		static (Type, MethodInfo) GetAssertEqualityComparerMetadata(IEqualityComparer itemComparer) =>
#endif
			(null, null);
	}
}

#endif  // XUNIT_AOT
