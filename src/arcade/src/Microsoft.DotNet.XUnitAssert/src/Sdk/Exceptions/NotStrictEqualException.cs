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
					string.Format(
						CultureInfo.CurrentCulture,
						"Assert.NotStrictEqual() Failure: Values are equal{0}Expected: Not {1}{2}Actual:       {3}",
						Environment.NewLine,
						Assert.GuardArgumentNotNull(nameof(expected), expected),
						Environment.NewLine,
						Assert.GuardArgumentNotNull(nameof(actual), actual)
					)
				);
	}
}
