#pragma warning disable CA1032 // Implement standard exception constructors
#pragma warning disable IDE0040 // Add accessibility modifiers
#pragma warning disable IDE0058 // Expression value is never used
#pragma warning disable IDE0090 // Use 'new(...)'
#pragma warning disable IDE0161 // Convert to file-scoped namespace

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

		/// <summary>
		/// Creates a new instance of the <see cref="IsNotTypeException"/> class to be thrown
		/// when the object is the exact type.
		/// </summary>
		/// <param name="type">The expected type</param>
		public static IsNotTypeException ForExactType(Type type)
		{
			Assert.GuardArgumentNotNull(nameof(type), type);

			var formattedType = ArgumentFormatter.Format(type);

			return new IsNotTypeException(
				string.Format(
					CultureInfo.CurrentCulture,
					"Assert.IsNotType() Failure: Value is the exact type{0}Expected: {1}{2}Actual:   {3}",
					Environment.NewLine,
					formattedType,
					Environment.NewLine,
					formattedType
				)
			);
		}
	}
}
