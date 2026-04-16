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
	/// Exception thrown when Assert.IsNotType fails.
	/// </summary>
#if XUNIT_VISIBILITY_INTERNAL
	internal
#else
	public
#endif
	partial class IsNotTypeException : XunitException
	{
		IsNotTypeException(string message) :
			base(message)
		{ }

		static IsNotTypeException Create(
			Type expectedType,
			Type actualType,
			string compatiblityMessage)
		{
			Assert.GuardArgumentNotNull(nameof(expectedType), expectedType);
			Assert.GuardArgumentNotNull(nameof(actualType), actualType);

			return new IsNotTypeException(
				string.Format(
					CultureInfo.CurrentCulture,
					"Assert.IsNotType() Failure: Value is {0}{1}Expected: {2}{3}Actual:   {4}",
					compatiblityMessage,
					Environment.NewLine,
					ArgumentFormatter.Format(expectedType),
					Environment.NewLine,
					ArgumentFormatter.Format(actualType)
				)
			);
		}

		/// <summary>
		/// Creates a new instance of the <see cref="IsNotTypeException"/> class to be thrown
		/// when the object is a compatible type.
		/// </summary>
		/// <param name="expectedType">The expected type</param>
		/// <param name="actualType">The actual type</param>
		public static IsNotTypeException ForCompatibleType(
			Type expectedType,
			Type actualType) =>
				Create(expectedType, actualType, "a compatible type");

		/// <summary>
		/// Creates a new instance of the <see cref="IsNotTypeException"/> class to be thrown
		/// when the object is the exact type.
		/// </summary>
		/// <param name="type">The expected type</param>
		public static IsNotTypeException ForExactType(Type type) =>
			Create(type, type, "the exact type");
	}
}
