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
	/// Exception thrown when Assert.ProperSubset fails.
	/// </summary>
#if XUNIT_VISIBILITY_INTERNAL
	internal
#else
	public
#endif
	partial class ProperSubsetException : XunitException
	{
		ProperSubsetException(string message) :
			base(message)
		{ }

		/// <summary>
		/// Creates a new instance of the <see cref="ProperSubsetException"/> class to be thrown
		/// when a set is not a proper subset of another set
		/// </summary>
		/// <param name="expected">The expected value</param>
		/// <param name="actual">The actual value</param>
		public static ProperSubsetException ForFailure(
			string expected,
			string actual) =>
				new ProperSubsetException(
					string.Format(
						CultureInfo.CurrentCulture,
						"Assert.ProperSubset() Failure: Value is not a proper subset{0}Expected: {1}{2}Actual:   {3}",
						Environment.NewLine,
						Assert.GuardArgumentNotNull(nameof(expected), expected),
						Environment.NewLine,
						Assert.GuardArgumentNotNull(nameof(actual), actual)
					)
				);
	}
}
