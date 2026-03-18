#pragma warning disable CA1032 // Implement standard exception constructors
#pragma warning disable IDE0090 // Use 'new(...)'

#if XUNIT_NULLABLE
#nullable enable
#endif

namespace Xunit.Sdk
{
	/// <summary>
	/// Exception thrown when Assert.NotEmpty fails.
	/// </summary>
#if XUNIT_VISIBILITY_INTERNAL
	internal
#else
	public
#endif
	partial class NotEmptyException : XunitException
	{
		NotEmptyException(string message) :
			base(message)
		{ }

		/// <summary>
		/// Creates a new instance of the <see cref="NotEmptyException"/> class to be thrown
		/// when a collection was unexpectedly empty.
		/// </summary>
		public static NotEmptyException ForNonEmptyCollection() =>
			new NotEmptyException("Assert.NotEmpty() Failure: Collection was empty");
	}
}
