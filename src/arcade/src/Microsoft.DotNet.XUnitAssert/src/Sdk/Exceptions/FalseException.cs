#if XUNIT_NULLABLE
#nullable enable
#endif

using System;
using System.Globalization;

namespace Xunit.Sdk
{
	/// <summary>
	/// Exception thrown when Assert.False fails.
	/// </summary>
#if XUNIT_VISIBILITY_INTERNAL
	internal
#else
	public
#endif
	partial class FalseException : XunitException
	{
		FalseException(string message) :
			base(message)
		{ }

		/// <summary>
		/// Creates a new instance of the <see cref="FalseException"/> class to be thrown when
		/// a non-<c>false</c> value was provided.
		/// </summary>
		/// <param name="message">The message to be displayed, or <c>null</c> for the default message</param>
		/// <param name="value">The actual value</param>
		public static FalseException ForNonFalseValue(
#if XUNIT_NULLABLE
			string? message,
#else
			string message,
#endif
			bool? value) =>
				new FalseException(
					message ?? string.Format(
						CultureInfo.CurrentCulture,
						"Assert.False() Failure{0}Expected: False{1}Actual:   {2}",
						Environment.NewLine,
						Environment.NewLine,
						value?.ToString() ?? "null"
					)
				);
	}
}
