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
	/// Exception thrown when Assert.StartsWith fails.
	/// </summary>
#if XUNIT_VISIBILITY_INTERNAL
	internal
#else
	public
#endif
	partial class StartsWithException : XunitException
	{
		StartsWithException(string message) :
			base(message)
		{ }

		/// <summary>
		/// Creates an instance of the <see cref="StartsWithException"/> class to be thrown
		/// when a string does not start with the given value.
		/// </summary>
		/// <param name="expected">The expected start</param>
		/// <param name="actual">The actual value</param>
		/// <returns></returns>
		public static StartsWithException ForStringNotFound(
#if XUNIT_NULLABLE
			string? expected,
			string? actual) =>
#else
			string expected,
			string actual) =>
#endif
				new StartsWithException(
					string.Format(
						CultureInfo.CurrentCulture,
						"Assert.StartsWith() Failure: String start does not match{0}String:         {1}{2}Expected start: {3}",
						Environment.NewLine,
						AssertHelper.ShortenAndEncodeString(actual),
						Environment.NewLine,
						AssertHelper.ShortenAndEncodeString(expected)
					)
				);
	}
}
