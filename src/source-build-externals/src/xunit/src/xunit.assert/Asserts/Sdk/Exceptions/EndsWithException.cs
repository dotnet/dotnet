#if XUNIT_NULLABLE
#nullable enable
#endif

using System;
using System.Globalization;

namespace Xunit.Sdk
{
	/// <summary>
	/// Exception thrown when a string does not end with the expected value.
	/// </summary>
#if XUNIT_VISIBILITY_INTERNAL
	internal
#else
	public
#endif
	class EndsWithException : XunitException
	{
		/// <summary>
		/// Creates a new instance of the <see cref="EndsWithException"/> class.
		/// </summary>
		/// <param name="expected">The expected string value</param>
		/// <param name="actual">The actual value</param>
#if XUNIT_NULLABLE
		public EndsWithException(string? expected, string? actual)
#else
		public EndsWithException(string expected, string actual)
#endif
			: base(
				string.Format(
					CultureInfo.CurrentCulture,
					"Assert.EndsWith() Failure:{2}Expected: {0}{2}Actual:   {1}",
					ShortenExpected(expected, actual) ?? "(null)",
					ShortenActual(expected, actual) ?? "(null)",
					Environment.NewLine
				)
			)
		{ }

#if XUNIT_NULLABLE
		static string? ShortenExpected(string? expected, string? actual)
#else
		static string ShortenExpected(string expected, string actual)
#endif
		{
			if (expected == null || actual == null || actual.Length <= expected.Length)
				return expected;

			return "   " + expected;
		}

#if XUNIT_NULLABLE
		static string? ShortenActual(string? expected, string? actual)
#else
		static string ShortenActual(string expected, string actual)
#endif
		{
			if (expected == null || actual == null || actual.Length <= expected.Length)
				return actual;

			return "···" + actual.Substring(actual.Length - expected.Length);
		}
	}
}
