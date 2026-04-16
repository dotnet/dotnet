#if !XUNIT_AOT

#pragma warning disable IDE0300 // Simplify collection initialization

#if XUNIT_NULLABLE
#nullable enable
#else
// In case this is source-imported with global nullable enabled but no XUNIT_NULLABLE
#pragma warning disable CS8603
#pragma warning disable CS8605
#pragma warning disable CS8619
#endif

using System;
using System.Collections;
using System.Linq;
using System.Reflection;

namespace Xunit.Sdk
{
	partial class CollectionTracker
	{
		static readonly MethodInfo openGenericCompareTypedSetsMethod =
			typeof(CollectionTracker)
				.GetRuntimeMethods()
				.Single(m => m.Name == nameof(CompareTypedSets));

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

			var genericCompareMethod = openGenericCompareTypedSetsMethod.MakeGenericMethod(elementTypeX);
#if XUNIT_NULLABLE
			return AssertEqualityResult.ForResult((bool)genericCompareMethod.Invoke(null, new object?[] { x.InnerEnumerable, y.InnerEnumerable, itemComparer })!, x.InnerEnumerable, y.InnerEnumerable);
#else
			return AssertEqualityResult.ForResult((bool)genericCompareMethod.Invoke(null, new object[] { x.InnerEnumerable, y.InnerEnumerable, itemComparer }), x.InnerEnumerable, y.InnerEnumerable);
#endif
		}

#if XUNIT_NULLABLE
		static (Type?, MethodInfo?) GetAssertEqualityComparerMetadata(IEqualityComparer itemComparer)
#else
		static (Type, MethodInfo) GetAssertEqualityComparerMetadata(IEqualityComparer itemComparer)
#endif
		{
			var assertQualityComparererType =
				itemComparer
					.GetType()
					.GetInterfaces()
					.FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IAssertEqualityComparer<>));
			var comparisonType = assertQualityComparererType?.GenericTypeArguments[0];
			var equalsMethod = assertQualityComparererType?.GetMethod("Equals");

			return (comparisonType, equalsMethod);
		}
	}
}

#endif  // !XUNIT_AOT
