#if XUNIT_NULLABLE
#nullable enable
#endif

namespace Xunit.Sdk
{
	/// <summary>
	/// Exception thrown when a value is unexpectedly true.
	/// </summary>
#if XUNIT_VISIBILITY_INTERNAL
	internal
#else
	public
#endif
	class FalseException : AssertActualExpectedException
	{
		/// <summary>
		/// Creates a new instance of the <see cref="FalseException"/> class.
		/// </summary>
		/// <param name="userMessage">The user message to be display, or null for the default message</param>
		/// <param name="value">The actual value</param>
#if XUNIT_NULLABLE
		public FalseException(string? userMessage, bool? value)
#else
		public FalseException(string userMessage, bool? value)
#endif
			: base("False", value?.ToString() ?? "(null)", userMessage ?? "Assert.False() Failure")
		{ }
	}
}
