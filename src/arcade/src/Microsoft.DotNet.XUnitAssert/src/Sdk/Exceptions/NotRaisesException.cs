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
	/// Exception thrown when Assert.NotRaisedAny fails.
	/// </summary>
#if XUNIT_VISIBILITY_INTERNAL
    internal
#else
	public
#endif
	partial class NotRaisesException : XunitException
	{
		NotRaisesException(string message) :
			base(message)
		{ }

		/// <summary>
		/// Creates a new instance of the <see cref="NotRaisesException" /> class to be thrown when
		/// an unexpected event was raised.
		/// </summary>
		public static NotRaisesException ForUnexpectedEvent() =>
			new NotRaisesException("Assert.NotRaisedAny() Failure: An unexpected event was raised");

		/// <summary>
		/// Creates a new instance of the <see cref="NotRaisesException" /> class to be thrown when
		/// an unexpected event (with data) was raised.
		/// </summary>
		/// <param name="unexpected">The type of the event args that was unexpected</param>
		public static NotRaisesException ForUnexpectedEvent(Type unexpected) =>
			new NotRaisesException(
				string.Format(
					CultureInfo.CurrentCulture,
					"Assert.NotRaisedAny() Failure: An unexpected event was raised{0}Unexpected: {1}{2}Actual:   An event was raised",
					Environment.NewLine,
					ArgumentFormatter.Format(Assert.GuardArgumentNotNull(nameof(unexpected), unexpected)),
					Environment.NewLine
				)
			);
	}
}
