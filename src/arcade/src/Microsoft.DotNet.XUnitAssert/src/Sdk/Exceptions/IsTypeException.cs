#pragma warning disable CA1032 // Implement standard exception constructors
#pragma warning disable IDE0090 // Use 'new(...)'

#if XUNIT_NULLABLE
#nullable enable
#endif

using System;
using System.Globalization;

namespace Xunit.Sdk
{
	/// <summary>
	/// Exception thrown when Assert.IsType fails.
	/// </summary>
#if XUNIT_VISIBILITY_INTERNAL
	internal
#else
	public
#endif
	partial class IsTypeException : XunitException
	{
		IsTypeException(string message) :
			base(message)
		{ }

		static IsTypeException Create(
			string expectedTypeName,
#if XUNIT_NULLABLE
			string? actualTypeName,
#else
			string actualTypeName,
#endif
			string compatibilityMessage) =>
				new IsTypeException(
					string.Format(
						CultureInfo.CurrentCulture,
						"Assert.IsType() Failure: Value is {0}{1}Expected: {2}{3}Actual:   {4}",
						actualTypeName == null ? "null" : compatibilityMessage,
						Environment.NewLine,
						Assert.GuardArgumentNotNull(nameof(expectedTypeName), expectedTypeName),
						Environment.NewLine,
						actualTypeName ?? "null"
					)
				);

		/// <summary>
		/// Creates a new instance of the <see cref="IsTypeException"/> class to be thrown
		/// when an object was not compatible with the given type
		/// </summary>
		/// <param name="expectedTypeName">The expected type name</param>
		/// <param name="actualTypeName">The actual type name</param>
		public static IsTypeException ForIncompatibleType(
			string expectedTypeName,
#if XUNIT_NULLABLE
			string? actualTypeName) =>
#else
			string actualTypeName) =>
#endif
				Create(expectedTypeName, actualTypeName, "an incompatible type");

		/// <summary>
		/// Creates a new instance of the <see cref="IsTypeException"/> class to be thrown
		/// when an object did not exactly match the given type
		/// </summary>
		/// <param name="expectedTypeName">The expected type name</param>
		/// <param name="actualTypeName">The actual type name</param>
		public static IsTypeException ForMismatchedType(
			string expectedTypeName,
#if XUNIT_NULLABLE
			string? actualTypeName) =>
#else
			string actualTypeName) =>
#endif
				Create(expectedTypeName, actualTypeName, "not the exact type");
	}
}
