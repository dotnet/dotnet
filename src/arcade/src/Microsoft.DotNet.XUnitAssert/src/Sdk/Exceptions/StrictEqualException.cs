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
					string.Format(
						CultureInfo.CurrentCulture,
						"Assert.StrictEqual() Failure: Values differ{0}Expected: {1}{2}Actual:   {3}",
						Environment.NewLine,
						Assert.GuardArgumentNotNull(nameof(expected), expected),
						Environment.NewLine,
						Assert.GuardArgumentNotNull(nameof(actual), actual)
					)
				);
	}
}
