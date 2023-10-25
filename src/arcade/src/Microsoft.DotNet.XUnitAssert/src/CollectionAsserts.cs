#if XUNIT_NULLABLE
#nullable enable
#else
// In case this is source-imported with global nullable enabled but no XUNIT_NULLABLE
#pragma warning disable CS8603
#pragma warning disable CS8604
#pragma warning disable CS8625
#endif

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Xunit.Sdk;

#if XUNIT_VALUETASK
using System.Threading.Tasks;
#endif

namespace Xunit
{
#if XUNIT_VISIBILITY_INTERNAL
	internal
#else
	public
#endif
	partial class Assert
	{
		/// <summary>
		/// Verifies that all items in the collection pass when executed against
		/// action.
		/// </summary>
		/// <typeparam name="T">The type of the object to be verified</typeparam>
		/// <param name="collection">The collection</param>
		/// <param name="action">The action to test each item against</param>
		/// <exception cref="AllException">Thrown when the collection contains at least one non-matching element</exception>
		public static void All<T>(
			IEnumerable<T> collection,
			Action<T> action)
		{
			GuardArgumentNotNull(nameof(collection), collection);
			GuardArgumentNotNull(nameof(action), action);

			All(collection, (item, index) => action(item));
		}

		/// <summary>
		/// Verifies that all items in the collection pass when executed against
		/// action. The item index is provided to the action, in addition to the item.
		/// </summary>
		/// <typeparam name="T">The type of the object to be verified</typeparam>
		/// <param name="collection">The collection</param>
		/// <param name="action">The action to test each item against</param>
		/// <exception cref="AllException">Thrown when the collection contains at least one non-matching element</exception>
		public static void All<T>(
			IEnumerable<T> collection,
			Action<T, int> action)
		{
			GuardArgumentNotNull(nameof(collection), collection);
			GuardArgumentNotNull(nameof(action), action);

			var errors = new List<Tuple<int, string, Exception>>();
			var idx = 0;

			foreach (var item in collection)
			{
				try
				{
					action(item, idx);
				}
				catch (Exception ex)
				{
					errors.Add(new Tuple<int, string, Exception>(idx, ArgumentFormatter.Format(item), ex));
				}

				++idx;
			}

			if (errors.Count > 0)
				throw AllException.ForFailures(idx, errors);
		}

#if XUNIT_VALUETASK
		/// <summary>
		/// Verifies that all items in the collection pass when executed against
		/// action.
		/// </summary>
		/// <typeparam name="T">The type of the object to be verified</typeparam>
		/// <param name="collection">The collection</param>
		/// <param name="action">The action to test each item against</param>
		/// <exception cref="AllException">Thrown when the collection contains at least one non-matching element</exception>
		public static async ValueTask AllAsync<T>(
			IEnumerable<T> collection,
			Func<T, ValueTask> action)
		{
			GuardArgumentNotNull(nameof(collection), collection);
			GuardArgumentNotNull(nameof(action), action);

			await AllAsync(collection, async (item, index) => await action(item));
		}

		/// <summary>
		/// Verifies that all items in the collection pass when executed against
		/// action. The item index is provided to the action, in addition to the item.
		/// </summary>
		/// <typeparam name="T">The type of the object to be verified</typeparam>
		/// <param name="collection">The collection</param>
		/// <param name="action">The action to test each item against</param>
		/// <exception cref="AllException">Thrown when the collection contains at least one non-matching element</exception>
		public static async ValueTask AllAsync<T>(
			IEnumerable<T> collection,
			Func<T, int, ValueTask> action)
		{
			GuardArgumentNotNull(nameof(collection), collection);
			GuardArgumentNotNull(nameof(action), action);

			var errors = new List<Tuple<int, string, Exception>>();
			var idx = 0;

			foreach (var item in collection)
			{
				try
				{
					await action(item, idx);
				}
				catch (Exception ex)
				{
					errors.Add(new Tuple<int, string, Exception>(idx, ArgumentFormatter.Format(item), ex));
				}

				++idx;
			}

			if (errors.Count > 0)
				throw AllException.ForFailures(idx, errors.ToArray());
		}
#endif

