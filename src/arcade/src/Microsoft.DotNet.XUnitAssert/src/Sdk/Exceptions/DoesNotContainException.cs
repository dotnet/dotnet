#pragma warning disable CA1032 // Implement standard exception constructors
#pragma warning disable CA1720 // Identifier contains type name
#pragma warning disable IDE0090 // Use 'new(...)'

#if XUNIT_NULLABLE
#nullable enable
#endif

using System;
using System.Globalization;
using Xunit.Internal;

namespace Xunit.Sdk
{
	/// <summary>
	/// Exception thrown when Assert.DoesNotContain fails.
	/// </summary>
#if XUNIT_VISIBILITY_INTERNAL
	internal
#else
	public
#endif
	partial class DoesNotContainException : XunitException
	{
		DoesNotContainException(string message) :
			base(message)
		{ }

		/// <summary>
		/// Creates a new instance of the <see cref="DoesNotContainException"/> class to be thrown
		/// when the requested filter matches an item in the collection.
		/// </summary>
		/// <param name="indexFailurePoint">The item index for where the item was found</param>
		/// <param name="failurePointerIndent">The number of spaces needed to indent the failure pointer</param>
		/// <param name="collection">The collection</param>
		public static DoesNotContainException ForCollectionFilterMatched(
			int indexFailurePoint,
			int? failurePointerIndent,
			string collection)
		{
			Assert.GuardArgumentNotNull(nameof(collection), collection);

			var message = "Assert.DoesNotContain() Failure: Filter matched in collection";

			if (failurePointerIndent.HasValue)
				message += string.Format(CultureInfo.CurrentCulture, "{0}            {1}\u2193 (pos {2})", Environment.NewLine, new string(' ', failurePointerIndent.Value), indexFailurePoint);

			message += string.Format(CultureInfo.CurrentCulture, "{0}Collection: {1}", Environment.NewLine, collection);

			return new DoesNotContainException(message);
		}

		/// <summary>
		/// Creates a new instance of the <see cref="DoesNotContainException"/> class to be thrown
		/// when the requested item was found in the collection.
		/// </summary>
		/// <param name="item">The item that was found in the collection</param>
		/// <param name="indexFailurePoint">The item index for where the item was found</param>
		/// <param name="failurePointerIndent">The number of spaces needed to indent the failure pointer</param>
		/// <param name="collection">The collection</param>
		public static DoesNotContainException ForCollectionItemFound(
			string item,
			int indexFailurePoint,
			int? failurePointerIndent,
			string collection)
		{
			Assert.GuardArgumentNotNull(nameof(item), item);
			Assert.GuardArgumentNotNull(nameof(collection), collection);

			var message = "Assert.DoesNotContain() Failure: Item found in collection";

			if (failurePointerIndent.HasValue)
				message += string.Format(CultureInfo.CurrentCulture, "{0}            {1}\u2193 (pos {2})", Environment.NewLine, new string(' ', failurePointerIndent.Value), indexFailurePoint);

			message += string.Format(CultureInfo.CurrentCulture, "{0}Collection: {1}{2}Found:      {3}", Environment.NewLine, collection, Environment.NewLine, item);

			return new DoesNotContainException(message);
		}

		/// <summary>
		/// Creates a new instance of the <see cref="DoesNotContainException"/> class to be thrown
		/// when the requested key was found in the dictionary.
		/// </summary>
		/// <param name="expectedKey">The expected key value</param>
		/// <param name="keys">The dictionary keys</param>
		public static DoesNotContainException ForKeyFound(
			string expectedKey,
			string keys) =>
				new DoesNotContainException(
					string.Format(
						CultureInfo.CurrentCulture,
						"Assert.DoesNotContain() Failure: Key found in dictionary{0}Keys:  {1}{2}Found: {3}",
						Environment.NewLine,
						Assert.GuardArgumentNotNull(nameof(keys), keys),
						Environment.NewLine,
						Assert.GuardArgumentNotNull(nameof(expectedKey), expectedKey)
					)
				);

