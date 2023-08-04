#if XUNIT_NULLABLE
#nullable enable
#endif

using System;
using System.Globalization;

namespace Xunit.Sdk
{
	/// <summary>
	/// Exception thrown when a string unexpectedly matches a regular expression.
	/// </summary>
#if XUNIT_VISIBILITY_INTERNAL
	internal
#else
	public
#endif
	class DoesNotMatchException : XunitException
	{
		/// <summary>
		/// Creates a new instance of the <see cref="DoesNotMatchException"/> class.
		/// </summary>
		/// <param name="expectedRegexPattern">The regular expression pattern expected not to match</param>
		/// <param name="actual">The actual value</param>
#if XUNIT_NULLABLE
		public DoesNotMatchException(string expectedRegexPattern, object? actual)
#else
		public DoesNotMatchException(string expectedRegexPattern, object actual)
#endif
			: base(string.Format(CultureInfo.CurrentCulture, "Assert.DoesNotMatch() Failure:{2}Regex: {0}{2}Value: {1}", expectedRegexPattern, actual, Environment.NewLine))
		{ }
	}
}
