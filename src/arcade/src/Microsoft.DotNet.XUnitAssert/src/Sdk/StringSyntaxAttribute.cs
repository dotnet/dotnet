#pragma warning disable IDE0301 // Simplify collection initialization

#if XUNIT_NULLABLE
#nullable enable
#endif

// Adapted from https://github.com/dotnet/runtime/blob/1e9c6a82aca4904828636b3638962c05a5f8d9c8/src/libraries/System.Private.CoreLib/src/System/Diagnostics/CodeAnalysis/StringSyntaxAttribute.cs
// to polyfill Visual Studio syntax coloring support for pre-.NET 7

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if !NET7_0_OR_GREATER

namespace System.Diagnostics.CodeAnalysis
{
	/// <summary>
	/// Specifies the syntax used in a string.
	/// </summary>
	[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
	internal sealed class StringSyntaxAttribute : Attribute
	{
		/// <summary>
		/// Initializes the <see cref="StringSyntaxAttribute"/> with the identifier of the syntax used.
		/// </summary>
		/// <param name="syntax">The syntax identifier.</param>
		public StringSyntaxAttribute(string syntax)
		{
			Syntax = syntax;
			Arguments = Array.Empty<object>();
		}

		/// <summary>
		/// Initializes the <see cref="StringSyntaxAttribute"/> with the identifier of the syntax used.
		/// </summary>
		/// <param name="syntax">The syntax identifier.</param>
		/// <param name="arguments">Optional arguments associated with the specific syntax employed.</param>
		public StringSyntaxAttribute(
			string syntax,
#if XUNIT_NULLABLE
			params object?[] arguments)
#else
			params object[] arguments)
#endif
		{
			Syntax = syntax;
			Arguments = arguments;
		}

		/// <summary>
		/// Gets the identifier of the syntax used.
		/// </summary>
		public string Syntax { get; }

		/// <summary>
		/// Optional arguments associated with the specific syntax employed.
		/// </summary>
#if XUNIT_NULLABLE
		public object?[] Arguments { get; }
#else
		public object[] Arguments { get; }
#endif

		/// <summary>
		/// The syntax identifier for strings containing composite formats for string formatting.
		/// </summary>
		public const string CompositeFormat = nameof(CompositeFormat);

		/// <summary>
		/// The syntax identifier for strings containing date format specifiers.
		/// </summary>
		public const string DateOnlyFormat = nameof(DateOnlyFormat);

		/// <summary>
		/// The syntax identifier for strings containing date and time format specifiers.
		/// </summary>
		public const string DateTimeFormat = nameof(DateTimeFormat);

		/// <summary>
		/// The syntax identifier for strings containing <see cref="Enum"/> format specifiers.
		/// </summary>
		public const string EnumFormat = nameof(EnumFormat);

		/// <summary>
		/// The syntax identifier for strings containing <see cref="Guid"/> format specifiers.
		/// </summary>
		public const string GuidFormat = nameof(GuidFormat);

		/// <summary>
		/// The syntax identifier for strings containing JavaScript Object Notation (JSON).
		/// </summary>
		public const string Json = nameof(Json);

		/// <summary>
		/// The syntax identifier for strings containing numeric format specifiers.
		/// </summary>
		public const string NumericFormat = nameof(NumericFormat);

		/// <summary>
		/// The syntax identifier for strings containing regular expressions.
		/// </summary>
		public const string Regex = nameof(Regex);

		/// <summary>
		/// The syntax identifier for strings containing time format specifiers.
		/// </summary>
		public const string TimeOnlyFormat = nameof(TimeOnlyFormat);

		/// <summary>
		/// The syntax identifier for strings containing <see cref="TimeSpan"/> format specifiers.
		/// </summary>
		public const string TimeSpanFormat = nameof(TimeSpanFormat);

		/// <summary>
		/// The syntax identifier for strings containing URIs.
		/// </summary>
		public const string Uri = nameof(Uri);

		/// <summary>
		/// The syntax identifier for strings containing XML.
		/// </summary>
		public const string Xml = nameof(Xml);
	}
}

#endif
