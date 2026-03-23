#pragma warning disable CA1032 // Implement standard exception constructors
#pragma warning disable IDE0090 // Use 'new(...)'

#if XUNIT_NULLABLE
#nullable enable
#endif

namespace Xunit.Sdk
{
	/// <summary>
	/// Exception thrown when Assert.NotSame fails.
	/// </summary>
#if XUNIT_VISIBILITY_INTERNAL
	internal
#else
	public
#endif
	partial class NotSameException : XunitException
	{
		NotSameException(string message) :
			base(message)
		{ }

		/// <summary>
		/// Creates a new instance of the <see cref="NotSameException"/> class to be thrown
		/// when two values are the same instance.
		/// </summary>
		public static NotSameException ForSameValues() =>
			new NotSameException("Assert.NotSame() Failure: Values are the same instance");
	}
}
