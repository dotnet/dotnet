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
	/// Exception thrown when Assert.Throws fails.
	/// </summary>
#if XUNIT_VISIBILITY_INTERNAL
	internal
#else
	public
#endif
	partial class ThrowsException : XunitException
	{
		ThrowsException(
			string message,
#if XUNIT_NULLABLE
			Exception? innerException = null) :
#else
			Exception innerException = null) :
#endif
				base(message, innerException)
		{ }

		/// <summary>
		/// Creates a new instance of the <see cref="ThrowsException"/> class to be thrown when
		/// an exception of the wrong type was thrown by Assert.Throws.
		/// </summary>
		/// <param name="expected">The expected exception type</param>
		/// <param name="actual">The actual exception</param>
		public static ThrowsException ForIncorrectExceptionType(
			Type expected,
			Exception actual) =>
				new ThrowsException(
					string.Format(
						CultureInfo.CurrentCulture,
						"Assert.Throws() Failure: Exception type was not an exact match{0}Expected: {1}{2}Actual:   {3}",
						Environment.NewLine,
						ArgumentFormatter.Format(Assert.GuardArgumentNotNull(nameof(expected), expected)),
						Environment.NewLine,
						ArgumentFormatter.Format(Assert.GuardArgumentNotNull(nameof(actual), actual).GetType())
					),
					actual
				);

		/// <summary>
		/// Creates a new instance of the <see cref="ThrowsException"/> class to be thrown when
		/// an <see cref="ArgumentException"/> is thrown with the wrong parameter name.
		/// </summary>
		/// <param name="expected">The exception type</param>
		/// <param name="expectedParamName">The expected parameter name</param>
		/// <param name="actualParamName">The actual parameter name</param>
		public static ThrowsException ForIncorrectParameterName(
			Type expected,
#if XUNIT_NULLABLE
			string? expectedParamName,
			string? actualParamName) =>
#else
			string expectedParamName,
			string actualParamName) =>
#endif
				new ThrowsException(
					string.Format(
						CultureInfo.CurrentCulture,
						"Assert.Throws() Failure: Incorrect parameter name{0}Exception: {1}{2}Expected:  {3}{4}Actual:    {5}",
						Environment.NewLine,
						ArgumentFormatter.Format(Assert.GuardArgumentNotNull(nameof(expected), expected)),
						Environment.NewLine,
						ArgumentFormatter.Format(expectedParamName),
						Environment.NewLine,
						ArgumentFormatter.Format(actualParamName)
					)
				);

		/// <summary>
		/// Creates a new instance of the <see cref="ThrowsException"/> class to be thrown when
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
				new ThrowsException(string.Format(CultureInfo.CurrentCulture, "Assert.Throws() Failure: {0}", message), innerException);

		/// <summary>
		/// Creates a new instance of the <see cref="ThrowsException"/> class to be thrown when
		/// an exception wasn't thrown by Assert.Throws.
		/// </summary>
		/// <param name="expected">The expected exception type</param>
		public static ThrowsException ForNoException(Type expected) =>
			new ThrowsException(
				string.Format(
					CultureInfo.CurrentCulture,
					"Assert.Throws() Failure: No exception was thrown{0}Expected: {1}",
					Environment.NewLine,
					ArgumentFormatter.Format(Assert.GuardArgumentNotNull(nameof(expected), expected))
				)
			);
	}
}
