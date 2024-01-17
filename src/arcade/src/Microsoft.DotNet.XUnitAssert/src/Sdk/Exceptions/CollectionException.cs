#if XUNIT_NULLABLE
#nullable enable
#endif

using System;
using System.Globalization;
using System.Linq;

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
			var lines =
				innerException
					.Message
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
