#pragma warning disable CA1032 // Implement standard exception constructors
#pragma warning disable IDE0040 // Add accessibility modifiers
#pragma warning disable IDE0058 // Expression value is never used
#pragma warning disable IDE0090 // Use 'new(...)'
#pragma warning disable IDE0161 // Convert to file-scoped namespace
#pragma warning disable IDE0300 // Simplify collection initialization

#if XUNIT_NULLABLE
#nullable enable
#else
// In case this is source-imported with global nullable enabled but no XUNIT_NULLABLE
#pragma warning disable CS8604
#endif

using System;
using System.Globalization;
using System.Linq;

// An "ExceptionUtility" class exists in both xunit.assert (Xunit.Internal.ExceptionUtility) as well as xunit.execution.dotnet (Xunit.Sdk.ExceptionUtility)
// This causes an compile-time error in projects that reference both the xunit.assert.source and xunit.core packages.
// To resolve this issue, add an alias for the Xunit.Internal.ExceptionUtility to make sure, the xunit.assert core uses the right ExceptionUtility
using ExceptionUtilityInternal = Xunit.Internal.ExceptionUtility;

namespace Xunit.Sdk
{
	/// <summary>
	/// Exception thrown when Assert.Collection fails.
	/// </summary>
#if XUNIT_VISIBILITY_INTERNAL
	internal
#else
	public
#endif
	partial class CollectionException : XunitException
	{
		static readonly char[] crlfSeparators = new[] { '\r', '\n' };

		CollectionException(string message) :
			base(message)
		{ }

		static string FormatInnerException(Exception innerException)
		{
			var text = innerException.Message;
			var filteredStack = ExceptionUtilityInternal.TransformStackTrace(ExceptionUtilityInternal.FilterStackTrace(innerException.StackTrace), "  ");
			if (!string.IsNullOrWhiteSpace(filteredStack))
			{
				if (text.Length != 0)
					text += Environment.NewLine;

				text += "Stack Trace:" + Environment.NewLine + filteredStack;
			}

			var lines =
				text
					.Split(crlfSeparators, StringSplitOptions.RemoveEmptyEntries)
					.Select((value, idx) => idx > 0 ? "            " + value : value);

			return string.Join(Environment.NewLine, lines);
		}

		/// <summary>
		/// Creates an instance of the <see cref="CollectionException"/> class to be thrown
		/// when an item comparison failed
		/// </summary>
		/// <param name="exception">The exception that was thrown</param>
		/// <param name="indexFailurePoint">The item index for the failed item</param>
		/// <param name="failurePointerIndent">The number of spaces needed to indent the failure pointer</param>
		/// <param name="formattedCollection">The formatted collection</param>
		public static CollectionException ForMismatchedItem(
			Exception exception,
			int indexFailurePoint,
			int? failurePointerIndent,
			string formattedCollection)
		{
			Assert.GuardArgumentNotNull(nameof(exception), exception);

			var message = "Assert.Collection() Failure: Item comparison failure";

			if (failurePointerIndent.HasValue)
				message += string.Format(CultureInfo.CurrentCulture, "{0}            {1}\u2193 (pos {2})", Environment.NewLine, new string(' ', failurePointerIndent.Value), indexFailurePoint);

			message += string.Format(CultureInfo.CurrentCulture, "{0}Collection: {1}{2}Error:      {3}", Environment.NewLine, formattedCollection, Environment.NewLine, FormatInnerException(exception));

			return new CollectionException(message);
		}

		/// <summary>
		/// Creates an instance of the <see cref="CollectionException"/> class to be thrown
		/// when the item count in a collection does not match the expected count.
		/// </summary>
		/// <param name="expectedCount">The expected item count</param>
		/// <param name="actualCount">The actual item count</param>
		/// <param name="formattedCollection">The formatted collection</param>
		public static CollectionException ForMismatchedItemCount(
			int expectedCount,
			int actualCount,
			string formattedCollection) =>
				new CollectionException(
					string.Format(
						CultureInfo.CurrentCulture,
						"Assert.Collection() Failure: Mismatched item count{0}Collection:     {1}{2}Expected count: {3}{4}Actual count:   {5}",
						Environment.NewLine,
						Assert.GuardArgumentNotNull(nameof(formattedCollection), formattedCollection),
						Environment.NewLine,
						expectedCount,
						Environment.NewLine,
						actualCount
					)
				);
	}
}
