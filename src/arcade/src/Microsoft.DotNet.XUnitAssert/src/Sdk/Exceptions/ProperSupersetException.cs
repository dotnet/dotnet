#if XUNIT_NULLABLE
#nullable enable
#endif

using System;

namespace Xunit.Sdk
{
	/// <summary>
	/// Exception thrown when Assert.ProperSuperset fails.
	/// </summary>
#if XUNIT_VISIBILITY_INTERNAL
	internal
#else
	public
#endif
	partial class ProperSupersetException : XunitException
	{
		ProperSupersetException(string message) :
			base(message)
		{ }

		/// <summary>
		/// Creates a new instance of the <see cref="ProperSupersetException"/> class to be thrown
		/// when a set is not a proper superset of another set
		/// </summary>
		/// <param name="expected">The expected value</param>
		/// <param name="actual">The actual value</param>
		public static ProperSupersetException ForFailure(
			string expected,
			string actual) =>
				new ProperSupersetException(
					"Assert.ProperSuperset() Failure: Value is not a proper superset" + Environment.NewLine +
					"Expected: " + expected + Environment.NewLine +
					"Actual:   " + actual
				);
	}
}
