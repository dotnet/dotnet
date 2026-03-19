#if XUNIT_AOT

#if XUNIT_NULLABLE
#nullable enable
#else
// In case this is source-imported with global nullable enabled but no XUNIT_NULLABLE
#pragma warning disable CS8604
#endif

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Xunit.Sdk;

namespace Xunit
{
	partial class Assert
	{
		/// <summary>
		/// Verifies that two values in a <see cref="KeyValuePair{TKey, TValue}"/> are equal.
		/// </summary>
		/// <typeparam name="TKey">The type of the key.</typeparam>
		/// <typeparam name="TValue">The type of the value.</typeparam>
		/// <param name="expected">The expected key/value pair</param>
		/// <param name="actual">The actual key/value pair</param>
		[OverloadResolutionPriority(1)]
		public static void Equal<TKey, TValue>(
			KeyValuePair<TKey, TValue>? expected,
			KeyValuePair<TKey, TValue>? actual)
		{
			if (!expected.HasValue)
			{
				if (!actual.HasValue)
					return;

				throw EqualException.ForMismatchedValues(ArgumentFormatter.Format(expected), ArgumentFormatter.Format(actual));
			}

			if (actual == null)
				throw EqualException.ForMismatchedValues(ArgumentFormatter.Format(expected), ArgumentFormatter.Format(actual));

			var keyComparer = new AssertEqualityComparer<TKey>();
			if (!keyComparer.Equals(expected.Value.Key, actual.Value.Key))
				throw EqualException.ForMismatchedValues(
					ArgumentFormatter.Format(expected.Value.Key),
					ArgumentFormatter.Format(actual.Value.Key),
					"Keys differ in KeyValuePair"
				);

			var valueComparer = new AssertEqualityComparer<TValue>();
			if (!valueComparer.Equals(expected.Value.Value, actual.Value.Value))
				throw EqualException.ForMismatchedValues(
					ArgumentFormatter.Format(expected.Value.Value),
					ArgumentFormatter.Format(actual.Value.Value),
					"Values differ in KeyValuePair"
				);
		}

#pragma warning disable IDE0060 // Remove unused parameter

#if XUNIT_NULLABLE
		static string? GetCollectionDisplay(
			Type? expectedType,
			Type? expectedTypeDefinition,
			Type? actualType,
			Type? actualTypeDefinition)
#else
		static string? GetCollectionDisplay(
			Type expectedType,
			Type expectedTypeDefinition,
			Type actualType,
			Type actualTypeDefinition)
#endif
		{
			if (expectedTypeDefinition == typeofDictionary && actualTypeDefinition == typeofDictionary)
				return "Dictionaries";
			else if (expectedTypeDefinition == typeofHashSet && actualTypeDefinition == typeofHashSet)
				return "HashSets";

			return null;
		}

#pragma warning restore IDE0060 // Remove unused parameter

		/// <summary>
		/// Verifies that two values in a <see cref="KeyValuePair{TKey, TValue}"/> are not equal.
		/// </summary>
		/// <typeparam name="TKey">The type of the key.</typeparam>
		/// <typeparam name="TValue">The type of the value.</typeparam>
		/// <param name="expected">The expected key/value pair</param>
		/// <param name="actual">The actual key/value pair</param>
		[OverloadResolutionPriority(1)]
		public static void NotEqual<TKey, TValue>(
			KeyValuePair<TKey, TValue>? expected,
			KeyValuePair<TKey, TValue>? actual)
		{
			if (expected == null)
			{
				if (actual != null)
					return;

				throw NotEqualException.ForEqualValues("null", "null");
			}

			if (actual == null)
				return;

			var keyComparer = new AssertEqualityComparer<TKey>();
			if (!keyComparer.Equals(expected.Value.Key, actual.Value.Key))
				return;

			var valueComparer = new AssertEqualityComparer<TValue>();
			if (!valueComparer.Equals(expected.Value.Value, actual.Value.Value))
				return;

			throw NotEqualException.ForEqualValues(ArgumentFormatter.Format(expected.Value), ArgumentFormatter.Format(actual.Value));
		}

		/// <summary>
		/// Verifies that two objects are strictly not equal, using <see cref="object.Equals(object, object)"/>.
		/// </summary>
		/// <param name="expected">The expected object</param>
		/// <param name="actual">The actual object</param>
		public static void NotStrictEqual(
#if XUNIT_NULLABLE
			object? expected,
			object? actual)
#else
			object expected,
			object actual)
#endif
		{
			if (!object.Equals(expected, actual))
				return;

			throw NotStrictEqualException.ForEqualValues(
				ArgumentFormatter.Format(expected),
				ArgumentFormatter.Format(actual)
			);
		}

		/// <summary>
		/// Verifies that two objects are strictly equal, using <see cref="object.Equals(object, object)"/>.
		/// </summary>
		/// <param name="expected">The expected object</param>
		/// <param name="actual">The actual object</param>
		public static void StrictEqual(
#if XUNIT_NULLABLE
			object? expected,
			object? actual)
#else
			object expected,
			object actual)
#endif
		{
			if (object.Equals(expected, actual))
				return;

			throw StrictEqualException.ForEqualValues(
				ArgumentFormatter.Format(expected),
				ArgumentFormatter.Format(actual)
			);
		}
	}
}

#endif  // XUNIT_AOT
