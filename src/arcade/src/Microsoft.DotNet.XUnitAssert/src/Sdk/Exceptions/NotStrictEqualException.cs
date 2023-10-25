#if XUNIT_NULLABLE
#nullable enable
#endif

using System;

namespace Xunit.Sdk
{
	/// <summary>
	/// Exception thrown when Assert.NotStrictEqual fails.
	/// </summary>
#if XUNIT_VISIBILITY_INTERNAL
	internal
#else
	public
#endif
	partial class NotStrictEqualException : XunitException
	{
		NotStrictEqualException(string message) :
			base(message)
		{ }

		/// <summary>
		/// Creates a new instance of <see cref="NotStrictEqualException"/> to be thrown when two values
		/// are strictly equal.
		/// </summary>
		/// <param name="expected">The expected value</param>
		/// <param name="actual">The actual value</param>
		public static NotStrictEqualException ForEqualValues(
			string expected,
			string actual) =>
				new NotStrictEqualException(
					"Assert.NotStrictEqual() Failure: Values are equal" + Environment.NewLine +
					"Expected: Not " + Assert.GuardArgumentNotNull(nameof(expected), expected) + Environment.NewLine +
					"Actual:       " + Assert.GuardArgumentNotNull(nameof(actual), actual)
				);
	}
}
