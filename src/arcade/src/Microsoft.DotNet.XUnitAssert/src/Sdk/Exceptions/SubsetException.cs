#if XUNIT_NULLABLE
#nullable enable
#endif

using System;

namespace Xunit.Sdk
{
	/// <summary>
	/// Exception thrown when Assert.Subset fails.
	/// </summary>
#if XUNIT_VISIBILITY_INTERNAL
	internal
#else
	public
#endif
	partial class SubsetException : XunitException
	{
		SubsetException(string message) :
			base(message)
		{ }

		/// <summary>
		/// Creates a new instance of the <see cref="SubsetException"/> class to be thrown
		/// when a set is not a subset of another set
		/// </summary>
		/// <param name="expected">The expected value</param>
		/// <param name="actual">The actual value</param>
		public static SubsetException ForFailure(
			string expected,
			string actual) =>
				new SubsetException(
					"Assert.Subset() Failure: Value is not a subset" + Environment.NewLine +
					"Expected: " + expected + Environment.NewLine +
					"Actual:   " + actual
				);
	}
}
