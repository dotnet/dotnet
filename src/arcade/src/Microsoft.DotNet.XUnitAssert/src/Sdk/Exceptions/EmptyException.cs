#pragma warning disable CA1032 // Implement standard exception constructors
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
				string.Format(
					CultureInfo.CurrentCulture,
					"Assert.Empty() Failure: Collection was not empty{0}Collection: {1}",
					Environment.NewLine,
					Assert.GuardArgumentNotNull(nameof(collection), collection)
				)
			);

		/// <summary>
		/// Creates a new instance of the <see cref="EmptyException"/> to be thrown
		/// when the string is not empty.
		/// </summary>
		/// <param name="value">The non-empty string value</param>
		public static EmptyException ForNonEmptyString(string value) =>
			new EmptyException(
				string.Format(
					CultureInfo.CurrentCulture,
					"Assert.Empty() Failure: String was not empty{0}String: {1}",
					Environment.NewLine,
					AssertHelper.ShortenAndEncodeString(Assert.GuardArgumentNotNull(nameof(value), value))
				)
			);
	}
}
