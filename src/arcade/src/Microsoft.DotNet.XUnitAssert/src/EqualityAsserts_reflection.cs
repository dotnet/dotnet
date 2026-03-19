#if !XUNIT_AOT

#if XUNIT_NULLABLE
#nullable enable
#else
// In case this is source-imported with global nullable enabled but no XUNIT_NULLABLE
#pragma warning disable CS8603
#pragma warning disable CS8604
#endif

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xunit.Internal;
using Xunit.Sdk;

#if XUNIT_OVERLOAD_RESOLUTION_PRIORITY
using System.Runtime.CompilerServices;
#endif

namespace Xunit
{
	partial class Assert
	{
		static readonly Type typeofSet = typeof(ISet<>);

		/// <summary>
		/// Verifies that two arrays of un-managed type T are equal, using Span&lt;T&gt;.SequenceEqual.
		/// This can be significantly faster than generic enumerables, when the collections are actually
		/// equal, because the system can optimize packed-memory comparisons for value type arrays.
		/// </summary>
		/// <typeparam name="T">The type of items whose arrays are to be compared</typeparam>
		/// <param name="expected">The expected value</param>
		/// <param name="actual">The value to be compared against</param>
		/// <remarks>
		/// If <see cref="MemoryExtensions.SequenceEqual{T}(Span{T}, ReadOnlySpan{T})"/> fails, a call
		/// to <see cref="Assert.Equal{T}(T, T)"/> is made, to provide a more meaningful error message.
		/// </remarks>
		public static void Equal<T>(
#if XUNIT_NULLABLE
			T[]? expected,
			T[]? actual)
				where T : unmanaged, IEquatable<T>
#else
			T[] expected,
			T[] actual)
				where T : IEquatable<T>
#endif
		{
			if (expected == null && actual == null)
				return;

			if (expected == null || actual == null || !expected.AsSpan().SequenceEqual(actual))
				// Call into Equal<object> (even though we'll re-enumerate) so we get proper formatting
				// of the sequence, including the "first mismatch" pointer
				Equal<object>(expected, actual);
		}

#if XUNIT_NULLABLE
		static string? GetCollectionDisplay(
			Type? expectedType,
			Type? expectedTypeDefinition,
			Type? actualType,
			Type? actualTypeDefinition)
#else
		static string GetCollectionDisplay(
			Type expectedType,
			Type expectedTypeDefinition,
			Type actualType,
			Type actualTypeDefinition)
#endif
		{
			var expectedInterfaceTypeDefinitions = expectedType?.GetInterfaces().Where(i => i.IsGenericType).Select(i => i.GetGenericTypeDefinition());
			var actualInterfaceTypeDefinitions = actualType?.GetInterfaces().Where(i => i.IsGenericType).Select(i => i.GetGenericTypeDefinition());

			if (expectedTypeDefinition == typeofDictionary && actualTypeDefinition == typeofDictionary)
				return "Dictionaries";
			else if (expectedTypeDefinition == typeofHashSet && actualTypeDefinition == typeofHashSet)
				return "HashSets";
#pragma warning disable CA1508
			else if (expectedInterfaceTypeDefinitions != null && actualInterfaceTypeDefinitions != null && expectedInterfaceTypeDefinitions.Contains(typeofSet) && actualInterfaceTypeDefinitions.Contains(typeofSet))
#pragma warning restore CA1508
				return "Sets";

			return null;
		}

		/// <summary>
		/// Verifies that two arrays of un-managed type T are not equal, using Span&lt;T&gt;.SequenceEqual.
		/// </summary>
		/// <typeparam name="T">The type of items whose arrays are to be compared</typeparam>
		/// <param name="expected">The expected value</param>
		/// <param name="actual">The value to be compared against</param>
		public static void NotEqual<T>(
#if XUNIT_NULLABLE
			T[]? expected,
			T[]? actual)
				where T : unmanaged, IEquatable<T>
#else
			T[] expected,
			T[] actual)
				where T : IEquatable<T>
#endif
		{
			// Call into NotEqual<object> so we get proper formatting of the sequence
			if (expected == null && actual == null)
				NotEqual<object>(expected, actual);
			if (expected == null || actual == null)
				return;
			if (expected.AsSpan().SequenceEqual(actual))
				NotEqual<object>(expected, actual);
		}

		/// <summary>
		/// Verifies that two objects are strictly not equal, using the type's default comparer.
		/// </summary>
		/// <typeparam name="T">The type of the objects to be compared</typeparam>
		/// <param name="expected">The expected object</param>
		/// <param name="actual">The actual object</param>
		public static void NotStrictEqual<T>(
#if XUNIT_NULLABLE
			T? expected,
			T? actual)
#else
			T expected,
			T actual)
#endif
		{
			if (!EqualityComparer<T>.Default.Equals(expected, actual))
				return;

			throw NotStrictEqualException.ForEqualValues(
				ArgumentFormatter.Format(expected),
				ArgumentFormatter.Format(actual)
			);
		}

		/// <summary>
		/// Verifies that two objects are strictly equal, using the type's default comparer.
		/// </summary>
		/// <typeparam name="T">The type of the objects to be compared</typeparam>
		/// <param name="expected">The expected value</param>
		/// <param name="actual">The value to be compared against</param>
		public static void StrictEqual<T>(
#if XUNIT_NULLABLE
			T? expected,
			T? actual)
#else
			T expected,
			T actual)
#endif
		{
			if (EqualityComparer<T>.Default.Equals(expected, actual))
				return;

			throw StrictEqualException.ForEqualValues(
				ArgumentFormatter.Format(expected),
				ArgumentFormatter.Format(actual)
			);
		}
	}
}

#endif  // !XUNIT_AOT
