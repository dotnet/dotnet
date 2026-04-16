#if XUNIT_NULLABLE
#nullable enable
#else
// In case this is source-imported with global nullable enabled but no XUNIT_NULLABLE
#pragma warning disable CS8603
#pragma warning disable CS8604
#endif

using System.Collections;
using System.Collections.Generic;

#if XUNIT_NULLABLE
using System.Diagnostics.CodeAnalysis;
#endif

namespace Xunit.Sdk
{
	/// <summary>
	/// Extension methods related to <see cref="CollectionTracker{T}"/>.
	/// </summary>
#if XUNIT_VISIBILITY_INTERNAL
	internal
#else
	public
#endif
	static partial class CollectionTrackerExtensions
	{
#if XUNIT_NULLABLE
		internal static CollectionTracker? AsNonStringTracker(this object? value)
#else
		internal static CollectionTracker AsNonStringTracker(this object value)
#endif
		{
			if (value == null || value is string)
				return null;

			return AsTracker(value as IEnumerable);
		}

		/// <summary>
		/// Wraps the given enumerable in an instance of <see cref="CollectionTracker{T}"/>.
		/// </summary>
		/// <typeparam name="T">The item type of the collection</typeparam>
		/// <param name="enumerable">The enumerable to be wrapped</param>
#if XUNIT_NULLABLE
		[return: NotNullIfNotNull(nameof(enumerable))]
		public static CollectionTracker<T>? AsTracker<T>(this IEnumerable<T>? enumerable) =>
#else
		public static CollectionTracker<T> AsTracker<T>(this IEnumerable<T> enumerable) =>
#endif
			enumerable == null
				? null
				: enumerable as CollectionTracker<T> ?? CollectionTracker<T>.Wrap(enumerable);

		/// <summary>
		/// Enumerates the elements inside the collection tracker.
		/// </summary>
		public static IEnumerator GetEnumerator(this CollectionTracker tracker)
		{
			Assert.GuardArgumentNotNull(nameof(tracker), tracker);

			return tracker.GetSafeEnumerator();
		}
	}
}
