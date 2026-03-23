#if XUNIT_AOT

#if XUNIT_NULLABLE
#nullable enable
#else
// In case this is source-imported with global nullable enabled but no XUNIT_NULLABLE
#pragma warning disable CS8603
#endif

using System.Collections;

#if XUNIT_NULLABLE
using System.Diagnostics.CodeAnalysis;
#endif

namespace Xunit.Sdk
{
	partial class CollectionTrackerExtensions
	{
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

			return CollectionTracker.Wrap(enumerable);
		}
	}
}

#endif  // XUNIT_AOT