		/// <summary>
		/// Verifies that a collection contains exactly a given number of elements, which meet
		/// the criteria provided by the element inspectors.
		/// </summary>
		/// <typeparam name="T">The type of the object to be verified</typeparam>
		/// <param name="collection">The collection to be inspected</param>
		/// <param name="elementInspectors">The element inspectors, which inspect each element in turn. The
		/// total number of element inspectors must exactly match the number of elements in the collection.</param>
		public static void Collection<T>(
			IEnumerable<T> collection,
			params Action<T>[] elementInspectors)
		{
			GuardArgumentNotNull(nameof(collection), collection);
			GuardArgumentNotNull(nameof(elementInspectors), elementInspectors);

			using (var tracker = collection.AsTracker())
			{
				var index = 0;

				foreach (var item in tracker)
				{
					try
					{
						if (index < elementInspectors.Length)
							elementInspectors[index](item);
					}
					catch (Exception ex)
					{
						int? pointerIndent;
						var formattedCollection = tracker.FormatIndexedMismatch(index, out pointerIndent);
						throw CollectionException.ForMismatchedItem(ex, index, pointerIndent, formattedCollection);
					}

					index++;
				}

				if (tracker.IterationCount != elementInspectors.Length)
					throw CollectionException.ForMismatchedItemCount(elementInspectors.Length, tracker.IterationCount, tracker.FormatStart());
			}
		}

#if XUNIT_VALUETASK
		/// <summary>
		/// Verifies that a collection contains exactly a given number of elements, which meet
		/// the criteria provided by the element inspectors.
		/// </summary>
		/// <typeparam name="T">The type of the object to be verified</typeparam>
		/// <param name="collection">The collection to be inspected</param>
		/// <param name="elementInspectors">The element inspectors, which inspect each element in turn. The
		/// total number of element inspectors must exactly match the number of elements in the collection.</param>
		public static async ValueTask CollectionAsync<T>(
			IEnumerable<T> collection,
			params Func<T, ValueTask>[] elementInspectors)
		{
			GuardArgumentNotNull(nameof(collection), collection);
			GuardArgumentNotNull(nameof(elementInspectors), elementInspectors);

			using (var tracker = collection.AsTracker())
			{
				var index = 0;

				foreach (var item in tracker)
				{
					try
					{
						if (index < elementInspectors.Length)
							await elementInspectors[index](item);
					}
					catch (Exception ex)
					{
						int? pointerIndent;
						var formattedCollection = tracker.FormatIndexedMismatch(index, out pointerIndent);
						throw CollectionException.ForMismatchedItem(ex, index, pointerIndent, formattedCollection);
					}

					index++;
				}

				if (tracker.IterationCount != elementInspectors.Length)
					throw CollectionException.ForMismatchedItemCount(elementInspectors.Length, tracker.IterationCount, tracker.FormatStart());
			}
		}
#endif

