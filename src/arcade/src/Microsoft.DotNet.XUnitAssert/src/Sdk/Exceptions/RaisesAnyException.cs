#if XUNIT_NULLABLE
#nullable enable
#endif

using System;

namespace Xunit.Sdk
{
	/// <summary>
	/// Exception thrown when Assert.RaisesAny fails.
	/// </summary>
#if XUNIT_VISIBILITY_INTERNAL
	internal
#else
	public
#endif
	partial class RaisesAnyException : XunitException
	{
		RaisesAnyException(string message) :
			base(message)
		{ }

		/// <summary>
		/// Creates a new instance of the <see cref="RaisesAnyException" /> class to be thrown when
		/// no event was raised.
		/// </summary>
		/// <param name="expected">The type of the event args that was expected</param>
		public static RaisesAnyException ForNoEvent(Type expected) =>
			new RaisesAnyException(
				"Assert.RaisesAny() Failure: No event was raised" + Environment.NewLine +
				"Expected: " + ArgumentFormatter.Format(expected) + Environment.NewLine +
				"Actual:   No event was raised"
			);
	}
}
