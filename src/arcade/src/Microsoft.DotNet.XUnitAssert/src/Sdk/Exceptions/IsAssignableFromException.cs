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
	/// Exception thrown when Assert.IsAssignableFrom fails.
	/// </summary>
#if XUNIT_VISIBILITY_INTERNAL
	internal
#else
	public
#endif
	partial class IsAssignableFromException : XunitException
	{
		IsAssignableFromException(string message) :
			base(message)
		{ }

		/// <summary>
		/// Creates a new instance of the <see cref="IsTypeException"/> class to be thrown when
		/// the value is not compatible with the given type.
		/// </summary>
		/// <param name="expected">The expected type</param>
		/// <param name="actual">The actual object value</param>
		public static IsAssignableFromException ForIncompatibleType(
			Type expected,
#if XUNIT_NULLABLE
			object? actual) =>
#else
			object actual) =>
#endif
				new IsAssignableFromException(
					"Assert.IsAssignableFrom() Failure: Value is " + (actual == null ? "null" : "an incompatible type") + Environment.NewLine +
					"Expected: " + ArgumentFormatter.Format(Assert.GuardArgumentNotNull(nameof(expected), expected)) + Environment.NewLine +
					"Actual:   " + ArgumentFormatter.Format(actual?.GetType())
				);
	}
}
