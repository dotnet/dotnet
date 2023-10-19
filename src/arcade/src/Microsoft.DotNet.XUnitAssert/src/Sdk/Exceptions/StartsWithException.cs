#if XUNIT_NULLABLE
#nullable enable
#endif

using System;
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
					"Assert.StartsWith() Failure: String start does not match" + Environment.NewLine +
					"String:         " + AssertHelper.ShortenAndEncodeString(actual) + Environment.NewLine +
					"Expected start: " + AssertHelper.ShortenAndEncodeString(expected)
				);
	}
}
