#if XUNIT_NULLABLE
#nullable enable
#endif

namespace Xunit.Sdk
{
	/// <summary>
	/// Exception thrown when Assert.PropertyChanged fails.
	/// </summary>
#if XUNIT_VISIBILITY_INTERNAL
	internal
#else
	public
#endif
	partial class PropertyChangedException : XunitException
	{
		PropertyChangedException(string message) :
			base(message)
		{ }

		/// <summary>
		/// Creates a new instance of the <see cref="PropertyChangedException"/> class to be thrown
		/// when a property was unexpectedly not set.
		/// </summary>
		/// <param name="propertyName">The name of the property that was expected to be changed.</param>
		public static PropertyChangedException ForUnsetProperty(string propertyName) =>
			new PropertyChangedException($"Assert.PropertyChanged() failure: Property '{Assert.GuardArgumentNotNull(nameof(propertyName), propertyName)}' was not set");
	}
}
