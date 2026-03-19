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
	/// Exception thrown when Assert.NotInRange fails.
	/// </summary>
#if XUNIT_VISIBILITY_INTERNAL
	internal
#else
	public
#endif
	partial class NotInRangeException : XunitException
	{
		NotInRangeException(string message) :
			base(message)
		{ }

		/// <summary>
		/// Creates a new instance of the <see cref="NotInRangeException"/> class to be thrown when
		/// a value was unexpected with the range of two other values.
		/// </summary>
		/// <param name="actual">The actual object value</param>
		/// <param name="low">The low value of the range</param>
		/// <param name="high">The high value of the range</param>
		public static NotInRangeException ForValueInRange(
			object actual,
			object low,
			object high) =>
				new NotInRangeException(
					string.Format(
						CultureInfo.CurrentCulture,
						"Assert.NotInRange() Failure: Value in range{0}Range:  ({1} - {2}){3}Actual: {4}",
						Environment.NewLine,
						ArgumentFormatter.Format(Assert.GuardArgumentNotNull(nameof(low), low)),
						ArgumentFormatter.Format(Assert.GuardArgumentNotNull(nameof(high), high)),
						Environment.NewLine,
						ArgumentFormatter.Format(Assert.GuardArgumentNotNull(nameof(actual), actual))
					)
				);
	}
}
