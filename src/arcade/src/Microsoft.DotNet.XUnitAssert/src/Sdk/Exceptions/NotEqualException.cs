#if XUNIT_NULLABLE
#nullable enable
#else
// In case this is source-imported with global nullable enabled but no XUNIT_NULLABLE
#pragma warning disable CS8604
#pragma warning disable CS8625
#endif

using System;
using System.Globalization;

namespace Xunit.Sdk
{
	/// <summary>
	/// Exception thrown when Assert.NotEqual fails.
	/// </summary>
#if XUNIT_VISIBILITY_INTERNAL
	internal
#else
	public
#endif
	partial class NotEqualException : XunitException
	{
		NotEqualException(
			string message,
#if XUNIT_NULLABLE
			Exception? innerException = null) :
#else
			Exception innerException = null) :
#endif
				base(message, innerException)
		{ }

		/// <summary>
		/// Creates a new instance of <see cref="NotEqualException"/> to be thrown when two collections
		/// are equal.
		/// </summary>
		/// <param name="expected">The expected collection</param>
		/// <param name="actual">The actual collection</param>
		/// <param name="collectionDisplay">The display name for the collection type (defaults to "Collections")</param>
		public static NotEqualException ForEqualCollections(
			string expected,
			string actual,
#if XUNIT_NULLABLE
			string? collectionDisplay = null) =>
#else
			string collectionDisplay = null) =>
#endif
				ForEqualCollectionsWithError(null, expected, null, actual, null, null, collectionDisplay);

		/// <summary>
		/// Creates a new instance of <see cref="NotEqualException"/> to be thrown when two collections
		/// are equal, and an error has occurred during comparison.
		/// </summary>
		/// <param name="mismatchedIndex">The index at which the collections error occurred (should be <c>null</c>
		/// when <paramref name="error"/> is <c>null</c>)</param>
		/// <param name="expected">The expected collection</param>
		/// <param name="expectedPointer">The spacing into the expected collection where the difference occurs
		/// (should be <c>null</c> when <paramref name="error"/> is null)</param>
		/// <param name="actual">The actual collection</param>
		/// <param name="actualPointer">The spacing into the actual collection where the difference occurs
		/// (should be <c>null</c> when <paramref name="error"/> is null)</param>
		/// <param name="error">The optional exception that was thrown during comparison</param>
		/// <param name="collectionDisplay">The display name for the collection type (defaults to "Collections")</param>
		public static NotEqualException ForEqualCollectionsWithError(
			int? mismatchedIndex,
			string expected,
			int? expectedPointer,
			string actual,
			int? actualPointer,
#if XUNIT_NULLABLE
			Exception? error = null,
			string? collectionDisplay = null)
#else
			Exception error = null,
			string collectionDisplay = null)
#endif
		{
			Assert.GuardArgumentNotNull(nameof(expected), expected);
			Assert.GuardArgumentNotNull(nameof(actual), actual);

			error = ArgumentFormatter.UnwrapException(error);
			if (error is AssertEqualityComparer.OperationalFailureException)
				return new NotEqualException("Assert.NotEqual() Failure: " + error.Message);

			var message =
				error == null
					? string.Format(CultureInfo.CurrentCulture, "Assert.NotEqual() Failure: {0} are equal", collectionDisplay ?? "Collections")
					: "Assert.NotEqual() Failure: Exception thrown during comparison";

			if (expectedPointer.HasValue && mismatchedIndex.HasValue)
				message += string.Format(CultureInfo.CurrentCulture, "{0}              {1}\u2193 (pos {2})", Environment.NewLine, new string(' ', expectedPointer.Value), mismatchedIndex);

			message += string.Format(CultureInfo.CurrentCulture, "{0}Expected: Not {1}{2}Actual:       {3}", Environment.NewLine, expected, Environment.NewLine, actual);

			if (actualPointer.HasValue && mismatchedIndex.HasValue)
				message += string.Format(CultureInfo.CurrentCulture, "{0}              {1}\u2191 (pos {2})", Environment.NewLine, new string(' ', actualPointer.Value), mismatchedIndex);

			return new NotEqualException(message, error);
		}

		/// <summary>
		/// Creates a new instance of <see cref="NotEqualException"/> to be thrown when two values
		/// are equal. This may be simple values (like intrinsics) or complex values (like
		/// classes or structs).
		/// </summary>
		/// <param name="expected">The expected value</param>
		/// <param name="actual">The actual value</param>
		/// <param name="banner">The banner to show; if <c>null</c>, then the standard
		/// banner of "Values are equal" will be used</param>
		public static NotEqualException ForEqualValues(
			string expected,
			string actual,
#if XUNIT_NULLABLE
			string? banner = null) =>
#else
			string banner = null) =>
#endif
				ForEqualValuesWithError(expected, actual, null, banner);

		/// <summary>
		/// Creates a new instance of <see cref="NotEqualException"/> to be thrown when two values
		/// are equal. This may be simple values (like intrinsics) or complex values (like
		/// classes or structs). Used when an error has occurred during comparison.
		/// </summary>
		/// <param name="expected">The expected value</param>
		/// <param name="actual">The actual value</param>
		/// <param name="error">The optional exception that was thrown during comparison</param>
		/// <param name="banner">The banner to show; if <c>null</c>, then the standard
		/// banner of "Values are equal" will be used. If <paramref name="error"/> is not <c>null</c>,
		/// then the banner used will always be "Exception thrown during comparison", regardless
		/// of the value passed here.</param>
		public static NotEqualException ForEqualValuesWithError(
			string expected,
			string actual,
#if XUNIT_NULLABLE
			Exception? error = null,
			string? banner = null)
#else
			Exception error = null,
			string banner = null)
#endif
		{
			var message =
				error == null
					? string.Format(CultureInfo.CurrentCulture, "Assert.NotEqual() Failure: {0}", banner ?? "Values are equal")
					: "Assert.NotEqual() Failure: Exception thrown during comparison";

			return new NotEqualException(
				string.Format(
					CultureInfo.CurrentCulture,
					"{0}{1}Expected: Not {2}{3}Actual:       {4}",
					message,
					Environment.NewLine,
					Assert.GuardArgumentNotNull(nameof(expected), expected),
					Environment.NewLine,
					Assert.GuardArgumentNotNull(nameof(actual), actual)
				),
				error
			);
		}
	}
}
