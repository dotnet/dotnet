#pragma warning disable CA1032 // Implement standard exception constructors
#pragma warning disable IDE0090 // Use 'new(...)'

#if XUNIT_NULLABLE
#nullable enable
#endif

using System;
using System.Globalization;

namespace Xunit.Sdk
{
	/// <summary>
	/// Exception thrown when Assert.Matches fails.
	/// </summary>
#if XUNIT_VISIBILITY_INTERNAL
	internal
#else
	public
#endif
	partial class MatchesException : XunitException
	{
		MatchesException(string message) :
			base(message)
		{ }

		/// <summary>
		/// Creates a new instance of the <see cref="MatchesException"/> class to be thrown when
		/// the regular expression pattern isn't found within the value.
		/// </summary>
		/// <param name="expectedRegexPattern">The expected regular expression pattern</param>
		/// <param name="actual">The actual value</param>
		public static MatchesException ForMatchNotFound(
			string expectedRegexPattern,
#if XUNIT_NULLABLE
			string? actual) =>
#else
			string actual) =>
#endif
				new MatchesException(
					string.Format(
						CultureInfo.CurrentCulture,
						"Assert.Matches() Failure: Pattern not found in value{0}Regex: {1}{2}Value: {3}",
						Environment.NewLine,
						ArgumentFormatter.Format(Assert.GuardArgumentNotNull(nameof(expectedRegexPattern), expectedRegexPattern)),
						Environment.NewLine,
						ArgumentFormatter.Format(actual)
					)
				);
	}
}
