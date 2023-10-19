#if XUNIT_NULLABLE
#nullable enable
#else
// In case this is source-imported with global nullable enabled but no XUNIT_NULLABLE
#pragma warning disable CS8625
#endif

using System;
using Xunit.Internal;

namespace Xunit.Sdk
{
	/// <summary>
	/// Exception thrown when Assert.Equal fails.
	/// </summary>
#if XUNIT_VISIBILITY_INTERNAL
	internal
#else
	public
#endif
	partial class EqualException : XunitException
	{
		static readonly string newLineAndIndent = Environment.NewLine + new string(' ', 10);  // Length of "Expected: " and "Actual:   "

		EqualException(string message) :
			base(message)
		{ }

		/// <summary>
		/// Creates a new instance of <see cref="EqualException"/> to be thrown when two collections
		/// are not equal.
		/// </summary>
		/// <param name="mismatchedIndex">The index at which the collections differ</param>
		/// <param name="expected">The expected collection</param>
		/// <param name="expectedPointer">The spacing into the expected collection where the difference occurs</param>
		/// <param name="expectedType">The type of the expected collection items, when they differ in type</param>
		/// <param name="actual">The actual collection</param>
		/// <param name="actualPointer">The spacing into the actual collection where the difference occurs</param>
		/// <param name="actualType">The type of the actual collection items, when they differ in type</param>
		/// <param name="collectionDisplay">The display name for the collection type (defaults to "Collections")</param>
		public static EqualException ForMismatchedCollections(
			int? mismatchedIndex,
			string expected,
			int? expectedPointer,
#if XUNIT_NULLABLE
			string? expectedType,
#else
			string expectedType,
#endif
			string actual,
			int? actualPointer,
#if XUNIT_NULLABLE
			string? actualType,
			string? collectionDisplay = null)
#else
			string actualType,
			string collectionDisplay = null)
#endif
		{
			var message = $"Assert.Equal() Failure: {collectionDisplay ?? "Collections"} differ";
			var expectedTypeText = expectedType != null && actualType != null && expectedType != actualType ? $", type {expectedType}" : "";
			var actualTypeText = expectedType != null && actualType != null && expectedType != actualType ? $", type {actualType}" : "";

			if (expectedPointer.HasValue && mismatchedIndex.HasValue)
				message += $"{Environment.NewLine}          {new string(' ', expectedPointer.Value)}↓ (pos {mismatchedIndex}{expectedTypeText})";

			message += $"{Environment.NewLine}Expected: {expected}{Environment.NewLine}Actual:   {actual}";

			if (actualPointer.HasValue && mismatchedIndex.HasValue)
				message += $"{Environment.NewLine}          {new string(' ', actualPointer.Value)}↑ (pos {mismatchedIndex}{actualTypeText})";

			return new EqualException(message);
		}

		/// <summary>
		/// Creates a new instance of <see cref="EqualException"/> to be thrown when two string
		/// values are not equal.
		/// </summary>
		/// <param name="expected">The expected value</param>
		/// <param name="actual">The actual value</param>
		/// <param name="expectedIndex">The index point in the expected string where the values differ</param>
		/// <param name="actualIndex">The index point in the actual string where the values differ</param>
		public static EqualException ForMismatchedStrings(
#if XUNIT_NULLABLE
			string? expected,
			string? actual,
#else
			string expected,
			string actual,
#endif
			int expectedIndex,
			int actualIndex)
		{
			var message = "Assert.Equal() Failure: Strings differ";

			int expectedPointer;
			int actualPointer;

			var formattedExpected = AssertHelper.ShortenAndEncodeString(expected, expectedIndex, out expectedPointer);
			var formattedActual = AssertHelper.ShortenAndEncodeString(actual, actualIndex, out actualPointer);

			if (expected != null && expectedIndex > -1 && expectedIndex < expected.Length)
				message += newLineAndIndent + new string(' ', expectedPointer) + $"↓ (pos {expectedIndex})";

			message +=
				Environment.NewLine + "Expected: " + formattedExpected +
				Environment.NewLine + "Actual:   " + formattedActual;

			if (actual != null && expectedIndex > -1 && actualIndex < actual.Length)
				message += newLineAndIndent + new string(' ', actualPointer) + $"↑ (pos {actualIndex})";

			return new EqualException(message);
		}

		/// <summary>
		/// Creates a new instance of <see cref="EqualException"/> to be thrown when two values
		/// are not equal. This may be simple values (like intrinsics) or complex values (like
		/// classes or structs).
		/// </summary>
		/// <param name="expected">The expected value</param>
		/// <param name="actual">The actual value</param>
		/// <param name="banner">The banner to show; if <c>null</c>, then the standard
		/// banner of "Values differ" will be used</param>
		public static EqualException ForMismatchedValues(
#if XUNIT_NULLABLE
			object? expected,
			object? actual,
			string? banner = null)
#else
			object expected,
			object actual,
			string banner = null)
#endif
		{
			// Strings normally come through ForMismatchedStrings, so we want to make sure any
			// string value that comes through here isn't re-formatted/truncated. This is for
			// two reasons: (a) to support Assert.Equal<object>(string1, string2) to get a full
			// printout of the raw string values, which is useful when debugging; and (b) to
			// allow the assertion functions to pre-format the value themselves, perhaps with
			// additional information (like DateTime/DateTimeOffset when providing the precision
			// of the comparison).
			var expectedText = expected as string ?? ArgumentFormatter.Format(expected);
			var actualText = actual as string ?? ArgumentFormatter.Format(actual);

			return new EqualException(
				"Assert.Equal() Failure: " + (banner ?? "Values differ") + Environment.NewLine +
				"Expected: " + expectedText.Replace(Environment.NewLine, newLineAndIndent) + Environment.NewLine +
				"Actual:   " + actualText.Replace(Environment.NewLine, newLineAndIndent)
			);
		}
	}
}
