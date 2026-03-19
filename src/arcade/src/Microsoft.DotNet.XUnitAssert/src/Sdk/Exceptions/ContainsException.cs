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
				string.Format(
					CultureInfo.CurrentCulture,
					"Assert.Contains() Failure: Filter not matched in collection{0}Collection: {1}",
					Environment.NewLine,
					Assert.GuardArgumentNotNull(nameof(collection), collection)
				)
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
					string.Format(
						CultureInfo.CurrentCulture,
						"Assert.Contains() Failure: Item not found in collection{0}Collection: {1}{2}Not found:  {3}",
						Environment.NewLine,
						Assert.GuardArgumentNotNull(nameof(collection), collection),
						Environment.NewLine,
						Assert.GuardArgumentNotNull(nameof(item), item)
					)
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
					string.Format(
						CultureInfo.CurrentCulture,
						"Assert.Contains() Failure: Key not found in dictionary{0}Keys:      {1}{2}Not found: {3}",
						Environment.NewLine,
						Assert.GuardArgumentNotNull(nameof(keys), keys),
						Environment.NewLine,
						Assert.GuardArgumentNotNull(nameof(expectedKey), expectedKey)
					)
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
					string.Format(
						CultureInfo.CurrentCulture,
						"Assert.Contains() Failure: Item not found in set{0}Set:       {1}{2}Not found: {3}",
						Environment.NewLine,
						Assert.GuardArgumentNotNull(nameof(set), set),
						Environment.NewLine,
						Assert.GuardArgumentNotNull(nameof(item), item)
					)
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
					string.Format(
						CultureInfo.CurrentCulture,
						"Assert.Contains() Failure: Sub-memory not found{0}Memory:    {1}{2}Not found: {3}",
						Environment.NewLine,
						Assert.GuardArgumentNotNull(nameof(memory), memory),
						Environment.NewLine,
						Assert.GuardArgumentNotNull(nameof(expectedSubMemory), expectedSubMemory)
					)
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
					string.Format(
						CultureInfo.CurrentCulture,
						"Assert.Contains() Failure: Sub-span not found{0}Span:      {1}{2}Not found: {3}",
						Environment.NewLine,
						Assert.GuardArgumentNotNull(nameof(span), span),
						Environment.NewLine,
						Assert.GuardArgumentNotNull(nameof(expectedSubSpan), expectedSubSpan)
					)
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
					string.Format(
						CultureInfo.CurrentCulture,
						"Assert.Contains() Failure: Sub-string not found{0}String:    {1}{2}Not found: {3}",
						Environment.NewLine,
						AssertHelper.ShortenAndEncodeString(@string),
						Environment.NewLine,
						AssertHelper.ShortenAndEncodeString(Assert.GuardArgumentNotNull(nameof(expectedSubString), expectedSubString))
					)
				);
	}
}
