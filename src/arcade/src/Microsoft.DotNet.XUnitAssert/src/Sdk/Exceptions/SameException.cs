#if XUNIT_NULLABLE
#nullable enable
#endif

using System;

namespace Xunit.Sdk
{
	/// <summary>
	/// Exception thrown when Assert.Same fails.
	/// </summary>
#if XUNIT_VISIBILITY_INTERNAL
	internal
#else
	public
#endif
	partial class SameException : XunitException
	{
		SameException(string message) :
			base(message)
		{ }

		/// <summary>
		/// Creates a new instance of the <see cref="SameException"/> class to be thrown
		/// when two values are not the same instance.
		/// </summary>
		/// <param name="expected">The expected value</param>
		/// <param name="actual">The actual value</param>
		public static SameException ForFailure(
			string expected,
			string actual) =>
				new SameException(
					"Assert.Same() Failure: Values are not the same instance" + Environment.NewLine +
					"Expected: " + Assert.GuardArgumentNotNull(nameof(expected), expected) + Environment.NewLine +
					"Actual:   " + Assert.GuardArgumentNotNull(nameof(actual), actual)
				);
	}
}
