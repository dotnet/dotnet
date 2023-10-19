#if XUNIT_NULLABLE
#nullable enable
#else
// In case this is source-imported with global nullable enabled but no XUNIT_NULLABLE
#pragma warning disable CS8604
#endif

using System;

namespace Xunit.Sdk
{
	/// <summary>
	/// Exception thrown when Assert.Null fails.
	/// </summary>
#if XUNIT_VISIBILITY_INTERNAL
	internal
#else
	public
#endif
	partial class NullException : XunitException
	{
		NullException(string message) :
			base(message)
		{ }

		/// <summary>
		/// Creates a new instance of the <see cref="NullException"/> class to be thrown
		/// when the given nullable struct was unexpectedly not null.
		/// </summary>
		/// <param name="type">The inner type of the value</param>
		/// <param name="actual">The actual non-<c>null</c> value</param>
		public static Exception ForNonNullStruct<T>(
			Type type,
			T? actual)
				where T : struct =>
					new NullException(
						$"Assert.Null() Failure: Value of type 'Nullable<{ArgumentFormatter.FormatTypeName(type)}>' has a value" + Environment.NewLine +
						"Expected: null" + Environment.NewLine +
						"Actual:   " + ArgumentFormatter.Format(actual)
					);

		/// <summary>
		/// Creates a new instance of the <see cref="NullException"/> class to be thrown
		/// when the given value was unexpectedly not null.
		/// </summary>
		/// <param name="actual">The actual non-<c>null</c> value</param>
		public static NullException ForNonNullValue(object actual) =>
			new NullException(
				"Assert.Null() Failure: Value is not null" + Environment.NewLine +
				"Expected: null" + Environment.NewLine +
				"Actual:   " + ArgumentFormatter.Format(actual)
			);
	}
}
