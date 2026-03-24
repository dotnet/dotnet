#pragma warning disable CA1032 // Implement standard exception constructors
#pragma warning disable IDE0090 // Use 'new(...)'

#if XUNIT_NULLABLE
#nullable enable
#else
// In case this is source-imported with global nullable enabled but no XUNIT_NULLABLE
#pragma warning disable CS8604
#endif

using System;
using System.Globalization;

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
		/// when the given pointer value was unexpectedly not null.
		/// </summary>
		/// <param name="type">The inner type of the value</param>
		public static NullException ForNonNullPointer(Type type) =>
			new NullException(
				string.Format(
					CultureInfo.CurrentCulture,
					"Assert.Null() Failure: Value of type '{0}*' is not null",
					ArgumentFormatter.FormatTypeName(Assert.GuardArgumentNotNull(nameof(type), type))
				)
			);

		/// <summary>
		/// Creates a new instance of the <see cref="NullException"/> class to be thrown
		/// when the given nullable struct was unexpectedly not null.
		/// </summary>
		/// <param name="type">The inner type of the value</param>
		/// <param name="actual">The actual non-<see langword="null"/> value</param>
		public static Exception ForNonNullStruct<T>(
			Type type,
			T? actual)
				where T : struct =>
					new NullException(
						string.Format(
							CultureInfo.CurrentCulture,
							"Assert.Null() Failure: Value of type 'Nullable<{0}>' has a value{1}Expected: null{2}Actual:   {3}",
							ArgumentFormatter.FormatTypeName(Assert.GuardArgumentNotNull(nameof(type), type)),
							Environment.NewLine,
							Environment.NewLine,
							ArgumentFormatter.Format(actual)
						)
					);

		/// <summary>
		/// Creates a new instance of the <see cref="NullException"/> class to be thrown
		/// when the given value was unexpectedly not null.
		/// </summary>
		/// <param name="actual">The actual non-<see langword="null"/> value</param>
		public static NullException ForNonNullValue(object actual) =>
			new NullException(
				string.Format(
					CultureInfo.CurrentCulture,
					"Assert.Null() Failure: Value is not null{0}Expected: null{1}Actual:   {2}",
					Environment.NewLine,
					Environment.NewLine,
					ArgumentFormatter.Format(Assert.GuardArgumentNotNull(nameof(actual), actual))
				)
			);
	}
}
