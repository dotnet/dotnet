#if XUNIT_NULLABLE
#nullable enable
#else
// In case this is source-imported with global nullable enabled but no XUNIT_NULLABLE
#pragma warning disable CS8604
#pragma warning disable CS8714
#endif

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using Xunit.Sdk;

#if XUNIT_IMMUTABLE_COLLECTIONS
using System.Collections.Immutable;
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
		/// Verifies that a dictionary contains a given key.
		/// </summary>
		/// <typeparam name="TKey">The type of the keys of the object to be verified.</typeparam>
		/// <typeparam name="TValue">The type of the values of the object to be verified.</typeparam>
		/// <param name="expected">The object expected to be in the collection.</param>
		/// <param name="collection">The collection to be inspected.</param>
		/// <returns>The value associated with <paramref name="expected"/>.</returns>
		/// <exception cref="ContainsException">Thrown when the object is not present in the collection</exception>
		public static TValue Contains<[DynamicallyAccessedMembers(
					DynamicallyAccessedMemberTypes.PublicFields
					| DynamicallyAccessedMemberTypes.NonPublicFields
					| DynamicallyAccessedMemberTypes.PublicProperties
					| DynamicallyAccessedMemberTypes.NonPublicProperties
					| DynamicallyAccessedMemberTypes.PublicMethods)] TKey, TValue>(
			TKey expected,
			IDictionary<TKey, TValue> collection)
#if XUNIT_NULLABLE
				where TKey : notnull
#endif
		{
			GuardArgumentNotNull(nameof(expected), expected);
			GuardArgumentNotNull(nameof(collection), collection);

			var value = default(TValue);
			if (!collection.TryGetValue(expected, out value))
				throw ContainsException.ForKeyNotFound(
					ArgumentFormatter.Format(expected),
					CollectionTracker<TKey>.FormatStart(collection.Keys)
				);

			return value;
		}

		/// <summary>
		/// Verifies that a read-only dictionary contains a given key.
		/// </summary>
		/// <typeparam name="TKey">The type of the keys of the object to be verified.</typeparam>
		/// <typeparam name="TValue">The type of the values of the object to be verified.</typeparam>
		/// <param name="expected">The object expected to be in the collection.</param>
		/// <param name="collection">The collection to be inspected.</param>
		/// <returns>The value associated with <paramref name="expected"/>.</returns>
		/// <exception cref="ContainsException">Thrown when the object is not present in the collection</exception>
		public static TValue Contains<[DynamicallyAccessedMembers(
					DynamicallyAccessedMemberTypes.PublicFields
					| DynamicallyAccessedMemberTypes.NonPublicFields
					| DynamicallyAccessedMemberTypes.PublicProperties
					| DynamicallyAccessedMemberTypes.NonPublicProperties
					| DynamicallyAccessedMemberTypes.PublicMethods)] TKey, TValue>(
			TKey expected,
			IReadOnlyDictionary<TKey, TValue> collection)
#if XUNIT_NULLABLE
				where TKey : notnull
#endif
		{
			GuardArgumentNotNull(nameof(expected), expected);
			GuardArgumentNotNull(nameof(collection), collection);

			var value = default(TValue);
			if (!collection.TryGetValue(expected, out value))
				throw ContainsException.ForKeyNotFound(
					ArgumentFormatter.Format(expected),
					CollectionTracker<TKey>.FormatStart(collection.Keys)
				);

			return value;
		}

		/// <summary>
		/// Verifies that a dictionary contains a given key.
		/// </summary>
		/// <typeparam name="TKey">The type of the keys of the object to be verified.</typeparam>
		/// <typeparam name="TValue">The type of the values of the object to be verified.</typeparam>
		/// <param name="expected">The object expected to be in the collection.</param>
		/// <param name="collection">The collection to be inspected.</param>
		/// <returns>The value associated with <paramref name="expected"/>.</returns>
		/// <exception cref="ContainsException">Thrown when the object is not present in the collection</exception>
		public static TValue Contains<[DynamicallyAccessedMembers(
					DynamicallyAccessedMemberTypes.PublicFields
					| DynamicallyAccessedMemberTypes.NonPublicFields
					| DynamicallyAccessedMemberTypes.PublicProperties
					| DynamicallyAccessedMemberTypes.NonPublicProperties
					| DynamicallyAccessedMemberTypes.PublicMethods)] TKey, TValue>(
			TKey expected,
			ConcurrentDictionary<TKey, TValue> collection)
#if XUNIT_NULLABLE
				where TKey : notnull