		/// <summary>
		/// Creates a new instance of the <see cref="DoesNotContainException"/> class to be thrown
		/// when the requested item was found in the set.
		/// </summary>
		/// <param name="item">The item that was found in the collection</param>
		/// <param name="set">The set</param>
		public static DoesNotContainException ForSetItemFound(
			string item,
			string set) =>
				new DoesNotContainException(
					string.Format(
						CultureInfo.CurrentCulture,
						"Assert.DoesNotContain() Failure: Item found in set{0}Set:   {1}{2}Found: {3}",
						Environment.NewLine,
						Assert.GuardArgumentNotNull(nameof(set), set),
						Environment.NewLine,
						Assert.GuardArgumentNotNull(nameof(item), item)
					)
				);

		/// <summary>
		/// Creates a new instance of the <see cref="DoesNotContainException"/> class to be thrown
		/// when the requested sub-memory was found in the memory.
		/// </summary>
		/// <param name="expectedSubMemory">The expected sub-memory</param>
		/// <param name="indexFailurePoint">The item index for where the item was found</param>
		/// <param name="failurePointerIndent">The number of spaces needed to indent the failure pointer</param>
		/// <param name="memory">The memory</param>
		public static DoesNotContainException ForSubMemoryFound(
			string expectedSubMemory,
			int indexFailurePoint,
			int? failurePointerIndent,
			string memory)
		{
			Assert.GuardArgumentNotNull(nameof(expectedSubMemory), expectedSubMemory);
			Assert.GuardArgumentNotNull(nameof(memory), memory);

			var message = "Assert.DoesNotContain() Failure: Sub-memory found";

			if (failurePointerIndent.HasValue)
				message += string.Format(CultureInfo.CurrentCulture, "{0}        {1}\u2193 (pos {2})", Environment.NewLine, new string(' ', failurePointerIndent.Value), indexFailurePoint);

			message += string.Format(CultureInfo.CurrentCulture, "{0}Memory: {1}{2}Found:  {3}", Environment.NewLine, memory, Environment.NewLine, expectedSubMemory);

			return new DoesNotContainException(message);
		}

		/// <summary>
		/// Creates a new instance of the <see cref="DoesNotContainException"/> class to be thrown
		/// when the requested sub-span was found in the span.
		/// </summary>
		/// <param name="expectedSubSpan">The expected sub-span</param>
		/// <param name="indexFailurePoint">The item index for where the item was found</param>
		/// <param name="failurePointerIndent">The number of spaces needed to indent the failure pointer</param>
		/// <param name="span">The span</param>
		public static DoesNotContainException ForSubSpanFound(
			string expectedSubSpan,
			int indexFailurePoint,
			int? failurePointerIndent,
			string span)
		{
			Assert.GuardArgumentNotNull(nameof(expectedSubSpan), expectedSubSpan);
			Assert.GuardArgumentNotNull(nameof(span), span);

			var message = "Assert.DoesNotContain() Failure: Sub-span found";

			if (failurePointerIndent.HasValue)
				message += string.Format(CultureInfo.CurrentCulture, "{0}       {1}\u2193 (pos {2})", Environment.NewLine, new string(' ', failurePointerIndent.Value), indexFailurePoint);

			message += string.Format(CultureInfo.CurrentCulture, "{0}Span:  {1}{2}Found: {3}", Environment.NewLine, span, Environment.NewLine, expectedSubSpan);

			return new DoesNotContainException(message);
		}

		/// <summary>
		/// Creates a new instance of the <see cref="DoesNotContainException"/> class to be thrown
		/// when the requested sub-string was found in the string.
		/// </summary>
		/// <param name="expectedSubString">The expected sub-string</param>
		/// <param name="indexFailurePoint">The item index for where the item was found</param>
		/// <param name="string">The string</param>
		public static DoesNotContainException ForSubStringFound(
			string expectedSubString,
			int indexFailurePoint,
			string @string)
		{
			Assert.GuardArgumentNotNull(nameof(expectedSubString), expectedSubString);
			Assert.GuardArgumentNotNull(nameof(@string), @string);

			var encodedString = AssertHelper.ShortenAndEncodeString(@string, indexFailurePoint, out var failurePointerIndent);

			return new DoesNotContainException(
				string.Format(
					CultureInfo.CurrentCulture,
					"Assert.DoesNotContain() Failure: Sub-string found{0}        {1}\u2193 (pos {2}){3}String: {4}{5}Found:  {6}",
					Environment.NewLine,
					new string(' ', failurePointerIndent),
					indexFailurePoint,
					Environment.NewLine,
					encodedString,
					Environment.NewLine,
					AssertHelper.ShortenAndEncodeString(expectedSubString)
				)
			);
		}
	}
}
