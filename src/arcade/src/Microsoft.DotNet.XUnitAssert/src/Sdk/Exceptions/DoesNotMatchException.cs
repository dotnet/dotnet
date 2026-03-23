#pragma warning disable CA1032 // Implement standard exception constructors
#pragma warning disable CA1720 // Identifier contains type name
#pragma warning disable IDE0090 // Use 'new(...)'

#if XUNIT_NULLABLE
#nullable enable
#endif

using System;
using System.Globalization;

namespace Xunit.Sdk
{
	/// <summary>
	/// Exception thrown when Assert.DoesNotMatch fails.
	/// </summary>
#if XUNIT_VISIBILITY_INTERNAL
	internal
#else
	public
#endif
	partial class DoesNotMatchException : XunitException
	{
		DoesNotMatchException(string message) :
			base(message)
		{ }

		/// <summary>
		/// Creates a new instance of the <see cref="DoesNotMatchException"/> class, thrown when
		/// a regular expression matches the input string.
		/// </summary>
		/// <param name="expectedRegexPattern">The expected regular expression pattern</param>
		/// <param name="indexFailurePoint">The item index for where the item was found</param>
		/// <param name="failurePointerIndent">The number of spaces needed to indent the failure pointer</param>
		/// <param name="string">The string matched again</param>
		/// <exception cref="InvalidOperationException"></exception>
		public static DoesNotMatchException ForMatch(
			string expectedRegexPattern,
			int indexFailurePoint,
			int failurePointerIndent,
			string @string) =>
				new DoesNotMatchException(
					string.Format(
						CultureInfo.CurrentCulture,
						"Assert.DoesNotMatch() Failure: Match found{0}        {1}\u2193 (pos {2}){3}String: {4}{5}RegEx:  {6}",
						Environment.NewLine,
						new string(' ', failurePointerIndent),
						indexFailurePoint,
						Environment.NewLine,
						Assert.GuardArgumentNotNull(nameof(@string), @string),
						Environment.NewLine,
						Assert.GuardArgumentNotNull(nameof(expectedRegexPattern), expectedRegexPattern)
					)
				);
	}
}
