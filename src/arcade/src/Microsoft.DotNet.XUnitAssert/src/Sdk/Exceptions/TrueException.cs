#if XUNIT_NULLABLE
#nullable enable
#endif

using System;
using System.Globalization;

namespace Xunit.Sdk
{
	/// <summary>
	/// Exception thrown when Assert.True fails.
	/// </summary>
#if XUNIT_VISIBILITY_INTERNAL
	internal
#else
	public
#endif
	partial class TrueException : XunitException
	{
		TrueException(string message) :
			base(message)
		{ }

		/// <summary>
		/// Creates a new instance of the <see cref="TrueException"/> class to be thrown when
		/// a non-<c>true</c> value was provided.
		/// </summary>
		/// <param name="message">The message to be displayed, or <c>null</c> for the default message</param>
		/// <param name="value">The actual value</param>
		public static TrueException ForNonTrueValue(
#if XUNIT_NULLABLE
			string? message,
#else
			string message,
#endif
			bool? value) =>
				new TrueException(
					message ??
					string.Format(
						CultureInfo.CurrentCulture,
						"Assert.True() Failure{0}Expected: True{1}Actual:   {2}",
						Environment.NewLine,
						Environment.NewLine,
						value?.ToString() ?? "null"
					)
				);
	}
}
