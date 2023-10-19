#if XUNIT_NULLABLE
#nullable enable
#endif

using System;
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
					"Assert.EndsWith() Failure: String end does not match" + Environment.NewLine +
					"String:       " + AssertHelper.ShortenAndEncodeStringEnd(actual) + Environment.NewLine +
					"Expected end: " + AssertHelper.ShortenAndEncodeString(expected)
				);
	}
}
