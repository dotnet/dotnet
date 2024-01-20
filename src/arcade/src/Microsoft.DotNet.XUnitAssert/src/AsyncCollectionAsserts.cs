#if NETCOREAPP3_0_OR_GREATER

#if XUNIT_NULLABLE
#nullable enable
#endif

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Xunit.Internal;
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
		/// <summary>
		/// Verifies that all items in the collection pass when executed against
		/// action.
		/// </summary>
		/// <typeparam name="T">The type of the object to be verified</typeparam>
		/// <param name="collection">The collection</param>
		/// <param name="action">The action to test each item against</param>
		/// <exception cref="AllException">Thrown when the collection contains at least one non-matching element</exception>
		public static void All<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties | DynamicallyAccessedMemberTypes.PublicMethods)] T>(
			IAsyncEnumerable<T> collection,
			Action<T> action) =>
				All(AssertHelper.ToEnumerable(collection), action);

		/// <summary>
		/// Verifies that all items in the collection pass when executed against
		/// action. The item index is provided to the action, in addition to the item.
		/// </summary>
		/// <typeparam name="T">The type of the object to be verified</typeparam>
		/// <param name="collection">The collection</param>
		/// <param name="action">The action to test each item against</param>
		/// <exception cref="AllException">Thrown when the collection contains at least one non-matching element</exception>
		public static void All<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties | DynamicallyAccessedMemberTypes.PublicMethods)] T>(
			IAsyncEnumerable<T> collection,
			Action<T, int> action) =>
				All(AssertHelper.ToEnumerable(collection), action);

		/// <summary>
		/// Verifies that all items in the collection pass when executed against
		/// action.
		/// </summary>
		/// <typeparam name="T">The type of the object to be verified</typeparam>
		/// <param name="collection">The collection</param>
		/// <param name="action">The action to test each item against</param>
		/// <exception cref="AllException">Thrown when the collection contains at least one non-matching element</exception>
		public static Task AllAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties | DynamicallyAccessedMemberTypes.PublicMethods)] T>(
			IAsyncEnumerable<T> collection,
			Func<T, Task> action) =>
				AllAsync(AssertHelper.ToEnumerable(collection), action);

		/// <summary>
		/// Verifies that all items in the collection pass when executed against
		/// action. The item index is provided to the action, in addition to the item.
		/// </summary>
		/// <typeparam name="T">The type of the object to be verified</typeparam>
		/// <param name="collection">The collection</param>
		/// <param name="action">The action to test each item against</param>
		/// <exception cref="AllException">Thrown when the collection contains at least one non-matching element</exception>
		public static Task AllAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties | DynamicallyAccessedMemberTypes.PublicMethods)] T>(
			IAsyncEnumerable<T> collection,
			Func<T, int, Task> action) =>
				AllAsync(AssertHelper.ToEnumerable(collection), action);

		/// <summary>
		/// Verifies that a collection contains exactly a given number of elements, which meet
		/// the criteria provided by the element inspectors.
		/// </summary>
		/// <typeparam name="T">The type of the object to be verified</typeparam>
		/// <param name="collection">The collection to be inspected</param>
		/// <param name="elementInspectors">The element inspectors, which inspect each element in turn. The
		/// total number of element inspectors must exactly match the number of elements in the collection.</param>
		public static void Collection<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties | DynamicallyAccessedMemberTypes.PublicMethods)] T>(
			IAsyncEnumerable<T> collection,
			params Action<T>[] elementInspectors) =>
				Collection(AssertHelper.ToEnumerable(collection), elementInspectors);

		/// <summary>
		/// Verifies that a collection contains exactly a given number of elements, which meet
		/// the criteria provided by the element inspectors.
		/// </summary>
		/// <typeparam name="T">The type of the object to be verified</typeparam>
		/// <param name="collection">The collection to be inspected</param>
		/// <param name="elementInspectors">The element inspectors, which inspect each element in turn. The
		/// total number of element inspectors must exactly match the number of elements in the collection.</param>
		public static Task CollectionAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties | DynamicallyAccessedMemberTypes.PublicMethods)] T>(
			IAsyncEnumerable<T> collection,
			params Func<T, Task>[] elementInspectors) =>
				CollectionAsync(AssertHelper.ToEnumerable(collection), elementInspectors);

		/// <summary>
		/// Verifies that a collection contains a given object.
		/// </summary>
		/// <typeparam name="T">The type of the object to be verified</typeparam>
		/// <param name="expected">The object expected to be in the collection</param>
		/// <param name="collection">The collection to be inspected</param>
		/// <exception cref="ContainsException">Thrown when the object is not present in the collection</exception>
		public static void Contains<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties | DynamicallyAccessedMemberTypes.PublicMethods)] T>(
			T expected,
			IAsyncEnumerable<T> collection) =>
				Contains(expected, AssertHelper.ToEnumerable(collection));

		/// <summary>
		/// Verifies that a collection contains a given object, using an equality comparer.
		/// </summary>
		/// <typeparam name="T">The type of the object to be verified</typeparam>
		/// <param name="expected">The object expected to be in the collection</param>
		/// <param name="collection">The collection to be inspected</param>
		/// <param name="comparer">The comparer used to equate objects in the collection with the expected object</param>
		/// <exception cref="ContainsException">Thrown when the object is not present in the collection</exception>
		public static void Contains<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties | DynamicallyAccessedMemberTypes.PublicMethods)] T>(
			T expected,
			IAsyncEnumerable<T> collection,
			IEqualityComparer<T> comparer) =>
				Contains(expected, AssertHelper.ToEnumerable(collection), comparer);

		/// <summary>
		/// Verifies that a collection contains a given object.
		/// </summary>
		/// <typeparam name="T">The type of the object to be verified</typeparam>
		/// <param name="collection">The collection to be inspected</param>
		/// <param name="filter">The filter used to find the item you're ensuring the collection contains</param>
		/// <exception cref="ContainsException">Thrown when the object is not present in the collection</exception>
		public static void Contains<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties | DynamicallyAccessedMemberTypes.PublicMethods)] T>(
			IAsyncEnumerable<T> collection,
			Predicate<T> filter) =>
				Contains(AssertHelper.ToEnumerable(collection), filter);

		/// <summary>
		/// Verifies that a collection contains each object only once.
		/// </summary>
		/// <typeparam name="T">The type of the object to be compared</typeparam>
		/// <param name="collection">The collection to be inspected</param>
		/// <exception cref="DistinctException">Thrown when an object is present inside the collection more than once</exception>
		public static void Distinct<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties | DynamicallyAccessedMemberTypes.PublicMethods)] T>(IAsyncEnumerable<T> collection) =>
			Distinct(AssertHelper.ToEnumerable(collection), EqualityComparer<T>.Default);

		/// <summary>
		/// Verifies that a collection contains each object only once.
		/// </summary>
		/// <typeparam name="T">The type of the object to be compared</typeparam>
		/// <param name="collection">The collection to be inspected</param>
		/// <param name="comparer">The comparer used to equate objects in the collection with the expected object</param>
		/// <exception cref="DistinctException">Thrown when an object is present inside the collection more than once</exception>
		public static void Distinct<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties | DynamicallyAccessedMemberTypes.PublicMethods)] T>(
			IAsyncEnumerable<T> collection,
			IEqualityComparer<T> comparer) =>
				Distinct(AssertHelper.ToEnumerable(collection), comparer);

		/// <summary>
		/// Verifies that a collection does not contain a given object.
		/// </summary>
		/// <typeparam name="T">The type of the object to be compared</typeparam>
		/// <param name="expected">The object that is expected not to be in the collection</param>
		/// <param name="collection">The collection to be inspected</param>
		/// <exception cref="DoesNotContainException">Thrown when the object is present inside the collection</exception>
		public static void DoesNotContain<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties | DynamicallyAccessedMemberTypes.PublicMethods)] T>(
			T expected,
			IAsyncEnumerable<T> collection) =>
				DoesNotContain(expected, AssertHelper.ToEnumerable(collection));

		/// <summary>
		/// Verifies that a collection does not contain a given object, using an equality comparer.
		/// </summary>
		/// <typeparam name="T">The type of the object to be compared</typeparam>
		/// <param name="expected">The object that is expected not to be in the collection</param>
		/// <param name="collection">The collection to be inspected</param>
		/// <param name="comparer">The comparer used to equate objects in the collection with the expected object</param>
		/// <exception cref="DoesNotContainException">Thrown when the object is present inside the collection</exception>
		public static void DoesNotContain<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties | DynamicallyAccessedMemberTypes.PublicMethods)] T>(
			T expected,
			IAsyncEnumerable<T> collection,
			IEqualityComparer<T> comparer) =>
				DoesNotContain(expected, AssertHelper.ToEnumerable(collection), comparer);

		/// <summary>
		/// Verifies that a collection does not contain a given object.
		/// </summary>
		/// <typeparam name="T">The type of the object to be compared</typeparam>
		/// <param name="collection">The collection to be inspected</param>
		/// <param name="filter">The filter used to find the item you're ensuring the collection does not contain</param>
		/// <exception cref="DoesNotContainException">Thrown when the object is present inside the collection</exception>
		public static void DoesNotContain<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties | DynamicallyAccessedMemberTypes.PublicMethods)] T>(
			IAsyncEnumerable<T> collection,
			Predicate<T> filter) =>
				DoesNotContain(AssertHelper.ToEnumerable(collection), filter);

		/// <summary>
		/// Verifies that a collection is empty.
		/// </summary>
		/// <param name="collection">The collection to be inspected</param>
		/// <exception cref="ArgumentNullException">Thrown when the collection is null</exception>
		/// <exception cref="EmptyException">Thrown when the collection is not empty</exception>
		public static void Empty<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties | DynamicallyAccessedMemberTypes.PublicMethods)] T>(IAsyncEnumerable<T> collection) =>
			Empty(AssertHelper.ToEnumerable(collection));

		/// <summary>
		/// Verifies that two sequences are equivalent, using a default comparer.
		/// </summary>
		/// <typeparam name="T">The type of the objects to be compared</typeparam>
		/// <param name="expected">The expected value</param>
		/// <param name="actual">The value to be compared against</param>
		/// <exception cref="EqualException">Thrown when the objects are not equal</exception>
		public static void Equal<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties | DynamicallyAccessedMemberTypes.PublicMethods)] T>(
#if XUNIT_NULLABLE
			IEnumerable<T>? expected,
			IAsyncEnumerable<T>? actual) =>
