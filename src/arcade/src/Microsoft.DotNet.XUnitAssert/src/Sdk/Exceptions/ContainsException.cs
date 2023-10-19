#if XUNIT_NULLABLE
#nullable enable
#endif

using System;
using Xunit.Internal;

namespace Xunit.Sdk
{
	/// <summary>
	/// Exception thrown when Assert.Contains fails.
	/// </summary>
#if XUNIT_VISIBILITY_INTERNAL
	internal
#else
	public
#endif
	partial class ContainsException : XunitException
	{
		ContainsException(string message) :
			base(message)
		{ }

		/// <summary>
		/// Creates a new instance of the <see cref="ContainsException"/> class to be thrown
		/// when the requested filter did not match any items in the collection.
		/// </summary>
		/// <param name="collection">The collection</param>
		public static ContainsException ForCollectionFilterNotMatched(string collection) =>
			new ContainsException(
				"Assert.Contains() Failure: Filter not matched in collection" + Environment.NewLine +
				"Collection: " + collection
			);

		/// <summary>
		/// Creates a new instance of the <see cref="ContainsException"/> class to be thrown
		/// when the requested item was not available in the collection.
		/// </summary>
		/// <param name="item">The expected item value</param>
		/// <param name="collection">The collection</param>
		public static ContainsException ForCollectionItemNotFound(
			string item,
			string collection) =>
				new ContainsException(
					"Assert.Contains() Failure: Item not found in collection" + Environment.NewLine +
					"Collection: " + collection + Environment.NewLine +
					"Not found:  " + item
				);

		/// <summary>
		/// Creates a new instance of the <see cref="ContainsException"/> class to be thrown
		/// when the requested key was not available in the dictionary.
		/// </summary>
		/// <param name="expectedKey">The expected key value</param>
		/// <param name="keys">The dictionary keys</param>
		public static ContainsException ForKeyNotFound(
			string expectedKey,
			string keys) =>
				new ContainsException(
					"Assert.Contains() Failure: Key not found in dictionary" + Environment.NewLine +
					"Keys:      " + keys + Environment.NewLine +
					"Not found: " + expectedKey
				);

		/// <summary>
		/// Creates a new instance of the <see cref="ContainsException"/> class to be thrown
		/// when the requested item was not found in the set.
		/// </summary>
		/// <param name="item">The expected item</param>
		/// <param name="set">The set</param>
		public static ContainsException ForSetItemNotFound(
			string item,
			string set) =>
				new ContainsException(
					"Assert.Contains() Failure: Item not found in set" + Environment.NewLine +
					"Set:       " + set + Environment.NewLine +
					"Not found: " + item
				);

		/// <summary>
		/// Creates a new instance of the <see cref="ContainsException"/> class to be thrown
		/// when the requested sub-memory was not found in the memory.
		/// </summary>
		/// <param name="expectedSubMemory">The expected sub-memory</param>
		/// <param name="memory">The memory</param>
		public static ContainsException ForSubMemoryNotFound(
			string expectedSubMemory,
			string memory) =>
				new ContainsException(
					"Assert.Contains() Failure: Sub-memory not found" + Environment.NewLine +
					"Memory:    " + memory + Environment.NewLine +
					"Not found: " + expectedSubMemory
				);

		/// <summary>
		/// Creates a new instance of the <see cref="ContainsException"/> class to be thrown
		/// when the requested sub-span was not found in the span.
		/// </summary>
		/// <param name="expectedSubSpan">The expected sub-span</param>
		/// <param name="span">The span</param>
		public static ContainsException ForSubSpanNotFound(
			string expectedSubSpan,
			string span) =>
				new ContainsException(
					"Assert.Contains() Failure: Sub-span not found" + Environment.NewLine +
					"Span:      " + span + Environment.NewLine +
					"Not found: " + expectedSubSpan
				);

		/// <summary>
		/// Creates a new instance of the <see cref="ContainsException"/> class to be thrown
		/// when the requested sub-string was not found in the string.
		/// </summary>
		/// <param name="expectedSubString">The expected sub-string</param>
		/// <param name="string">The string</param>
		public static ContainsException ForSubStringNotFound(
			string expectedSubString,
#if XUNIT_NULLABLE
			string? @string) =>
#else
			string @string) =>
#endif
				new ContainsException(
					"Assert.Contains() Failure: Sub-string not found" + Environment.NewLine +
					"String:    " + AssertHelper.ShortenAndEncodeString(@string) + Environment.NewLine +
					"Not found: " + AssertHelper.ShortenAndEncodeString(expectedSubString)
				);
	}
}
