#if XUNIT_NULLABLE
#nullable enable
#endif

using System;

namespace Xunit.Sdk
{
	/// <summary>
	/// Exception thrown when Assert.Raises fails.
	/// </summary>
#if XUNIT_VISIBILITY_INTERNAL
	internal
#else
	public
#endif
	partial class RaisesException : XunitException
	{
		RaisesException(string message) :
			base(message)
		{ }

		/// <summary>
		/// Creates a new instance of the <see cref="RaisesException" /> class to be thrown when
		/// the raised event wasn't the expected type.
		/// </summary>
		/// <param name="expected">The type of the event args that was expected</param>
		/// <param name="actual">The type of the event args that was actually raised</param>
		public static RaisesException ForIncorrectType(
			Type expected,
			Type actual) =>
				new RaisesException(
					"Assert.Raises() Failure: Wrong event type was raised" + Environment.NewLine +
					"Expected: " + ArgumentFormatter.Format(Assert.GuardArgumentNotNull(nameof(expected), expected)) + Environment.NewLine +
					"Actual:   " + ArgumentFormatter.Format(Assert.GuardArgumentNotNull(nameof(actual), actual))
				);

		/// <summary>
		/// Creates a new instance of the <see cref="RaisesException" /> class to be thrown when
		/// no event was raised.
		/// </summary>
		/// <param name="expected">The type of the event args that was expected</param>
		public static RaisesException ForNoEvent(Type expected) =>
			new RaisesException(
				"Assert.Raises() Failure: No event was raised" + Environment.NewLine +
				"Expected: " + ArgumentFormatter.Format(Assert.GuardArgumentNotNull(nameof(expected), expected)) + Environment.NewLine +
				"Actual:   No event was raised"
			);
	}
}
