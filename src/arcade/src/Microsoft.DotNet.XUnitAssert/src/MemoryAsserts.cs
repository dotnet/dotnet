#if XUNIT_SPAN

#if XUNIT_NULLABLE
#nullable enable
#endif

using System;
using System.Diagnostics.CodeAnalysis;
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
		// While there is an implicit conversion operator from Memory<T> to ReadOnlyMemory<T>, the
		// compiler still stumbles to do this automatically, which means we end up with lots of overloads
		// with various arrangements of Memory<T> and ReadOnlyMemory<T>.

		// Also note that these classes will convert nulls into empty arrays automatically, since there
		// is no way to represent a null readonly struct.

		/// <summary>
		/// Verifies that a Memory contains a given sub-Memory, using the default <see cref="StringComparison.CurrentCulture"/> comparison type.
		/// </summary>
		/// <param name="expectedSubMemory">The sub-Memory expected to be in the Memory</param>
		/// <param name="actualMemory">The Memory to be inspected</param>
		/// <exception cref="ContainsException">Thrown when the sub-Memory is not present inside the Memory</exception>
		public static void Contains(
			Memory<char> expectedSubMemory,
			Memory<char> actualMemory) =>
				Contains((ReadOnlyMemory<char>)expectedSubMemory, (ReadOnlyMemory<char>)actualMemory, StringComparison.CurrentCulture);

		/// <summary>
		/// Verifies that a Memory contains a given sub-Memory, using the default <see cref="StringComparison.CurrentCulture"/> comparison type.
		/// </summary>
		/// <param name="expectedSubMemory">The sub-Memory expected to be in the Memory</param>
		/// <param name="actualMemory">The Memory to be inspected</param>
		/// <exception cref="ContainsException">Thrown when the sub-Memory is not present inside the Memory</exception>
		public static void Contains(
			Memory<char> expectedSubMemory,
			ReadOnlyMemory<char> actualMemory) =>
				Contains((ReadOnlyMemory<char>)expectedSubMemory, actualMemory, StringComparison.CurrentCulture);

		/// <summary>
		/// Verifies that a Memory contains a given sub-Memory, using the default <see cref="StringComparison.CurrentCulture"/> comparison type.
		/// </summary>
		/// <param name="expectedSubMemory">The sub-Memory expected to be in the Memory</param>
		/// <param name="actualMemory">The Memory to be inspected</param>
		/// <exception cref="ContainsException">Thrown when the sub-Memory is not present inside the Memory</exception>
		public static void Contains(
			ReadOnlyMemory<char> expectedSubMemory,
			Memory<char> actualMemory) =>
				Contains(expectedSubMemory, (ReadOnlyMemory<char>)actualMemory, StringComparison.CurrentCulture);

		/// <summary>
		/// Verifies that a Memory contains a given sub-Memory, using the default <see cref="StringComparison.CurrentCulture"/> comparison type.
		/// </summary>
		/// <param name="expectedSubMemory">The sub-Memory expected to be in the Memory</param>
		/// <param name="actualMemory">The Memory to be inspected</param>
		/// <exception cref="ContainsException">Thrown when the sub-Memory is not present inside the Memory</exception>
		public static void Contains(
			ReadOnlyMemory<char> expectedSubMemory,
			ReadOnlyMemory<char> actualMemory) =>
				Contains(expectedSubMemory, actualMemory, StringComparison.CurrentCulture);

		/// <summary>
		/// Verifies that a Memory contains a given sub-Memory, using the given comparison type.
		/// </summary>
		/// <param name="expectedSubMemory">The sub-Memory expected to be in the Memory</param>
		/// <param name="actualMemory">The Memory to be inspected</param>
		/// <param name="comparisonType">The type of string comparison to perform</param>
		/// <exception cref="ContainsException">Thrown when the sub-Memory is not present inside the Memory</exception>
		public static void Contains(
			Memory<char> expectedSubMemory,
			Memory<char> actualMemory,
			StringComparison comparisonType = StringComparison.CurrentCulture) =>
				Contains((ReadOnlyMemory<char>)expectedSubMemory, (ReadOnlyMemory<char>)actualMemory, comparisonType);

		/// <summary>
		/// Verifies that a Memory contains a given sub-Memory, using the given comparison type.
		/// </summary>
		/// <param name="expectedSubMemory">The sub-Memory expected to be in the Memory</param>
		/// <param name="actualMemory">The Memory to be inspected</param>
		/// <param name="comparisonType">The type of string comparison to perform</param>
		/// <exception cref="ContainsException">Thrown when the sub-Memory is not present inside the Memory</exception>
		public static void Contains(
			Memory<char> expectedSubMemory,
			ReadOnlyMemory<char> actualMemory,
			StringComparison comparisonType = StringComparison.CurrentCulture) =>
				Contains((ReadOnlyMemory<char>)expectedSubMemory, actualMemory, comparisonType);

		/// <summary>
		/// Verifies that a Memory contains a given sub-Memory, using the given comparison type.
		/// </summary>
		/// <param name="expectedSubMemory">The sub-Memory expected to be in the Memory</param>
		/// <param name="actualMemory">The Memory to be inspected</param>
		/// <param name="comparisonType">The type of string comparison to perform</param>
		/// <exception cref="ContainsException">Thrown when the sub-Memory is not present inside the Memory</exception>
		public static void Contains(
			ReadOnlyMemory<char> expectedSubMemory,
			Memory<char> actualMemory,
			StringComparison comparisonType = StringComparison.CurrentCulture) =>
				Contains(expectedSubMemory, (ReadOnlyMemory<char>)actualMemory, comparisonType);

		/// <summary>
		/// Verifies that a Memory contains a given sub-Memory, using the given comparison type.
		/// </summary>
		/// <param name="expectedSubMemory">The sub-Memory expected to be in the Memory</param>
		/// <param name="actualMemory">The Memory to be inspected</param>
		/// <param name="comparisonType">The type of string comparison to perform</param>
		/// <exception cref="ContainsException">Thrown when the sub-Memory is not present inside the Memory</exception>
		public static void Contains(
			ReadOnlyMemory<char> expectedSubMemory,
			ReadOnlyMemory<char> actualMemory,
			StringComparison comparisonType = StringComparison.CurrentCulture)
		{
			GuardArgumentNotNull(nameof(expectedSubMemory), expectedSubMemory);

			Contains(expectedSubMemory.Span, actualMemory.Span, comparisonType);
		}

		/// <summary>
		/// Verifies that a Memory contains a given sub-Memory
		/// </summary>
		/// <param name="expectedSubMemory">The sub-Memory expected to be in the Memory</param>
		/// <param name="actualMemory">The Memory to be inspected</param>
		/// <exception cref="ContainsException">Thrown when the sub-Memory is not present inside the Memory</exception>
		public static void Contains<[DynamicallyAccessedMembers(
					DynamicallyAccessedMemberTypes.PublicFields
					| DynamicallyAccessedMemberTypes.NonPublicFields
					| DynamicallyAccessedMemberTypes.PublicProperties
					| DynamicallyAccessedMemberTypes.NonPublicProperties
					| DynamicallyAccessedMemberTypes.PublicMethods)] T>(
			Memory<T> expectedSubMemory,
			Memory<T> actualMemory)
				where T : IEquatable<T> =>
					Contains((ReadOnlyMemory<T>)expectedSubMemory, (ReadOnlyMemory<T>)actualMemory);

		/// <summary>
		/// Verifies that a Memory contains a given sub-Memory
		/// </summary>
		/// <param name="expectedSubMemory">The sub-Memory expected to be in the Memory</param>
		/// <param name="actualMemory">The Memory to be inspected</param>
		/// <exception cref="ContainsException">Thrown when the sub-Memory is not present inside the Memory</exception>
		public static void Contains<[DynamicallyAccessedMembers(
					DynamicallyAccessedMemberTypes.PublicFields
					| DynamicallyAccessedMemberTypes.NonPublicFields
					| DynamicallyAccessedMemberTypes.PublicProperties
					| DynamicallyAccessedMemberTypes.NonPublicProperties
					| DynamicallyAccessedMemberTypes.PublicMethods)] T>(
			Memory<T> expectedSubMemory,
			ReadOnlyMemory<T> actualMemory)
				where T : IEquatable<T> =>
					Contains((ReadOnlyMemory<T>)expectedSubMemory, actualMemory);

		/// <summary>
		/// Verifies that a Memory contains a given sub-Memory
		/// </summary>
		/// <param name="expectedSubMemory">The sub-Memory expected to be in the Memory</param>
		/// <param name="actualMemory">The Memory to be inspected</param>
		/// <exception cref="ContainsException">Thrown when the sub-Memory is not present inside the Memory</exception>
		public static void Contains<[DynamicallyAccessedMembers(
					DynamicallyAccessedMemberTypes.PublicFields
					| DynamicallyAccessedMemberTypes.NonPublicFields
					| DynamicallyAccessedMemberTypes.PublicProperties
					| DynamicallyAccessedMemberTypes.NonPublicProperties
					| DynamicallyAccessedMemberTypes.PublicMethods)] T>(
			ReadOnlyMemory<T> expectedSubMemory,
			Memory<T> actualMemory)
				where T : IEquatable<T> =>
					Contains(expectedSubMemory, (ReadOnlyMemory<T>)actualMemory);

		/// <summary>
		/// Verifies that a Memory contains a given sub-Memory
		/// </summary>
		/// <param name="expectedSubMemory">The sub-Memory expected to be in the Memory</param>
		/// <param name="actualMemory">The Memory to be inspected</param>
		/// <exception cref="ContainsException">Thrown when the sub-Memory is not present inside the Memory</exception>
		public static void Contains<[DynamicallyAccessedMembers(
					DynamicallyAccessedMemberTypes.PublicFields
					| DynamicallyAccessedMemberTypes.NonPublicFields
					| DynamicallyAccessedMemberTypes.PublicProperties
					| DynamicallyAccessedMemberTypes.NonPublicProperties
					| DynamicallyAccessedMemberTypes.PublicMethods)] T>(
			ReadOnlyMemory<T> expectedSubMemory,
			ReadOnlyMemory<T> actualMemory)
				where T : IEquatable<T>
		{
			GuardArgumentNotNull(nameof(expectedSubMemory), expectedSubMemory);

			if (actualMemory.Span.IndexOf(expectedSubMemory.Span) < 0)
				throw ContainsException.ForSubMemoryNotFound(
					CollectionTracker<T>.FormatStart(expectedSubMemory.Span),
					CollectionTracker<T>.FormatStart(actualMemory.Span)
				);
		}

		/// <summary>
		/// Verifies that a Memory does not contain a given sub-Memory, using the default <see cref="StringComparison.CurrentCulture"/> comparison type.
		/// </summary>
		/// <param name="expectedSubMemory">The sub-Memory expected not to be in the Memory</param>
		/// <param name="actualMemory">The Memory to be inspected</param>
		/// <exception cref="DoesNotContainException">Thrown when the sub-Memory is present inside the Memory</exception>
		public static void DoesNotContain(
			Memory<char> expectedSubMemory,
			Memory<char> actualMemory) =>
				DoesNotContain((ReadOnlyMemory<char>)expectedSubMemory, (ReadOnlyMemory<char>)actualMemory, StringComparison.CurrentCulture);

		/// <summary>
		/// Verifies that a Memory does not contain a given sub-Memory, using the default <see cref="StringComparison.CurrentCulture"/> comparison type.
		/// </summary>
		/// <param name="expectedSubMemory">The sub-Memory expected not to be in the Memory</param>
		/// <param name="actualMemory">The Memory to be inspected</param>
		/// <exception cref="DoesNotContainException">Thrown when the sub-Memory is present inside the Memory</exception>
		public static void DoesNotContain(
			Memory<char> expectedSubMemory,
			ReadOnlyMemory<char> actualMemory) =>
				DoesNotContain((ReadOnlyMemory<char>)expectedSubMemory, actualMemory, StringComparison.CurrentCulture);

		/// <summary>
		/// Verifies that a Memory does not contain a given sub-Memory, using the default <see cref="StringComparison.CurrentCulture"/> comparison type.
		/// </summary>
		/// <param name="expectedSubMemory">The sub-Memory expected not to be in the Memory</param>
		/// <param name="actualMemory">The Memory to be inspected</param>
		/// <exception cref="DoesNotContainException">Thrown when the sub-Memory is present inside the Memory</exception>
		public static void DoesNotContain(
			ReadOnlyMemory<char> expectedSubMemory,
			Memory<char> actualMemory) =>
				DoesNotContain(expectedSubMemory, (ReadOnlyMemory<char>)actualMemory, StringComparison.CurrentCulture);

		/// <summary>
		/// Verifies that a Memory does not contain a given sub-Memory, using the default <see cref="StringComparison.CurrentCulture"/> comparison type.
		/// </summary>
		/// <param name="expectedSubMemory">The sub-Memory expected not to be in the Memory</param>
		/// <param name="actualMemory">The Memory to be inspected</param>
		/// <exception cref="DoesNotContainException">Thrown when the sub-Memory is present inside the Memory</exception>
		public static void DoesNotContain(
			ReadOnlyMemory<char> expectedSubMemory,
			ReadOnlyMemory<char> actualMemory) =>
				DoesNotContain(expectedSubMemory, actualMemory, StringComparison.CurrentCulture);

		/// <summary>
		/// Verifies that a Memory does not contain a given sub-Memory, using the given comparison type.
		/// </summary>
		/// <param name="expectedSubMemory">The sub-Memory expected not to be in the Memory</param>
		/// <param name="actualMemory">The Memory to be inspected</param>
		/// <param name="comparisonType">The type of string comparison to perform</param>
		/// <exception cref="DoesNotContainException">Thrown when the sub-Memory is present inside the Memory</exception>
		public static void DoesNotContain(
			Memory<char> expectedSubMemory,
			Memory<char> actualMemory,
			StringComparison comparisonType = StringComparison.CurrentCulture) =>
				DoesNotContain((ReadOnlyMemory<char>)expectedSubMemory, (ReadOnlyMemory<char>)actualMemory, comparisonType);

		/// <summary>
		/// Verifies that a Memory does not contain a given sub-Memory, using the given comparison type.
		/// </summary>
		/// <param name="expectedSubMemory">The sub-Memory expected not to be in the Memory</param>
		/// <param name="actualMemory">The Memory to be inspected</param>
		/// <param name="comparisonType">The type of string comparison to perform</param>
		/// <exception cref="DoesNotContainException">Thrown when the sub-Memory is present inside the Memory</exception>
		public static void DoesNotContain(
			Memory<char> expectedSubMemory,
			ReadOnlyMemory<char> actualMemory,
			StringComparison comparisonType = StringComparison.CurrentCulture) =>
				DoesNotContain((ReadOnlyMemory<char>)expectedSubMemory, actualMemory, comparisonType);

		/// <summary>
		/// Verifies that a Memory does not contain a given sub-Memory, using the given comparison type.
		/// </summary>
		/// <param name="expectedSubMemory">The sub-Memory expected not to be in the Memory</param>
		/// <param name="actualMemory">The Memory to be inspected</param>
		/// <param name="comparisonType">The type of string comparison to perform</param>
		/// <exception cref="DoesNotContainException">Thrown when the sub-Memory is present inside the Memory</exception>
		public static void DoesNotContain(
			ReadOnlyMemory<char> expectedSubMemory,
			Memory<char> actualMemory,
			StringComparison comparisonType = StringComparison.CurrentCulture) =>
				DoesNotContain(expectedSubMemory, (ReadOnlyMemory<char>)actualMemory, comparisonType);

		/// <summary>
		/// Verifies that a Memory does not contain a given sub-Memory, using the given comparison type.
		/// </summary>
		/// <param name="expectedSubMemory">The sub-Memory expected not to be in the Memory</param>
		/// <param name="actualMemory">The Memory to be inspected</param>
		/// <param name="comparisonType">The type of string comparison to perform</param>
		/// <exception cref="DoesNotContainException">Thrown when the sub-Memory is present inside the Memory</exception>
		public static void DoesNotContain(
			ReadOnlyMemory<char> expectedSubMemory,
			ReadOnlyMemory<char> actualMemory,
			StringComparison comparisonType = StringComparison.CurrentCulture)
		{
			GuardArgumentNotNull(nameof(expectedSubMemory), expectedSubMemory);

			DoesNotContain(expectedSubMemory.Span, actualMemory.Span, comparisonType);
		}

		/// <summary>
		/// Verifies that a Memory does not contain a given sub-Memory
		/// </summary>
		/// <param name="expectedSubMemory">The sub-Memory expected not to be in the Memory</param>
		/// <param name="actualMemory">The Memory to be inspected</param>
		/// <exception cref="DoesNotContainException">Thrown when the sub-Memory is present inside the Memory</exception>
		public static void DoesNotContain<[DynamicallyAccessedMembers(
					DynamicallyAccessedMemberTypes.PublicFields
					| DynamicallyAccessedMemberTypes.NonPublicFields
					| DynamicallyAccessedMemberTypes.PublicProperties
					| DynamicallyAccessedMemberTypes.NonPublicProperties
					| DynamicallyAccessedMemberTypes.PublicMethods)] T>(
			Memory<T> expectedSubMemory,
			Memory<T> actualMemory)
				where T : IEquatable<T> =>
					DoesNotContain((ReadOnlyMemory<T>)expectedSubMemory, (ReadOnlyMemory<T>)actualMemory);

		/// <summary>
		/// Verifies that a Memory does not contain a given sub-Memory
		/// </summary>
		/// <param name="expectedSubMemory">The sub-Memory expected not to be in the Memory</param>
		/// <param name="actualMemory">The Memory to be inspected</param>
		/// <exception cref="DoesNotContainException">Thrown when the sub-Memory is present inside the Memory</exception>
		public static void DoesNotContain<[DynamicallyAccessedMembers(
					DynamicallyAccessedMemberTypes.PublicFields
					| DynamicallyAccessedMemberTypes.NonPublicFields
					| DynamicallyAccessedMemberTypes.PublicProperties
					| DynamicallyAccessedMemberTypes.NonPublicProperties
					| DynamicallyAccessedMemberTypes.PublicMethods)] T>(
			Memory<T> expectedSubMemory,
			ReadOnlyMemory<T> actualMemory)
				where T : IEquatable<T> =>
					DoesNotContain((ReadOnlyMemory<T>)expectedSubMemory, actualMemory);

		/// <summary>
		/// Verifies that a Memory does not contain a given sub-Memory
		/// </summary>
		/// <param name="expectedSubMemory">The sub-Memory expected not to be in the Memory</param>
		/// <param name="actualMemory">The Memory to be inspected</param>
		/// <exception cref="DoesNotContainException">Thrown when the sub-Memory is present inside the Memory</exception>
		public static void DoesNotContain<[DynamicallyAccessedMembers(
					DynamicallyAccessedMemberTypes.PublicFields
					| DynamicallyAccessedMemberTypes.NonPublicFields
					| DynamicallyAccessedMemberTypes.PublicProperties
					| DynamicallyAccessedMemberTypes.NonPublicProperties
					| DynamicallyAccessedMemberTypes.PublicMethods)] T>(
			ReadOnlyMemory<T> expectedSubMemory,
			Memory<T> actualMemory)
				where T : IEquatable<T> =>
					DoesNotContain(expectedSubMemory, (ReadOnlyMemory<T>)actualMemory);

		/// <summary>
		/// Verifies that a Memory does not contain a given sub-Memory
		/// </summary>
		/// <param name="expectedSubMemory">The sub-Memory expected not to be in the Memory</param>
		/// <param name="actualMemory">The Memory to be inspected</param>
		/// <exception cref="DoesNotContainException">Thrown when the sub-Memory is present inside the Memory</exception>
		public static void DoesNotContain<[DynamicallyAccessedMembers(
					DynamicallyAccessedMemberTypes.PublicFields
					| DynamicallyAccessedMemberTypes.NonPublicFields
					| DynamicallyAccessedMemberTypes.PublicProperties
					| DynamicallyAccessedMemberTypes.NonPublicProperties
					| DynamicallyAccessedMemberTypes.PublicMethods)] T>(
			ReadOnlyMemory<T> expectedSubMemory,
			ReadOnlyMemory<T> actualMemory)
				where T : IEquatable<T>
		{
			GuardArgumentNotNull(nameof(expectedSubMemory), expectedSubMemory);

			var expectedSpan = expectedSubMemory.Span;
			var actualSpan = actualMemory.Span;
			var idx = actualSpan.IndexOf(expectedSpan);

			if (idx > -1)
			{
				int? failurePointerIndent;
				var formattedExpected = CollectionTracker<T>.FormatStart(expectedSpan);
				var formattedActual = CollectionTracker<T>.FormatIndexedMismatch(actualSpan, idx, out failurePointerIndent);

				throw DoesNotContainException.ForSubMemoryFound(formattedExpected, idx, failurePointerIndent, formattedActual);
			}
		}

		/// <summary>
		/// Verifies that a Memory ends with a given sub-Memory, using the default <see cref="StringComparison.CurrentCulture"/> comparison type.
		/// </summary>
		/// <param name="expectedEndMemory">The sub-Memory expected to be at the end of the Memory</param>
		/// <param name="actualMemory">The Memory to be inspected</param>
		/// <exception cref="EndsWithException">Thrown when the Memory does not end with the expected subMemory</exception>
		public static void EndsWith(
			Memory<char> expectedEndMemory,
			Memory<char> actualMemory) =>
				EndsWith((ReadOnlyMemory<char>)expectedEndMemory, (ReadOnlyMemory<char>)actualMemory, StringComparison.CurrentCulture);

		/// <summary>
		/// Verifies that a Memory ends with a given sub-Memory, using the default <see cref="StringComparison.CurrentCulture"/> comparison type.
		/// </summary>
		/// <param name="expectedEndMemory">The sub-Memory expected to be at the end of the Memory</param>
		/// <param name="actualMemory">The Memory to be inspected</param>
		/// <exception cref="EndsWithException">Thrown when the Memory does not end with the expected subMemory</exception>
		public static void EndsWith(
			Memory<char> expectedEndMemory,
			ReadOnlyMemory<char> actualMemory) =>
				EndsWith((ReadOnlyMemory<char>)expectedEndMemory, actualMemory, StringComparison.CurrentCulture);

		/// <summary>
		/// Verifies that a Memory ends with a given sub-Memory, using the default <see cref="StringComparison.CurrentCulture"/> comparison type.
		/// </summary>
		/// <param name="expectedEndMemory">The sub-Memory expected to be at the end of the Memory</param>
		/// <param name="actualMemory">The Memory to be inspected</param>
		/// <exception cref="EndsWithException">Thrown when the Memory does not end with the expected subMemory</exception>
		public static void EndsWith(
			ReadOnlyMemory<char> expectedEndMemory,
			Memory<char> actualMemory) =>
				EndsWith(expectedEndMemory, (ReadOnlyMemory<char>)actualMemory, StringComparison.CurrentCulture);

		/// <summary>
		/// Verifies that a Memory ends with a given sub-Memory, using the default <see cref="StringComparison.CurrentCulture"/> comparison type.
		/// </summary>
		/// <param name="expectedEndMemory">The sub-Memory expected to be at the end of the Memory</param>
		/// <param name="actualMemory">The Memory to be inspected</param>
		/// <exception cref="EndsWithException">Thrown when the Memory does not end with the expected subMemory</exception>
		public static void EndsWith(
			ReadOnlyMemory<char> expectedEndMemory,
			ReadOnlyMemory<char> actualMemory) =>
				EndsWith(expectedEndMemory, actualMemory, StringComparison.CurrentCulture);

		/// <summary>
		/// Verifies that a Memory ends with a given sub-Memory, using the given comparison type.
		/// </summary>
		/// <param name="expectedEndMemory">The sub-Memory expected to be at the end of the Memory</param>
		/// <param name="actualMemory">The Memory to be inspected</param>
		/// <param name="comparisonType">The type of string comparison to perform</param>
		/// <exception cref="EndsWithException">Thrown when the Memory does not end with the expected subMemory</exception>
		public static void EndsWith(
			Memory<char> expectedEndMemory,
			Memory<char> actualMemory,
			StringComparison comparisonType = StringComparison.CurrentCulture) =>
				EndsWith((ReadOnlyMemory<char>)expectedEndMemory, (ReadOnlyMemory<char>)actualMemory, comparisonType);

		/// <summary>
		/// Verifies that a Memory ends with a given sub-Memory, using the given comparison type.
		/// </summary>
		/// <param name="expectedEndMemory">The sub-Memory expected to be at the end of the Memory</param>
		/// <param name="actualMemory">The Memory to be inspected</param>
		/// <param name="comparisonType">The type of string comparison to perform</param>
		/// <exception cref="EndsWithException">Thrown when the Memory does not end with the expected subMemory</exception>
		public static void EndsWith(
			Memory<char> expectedEndMemory,
			ReadOnlyMemory<char> actualMemory,
			StringComparison comparisonType = StringComparison.CurrentCulture) =>
				EndsWith((ReadOnlyMemory<char>)expectedEndMemory, actualMemory, comparisonType);

		/// <summary>
		/// Verifies that a Memory ends with a given sub-Memory, using the given comparison type.
		/// </summary>
		/// <param name="expectedEndMemory">The sub-Memory expected to be at the end of the Memory</param>
		/// <param name="actualMemory">The Memory to be inspected</param>
		/// <param name="comparisonType">The type of string comparison to perform</param>
		/// <exception cref="EndsWithException">Thrown when the Memory does not end with the expected subMemory</exception>
		public static void EndsWith(
			ReadOnlyMemory<char> expectedEndMemory,
			Memory<char> actualMemory,
			StringComparison comparisonType = StringComparison.CurrentCulture) =>
				EndsWith(expectedEndMemory, (ReadOnlyMemory<char>)actualMemory, comparisonType);

		/// <summary>
		/// Verifies that a Memory ends with a given sub-Memory, using the given comparison type.
		/// </summary>
		/// <param name="expectedEndMemory">The sub-Memory expected to be at the end of the Memory</param>
		/// <param name="actualMemory">The Memory to be inspected</param>
		/// <param name="comparisonType">The type of string comparison to perform</param>
		/// <exception cref="EndsWithException">Thrown when the Memory does not end with the expected subMemory</exception>
		public static void EndsWith(
			ReadOnlyMemory<char> expectedEndMemory,
			ReadOnlyMemory<char> actualMemory,
			StringComparison comparisonType = StringComparison.CurrentCulture)
		{
			GuardArgumentNotNull(nameof(expectedEndMemory), expectedEndMemory);

			EndsWith(expectedEndMemory.Span, actualMemory.Span, comparisonType);
		}

		/// <summary>
		/// Verifies that two Memory values are equivalent.
		/// </summary>
		/// <param name="expectedMemory">The expected Memory value.</param>
		/// <param name="actualMemory">The actual Memory value.</param>
		/// <exception cref="EqualException">Thrown when the Memory values are not equivalent.</exception>
		public static void Equal(
			Memory<char> expectedMemory,
			Memory<char> actualMemory) =>
				Equal((ReadOnlyMemory<char>)expectedMemory, (ReadOnlyMemory<char>)actualMemory, false, false, false);

		/// <summary>
		/// Verifies that two Memory values are equivalent.
		/// </summary>
		/// <param name="expectedMemory">The expected Memory value.</param>
		/// <param name="actualMemory">The actual Memory value.</param>
		/// <exception cref="EqualException">Thrown when the Memory values are not equivalent.</exception>
		public static void Equal(
			Memory<char> expectedMemory,
			ReadOnlyMemory<char> actualMemory) =>
				Equal((ReadOnlyMemory<char>)expectedMemory, actualMemory, false, false, false);

		/// <summary>
		/// Verifies that two Memory values are equivalent.
		/// </summary>
		/// <param name="expectedMemory">The expected Memory value.</param>
		/// <param name="actualMemory">The actual Memory value.</param>
		/// <exception cref="EqualException">Thrown when the Memory values are not equivalent.</exception>
		public static void Equal(
			ReadOnlyMemory<char> expectedMemory,
			Memory<char> actualMemory) =>
				Equal(expectedMemory, (ReadOnlyMemory<char>)actualMemory, false, false, false);

		/// <summary>
		/// Verifies that two Memory values are equivalent.
		/// </summary>
		/// <param name="expectedMemory">The expected Memory value.</param>
		/// <param name="actualMemory">The actual Memory value.</param>
		/// <exception cref="EqualException">Thrown when the Memory values are not equivalent.</exception>
		public static void Equal(
			ReadOnlyMemory<char> expectedMemory,
			ReadOnlyMemory<char> actualMemory) =>
				Equal(expectedMemory, actualMemory, false, false, false);

		/// <summary>
		/// Verifies that two Memory values are equivalent.
		/// </summary>
		/// <param name="expectedMemory">The expected Memory value.</param>
		/// <param name="actualMemory">The actual Memory value.</param>
		/// <param name="ignoreCase">If set to <c>true</c>, ignores cases differences. The invariant culture is used.</param>
		/// <param name="ignoreLineEndingDifferences">If set to <c>true</c>, treats \r\n, \r, and \n as equivalent.</param>
		/// <param name="ignoreWhiteSpaceDifferences">If set to <c>true</c>, treats spaces and tabs (in any non-zero quantity) as equivalent.</param>
		/// <param name="ignoreAllWhiteSpace">If set to <c>true</c>, ignores all white space differences during comparison.</param>
		/// <exception cref="EqualException">Thrown when the Memory values are not equivalent.</exception>
		public static void Equal(
			Memory<char> expectedMemory,
			Memory<char> actualMemory,
			bool ignoreCase = false,
			bool ignoreLineEndingDifferences = false,
			bool ignoreWhiteSpaceDifferences = false,
			bool ignoreAllWhiteSpace = false) =>
				Equal((ReadOnlyMemory<char>)expectedMemory, (ReadOnlyMemory<char>)actualMemory, ignoreCase, ignoreLineEndingDifferences, ignoreWhiteSpaceDifferences, ignoreAllWhiteSpace);

		/// <summary>
		/// Verifies that two Memory values are equivalent.
		/// </summary>
		/// <param name="expectedMemory">The expected Memory value.</param>
		/// <param name="actualMemory">The actual Memory value.</param>
		/// <param name="ignoreCase">If set to <c>true</c>, ignores cases differences. The invariant culture is used.</param>
		/// <param name="ignoreLineEndingDifferences">If set to <c>true</c>, treats \r\n, \r, and \n as equivalent.</param>
		/// <param name="ignoreWhiteSpaceDifferences">If set to <c>true</c>, treats spaces and tabs (in any non-zero quantity) as equivalent.</param>
		/// <param name="ignoreAllWhiteSpace">If set to <c>true</c>, ignores all white space differences during comparison.</param>
		/// <exception cref="EqualException">Thrown when the Memory values are not equivalent.</exception>
		public static void Equal(
			Memory<char> expectedMemory,
			ReadOnlyMemory<char> actualMemory,
			bool ignoreCase = false,
			bool ignoreLineEndingDifferences = false,
			bool ignoreWhiteSpaceDifferences = false,
			bool ignoreAllWhiteSpace = false) =>
				Equal((ReadOnlyMemory<char>)expectedMemory, actualMemory, ignoreCase, ignoreLineEndingDifferences, ignoreWhiteSpaceDifferences, ignoreAllWhiteSpace);

		/// <summary>
		/// Verifies that two Memory values are equivalent.
		/// </summary>
		/// <param name="expectedMemory">The expected Memory value.</param>
		/// <param name="actualMemory">The actual Memory value.</param>
		/// <param name="ignoreCase">If set to <c>true</c>, ignores cases differences. The invariant culture is used.</param>
		/// <param name="ignoreLineEndingDifferences">If set to <c>true</c>, treats \r\n, \r, and \n as equivalent.</param>
		/// <param name="ignoreWhiteSpaceDifferences">If set to <c>true</c>, treats spaces and tabs (in any non-zero quantity) as equivalent.</param>
		/// <param name="ignoreAllWhiteSpace">If set to <c>true</c>, ignores all white space differences during comparison.</param>
		/// <exception cref="EqualException">Thrown when the Memory values are not equivalent.</exception>
		public static void Equal(
			ReadOnlyMemory<char> expectedMemory,
			Memory<char> actualMemory,
			bool ignoreCase = false,
			bool ignoreLineEndingDifferences = false,
			bool ignoreWhiteSpaceDifferences = false,
			bool ignoreAllWhiteSpace = false) =>
				Equal(expectedMemory, (ReadOnlyMemory<char>)actualMemory, ignoreCase, ignoreLineEndingDifferences, ignoreWhiteSpaceDifferences, ignoreAllWhiteSpace);

		/// <summary>
		/// Verifies that two Memory values are equivalent.
		/// </summary>
		/// <param name="expectedMemory">The expected Memory value.</param>
		/// <param name="actualMemory">The actual Memory value.</param>
		/// <param name="ignoreCase">If set to <c>true</c>, ignores cases differences. The invariant culture is used.</param>
		/// <param name="ignoreLineEndingDifferences">If set to <c>true</c>, treats \r\n, \r, and \n as equivalent.</param>
		/// <param name="ignoreWhiteSpaceDifferences">If set to <c>true</c>, treats spaces and tabs (in any non-zero quantity) as equivalent.</param>
		/// <param name="ignoreAllWhiteSpace">If set to <c>true</c>, ignores all white space differences during comparison.</param>
		/// <exception cref="EqualException">Thrown when the Memory values are not equivalent.</exception>
		public static void Equal(
			ReadOnlyMemory<char> expectedMemory,
			ReadOnlyMemory<char> actualMemory,
			bool ignoreCase = false,
			bool ignoreLineEndingDifferences = false,
			bool ignoreWhiteSpaceDifferences = false,
			bool ignoreAllWhiteSpace = false)
		{
			GuardArgumentNotNull(nameof(expectedMemory), expectedMemory);

			Equal(
				expectedMemory.Span,
				actualMemory.Span,
				ignoreCase,
				ignoreLineEndingDifferences,
				ignoreWhiteSpaceDifferences,
				ignoreAllWhiteSpace
			);
		}

		/// <summary>
		/// Verifies that two Memory values are equivalent.
		/// </summary>
		/// <param name="expectedMemory">The expected Memory value.</param>
		/// <param name="actualMemory">The actual Memory value.</param>
		/// <exception cref="EqualException">Thrown when the Memory values are not equivalent.</exception>
		public static void Equal<T>(
			Memory<T> expectedMemory,
			Memory<T> actualMemory)
				where T : IEquatable<T> =>
					Equal((ReadOnlyMemory<T>)expectedMemory, (ReadOnlyMemory<T>)actualMemory);

		/// <summary>
		/// Verifies that two Memory values are equivalent.
		/// </summary>
		/// <param name="expectedMemory">The expected Memory value.</param>
		/// <param name="actualMemory">The actual Memory value.</param>
		/// <exception cref="EqualException">Thrown when the Memory values are not equivalent.</exception>
		public static void Equal<T>(
			Memory<T> expectedMemory,
			ReadOnlyMemory<T> actualMemory)
				where T : IEquatable<T> =>
					Equal((ReadOnlyMemory<T>)expectedMemory, actualMemory);

		/// <summary>
		/// Verifies that two Memory values are equivalent.
		/// </summary>
		/// <param name="expectedMemory">The expected Memory value.</param>
		/// <param name="actualMemory">The actual Memory value.</param>
		/// <exception cref="EqualException">Thrown when the Memory values are not equivalent.</exception>
		public static void Equal<T>(
			ReadOnlyMemory<T> expectedMemory,
			Memory<T> actualMemory)
				where T : IEquatable<T> =>
					Equal(expectedMemory, (ReadOnlyMemory<T>)actualMemory);

		/// <summary>
		/// Verifies that two Memory values are equivalent.
		/// </summary>
		/// <param name="expectedMemory">The expected Memory value.</param>
		/// <param name="actualMemory">The actual Memory value.</param>
		/// <exception cref="EqualException">Thrown when the Memory values are not equivalent.</exception>
		public static void Equal<T>(
			ReadOnlyMemory<T> expectedMemory,
			ReadOnlyMemory<T> actualMemory)
				where T : IEquatable<T>
		{
			GuardArgumentNotNull(nameof(expectedMemory), expectedMemory);

			if (!expectedMemory.Span.SequenceEqual(actualMemory.Span))
				Equal<object>(expectedMemory.Span.ToArray(), actualMemory.Span.ToArray());
		}

		/// <summary>
		/// Verifies that a Memory starts with a given sub-Memory, using the default <see cref="StringComparison.CurrentCulture"/> comparison type.
		/// </summary>
		/// <param name="expectedStartMemory">The sub-Memory expected to be at the start of the Memory</param>
		/// <param name="actualMemory">The Memory to be inspected</param>
		/// <exception cref="StartsWithException">Thrown when the Memory does not start with the expected subMemory</exception>
		public static void StartsWith(
			Memory<char> expectedStartMemory,
			Memory<char> actualMemory) =>
				StartsWith((ReadOnlyMemory<char>)expectedStartMemory, (ReadOnlyMemory<char>)actualMemory, StringComparison.CurrentCulture);

		/// <summary>
		/// Verifies that a Memory starts with a given sub-Memory, using the default <see cref="StringComparison.CurrentCulture"/> comparison type.
		/// </summary>
		/// <param name="expectedStartMemory">The sub-Memory expected to be at the start of the Memory</param>
		/// <param name="actualMemory">The Memory to be inspected</param>
		/// <exception cref="StartsWithException">Thrown when the Memory does not start with the expected subMemory</exception>
		public static void StartsWith(
			Memory<char> expectedStartMemory,
			ReadOnlyMemory<char> actualMemory) =>
				StartsWith((ReadOnlyMemory<char>)expectedStartMemory, actualMemory, StringComparison.CurrentCulture);

		/// <summary>
		/// Verifies that a Memory starts with a given sub-Memory, using the default <see cref="StringComparison.CurrentCulture"/> comparison type.
		/// </summary>
		/// <param name="expectedStartMemory">The sub-Memory expected to be at the start of the Memory</param>
		/// <param name="actualMemory">The Memory to be inspected</param>
		/// <exception cref="StartsWithException">Thrown when the Memory does not start with the expected subMemory</exception>
		public static void StartsWith(
			ReadOnlyMemory<char> expectedStartMemory,
			Memory<char> actualMemory) =>
				StartsWith(expectedStartMemory, (ReadOnlyMemory<char>)actualMemory, StringComparison.CurrentCulture);

		/// <summary>
		/// Verifies that a Memory starts with a given sub-Memory, using the default StringComparison.CurrentCulture comparison type.
		/// </summary>
		/// <param name="expectedStartMemory">The sub-Memory expected to be at the start of the Memory</param>
		/// <param name="actualMemory">The Memory to be inspected</param>
		/// <exception cref="StartsWithException">Thrown when the Memory does not start with the expected subMemory</exception>
		public static void StartsWith(
			ReadOnlyMemory<char> expectedStartMemory,
			ReadOnlyMemory<char> actualMemory) =>
				StartsWith(expectedStartMemory, actualMemory, StringComparison.CurrentCulture);

		/// <summary>
		/// Verifies that a Memory starts with a given sub-Memory, using the given comparison type.
		/// </summary>
		/// <param name="expectedStartMemory">The sub-Memory expected to be at the start of the Memory</param>
		/// <param name="actualMemory">The Memory to be inspected</param>
		/// <param name="comparisonType">The type of string comparison to perform</param>
		/// <exception cref="StartsWithException">Thrown when the Memory does not start with the expected subMemory</exception>
		public static void StartsWith(
			Memory<char> expectedStartMemory,
			Memory<char> actualMemory,
			StringComparison comparisonType = StringComparison.CurrentCulture) =>
				StartsWith((ReadOnlyMemory<char>)expectedStartMemory, (ReadOnlyMemory<char>)actualMemory, comparisonType);

		/// <summary>
		/// Verifies that a Memory starts with a given sub-Memory, using the given comparison type.
		/// </summary>
		/// <param name="expectedStartMemory">The sub-Memory expected to be at the start of the Memory</param>
		/// <param name="actualMemory">The Memory to be inspected</param>
		/// <param name="comparisonType">The type of string comparison to perform</param>
		/// <exception cref="StartsWithException">Thrown when the Memory does not start with the expected subMemory</exception>
		public static void StartsWith(
			Memory<char> expectedStartMemory,
			ReadOnlyMemory<char> actualMemory,
			StringComparison comparisonType = StringComparison.CurrentCulture) =>
				StartsWith((ReadOnlyMemory<char>)expectedStartMemory, actualMemory, comparisonType);

		/// <summary>
		/// Verifies that a Memory starts with a given sub-Memory, using the given comparison type.
		/// </summary>
		/// <param name="expectedStartMemory">The sub-Memory expected to be at the start of the Memory</param>
		/// <param name="actualMemory">The Memory to be inspected</param>
		/// <param name="comparisonType">The type of string comparison to perform</param>
		/// <exception cref="StartsWithException">Thrown when the Memory does not start with the expected subMemory</exception>
		public static void StartsWith(
			ReadOnlyMemory<char> expectedStartMemory,
			Memory<char> actualMemory,
			StringComparison comparisonType = StringComparison.CurrentCulture) =>
				StartsWith(expectedStartMemory, (ReadOnlyMemory<char>)actualMemory, comparisonType);

		/// <summary>
		/// Verifies that a Memory starts with a given sub-Memory, using the given comparison type.
		/// </summary>
		/// <param name="expectedStartMemory">The sub-Memory expected to be at the start of the Memory</param>
		/// <param name="actualMemory">The Memory to be inspected</param>
		/// <param name="comparisonType">The type of string comparison to perform</param>
		/// <exception cref="StartsWithException">Thrown when the Memory does not start with the expected subMemory</exception>
		public static void StartsWith(
			ReadOnlyMemory<char> expectedStartMemory,
			ReadOnlyMemory<char> actualMemory,
			StringComparison comparisonType = StringComparison.CurrentCulture)
		{
			GuardArgumentNotNull(nameof(expectedStartMemory), expectedStartMemory);

			StartsWith(expectedStartMemory.Span, actualMemory.Span, comparisonType);
		}
	}
}

#endif
