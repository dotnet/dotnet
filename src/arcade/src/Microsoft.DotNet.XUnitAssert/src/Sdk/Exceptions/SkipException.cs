#if XUNIT_NULLABLE
#nullable enable
#endif

namespace Xunit.Sdk
{
	/// <summary>
	/// Exception thrown when Assert.Skip is called.
	/// </summary>
#if XUNIT_VISIBILITY_INTERNAL
	internal
#else
	public
#endif
	partial class SkipException : XunitException
	{
		SkipException(string message) :
			base(message)
		{ }

		/// <summary>
		/// Creates a new instance of the <see cref="SkipException"/> class to be thrown
		/// when a user wants to dynamically skip a test. Note that this only works in
		/// v3 and later of xUnit.net, as it requires runtime infrastructure changes.
		/// </summary>
		public static SkipException ForSkip(string message) =>
			new SkipException($"{DynamicSkipToken.Value}{message}");
	}
}