#endif
		{
			return Contains(expected, (IReadOnlyDictionary<TKey, TValue>)collection);
		}

		/// <summary>
		/// Verifies that a dictionary contains a given key.
		/// </summary>
		/// <typeparam name="TKey">The type of the keys of the object to be verified.</typeparam>
		/// <typeparam name="TValue">The type of the values of the object to be verified.</typeparam>
		/// <param name="expected">The object expected to be in the collection.</param>
		/// <param name="collection">The collection to be inspected.</param>
		/// <returns>The value associated with <paramref name="expected"/>.</returns>
		/// <exception cref="ContainsException">Thrown when the object is not present in the collection</exception>
		public static TValue Contains<[DynamicallyAccessedMembers(
					DynamicallyAccessedMemberTypes.PublicFields
					| DynamicallyAccessedMemberTypes.NonPublicFields
					| DynamicallyAccessedMemberTypes.PublicProperties
					| DynamicallyAccessedMemberTypes.NonPublicProperties
					| DynamicallyAccessedMemberTypes.PublicMethods)] TKey, TValue>(
			TKey expected,
			Dictionary<TKey, TValue> collection)
#if XUNIT_NULLABLE
				where TKey : notnull
#endif
		{
			return Contains(expected, (IReadOnlyDictionary<TKey, TValue>)collection);
		}

		/// <summary>
		/// Verifies that a dictionary contains a given key.
		/// </summary>
		/// <typeparam name="TKey">The type of the keys of the object to be verified.</typeparam>
		/// <typeparam name="TValue">The type of the values of the object to be verified.</typeparam>
		/// <param name="expected">The object expected to be in the collection.</param>
		/// <param name="collection">The collection to be inspected.</param>
		/// <returns>The value associated with <paramref name="expected"/>.</returns>
		/// <exception cref="ContainsException">Thrown when the object is not present in the collection</exception>
		public static TValue Contains<[DynamicallyAccessedMembers(
					DynamicallyAccessedMemberTypes.PublicFields
					| DynamicallyAccessedMemberTypes.NonPublicFields
					| DynamicallyAccessedMemberTypes.PublicProperties
					| DynamicallyAccessedMemberTypes.NonPublicProperties
					| DynamicallyAccessedMemberTypes.PublicMethods)] TKey, TValue>(
			TKey expected,
			ReadOnlyDictionary<TKey, TValue> collection)
#if XUNIT_NULLABLE
				where TKey : notnull
#endif
		{
			return Contains(expected, (IReadOnlyDictionary<TKey, TValue>)collection);
		}

#if XUNIT_IMMUTABLE_COLLECTIONS
		/// <summary>
		/// Verifies that a dictionary contains a given key.
		/// </summary>
		/// <typeparam name="TKey">The type of the keys of the object to be verified.</typeparam>
		/// <typeparam name="TValue">The type of the values of the object to be verified.</typeparam>
		/// <param name="expected">The object expected to be in the collection.</param>
		/// <param name="collection">The collection to be inspected.</param>
		/// <returns>The value associated with <paramref name="expected"/>.</returns>
		/// <exception cref="ContainsException">Thrown when the object is not present in the collection</exception>
		public static TValue Contains<[DynamicallyAccessedMembers(
					DynamicallyAccessedMemberTypes.PublicFields
					| DynamicallyAccessedMemberTypes.NonPublicFields
					| DynamicallyAccessedMemberTypes.PublicProperties
					| DynamicallyAccessedMemberTypes.NonPublicProperties
					| DynamicallyAccessedMemberTypes.PublicMethods)]TKey, TValue>(
			TKey expected,
			ImmutableDictionary<TKey, TValue> collection)
#if XUNIT_NULLABLE
				where TKey : notnull
#endif
		{
			return Contains(expected, (IReadOnlyDictionary<TKey, TValue>)collection);
		}
#endif

		/// <summary>
		/// Verifies that a dictionary does not contain a given key.
		/// </summary>
		/// <typeparam name="TKey">The type of the keys of the object to be verified.</typeparam>
		/// <typeparam name="TValue">The type of the values of the object to be verified.</typeparam>
		/// <param name="expected">The object expected to be in the collection.</param>
		/// <param name="collection">The collection to be inspected.</param>
		/// <exception cref="DoesNotContainException">Thrown when the object is present in the collection</exception>
		public static void DoesNotContain<[DynamicallyAccessedMembers(
					DynamicallyAccessedMemberTypes.PublicFields
					| DynamicallyAccessedMemberTypes.NonPublicFields
					| DynamicallyAccessedMemberTypes.PublicProperties
					| DynamicallyAccessedMemberTypes.NonPublicProperties
					| DynamicallyAccessedMemberTypes.PublicMethods)] TKey, TValue>(
			TKey expected,
			IDictionary<TKey, TValue> collection)
#if XUNIT_NULLABLE
				where TKey : notnull
