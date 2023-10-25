#if XUNIT_NULLABLE
#nullable enable
#endif

using System;
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
		CollectionException(string message) :
			base(message)
		{ }

		static string FormatInnerException(Exception innerException)
		{
			var lines =
				innerException
					.Message
					.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
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
				message += $"{Environment.NewLine}            {new string(' ', failurePointerIndent.Value)}↓ (pos {indexFailurePoint})";

			message += $"{Environment.NewLine}Collection: {formattedCollection}{Environment.NewLine}Error:      {FormatInnerException(exception)}";

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
					"Assert.Collection() Failure: Mismatched item count" + Environment.NewLine +
					"Collection:     " + Assert.GuardArgumentNotNull(nameof(formattedCollection), formattedCollection) + Environment.NewLine +
					"Expected count: " + expectedCount + Environment.NewLine +
					"Actual count:   " + actualCount
				);
	}
}
