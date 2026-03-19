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
					string.Format(
						CultureInfo.CurrentCulture,
						"Assert.Same() Failure: Values are not the same instance{0}Expected: {1}{2}Actual:   {3}",
						Environment.NewLine,
						Assert.GuardArgumentNotNull(nameof(expected), expected),
						Environment.NewLine,
						Assert.GuardArgumentNotNull(nameof(actual), actual)
					)
				);
	}
}
