#if XUNIT_NULLABLE
#nullable enable
#endif

using System;
using Xunit.Internal;

namespace Xunit.Sdk
{
	/// <summary>
	/// Exception thrown when Assert.Empty fails.
	/// </summary>
#if XUNIT_VISIBILITY_INTERNAL
	internal
#else
	public
#endif
	partial class EmptyException : XunitException
	{
		EmptyException(string message) :
			base(message)
		{ }

		/// <summary>
		/// Creates a new instance of the <see cref="EmptyException"/> to be thrown
		/// when the collection is not empty.
		/// </summary>
		/// <param name="collection">The non-empty collection</param>
		public static EmptyException ForNonEmptyCollection(string collection) =>
			new EmptyException(
				"Assert.Empty() Failure: Collection was not empty" + Environment.NewLine +
				"Collection: " + Assert.GuardArgumentNotNull(nameof(collection), collection)
			);

		/// <summary>
		/// Creates a new instance of the <see cref="EmptyException"/> to be thrown
		/// when the string is not empty.
		/// </summary>
		/// <param name="value">The non-empty string value</param>
		public static EmptyException ForNonEmptyString(string value) =>
			new EmptyException(
				"Assert.Empty() Failure: String was not empty" + Environment.NewLine +
				"String: " + AssertHelper.ShortenAndEncodeString(Assert.GuardArgumentNotNull(nameof(value), value))
			);
	}
}
