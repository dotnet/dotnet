#pragma warning disable CA1052 // Static holder types should be static
#pragma warning disable IDE0018 // Inline variable declaration
#pragma warning disable IDE0058 // Expression value is never used
#pragma warning disable IDE0161 // Convert to file-scoped namespace

#if XUNIT_SPAN

#if XUNIT_NULLABLE
#nullable enable
#endif

using System;
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
		/// Verifies that a Memory contains a given sub-Memory
		/// </summary>
		/// <param name="expectedSubMemory">The sub-Memory expected to be in the Memory</param>
		/// <param name="actualMemory">The Memory to be inspected</param>
		/// <exception cref="ContainsException">Thrown when the sub-Memory is not present inside the Memory</exception>
		public static void Contains<T>(
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
		public static void Contains<T>(
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
		public static void Contains<T>(
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
		public static void Contains<T>(
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
		/// Verifies that a Memory does not contain a given sub-Memory
		/// </summary>
		/// <param name="expectedSubMemory">The sub-Memory expected not to be in the Memory</param>
		/// <param name="actualMemory">The Memory to be inspected</param>
		/// <exception cref="DoesNotContainException">Thrown when the sub-Memory is present inside the Memory</exception>
		public static void DoesNotContain<T>(
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
		public static void DoesNotContain<T>(
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
		public static void DoesNotContain<T>(
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
		public static void DoesNotContain<T>(
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
	}
}

#endif
