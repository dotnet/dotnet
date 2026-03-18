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
					string.Format(
						CultureInfo.CurrentCulture,
						"Assert.Subset() Failure: Value is not a subset{0}Expected: {1}{2}Actual:   {3}",
						Environment.NewLine,
						Assert.GuardArgumentNotNull(nameof(expected), expected),
						Environment.NewLine,
						Assert.GuardArgumentNotNull(nameof(actual), actual)
					)
				);
	}
}
