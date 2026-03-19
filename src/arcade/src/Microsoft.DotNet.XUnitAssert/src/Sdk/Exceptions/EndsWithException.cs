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
	/// Exception thrown when Assert.EndsWith fails.
	/// </summary>
#if XUNIT_VISIBILITY_INTERNAL
	internal
#else
	public
#endif
	partial class EndsWithException : XunitException
	{
		EndsWithException(string message) :
			base(message)
		{ }

		/// <summary>
		/// Creates an instance of the <see cref="EndsWithException"/> class to be thrown
		/// when a string does not end with the given value.
		/// </summary>
		/// <param name="expected">The expected ending</param>
		/// <param name="actual">The actual value</param>
		/// <returns></returns>
		public static EndsWithException ForStringNotFound(
#if XUNIT_NULLABLE
			string? expected,
			string? actual) =>
#else
			string expected,
			string actual) =>
#endif
				new EndsWithException(
					string.Format(
						CultureInfo.CurrentCulture,
						"Assert.EndsWith() Failure: String end does not match{0}String:       {1}{2}Expected end: {3}",
						Environment.NewLine,
						AssertHelper.ShortenAndEncodeStringEnd(actual),
						Environment.NewLine,
						AssertHelper.ShortenAndEncodeString(expected)
					)
				);
	}
}
