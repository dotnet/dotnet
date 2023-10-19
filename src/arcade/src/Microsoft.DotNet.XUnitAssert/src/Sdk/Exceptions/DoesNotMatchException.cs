#if XUNIT_NULLABLE
#nullable enable
#endif

using System;

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
					"Assert.DoesNotMatch() Failure: Match found" + Environment.NewLine +
					"        " + new string(' ', failurePointerIndent) + "↓ (pos " + indexFailurePoint + ")" + Environment.NewLine +
					"String: " + @string + Environment.NewLine +
					"RegEx:  " + expectedRegexPattern
				);
	}
}
