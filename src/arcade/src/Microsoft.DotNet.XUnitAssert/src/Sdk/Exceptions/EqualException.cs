#if XUNIT_NULLABLE
#nullable enable
#else
// In case this is source-imported with global nullable enabled but no XUNIT_NULLABLE
#pragma warning disable CS8604
#pragma warning disable CS8625
#endif

using System;
using System.Globalization;
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

		EqualException(
			string message,
#if XUNIT_NULLABLE
			Exception? innerException = null) :
#else
			Exception innerException = null) :
#endif
				base(message, innerException)
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
			string? collectionDisplay = null) =>
#else
			string actualType,
			string collectionDisplay = null) =>
#endif
				ForMismatchedCollectionsWithError(mismatchedIndex, expected, expectedPointer, expectedType, actual, actualPointer, actualType, null, collectionDisplay);

		/// <summary>
		/// Creates a new instance of <see cref="EqualException"/> to be thrown when two collections
		/// are not equal, and an error has occurred during comparison.
		/// </summary>
		/// <param name="mismatchedIndex">The index at which the collections differ</param>
		/// <param name="expected">The expected collection</param>
		/// <param name="expectedPointer">The spacing into the expected collection where the difference occurs</param>
		/// <param name="expectedType">The type of the expected collection items, when they differ in type</param>
		/// <param name="actual">The actual collection</param>
		/// <param name="actualPointer">The spacing into the actual collection where the difference occurs</param>
		/// <param name="actualType">The type of the actual collection items, when they differ in type</param>
		/// <param name="error">The optional exception that was thrown during comparison</param>
		/// <param name="collectionDisplay">The display name for the collection type (defaults to "Collections")</param>
		public static EqualException ForMismatchedCollectionsWithError(
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
			Exception? error,
			string? collectionDisplay = null)
#else
			string actualType,
			Exception error,
			string collectionDisplay = null)
#endif
		{
			Assert.GuardArgumentNotNull(nameof(actual), actual);

			error = ArgumentFormatter.UnwrapException(error);
			if (error is AssertEqualityComparer.OperationalFailureException)
				return new EqualException("Assert.Equal() Failure: " + error.Message);

			var message =
				error == null
					? string.Format(CultureInfo.CurrentCulture, "Assert.Equal() Failure: {0} differ", collectionDisplay ?? "Collections")
					: "Assert.Equal() Failure: Exception thrown during comparison";

			var expectedTypeText = expectedType != null && actualType != null && expectedType != actualType ? string.Format(CultureInfo.CurrentCulture, ", type {0}", expectedType) : "";
			var actualTypeText = expectedType != null && actualType != null && expectedType != actualType ? string.Format(CultureInfo.CurrentCulture, ", type {0}", actualType) : "";

			if (expectedPointer.HasValue && mismatchedIndex.HasValue)
				message += string.Format(CultureInfo.CurrentCulture, "{0}          {1}\u2193 (pos {2}{3})", Environment.NewLine, new string(' ', expectedPointer.Value), mismatchedIndex, expectedTypeText);

			message += string.Format(CultureInfo.CurrentCulture, "{0}Expected: {1}{2}Actual:   {3}", Environment.NewLine, expected, Environment.NewLine, actual);

			if (actualPointer.HasValue && mismatchedIndex.HasValue)
				message += string.Format(CultureInfo.CurrentCulture, "{0}          {1}\u2191 (pos {2}{3})", Environment.NewLine, new string(' ', actualPointer.Value), mismatchedIndex, actualTypeText);

			return new EqualException(message, error);
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
				message += string.Format(CultureInfo.CurrentCulture, "{0}{1}\u2193 (pos {2})", newLineAndIndent, new string(' ', expectedPointer), expectedIndex);

			message += string.Format(CultureInfo.CurrentCulture, "{0}Expected: {1}{2}Actual:   {3}", Environment.NewLine, formattedExpected, Environment.NewLine, formattedActual);

			if (actual != null && expectedIndex > -1 && actualIndex < actual.Length)
				message += string.Format(CultureInfo.CurrentCulture, "{0}{1}\u2191 (pos {2})", newLineAndIndent, new string(' ', actualPointer), actualIndex);

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
			string? banner = null) =>
#else
			object expected,
			object actual,
			string banner = null) =>
#endif
				ForMismatchedValuesWithError(expected, actual, null, banner);

		/// <summary>
		/// Creates a new instance of <see cref="EqualException"/> to be thrown when two values
		/// are not equal. This may be simple values (like intrinsics) or complex values (like
		/// classes or structs). Used when an error has occurred during comparison.
		/// </summary>
		/// <param name="expected">The expected value</param>
		/// <param name="actual">The actual value</param>
		/// <param name="error">The optional exception that was thrown during comparison</param>
		/// <param name="banner">The banner to show; if <c>null</c>, then the standard
		/// banner of "Values differ" will be used. If <paramref name="error"/> is not <c>null</c>,
		/// then the banner used will always be "Exception thrown during comparison", regardless
		/// of the value passed here.</param>
		public static EqualException ForMismatchedValuesWithError(
#if XUNIT_NULLABLE
			object? expected,
			object? actual,
			Exception? error = null,
			string? banner = null)
#else
			object expected,
			object actual,
			Exception error = null,
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

			var message =
				error == null
					? string.Format(CultureInfo.CurrentCulture, "Assert.Equal() Failure: {0}", banner ?? "Values differ")
					: "Assert.Equal() Failure: Exception thrown during comparison";

			return new EqualException(
				string.Format(
					CultureInfo.CurrentCulture,
					"{0}{1}Expected: {2}{3}Actual:   {4}",
					message,
					Environment.NewLine,
#if NETCOREAPP2_0_OR_GREATER
					expectedText.Replace(Environment.NewLine, newLineAndIndent, StringComparison.Ordinal),
#else
					expectedText.Replace(Environment.NewLine, newLineAndIndent),
#endif
					Environment.NewLine,
#if NETCOREAPP2_0_OR_GREATER
					actualText.Replace(Environment.NewLine, newLineAndIndent, StringComparison.Ordinal)
#else
					actualText.Replace(Environment.NewLine, newLineAndIndent)
#endif
				),
				error
			);
		}
	}
}
