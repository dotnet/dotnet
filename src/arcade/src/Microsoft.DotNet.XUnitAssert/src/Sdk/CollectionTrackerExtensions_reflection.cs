#if !XUNIT_AOT

#pragma warning disable IDE0090 // Use 'new(...)'
#pragma warning disable IDE0300 // Simplify collection initialization

#if XUNIT_NULLABLE
#nullable enable
#else
// In case this is source-imported with global nullable enabled but no XUNIT_NULLABLE
#pragma warning disable CS8601
#pragma warning disable CS8603
#endif

using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#if XUNIT_NULLABLE
using System.Diagnostics.CodeAnalysis;
#endif

namespace Xunit.Sdk
{
	partial class CollectionTrackerExtensions
	{
#if XUNIT_NULLABLE
		static readonly MethodInfo? asTrackerOpenGeneric =
#else
		static readonly MethodInfo asTrackerOpenGeneric =
#endif
			typeof(CollectionTrackerExtensions).GetRuntimeMethods().FirstOrDefault(m => m.Name == nameof(AsTracker) && m.IsGenericMethod);

		static readonly ConcurrentDictionary<Type, MethodInfo> cacheOfAsTrackerByType = new ConcurrentDictionary<Type, MethodInfo>();

		/// <summary>
		/// Wraps the given enumerable in an instance of <see cref="CollectionTracker{T}"/>.
		/// </summary>
		/// <param name="enumerable">The enumerable to be wrapped</param>
#if XUNIT_NULLABLE
		[return: NotNullIfNotNull(nameof(enumerable))]
		public static CollectionTracker? AsTracker(this IEnumerable? enumerable)
#else
		public static CollectionTracker AsTracker(this IEnumerable enumerable)
#endif
		{
			if (enumerable == null)
				return null;

			if (enumerable is CollectionTracker result)
				return result;

#if XUNIT_AOT
			return CollectionTracker.Wrap(enumerable);
#else
			// CollectionTracker.Wrap for the non-T enumerable uses the CastIterator, which has terrible
			// performance during iteration. We do our best to try to get a T and dynamically invoke the
			// generic version of AsTracker as we can.
			var iEnumerableOfT = enumerable.GetType().GetInterfaces().FirstOrDefault(i => i.IsConstructedGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>));
			if (iEnumerableOfT == null)
				return CollectionTracker.Wrap(enumerable);

			var enumerableType = iEnumerableOfT.GenericTypeArguments[0];
#if XUNIT_NULLABLE
			var method = cacheOfAsTrackerByType.GetOrAdd(enumerableType, t => asTrackerOpenGeneric!.MakeGenericMethod(enumerableType));
#else
			var method = cacheOfAsTrackerByType.GetOrAdd(enumerableType, t => asTrackerOpenGeneric.MakeGenericMethod(enumerableType));
#endif

			return method.Invoke(null, new object[] { enumerable }) as CollectionTracker ?? CollectionTracker.Wrap(enumerable);
#endif  // XUNIT_AOT
		}
	}
}

#endif  // !XUNIT_AOT