#else
			IEnumerable<T> expected,
			IAsyncEnumerable<T> actual) =>
#endif
				Equal(expected, AssertHelper.ToEnumerable(actual), GetEqualityComparer<T>());

		/// <summary>
		/// Verifies that two sequences are equivalent, using a default comparer.
		/// </summary>
		/// <typeparam name="T">The type of the objects to be compared</typeparam>
		/// <param name="expected">The expected value</param>
		/// <param name="actual">The value to be compared against</param>
		/// <exception cref="EqualException">Thrown when the objects are not equal</exception>
		public static void Equal<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties | DynamicallyAccessedMemberTypes.PublicMethods)] T>(
#if XUNIT_NULLABLE
			IAsyncEnumerable<T>? expected,
			IAsyncEnumerable<T>? actual) =>
#else
			IAsyncEnumerable<T> expected,
			IAsyncEnumerable<T> actual) =>
#endif
				Equal(AssertHelper.ToEnumerable(expected), AssertHelper.ToEnumerable(actual), GetEqualityComparer<T>());

		/// <summary>
		/// Verifies that two sequences are equivalent, using a custom equatable comparer.
		/// </summary>
		/// <typeparam name="T">The type of the objects to be compared</typeparam>
		/// <param name="expected">The expected value</param>
		/// <param name="actual">The value to be compared against</param>
		/// <param name="comparer">The comparer used to compare the two objects</param>
		/// <exception cref="EqualException">Thrown when the objects are not equal</exception>
		public static void Equal<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties | DynamicallyAccessedMemberTypes.PublicMethods)] T>(
#if XUNIT_NULLABLE
			IEnumerable<T>? expected,
			IAsyncEnumerable<T>? actual,
#else
			IEnumerable<T> expected,
			IAsyncEnumerable<T> actual,
#endif
			IEqualityComparer<T> comparer) =>
				Equal(expected, AssertHelper.ToEnumerable(actual), GetEqualityComparer<IEnumerable<T>>(new AssertEqualityComparerAdapter<T>(comparer)));

		/// <summary>
		/// Verifies that two sequences are equivalent, using a custom equatable comparer.
		/// </summary>
		/// <typeparam name="T">The type of the objects to be compared</typeparam>
		/// <param name="expected">The expected value</param>
		/// <param name="actual">The value to be compared against</param>
		/// <param name="comparer">The comparer used to compare the two objects</param>
		/// <exception cref="EqualException">Thrown when the objects are not equal</exception>
		public static void Equal<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties | DynamicallyAccessedMemberTypes.PublicMethods)] T>(
#if XUNIT_NULLABLE
			IAsyncEnumerable<T>? expected,
			IAsyncEnumerable<T>? actual,
#else
			IAsyncEnumerable<T> expected,
			IAsyncEnumerable<T> actual,
#endif
			IEqualityComparer<T> comparer) =>
				Equal(AssertHelper.ToEnumerable(expected), AssertHelper.ToEnumerable(actual), GetEqualityComparer<IEnumerable<T>>(new AssertEqualityComparerAdapter<T>(comparer)));

		/// <summary>
		/// Verifies that two collections are equal, using a comparer function against
		/// items in the two collections.
		/// </summary>
		/// <typeparam name="T">The type of the objects to be compared</typeparam>
		/// <param name="expected">The expected value</param>
		/// <param name="actual">The value to be compared against</param>
		/// <param name="comparer">The function to compare two items for equality</param>
		public static void Equal<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties | DynamicallyAccessedMemberTypes.PublicMethods)] T>(
#if XUNIT_NULLABLE
			IEnumerable<T>? expected,
			IAsyncEnumerable<T>? actual,
#else
			IEnumerable<T> expected,
			IAsyncEnumerable<T> actual,
#endif
			Func<T, T, bool> comparer) =>
				Equal(expected, AssertHelper.ToEnumerable(actual), AssertEqualityComparer<T>.FromComparer(comparer));

		/// <summary>
		/// Verifies that two collections are equal, using a comparer function against
		/// items in the two collections.
		/// </summary>
		/// <typeparam name="T">The type of the objects to be compared</typeparam>
		/// <param name="expected">The expected value</param>
		/// <param name="actual">The value to be compared against</param>
		/// <param name="comparer">The function to compare two items for equality</param>
		public static void Equal<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties | DynamicallyAccessedMemberTypes.PublicMethods)] T>(
#if XUNIT_NULLABLE
			IAsyncEnumerable<T>? expected,
			IAsyncEnumerable<T>? actual,
#else
			IAsyncEnumerable<T> expected,
			IAsyncEnumerable<T> actual,
#endif
			Func<T, T, bool> comparer) =>
				Equal(AssertHelper.ToEnumerable(expected), AssertHelper.ToEnumerable(actual), AssertEqualityComparer<T>.FromComparer(comparer));

		/// <summary>
		/// Verifies that a collection is not empty.
		/// </summary>
		/// <param name="collection">The collection to be inspected</param>
		/// <exception cref="ArgumentNullException">Thrown when a null collection is passed</exception>
		/// <exception cref="NotEmptyException">Thrown when the collection is empty</exception>
		public static void NotEmpty<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties | DynamicallyAccessedMemberTypes.PublicMethods)] T>(IAsyncEnumerable<T> collection) =>
			NotEmpty(AssertHelper.ToEnumerable(collection));

		/// <summary>
		/// Verifies that two sequences are not equivalent, using a default comparer.
		/// </summary>
		/// <typeparam name="T">The type of the objects to be compared</typeparam>
		/// <param name="expected">The expected object</param>
		/// <param name="actual">The actual object</param>
		/// <exception cref="NotEqualException">Thrown when the objects are equal</exception>
		public static void NotEqual<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties | DynamicallyAccessedMemberTypes.PublicMethods)] T>(
#if XUNIT_NULLABLE
			IEnumerable<T>? expected,
			IAsyncEnumerable<T>? actual) =>