		/// <summary>
		/// Verifies that a collection contains a given object.
		/// </summary>
		/// <typeparam name="T">The type of the object to be verified</typeparam>
		/// <param name="expected">The object expected to be in the collection</param>
		/// <param name="collection">The collection to be inspected</param>
		/// <exception cref="ContainsException">Thrown when the object is not present in the collection</exception>
		public static void Contains<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)] T>(
			T expected,
			IEnumerable<T> collection)
		{
			GuardArgumentNotNull(nameof(collection), collection);

			// We special case HashSet<T> because it has a custom Contains implementation that is based on the comparer
			// passed into their constructors, which we don't have access to.
			var hashSet = collection as HashSet<T>;
			if (hashSet != null)
				Contains(expected, hashSet);
			else
				Contains(expected, collection, GetEqualityComparer<T>());
		}

		/// <summary>
		/// Verifies that a collection contains a given object, using an equality comparer.
		/// </summary>
		/// <typeparam name="T">The type of the object to be verified</typeparam>
		/// <param name="expected">The object expected to be in the collection</param>
		/// <param name="collection">The collection to be inspected</param>
		/// <param name="comparer">The comparer used to equate objects in the collection with the expected object</param>
		/// <exception cref="ContainsException">Thrown when the object is not present in the collection</exception>
		public static void Contains<T>(
			T expected,
			IEnumerable<T> collection,
			IEqualityComparer<T> comparer)
		{
			GuardArgumentNotNull(nameof(collection), collection);
			GuardArgumentNotNull(nameof(comparer), comparer);

			using (var tracker = collection.AsTracker())
				if (!tracker.Contains(expected, comparer))
					throw ContainsException.ForCollectionItemNotFound(ArgumentFormatter.Format(expected), tracker.FormatStart());
		}

		/// <summary>
		/// Verifies that a collection contains a given object.
		/// </summary>
		/// <typeparam name="T">The type of the object to be verified</typeparam>
		/// <param name="collection">The collection to be inspected</param>
		/// <param name="filter">The filter used to find the item you're ensuring the collection contains</param>
		/// <exception cref="ContainsException">Thrown when the object is not present in the collection</exception>
		public static void Contains<T>(
			IEnumerable<T> collection,
			Predicate<T> filter)
		{
			GuardArgumentNotNull(nameof(collection), collection);
			GuardArgumentNotNull(nameof(filter), filter);

			using (var tracker = collection.AsTracker())
			{
				foreach (var item in tracker)
					if (filter(item))
						return;

				throw ContainsException.ForCollectionFilterNotMatched(tracker.FormatStart());
			}
		}

		/// <summary>
		/// Verifies that a collection contains each object only once.
		/// </summary>
		/// <typeparam name="T">The type of the object to be compared</typeparam>
		/// <param name="collection">The collection to be inspected</param>
		/// <exception cref="DistinctException">Thrown when an object is present inside the collection more than once</exception>
		public static void Distinct<T>(IEnumerable<T> collection) =>
			Distinct<T>(collection, EqualityComparer<T>.Default);

		/// <summary>
		/// Verifies that a collection contains each object only once.
		/// </summary>
		/// <typeparam name="T">The type of the object to be compared</typeparam>
		/// <param name="collection">The collection to be inspected</param>
		/// <param name="comparer">The comparer used to equate objects in the collection with the expected object</param>
		/// <exception cref="DistinctException">Thrown when an object is present inside the collection more than once</exception>
		public static void Distinct<T>(
			IEnumerable<T> collection,
			IEqualityComparer<T> comparer)
		{
			GuardArgumentNotNull(nameof(collection), collection);
			GuardArgumentNotNull(nameof(comparer), comparer);

			using (var tracker = collection.AsTracker())
			{
				var set = new HashSet<T>(comparer);

				foreach (var item in tracker)
					if (!set.Add(item))
						throw DistinctException.ForDuplicateItem(ArgumentFormatter.Format(item), tracker.FormatStart());
			}
		}

		/// <summary>
		/// Verifies that a collection does not contain a given object.
		/// </summary>
		/// <typeparam name="T">The type of the object to be compared</typeparam>
		/// <param name="expected">The object that is expected not to be in the collection</param>
		/// <param name="collection">The collection to be inspected</param>
		/// <exception cref="DoesNotContainException">Thrown when the object is present inside the container</exception>
		public static void DoesNotContain<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)] T>(
			T expected,
			IEnumerable<T> collection)
		{
			GuardArgumentNotNull(nameof(collection), collection);

			// We special case HashSet<T> because it has a custom Contains implementation that is based on the comparer
			// passed into their constructors, which we don't have access to.
			var hashSet = collection as HashSet<T>;
			if (hashSet != null)
				DoesNotContain(expected, hashSet);
			else
				DoesNotContain(expected, collection, GetEqualityComparer<T>());
		}

		/// <summary>
		/// Verifies that a collection does not contain a given object, using an equality comparer.
		/// </summary>
		/// <typeparam name="T">The type of the object to be compared</typeparam>
		/// <param name="expected">The object that is expected not to be in the collection</param>
		/// <param name="collection">The collection to be inspected</param>
		/// <param name="comparer">The comparer used to equate objects in the collection with the expected object</param>
		/// <exception cref="DoesNotContainException">Thrown when the object is present inside the collection</exception>
		public static void DoesNotContain<T>(
			T expected,
			IEnumerable<T> collection,
			IEqualityComparer<T> comparer)
		{
			GuardArgumentNotNull(nameof(collection), collection);
			GuardArgumentNotNull(nameof(comparer), comparer);

			using (var tracker = collection.AsTracker())
			{
				var index = 0;

				foreach (var item in tracker)
				{
					if (comparer.Equals(item, expected))
					{
						int? pointerIndent;
						var formattedCollection = tracker.FormatIndexedMismatch(index, out pointerIndent);

						throw DoesNotContainException.ForCollectionItemFound(
							ArgumentFormatter.Format(expected),
							index,
							pointerIndent,
							formattedCollection
						);
					}

					++index;
				}
			}
		}

		/// <summary>
		/// Verifies that a collection does not contain a given object.
		/// </summary>
		/// <typeparam name="T">The type of the object to be compared</typeparam>
		/// <param name="collection">The collection to be inspected</param>
		/// <param name="filter">The filter used to find the item you're ensuring the collection does not contain</param>
		/// <exception cref="DoesNotContainException">Thrown when the object is present inside the collection</exception>
		public static void DoesNotContain<T>(
			IEnumerable<T> collection,
			Predicate<T> filter)
		{
			GuardArgumentNotNull(nameof(collection), collection);
			GuardArgumentNotNull(nameof(filter), filter);

			using (var tracker = collection.AsTracker())
			{
				var index = 0;

				foreach (var item in tracker)
				{
					if (filter(item))
					{
						int? pointerIndent;
						var formattedCollection = tracker.FormatIndexedMismatch(index, out pointerIndent);

						throw DoesNotContainException.ForCollectionFilterMatched(
							index,
							pointerIndent,
							formattedCollection
						);
					}

					++index;
				}
			}
		}

		/// <summary>
		/// Verifies that a collection is empty.
		/// </summary>
		/// <param name="collection">The collection to be inspected</param>
		/// <exception cref="ArgumentNullException">Thrown when the collection is null</exception>
		/// <exception cref="EmptyException">Thrown when the collection is not empty</exception>
		public static void Empty(IEnumerable collection)
		{
			GuardArgumentNotNull(nameof(collection), collection);

			using (var tracker = collection.AsTracker())
			using (var enumerator = tracker.GetEnumerator())
				if (enumerator.MoveNext())
					throw EmptyException.ForNonEmptyCollection(tracker.FormatStart());
		}

		/// <summary>
		/// Verifies that two sequences are equivalent, using a default comparer.
		/// </summary>
		/// <typeparam name="T">The type of the objects to be compared</typeparam>
		/// <param name="expected">The expected value</param>
		/// <param name="actual">The value to be compared against</param>
		/// <exception cref="EqualException">Thrown when the objects are not equal</exception>
		public static void Equal<T>(
#if XUNIT_NULLABLE
			IEnumerable<T>? expected,
			IEnumerable<T>? actual) =>
