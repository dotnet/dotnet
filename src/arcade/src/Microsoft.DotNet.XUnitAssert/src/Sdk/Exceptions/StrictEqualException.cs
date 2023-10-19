#if XUNIT_NULLABLE
#nullable enable
#endif

using System;

namespace Xunit.Sdk
{
	/// <summary>
	/// Exception thrown when Assert.StrictEqual fails.
	/// </summary>
#if XUNIT_VISIBILITY_INTERNAL
	internal
#else
	public
#endif
	partial class StrictEqualException : XunitException
	{
		StrictEqualException(string message) :
			base(message)
		{ }

		/// <summary>
		/// Creates a new instance of <see cref="StrictEqualException"/> to be thrown when two values
		/// are not strictly equal.
		/// </summary>
		/// <param name="expected">The expected value</param>
		/// <param name="actual">The actual value</param>
		public static StrictEqualException ForEqualValues(
			string expected,
			string actual) =>
				new StrictEqualException(
					"Assert.StrictEqual() Failure: Values differ" + Environment.NewLine +
					"Expected: " + expected + Environment.NewLine +
					"Actual:   " + actual
				);
	}
}
