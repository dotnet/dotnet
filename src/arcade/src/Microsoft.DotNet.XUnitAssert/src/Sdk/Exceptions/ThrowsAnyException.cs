#pragma warning disable CA1032 // Implement standard exception constructors
#pragma warning disable IDE0090 // Use 'new(...)'

#if XUNIT_NULLABLE
#nullable enable
#else
// In case this is source-imported with global nullable enabled but no XUNIT_NULLABLE
#pragma warning disable CS8625
#endif

using System;
using System.Globalization;

namespace Xunit.Sdk
{
	/// <summary>
	/// Exception thrown when Assert.ThrowsAny fails.
	/// </summary>
#if XUNIT_VISIBILITY_INTERNAL
	internal
#else
	public
#endif
	partial class ThrowsAnyException : XunitException
	{
		ThrowsAnyException(
			string message,
#if XUNIT_NULLABLE
			Exception? innerException = null) :
#else
			Exception innerException = null) :
#endif
				base(message, innerException)
		{ }

		/// <summary>
		/// Creates a new instance of the <see cref="ThrowsAnyException"/> class to be thrown when
		/// an exception of the wrong type was thrown by Assert.ThrowsAny.
		/// </summary>
		/// <param name="expected">The expected exception type</param>
		/// <param name="actual">The actual exception</param>
		public static ThrowsAnyException ForIncorrectExceptionType(
			Type expected,
			Exception actual) =>
				new ThrowsAnyException(
					string.Format(
						CultureInfo.CurrentCulture,
						"Assert.ThrowsAny() Failure: Exception type was not compatible{0}Expected: {1}{2}Actual:   {3}",
						Environment.NewLine,
						ArgumentFormatter.Format(Assert.GuardArgumentNotNull(nameof(expected), expected)),
						Environment.NewLine,
						ArgumentFormatter.Format(Assert.GuardArgumentNotNull(nameof(actual), actual).GetType())
					),
					actual
				);

		/// <summary>
		/// Creates a new instance of the <see cref="ThrowsAnyException"/> class to be thrown when
		/// an inspector rejected the exception.
		/// </summary>
		/// <param name="message">The custom message</param>
		/// <param name="innerException">The optional exception thrown by the inspector</param>
		public static Exception ForInspectorFailure(
			string message,
#if XUNIT_NULLABLE
			Exception? innerException = null) =>
#else
			Exception innerException = null) =>
#endif
				new ThrowsAnyException(string.Format(CultureInfo.CurrentCulture, "Assert.ThrowsAny() Failure: {0}", message), innerException);

		/// <summary>
		/// Creates a new instance of the <see cref="ThrowsAnyException"/> class to be thrown when
		/// an exception wasn't thrown by Assert.ThrowsAny.
		/// </summary>
		/// <param name="expected">The expected exception type</param>
		public static ThrowsAnyException ForNoException(Type expected) =>
			new ThrowsAnyException(
				string.Format(
					CultureInfo.CurrentCulture,
					"Assert.ThrowsAny() Failure: No exception was thrown{0}Expected: {1}",
					Environment.NewLine,
					ArgumentFormatter.Format(Assert.GuardArgumentNotNull(nameof(expected), expected))
				)
			);
	}
}