#else
			IEnumerable<T> expected,
			IEnumerable<T> actual) =>
#endif
				Equal(expected, actual, GetEqualityComparer<IEnumerable<T>>());

		/// <summary>
		/// Verifies that two sequences are equivalent, using a custom equatable comparer.
		/// </summary>
		/// <typeparam name="T">The type of the objects to be compared</typeparam>
		/// <param name="expected">The expected value</param>
		/// <param name="actual">The value to be compared against</param>
		/// <param name="comparer">The comparer used to compare the two objects</param>
		/// <exception cref="EqualException">Thrown when the objects are not equal</exception>
		public static void Equal<T>(
#if XUNIT_NULLABLE
			IEnumerable<T>? expected,
			IEnumerable<T>? actual,
#else
			IEnumerable<T> expected,
			IEnumerable<T> actual,
#endif
			IEqualityComparer<T> comparer) =>
				Equal(expected, actual, GetEqualityComparer<IEnumerable<T>>(new AssertEqualityComparerAdapter<T>(comparer)));

		/// <summary>
		/// Verifies that two collections are equal, using a comparer function against
		/// items in the two collections.
		/// </summary>
		/// <typeparam name="T">The type of the objects to be compared</typeparam>
		/// <param name="expected">The expected value</param>
		/// <param name="actual">The value to be compared against</param>
		/// <param name="comparer">The function to compare two items for equality</param>
		public static void Equal<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)] T>(
#if XUNIT_NULLABLE
			IEnumerable<T>? expected,
			IEnumerable<T>? actual,
#else
			IEnumerable<T> expected,
			IEnumerable<T> actual,
#endif
			Func<T, T, bool> comparer) =>
				Equal(expected, actual, AssertEqualityComparer<T>.FromComparer(comparer));

		/// <summary>
		/// Verifies that a collection is not empty.
		/// </summary>
		/// <param name="collection">The collection to be inspected</param>
		/// <exception cref="ArgumentNullException">Thrown when a null collection is passed</exception>
		/// <exception cref="NotEmptyException">Thrown when the collection is empty</exception>
		public static void NotEmpty(IEnumerable collection)
		{
			GuardArgumentNotNull(nameof(collection), collection);

			var enumerator = collection.GetEnumerator();
			try
			{
				if (!enumerator.MoveNext())
					throw NotEmptyException.ForNonEmptyCollection();
			}
			finally
			{
				(enumerator as IDisposable)?.Dispose();
			}
		}

		/// <summary>
		/// Verifies that two sequences are not equivalent, using a default comparer.
		/// </summary>
		/// <typeparam name="T">The type of the objects to be compared</typeparam>
		/// <param name="expected">The expected object</param>
		/// <param name="actual">The actual object</param>
		/// <exception cref="NotEqualException">Thrown when the objects are equal</exception>
		public static void NotEqual<T>(
#if XUNIT_NULLABLE
			IEnumerable<T>? expected,
			IEnumerable<T>? actual) =>
