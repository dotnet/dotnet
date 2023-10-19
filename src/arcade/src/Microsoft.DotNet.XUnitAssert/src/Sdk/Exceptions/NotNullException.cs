#if XUNIT_NULLABLE
#nullable enable
#endif

using System;

namespace Xunit.Sdk
{
	/// <summary>
	/// Exception thrown when Assert.NotNull fails.
	/// </summary>
#if XUNIT_VISIBILITY_INTERNAL
	internal
#else
	public
#endif
	partial class NotNullException : XunitException
	{
		NotNullException(string message) :
			base(message)
		{ }

		/// <summary>
		/// Creates a new instance of the <see cref="NotNullException"/> class to be
		/// throw when a nullable struct is <c>null</c>.
		/// </summary>
		/// <param name="type">The inner type of the value</param>
		public static Exception ForNullStruct(Type type) =>
			new NotNullException($"Assert.NotNull() Failure: Value of type 'Nullable<{ArgumentFormatter.FormatTypeName(type)}>' does not have a value");

		/// <summary>
		/// Creates a new instance of the <see cref="NotNullException"/> class to be
		/// thrown when a reference value is <c>null</c>.
		/// </summary>
		public static NotNullException ForNullValue() =>
			new NotNullException("Assert.NotNull() Failure: Value is null");
	}
}
