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
	/// Exception thrown when Assert.InRange fails.
	/// </summary>
#if XUNIT_VISIBILITY_INTERNAL
	internal
#else
	public
#endif
	partial class InRangeException : XunitException
	{
		InRangeException(string message) :
			base(message)
		{ }

		/// <summary>
		/// Creates a new instance of the <see cref="InRangeException"/> class to be thrown when
		/// the given value is not in the given range.
		/// </summary>
		/// <param name="actual">The actual object value</param>
		/// <param name="low">The low value of the range</param>
		/// <param name="high">The high value of the range</param>
		public static InRangeException ForValueNotInRange(
			object actual,
			object low,
			object high) =>
				new InRangeException(
					string.Format(
						CultureInfo.CurrentCulture,
						"Assert.InRange() Failure: Value not in range{0}Range:  ({1} - {2}){3}Actual: {4}",
						Environment.NewLine,
						ArgumentFormatter.Format(Assert.GuardArgumentNotNull(nameof(low), low)),
						ArgumentFormatter.Format(Assert.GuardArgumentNotNull(nameof(high), high)),
						Environment.NewLine,
						ArgumentFormatter.Format(Assert.GuardArgumentNotNull(nameof(actual), actual))
					)
				);
	}
}