#else
			IEnumerable<T> expected,
			IEnumerable<T> actual) =>
#endif
				NotEqual(expected, actual, GetEqualityComparer<IEnumerable<T>>());

		/// <summary>
		/// Verifies that two sequences are not equivalent, using a custom equality comparer.
		/// </summary>
		/// <typeparam name="T">The type of the objects to be compared</typeparam>
		/// <param name="expected">The expected object</param>
		/// <param name="actual">The actual object</param>
		/// <param name="comparer">The comparer used to compare the two objects</param>
		/// <exception cref="NotEqualException">Thrown when the objects are equal</exception>
		public static void NotEqual<T>(
#if XUNIT_NULLABLE
			IEnumerable<T>? expected,
			IEnumerable<T>? actual,
#else
			IEnumerable<T> expected,
			IEnumerable<T> actual,
#endif
			IEqualityComparer<T> comparer) =>
				NotEqual(expected, actual, GetEqualityComparer<IEnumerable<T>>(new AssertEqualityComparerAdapter<T>(comparer)));

		/// <summary>
		/// Verifies that two collections are not equal, using a comparer function against
		/// items in the two collections.
		/// </summary>
		/// <typeparam name="T">The type of the objects to be compared</typeparam>
		/// <param name="expected">The expected value</param>
		/// <param name="actual">The value to be compared against</param>
		/// <param name="comparer">The function to compare two items for equality</param>
		public static void NotEqual<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)] T>(
#if XUNIT_NULLABLE
			IEnumerable<T>? expected,
			IEnumerable<T>? actual,
#else
			IEnumerable<T> expected,
			IEnumerable<T> actual,
#endif
			Func<T, T, bool> comparer) =>
				NotEqual(expected, actual, AssertEqualityComparer<T>.FromComparer(comparer));

		/// <summary>
		/// Verifies that the given collection contains only a single
		/// element of the given type.
		/// </summary>
		/// <param name="collection">The collection.</param>
		/// <returns>The single item in the collection.</returns>
		/// <exception cref="SingleException">Thrown when the collection does not contain
		/// exactly one element.</exception>