#else
			IEnumerable<T> expected,
			IAsyncEnumerable<T> actual) =>
#endif
				NotEqual(expected, AssertHelper.ToEnumerable(actual), GetEqualityComparer<T>());

		/// <summary>
		/// Verifies that two sequences are not equivalent, using a default comparer.
		/// </summary>
		/// <typeparam name="T">The type of the objects to be compared</typeparam>
		/// <param name="expected">The expected object</param>
		/// <param name="actual">The actual object</param>
		/// <exception cref="NotEqualException">Thrown when the objects are equal</exception>
		public static void NotEqual<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties | DynamicallyAccessedMemberTypes.PublicMethods)] T>(
#if XUNIT_NULLABLE
			IAsyncEnumerable<T>? expected,
			IAsyncEnumerable<T>? actual) =>
#else
			IAsyncEnumerable<T> expected,
			IAsyncEnumerable<T> actual) =>
#endif
				NotEqual(AssertHelper.ToEnumerable(expected), AssertHelper.ToEnumerable(actual), GetEqualityComparer<T>());

		/// <summary>
		/// Verifies that two sequences are not equivalent, using a custom equality comparer.
		/// </summary>
		/// <typeparam name="T">The type of the objects to be compared</typeparam>
		/// <param name="expected">The expected object</param>
		/// <param name="actual">The actual object</param>
		/// <param name="comparer">The comparer used to compare the two objects</param>
		/// <exception cref="NotEqualException">Thrown when the objects are equal</exception>
		public static void NotEqual<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties | DynamicallyAccessedMemberTypes.PublicMethods)] T>(
#if XUNIT_NULLABLE
			IEnumerable<T>? expected,
			IAsyncEnumerable<T>? actual,
#else
			IEnumerable<T> expected,
			IAsyncEnumerable<T> actual,
#endif
			IEqualityComparer<T> comparer) =>
				NotEqual(expected, AssertHelper.ToEnumerable(actual), GetEqualityComparer<IEnumerable<T>>(new AssertEqualityComparerAdapter<T>(comparer)));

		/// <summary>
		/// Verifies that two sequences are not equivalent, using a custom equality comparer.
		/// </summary>
		/// <typeparam name="T">The type of the objects to be compared</typeparam>
		/// <param name="expected">The expected object</param>
		/// <param name="actual">The actual object</param>
		/// <param name="comparer">The comparer used to compare the two objects</param>
		/// <exception cref="NotEqualException">Thrown when the objects are equal</exception>
		public static void NotEqual<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties | DynamicallyAccessedMemberTypes.PublicMethods)] T>(
#if XUNIT_NULLABLE
			IAsyncEnumerable<T>? expected,
			IAsyncEnumerable<T>? actual,
#else
			IAsyncEnumerable<T> expected,
			IAsyncEnumerable<T> actual,
#endif
			IEqualityComparer<T> comparer) =>
				NotEqual(AssertHelper.ToEnumerable(expected), AssertHelper.ToEnumerable(actual), GetEqualityComparer<IEnumerable<T>>(new AssertEqualityComparerAdapter<T>(comparer)));

		/// <summary>
		/// Verifies that two collections are not equal, using a comparer function against
		/// items in the two collections.
		/// </summary>
		/// <typeparam name="T">The type of the objects to be compared</typeparam>
		/// <param name="expected">The expected value</param>
		/// <param name="actual">The value to be compared against</param>
		/// <param name="comparer">The function to compare two items for equality</param>
		public static void NotEqual<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties | DynamicallyAccessedMemberTypes.PublicMethods)] T>(
#if XUNIT_NULLABLE
			IEnumerable<T>? expected,
			IAsyncEnumerable<T>? actual,
#else
			IEnumerable<T> expected,
			IAsyncEnumerable<T> actual,
#endif
			Func<T, T, bool> comparer) =>
				NotEqual(expected, AssertHelper.ToEnumerable(actual), AssertEqualityComparer<T>.FromComparer(comparer));

		/// <summary>
		/// Verifies that two collections are not equal, using a comparer function against
		/// items in the two collections.
		/// </summary>
		/// <typeparam name="T">The type of the objects to be compared</typeparam>
		/// <param name="expected">The expected value</param>
		/// <param name="actual">The value to be compared against</param>
		/// <param name="comparer">The function to compare two items for equality</param>
		public static void NotEqual<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties | DynamicallyAccessedMemberTypes.PublicMethods)] T>(
#if XUNIT_NULLABLE
			IAsyncEnumerable<T>? expected,
			IAsyncEnumerable<T>? actual,
#else
			IAsyncEnumerable<T> expected,
			IAsyncEnumerable<T> actual,
#endif
			Func<T, T, bool> comparer) =>
				NotEqual(AssertHelper.ToEnumerable(expected), AssertHelper.ToEnumerable(actual), AssertEqualityComparer<T>.FromComparer(comparer));

		/// <summary>
		/// Verifies that the given collection contains only a single
		/// element of the given type.
		/// </summary>
		/// <typeparam name="T">The collection type.</typeparam>
		/// <param name="collection">The collection.</param>
		/// <returns>The single item in the collection.</returns>
		/// <exception cref="SingleException">Thrown when the collection does not contain
		/// exactly one element.</exception>
		public static T Single<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties | DynamicallyAccessedMemberTypes.PublicMethods)] T>(IAsyncEnumerable<T> collection) =>
			Single(AssertHelper.ToEnumerable(collection));

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
		public static T Single<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties | DynamicallyAccessedMemberTypes.PublicMethods)] T>(
			IAsyncEnumerable<T> collection,
			Predicate<T> predicate) =>
				Single(AssertHelper.ToEnumerable(collection), predicate);
	}
}

#endif
