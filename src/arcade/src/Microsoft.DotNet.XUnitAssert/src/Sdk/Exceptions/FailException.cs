#pragma warning disable CA1032 // Implement standard exception constructors
#pragma warning disable IDE0090 // Use 'new(...)'

#if XUNIT_NULLABLE
#nullable enable
#endif

namespace Xunit.Sdk
{
	/// <summary>
	/// Exception thrown when Assert.Fail is called.
	/// </summary>
#if XUNIT_VISIBILITY_INTERNAL
	internal
#else
	public
#endif
	partial class FailException : XunitException
	{
		FailException(string message) :
			base(message)
		{ }

		/// <summary>
		/// Creates a new instance of the <see cref="FailException"/> class to be thrown when
		/// the user calls <see cref="Assert.Fail"/>.
		/// </summary>
		/// <param name="message">The user's failure message.</param>
#if XUNIT_NULLABLE
		public static FailException ForFailure(string? message) =>
#else
		public static FailException ForFailure(string message) =>
#endif
			new FailException(message ?? "Assert.Fail() Failure");
	}
}