#if XUNIT_NULLABLE
		public static object? Single(IEnumerable collection)
#else
		public static object Single(IEnumerable collection)
#endif
		{
			GuardArgumentNotNull(nameof(collection), collection);

			return Single(collection.Cast<object>());
		}

		/// <summary>
		/// Verifies that the given collection contains only a single
		/// element of the given value. The collection may or may not
		/// contain other values.
		/// </summary>
		/// <param name="collection">The collection.</param>
		/// <param name="expected">The value to find in the collection.</param>
		/// <returns>The single item in the collection.</returns>
		/// <exception cref="SingleException">Thrown when the collection does not contain
		/// exactly one element.</exception>
		public static void Single(
			IEnumerable collection,
#if XUNIT_NULLABLE
			object? expected)
#else
			object expected)
#endif
		{
			GuardArgumentNotNull(nameof(collection), collection);

			GetSingleResult(collection.Cast<object>(), item => object.Equals(item, expected), ArgumentFormatter.Format(expected));
		}

		/// <summary>
		/// Verifies that the given collection contains only a single
		/// element of the given type.
		/// </summary>
		/// <typeparam name="T">The collection type.</typeparam>
		/// <param name="collection">The collection.</param>
		/// <returns>The single item in the collection.</returns>
		/// <exception cref="SingleException">Thrown when the collection does not contain
		/// exactly one element.</exception>
		public static T Single<T>(IEnumerable<T> collection)
		{
			GuardArgumentNotNull(nameof(collection), collection);

			return GetSingleResult(collection, null, null);
		}

		/// <summary>
		/// Verifies that the given collection contains only a single
		/// element of the given type which matches the given predicate. The
		/// collection may or may not contain other values which do not
		/// match the given predicate.
		/// </summary>
		/// <typeparam name="T">The collection type.</typeparam>
		/// <param name="collection">The collection.</param>
		/// <param name="predicate">The item matching predicate.</param>
		/// <returns>The single item in the filtered collection.</returns>
		/// <exception cref="SingleException">Thrown when the filtered collection does
		/// not contain exactly one element.</exception>
		public static T Single<T>(
			IEnumerable<T> collection,
			Predicate<T> predicate)
		{
			GuardArgumentNotNull(nameof(collection), collection);
			GuardArgumentNotNull(nameof(predicate), predicate);

			return GetSingleResult(collection, predicate, "(predicate expression)");
		}

		static T GetSingleResult<T>(
			IEnumerable<T> collection,
#if XUNIT_NULLABLE
			Predicate<T>? predicate,
			string? expected)
#else
			Predicate<T> predicate,
			string expected)
#endif
		{
			var count = 0;
			var index = 0;
			var matchIndices = new List<int>();
			var result = default(T);

			using (var tracker = collection.AsTracker())
			{
				foreach (var item in tracker)
				{
					if (predicate == null || predicate(item))
					{
						if (++count == 1)
							result = item;
						if (predicate != null)
							matchIndices.Add(index);
					}

					++index;
				}

				switch (count)
				{
					case 0:
						throw SingleException.Empty(expected, tracker.FormatStart());
					case 1:
#if XUNIT_NULLABLE
						return result!;
#else
						return result;
#endif
					default:
						throw SingleException.MoreThanOne(count, expected, tracker.FormatStart(), matchIndices);
				}
			}
		}
	}
}