#endif
		{
			GuardArgumentNotNull(nameof(expected), expected);
			GuardArgumentNotNull(nameof(collection), collection);

			// Do not forward to DoesNotContain(expected, collection.Keys) as we want the default SDK behavior
			if (collection.ContainsKey(expected))
				throw DoesNotContainException.ForKeyFound(
					ArgumentFormatter.Format(expected),
					CollectionTracker<TKey>.FormatStart(collection.Keys)
				);
		}

		/// <summary>
		/// Verifies that a dictionary does not contain a given key.
		/// </summary>
		/// <typeparam name="TKey">The type of the keys of the object to be verified.</typeparam>
		/// <typeparam name="TValue">The type of the values of the object to be verified.</typeparam>
		/// <param name="expected">The object expected to be in the collection.</param>
		/// <param name="collection">The collection to be inspected.</param>
		/// <exception cref="DoesNotContainException">Thrown when the object is present in the collection</exception>
		public static void DoesNotContain<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties | DynamicallyAccessedMemberTypes.PublicMethods)] TKey, TValue>(
			TKey expected,
			IReadOnlyDictionary<TKey, TValue> collection)
#if XUNIT_NULLABLE
				where TKey : notnull
#endif
		{
			GuardArgumentNotNull(nameof(expected), expected);
			GuardArgumentNotNull(nameof(collection), collection);

			// Do not forward to DoesNotContain(expected, collection.Keys) as we want the default SDK behavior
			if (collection.ContainsKey(expected))
				throw DoesNotContainException.ForKeyFound(
					ArgumentFormatter.Format(expected),
					CollectionTracker<TKey>.FormatStart(collection.Keys)
				);
		}

		/// <summary>
		/// Verifies that a dictionary does not contain a given key.
		/// </summary>
		/// <typeparam name="TKey">The type of the keys of the object to be verified.</typeparam>
		/// <typeparam name="TValue">The type of the values of the object to be verified.</typeparam>
		/// <param name="expected">The object expected to be in the collection.</param>
		/// <param name="collection">The collection to be inspected.</param>
		/// <exception cref="DoesNotContainException">Thrown when the object is present in the collection</exception>
		public static void DoesNotContain<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties | DynamicallyAccessedMemberTypes.PublicMethods)] TKey, TValue>(
			TKey expected,
			ConcurrentDictionary<TKey, TValue> collection)
#if XUNIT_NULLABLE
				where TKey : notnull
#endif
		{
			DoesNotContain(expected, (IReadOnlyDictionary<TKey, TValue>)collection);
		}

		/// <summary>
		/// Verifies that a dictionary does not contain a given key.
		/// </summary>
		/// <typeparam name="TKey">The type of the keys of the object to be verified.</typeparam>
		/// <typeparam name="TValue">The type of the values of the object to be verified.</typeparam>
		/// <param name="expected">The object expected to be in the collection.</param>
		/// <param name="collection">The collection to be inspected.</param>
		/// <exception cref="DoesNotContainException">Thrown when the object is present in the collection</exception>
		public static void DoesNotContain<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties | DynamicallyAccessedMemberTypes.PublicMethods)] TKey, TValue>(
			TKey expected,
			Dictionary<TKey, TValue> collection)
#if XUNIT_NULLABLE
				where TKey : notnull
#endif
		{
			DoesNotContain(expected, (IReadOnlyDictionary<TKey, TValue>)collection);
		}

		/// <summary>
		/// Verifies that a dictionary does not contain a given key.
		/// </summary>
		/// <typeparam name="TKey">The type of the keys of the object to be verified.</typeparam>
		/// <typeparam name="TValue">The type of the values of the object to be verified.</typeparam>
		/// <param name="expected">The object expected to be in the collection.</param>
		/// <param name="collection">The collection to be inspected.</param>
		/// <exception cref="DoesNotContainException">Thrown when the object is present in the collection</exception>
		public static void DoesNotContain<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties | DynamicallyAccessedMemberTypes.PublicMethods)] TKey, TValue>(
			TKey expected,
			ReadOnlyDictionary<TKey, TValue> collection)
#if XUNIT_NULLABLE
				where TKey : notnull
#endif
		{
			DoesNotContain(expected, (IReadOnlyDictionary<TKey, TValue>)collection);
		}

#if XUNIT_IMMUTABLE_COLLECTIONS
		/// <summary>
		/// Verifies that a dictionary does not contain a given key.
		/// </summary>
		/// <typeparam name="TKey">The type of the keys of the object to be verified.</typeparam>
		/// <typeparam name="TValue">The type of the values of the object to be verified.</typeparam>
		/// <param name="expected">The object expected to be in the collection.</param>
		/// <param name="collection">The collection to be inspected.</param>
		/// <exception cref="DoesNotContainException">Thrown when the object is present in the collection</exception>
		public static void DoesNotContain<[DynamicallyAccessedMembers(
					DynamicallyAccessedMemberTypes.PublicFields
					| DynamicallyAccessedMemberTypes.NonPublicFields
					| DynamicallyAccessedMemberTypes.PublicProperties
					| DynamicallyAccessedMemberTypes.NonPublicProperties
					| DynamicallyAccessedMemberTypes.PublicMethods)]TKey, TValue>(
			TKey expected,
			ImmutableDictionary<TKey, TValue> collection)
#if XUNIT_NULLABLE
				where TKey : notnull
#endif
		{
			DoesNotContain(expected, (IReadOnlyDictionary<TKey, TValue>)collection);
		}
#endif
	}
}
