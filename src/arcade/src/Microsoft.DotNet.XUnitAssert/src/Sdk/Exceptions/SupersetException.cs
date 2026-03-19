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
	/// Exception thrown when Assert.Superset fails.
	/// </summary>
#if XUNIT_VISIBILITY_INTERNAL
	internal
#else
	public
#endif
	partial class SupersetException : XunitException
	{
		SupersetException(string message) :
			base(message)
		{ }

		/// <summary>
		/// Creates a new instance of the <see cref="SupersetException"/> class to be thrown
		/// when a set is not a superset of another set
		/// </summary>
		/// <param name="expected">The expected value</param>
		/// <param name="actual">The actual value</param>
		public static SupersetException ForFailure(
			string expected,
			string actual) =>
				new SupersetException(
					string.Format(
						CultureInfo.CurrentCulture,
						"Assert.Superset() Failure: Value is not a superset{0}Expected: {1}{2}Actual:   {3}",
						Environment.NewLine,
						Assert.GuardArgumentNotNull(nameof(expected), expected),
						Environment.NewLine,
						Assert.GuardArgumentNotNull(nameof(actual), actual)
					)
				);
	}
}
