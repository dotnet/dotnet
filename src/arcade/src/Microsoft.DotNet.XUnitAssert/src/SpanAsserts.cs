#if XUNIT_SPAN

#if XUNIT_NULLABLE
#nullable enable
#endif

using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
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
		// While there is an implicit conversion operator from Span<T> to ReadOnlySpan<T>, the
		// compiler still stumbles to do this automatically, which means we end up with lots of overloads
		// with various arrangements of Span<T> and ReadOnlySpan<T>.

		// Also note that these classes will convert nulls into empty arrays automatically, since there
		// is no way to represent a null readonly struct.

		/// <summary>
		/// Verifies that a span contains a given sub-span
		/// </summary>
		/// <param name="expectedSubSpan">The sub-span expected to be in the span</param>
		/// <param name="actualSpan">The span to be inspected</param>
		/// <exception cref="ContainsException">Thrown when the sub-span is not present inside the span</exception>
		public static void Contains<[DynamicallyAccessedMembers(
					DynamicallyAccessedMemberTypes.PublicFields
					| DynamicallyAccessedMemberTypes.NonPublicFields
					| DynamicallyAccessedMemberTypes.PublicProperties
					| DynamicallyAccessedMemberTypes.NonPublicProperties
					| DynamicallyAccessedMemberTypes.PublicMethods)] T>(
			Span<T> expectedSubSpan,
			Span<T> actualSpan)
				where T : IEquatable<T> =>
					Contains((ReadOnlySpan<T>)expectedSubSpan, (ReadOnlySpan<T>)actualSpan);

		/// <summary>
		/// Verifies that a span contains a given sub-span
		/// </summary>
		/// <param name="expectedSubSpan">The sub-span expected to be in the span</param>
		/// <param name="actualSpan">The span to be inspected</param>
		/// <exception cref="ContainsException">Thrown when the sub-span is not present inside the span</exception>
		public static void Contains<[DynamicallyAccessedMembers(
					DynamicallyAccessedMemberTypes.PublicFields
					| DynamicallyAccessedMemberTypes.NonPublicFields
					| DynamicallyAccessedMemberTypes.PublicProperties
					| DynamicallyAccessedMemberTypes.NonPublicProperties
					| DynamicallyAccessedMemberTypes.PublicMethods)] T>(
			Span<T> expectedSubSpan,
			ReadOnlySpan<T> actualSpan)
				where T : IEquatable<T> =>
					Contains((ReadOnlySpan<T>)expectedSubSpan, actualSpan);

		/// <summary>
		/// Verifies that a span contains a given sub-span
		/// </summary>
		/// <param name="expectedSubSpan">The sub-span expected to be in the span</param>
		/// <param name="actualSpan">The span to be inspected</param>
		/// <exception cref="ContainsException">Thrown when the sub-span is not present inside the span</exception>
		public static void Contains<[DynamicallyAccessedMembers(
					DynamicallyAccessedMemberTypes.PublicFields
					| DynamicallyAccessedMemberTypes.NonPublicFields
					| DynamicallyAccessedMemberTypes.PublicProperties
					| DynamicallyAccessedMemberTypes.NonPublicProperties
					| DynamicallyAccessedMemberTypes.PublicMethods)] T>(
			ReadOnlySpan<T> expectedSubSpan,
			Span<T> actualSpan)
				where T : IEquatable<T> =>
					Contains(expectedSubSpan, (ReadOnlySpan<T>)actualSpan);

		/// <summary>
		/// Verifies that a span contains a given sub-span
		/// </summary>
		/// <param name="expectedSubSpan">The sub-span expected to be in the span</param>
		/// <param name="actualSpan">The span to be inspected</param>
		/// <exception cref="ContainsException">Thrown when the sub-span is not present inside the span</exception>
		public static void Contains<[DynamicallyAccessedMembers(
					DynamicallyAccessedMemberTypes.PublicFields
					| DynamicallyAccessedMemberTypes.NonPublicFields
					| DynamicallyAccessedMemberTypes.PublicProperties
					| DynamicallyAccessedMemberTypes.NonPublicProperties
					| DynamicallyAccessedMemberTypes.PublicMethods)] T>(
			ReadOnlySpan<T> expectedSubSpan,
			ReadOnlySpan<T> actualSpan)
				where T : IEquatable<T>
		{
			if (actualSpan.IndexOf(expectedSubSpan) < 0)
				throw ContainsException.ForSubSpanNotFound(
					CollectionTracker<T>.FormatStart(expectedSubSpan),
					CollectionTracker<T>.FormatStart(actualSpan)
				);
		}

		/// <summary>
		/// Verifies that a span does not contain a given sub-span
		/// </summary>
		/// <param name="expectedSubSpan">The sub-span expected not to be in the span</param>
		/// <param name="actualSpan">The span to be inspected</param>
		/// <exception cref="DoesNotContainException">Thrown when the sub-span is present inside the span</exception>
		public static void DoesNotContain<[DynamicallyAccessedMembers(
					DynamicallyAccessedMemberTypes.PublicFields
					| DynamicallyAccessedMemberTypes.NonPublicFields
					| DynamicallyAccessedMemberTypes.PublicProperties
					| DynamicallyAccessedMemberTypes.NonPublicProperties
					| DynamicallyAccessedMemberTypes.PublicMethods)] T>(
			Span<T> expectedSubSpan,
			Span<T> actualSpan)
				where T : IEquatable<T> =>
					DoesNotContain((ReadOnlySpan<T>)expectedSubSpan, (ReadOnlySpan<T>)actualSpan);

		/// <summary>
		/// Verifies that a span does not contain a given sub-span
		/// </summary>
		/// <param name="expectedSubSpan">The sub-span expected not to be in the span</param>
		/// <param name="actualSpan">The span to be inspected</param>
		/// <exception cref="DoesNotContainException">Thrown when the sub-span is present inside the span</exception>
		public static void DoesNotContain<[DynamicallyAccessedMembers(
					DynamicallyAccessedMemberTypes.PublicFields
					| DynamicallyAccessedMemberTypes.NonPublicFields
					| DynamicallyAccessedMemberTypes.PublicProperties
					| DynamicallyAccessedMemberTypes.NonPublicProperties
					| DynamicallyAccessedMemberTypes.PublicMethods)] T>(
			Span<T> expectedSubSpan,
			ReadOnlySpan<T> actualSpan)
				where T : IEquatable<T> =>
					DoesNotContain((ReadOnlySpan<T>)expectedSubSpan, actualSpan);

		/// <summary>
		/// Verifies that a span does not contain a given sub-span
		/// </summary>
		/// <param name="expectedSubSpan">The sub-span expected not to be in the span</param>
		/// <param name="actualSpan">The span to be inspected</param>
		/// <exception cref="DoesNotContainException">Thrown when the sub-span is present inside the span</exception>
		public static void DoesNotContain<[DynamicallyAccessedMembers(
					DynamicallyAccessedMemberTypes.PublicFields
					| DynamicallyAccessedMemberTypes.NonPublicFields
					| DynamicallyAccessedMemberTypes.PublicProperties
					| DynamicallyAccessedMemberTypes.NonPublicProperties
					| DynamicallyAccessedMemberTypes.PublicMethods)] T>(
			ReadOnlySpan<T> expectedSubSpan,
			Span<T> actualSpan)
				where T : IEquatable<T> =>
					DoesNotContain(expectedSubSpan, (ReadOnlySpan<T>)actualSpan);

		/// <summary>
		/// Verifies that a span does not contain a given sub-span
		/// </summary>
		/// <param name="expectedSubSpan">The sub-span expected not to be in the span</param>
		/// <param name="actualSpan">The span to be inspected</param>
		/// <exception cref="DoesNotContainException">Thrown when the sub-span is present inside the span</exception>
		public static void DoesNotContain<[DynamicallyAccessedMembers(
					DynamicallyAccessedMemberTypes.PublicFields
					| DynamicallyAccessedMemberTypes.NonPublicFields
					| DynamicallyAccessedMemberTypes.PublicProperties
					| DynamicallyAccessedMemberTypes.NonPublicProperties
					| DynamicallyAccessedMemberTypes.PublicMethods)] T>(
			ReadOnlySpan<T> expectedSubSpan,
			ReadOnlySpan<T> actualSpan)
				where T : IEquatable<T>
		{
			var idx = actualSpan.IndexOf(expectedSubSpan);
			if (idx > -1)
			{
				int? failurePointerIndent;
				var formattedExpected = CollectionTracker<T>.FormatStart(expectedSubSpan);
				var formattedActual = CollectionTracker<T>.FormatIndexedMismatch(actualSpan, idx, out failurePointerIndent);

				throw DoesNotContainException.ForSubSpanFound(
					formattedExpected,
					idx,
					failurePointerIndent,
					formattedActual
				);
			}
		}

		/// <summary>
		/// Verifies that two spans are equivalent.
		/// </summary>
		/// <param name="expectedSpan">The expected span value.</param>
		/// <param name="actualSpan">The actual span value.</param>
		/// <exception cref="EqualException">Thrown when the spans are not equivalent.</exception>
		public static void Equal<T>(
			Span<T> expectedSpan,
			Span<T> actualSpan)
				where T : IEquatable<T> =>
					Equal((ReadOnlySpan<T>)expectedSpan, (ReadOnlySpan<T>)actualSpan);

		/// <summary>
		/// Verifies that two spans are equivalent.
		/// </summary>
		/// <param name="expectedSpan">The expected span value.</param>
		/// <param name="actualSpan">The actual span value.</param>
		/// <exception cref="EqualException">Thrown when the spans are not equivalent.</exception>
		public static void Equal<T>(
			Span<T> expectedSpan,
			ReadOnlySpan<T> actualSpan)
				where T : IEquatable<T> =>
					Equal((ReadOnlySpan<T>)expectedSpan, actualSpan);

		/// <summary>
		/// Verifies that two spans are equivalent.
		/// </summary>
		/// <param name="expectedSpan">The expected span value.</param>
		/// <param name="actualSpan">The actual span value.</param>
		/// <exception cref="EqualException">Thrown when the spans are not equivalent.</exception>
		public static void Equal<T>(
			ReadOnlySpan<T> expectedSpan,
			Span<T> actualSpan)
				where T : IEquatable<T> =>
					Equal(expectedSpan, (ReadOnlySpan<T>)actualSpan);

		/// <summary>
		/// Verifies that two spans are equivalent.
		/// </summary>
		/// <param name="expectedSpan">The expected span value.</param>
		/// <param name="actualSpan">The actual span value.</param>
		/// <exception cref="EqualException">Thrown when the spans are not equivalent.</exception>
		public static void Equal<T>(
			ReadOnlySpan<T> expectedSpan,
			ReadOnlySpan<T> actualSpan)
				where T : IEquatable<T>
		{
			if (!expectedSpan.SequenceEqual(actualSpan))
				Equal<object>(expectedSpan.ToArray(), actualSpan.ToArray());
		}
	}
}

#endif
