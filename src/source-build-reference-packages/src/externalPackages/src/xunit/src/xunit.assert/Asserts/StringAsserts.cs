#pragma warning disable CA1052 // Static holder types should be static
#pragma warning disable IDE0018 // Inline variable declaration
#pragma warning disable IDE0028 // Simplify collection initialization
#pragma warning disable IDE0040 // Add accessibility modifiers
#pragma warning disable IDE0058 // Expression value is never used
#pragma warning disable IDE0090 // Use 'new(...)'
#pragma warning disable IDE0161 // Convert to file-scoped namespace

#if XUNIT_NULLABLE
#nullable enable
#else
// In case this is source-imported with global nullable enabled but no XUNIT_NULLABLE
#pragma warning disable CS8604
#endif

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using Xunit.Internal;
using Xunit.Sdk;

namespace Xunit
{
#if XUNIT_VISIBILITY_INTERNAL
	internal
#else
	public
#endif
	partial class Assert
	{
		/// <summary>
		/// Verifies that a string contains a given sub-string, using the current culture.
		/// </summary>
		/// <param name="expectedSubstring">The sub-string expected to be in the string</param>
		/// <param name="actualString">The string to be inspected</param>
		/// <exception cref="ContainsException">Thrown when the sub-string is not present inside the string</exception>
		public static void Contains(
			string expectedSubstring,
#if XUNIT_NULLABLE
			string? actualString) =>
#else
			string actualString) =>
#endif
				Contains(expectedSubstring, actualString, StringComparison.CurrentCulture);

		/// <summary>
		/// Verifies that a string contains a given sub-string, using the given comparison type.
		/// </summary>
		/// <param name="expectedSubstring">The sub-string expected to be in the string</param>
		/// <param name="actualString">The string to be inspected</param>
		/// <param name="comparisonType">The type of string comparison to perform</param>
		/// <exception cref="ContainsException">Thrown when the sub-string is not present inside the string</exception>
		public static void Contains(
			string expectedSubstring,
#if XUNIT_NULLABLE
			string? actualString,
#else
			string actualString,
#endif
			StringComparison comparisonType)
		{
			GuardArgumentNotNull(nameof(expectedSubstring), expectedSubstring);

			if (actualString == null || actualString.IndexOf(expectedSubstring, comparisonType) < 0)
				throw ContainsException.ForSubStringNotFound(expectedSubstring, actualString);
		}

#if XUNIT_SPAN

		/// <summary>
		/// Verifies that a string contains a given sub-string, using the current culture.
		/// </summary>
		/// <param name="expectedSubstring">The sub-string expected to be in the string</param>
		/// <param name="actualString">The string to be inspected</param>
		/// <exception cref="ContainsException">Thrown when the sub-string is not present inside the string</exception>
		public static void Contains(
			Memory<char> expectedSubstring,
			Memory<char> actualString) =>
				Contains((ReadOnlyMemory<char>)expectedSubstring, (ReadOnlyMemory<char>)actualString, StringComparison.CurrentCulture);

		/// <summary>
		/// Verifies that a string contains a given sub-string, using the current culture.
		/// </summary>
		/// <param name="expectedSubstring">The sub-string expected to be in the string</param>
		/// <param name="actualString">The string to be inspected</param>
		/// <exception cref="ContainsException">Thrown when the sub-string is not present inside the string</exception>
		public static void Contains(
			Memory<char> expectedSubstring,
			ReadOnlyMemory<char> actualString) =>
				Contains((ReadOnlyMemory<char>)expectedSubstring, actualString, StringComparison.CurrentCulture);

		/// <summary>
		/// Verifies that a string contains a given sub-string, using the current culture.
		/// </summary>
		/// <param name="expectedSubstring">The sub-string expected to be in the string</param>
		/// <param name="actualString">The string to be inspected</param>
		/// <exception cref="ContainsException">Thrown when the sub-string is not present inside the string</exception>
		public static void Contains(
			ReadOnlyMemory<char> expectedSubstring,
			Memory<char> actualString) =>
				Contains(expectedSubstring, (ReadOnlyMemory<char>)actualString, StringComparison.CurrentCulture);

		/// <summary>
		/// Verifies that a string contains a given sub-string, using the current culture.
		/// </summary>
		/// <param name="expectedSubstring">The sub-string expected to be in the string</param>
		/// <param name="actualString">The string to be inspected</param>
		/// <exception cref="ContainsException">Thrown when the sub-string is not present inside the string</exception>
		public static void Contains(
			ReadOnlyMemory<char> expectedSubstring,
			ReadOnlyMemory<char> actualString) =>
				Contains(expectedSubstring, actualString, StringComparison.CurrentCulture);

		/// <summary>
		/// Verifies that a string contains a given sub-string, using the given comparison type.
		/// </summary>
		/// <param name="expectedSubstring">The sub-string expected to be in the string</param>
		/// <param name="actualString">The string to be inspected</param>
		/// <param name="comparisonType">The type of string comparison to perform</param>
		/// <exception cref="ContainsException">Thrown when the sub-string is not present inside the string</exception>
		public static void Contains(
			Memory<char> expectedSubstring,
			Memory<char> actualString,
			StringComparison comparisonType = StringComparison.CurrentCulture) =>
				Contains((ReadOnlyMemory<char>)expectedSubstring, (ReadOnlyMemory<char>)actualString, comparisonType);

		/// <summary>
		/// Verifies that a string contains a given sub-string, using the given comparison type.
		/// </summary>
		/// <param name="expectedSubstring">The sub-string expected to be in the string</param>
		/// <param name="actualString">The string to be inspected</param>
		/// <param name="comparisonType">The type of string comparison to perform</param>
		/// <exception cref="ContainsException">Thrown when the sub-string is not present inside the string</exception>
		public static void Contains(
			Memory<char> expectedSubstring,
			ReadOnlyMemory<char> actualString,
			StringComparison comparisonType = StringComparison.CurrentCulture) =>
				Contains((ReadOnlyMemory<char>)expectedSubstring, actualString, comparisonType);

		/// <summary>
		/// Verifies that a string contains a given sub-string, using the given comparison type.
		/// </summary>
		/// <param name="expectedSubstring">The sub-string expected to be in the string</param>
		/// <param name="actualString">The string to be inspected</param>
		/// <param name="comparisonType">The type of string comparison to perform</param>
		/// <exception cref="ContainsException">Thrown when the sub-string is not present inside the string</exception>
		public static void Contains(
			ReadOnlyMemory<char> expectedSubstring,
			Memory<char> actualString,
			StringComparison comparisonType = StringComparison.CurrentCulture) =>
				Contains(expectedSubstring, (ReadOnlyMemory<char>)actualString, comparisonType);

		/// <summary>
		/// Verifies that a string contains a given sub-string, using the given comparison type.
		/// </summary>
		/// <param name="expectedSubstring">The sub-string expected to be in the string</param>
		/// <param name="actualString">The string to be inspected</param>
		/// <param name="comparisonType">The type of string comparison to perform</param>
		/// <exception cref="ContainsException">Thrown when the sub-string is not present inside the string</exception>
		public static void Contains(
			ReadOnlyMemory<char> expectedSubstring,
			ReadOnlyMemory<char> actualString,
			StringComparison comparisonType = StringComparison.CurrentCulture)
		{
			GuardArgumentNotNull(nameof(expectedSubstring), expectedSubstring);

			Contains(expectedSubstring.Span, actualString.Span, comparisonType);
		}

		/// <summary>
		/// Verifies that a string contains a given string, using the given comparison type.
		/// </summary>
		/// <param name="expectedSubstring">The string expected to be in the string</param>
		/// <param name="actualString">The string to be inspected</param>
		/// <param name="comparisonType">The type of string comparison to perform</param>
		/// <exception cref="ContainsException">Thrown when the string is not present inside the string</exception>
		public static void Contains(
			Span<char> expectedSubstring,
			Span<char> actualString,
			StringComparison comparisonType = StringComparison.CurrentCulture) =>
				Contains((ReadOnlySpan<char>)expectedSubstring, (ReadOnlySpan<char>)actualString, comparisonType);

		/// <summary>
		/// Verifies that a string contains a given string, using the given comparison type.
		/// </summary>
		/// <param name="expectedSubstring">The string expected to be in the string</param>
		/// <param name="actualString">The string to be inspected</param>
		/// <param name="comparisonType">The type of string comparison to perform</param>
		/// <exception cref="ContainsException">Thrown when the string is not present inside the string</exception>
		public static void Contains(
			Span<char> expectedSubstring,
			ReadOnlySpan<char> actualString,
			StringComparison comparisonType = StringComparison.CurrentCulture) =>
				Contains((ReadOnlySpan<char>)expectedSubstring, actualString, comparisonType);

		/// <summary>
		/// Verifies that a string contains a given string, using the given comparison type.
		/// </summary>
		/// <param name="expectedSubstring">The string expected to be in the string</param>
		/// <param name="actualString">The string to be inspected</param>
		/// <param name="comparisonType">The type of string comparison to perform</param>
		/// <exception cref="ContainsException">Thrown when the string is not present inside the string</exception>
		public static void Contains(
			ReadOnlySpan<char> expectedSubstring,
			Span<char> actualString,
			StringComparison comparisonType = StringComparison.CurrentCulture) =>
				Contains(expectedSubstring, (ReadOnlySpan<char>)actualString, comparisonType);

		/// <summary>
		/// Verifies that a string contains a given string, using the given comparison type.
		/// </summary>
		/// <param name="expectedSubstring">The string expected to be in the string</param>
		/// <param name="actualString">The string to be inspected</param>
		/// <param name="comparisonType">The type of string comparison to perform</param>
		/// <exception cref="ContainsException">Thrown when the string is not present inside the string</exception>
		public static void Contains(
			ReadOnlySpan<char> expectedSubstring,
			ReadOnlySpan<char> actualString,
			StringComparison comparisonType = StringComparison.CurrentCulture)
		{
			if (actualString.IndexOf(expectedSubstring, comparisonType) < 0)
				throw ContainsException.ForSubStringNotFound(
					expectedSubstring.ToString(),
					actualString.ToString()
				);
		}

		/// <summary>
		/// Verifies that a string contains a given string, using the current culture.
		/// </summary>
		/// <param name="expectedSubstring">The string expected to be in the string</param>
		/// <param name="actualString">The string to be inspected</param>
		/// <exception cref="ContainsException">Thrown when the string is not present inside the string</exception>
		public static void Contains(
			Span<char> expectedSubstring,
			Span<char> actualString) =>
				Contains((ReadOnlySpan<char>)expectedSubstring, (ReadOnlySpan<char>)actualString, StringComparison.CurrentCulture);

		/// <summary>
		/// Verifies that a string contains a given string, using the current culture.
		/// </summary>
		/// <param name="expectedSubstring">The string expected to be in the string</param>
		/// <param name="actualString">The string to be inspected</param>
		/// <exception cref="ContainsException">Thrown when the string is not present inside the string</exception>
		public static void Contains(
			Span<char> expectedSubstring,
			ReadOnlySpan<char> actualString) =>
				Contains((ReadOnlySpan<char>)expectedSubstring, actualString, StringComparison.CurrentCulture);

		/// <summary>
		/// Verifies that a string contains a given string, using the current culture.
		/// </summary>
		/// <param name="expectedSubstring">The string expected to be in the string</param>
		/// <param name="actualString">The string to be inspected</param>
		/// <exception cref="ContainsException">Thrown when the string is not present inside the string</exception>
		public static void Contains(
			ReadOnlySpan<char> expectedSubstring,
			Span<char> actualString) =>
				Contains(expectedSubstring, (ReadOnlySpan<char>)actualString, StringComparison.CurrentCulture);

		/// <summary>
		/// Verifies that a string contains a given string, using the current culture.
		/// </summary>
		/// <param name="expectedSubstring">The string expected to be in the string</param>
		/// <param name="actualString">The string to be inspected</param>
		/// <exception cref="ContainsException">Thrown when the string is not present inside the string</exception>
		public static void Contains(
			ReadOnlySpan<char> expectedSubstring,
			ReadOnlySpan<char> actualString) =>
				Contains(expectedSubstring, actualString, StringComparison.CurrentCulture);

#endif

		/// <summary>
		/// Verifies that a string does not contain a given sub-string, using the current culture.
		/// </summary>
		/// <param name="expectedSubstring">The sub-string expected not to be in the string</param>
		/// <param name="actualString">The string to be inspected</param>
		/// <exception cref="DoesNotContainException">Thrown when the sub-string is present inside the string</exception>
		public static void DoesNotContain(
			string expectedSubstring,
#if XUNIT_NULLABLE
			string? actualString) =>
#else
			string actualString) =>
#endif
				DoesNotContain(expectedSubstring, actualString, StringComparison.CurrentCulture);

		/// <summary>
		/// Verifies that a string does not contain a given sub-string, using the current culture.
		/// </summary>
		/// <param name="expectedSubstring">The sub-string expected not to be in the string</param>
		/// <param name="actualString">The string to be inspected</param>
		/// <param name="comparisonType">The type of string comparison to perform</param>
		/// <exception cref="DoesNotContainException">Thrown when the sub-string is present inside the string</exception>
		public static void DoesNotContain(
			string expectedSubstring,
#if XUNIT_NULLABLE
			string? actualString,
#else
			string actualString,
#endif
			StringComparison comparisonType)
		{
			GuardArgumentNotNull(nameof(expectedSubstring), expectedSubstring);

			if (actualString != null)
			{
				var idx = actualString.IndexOf(expectedSubstring, comparisonType);
				if (idx >= 0)
					throw DoesNotContainException.ForSubStringFound(expectedSubstring, idx, actualString);
			}
		}

#if XUNIT_SPAN

		/// <summary>
		/// Verifies that a string does not contain a given sub-string, using the current culture.
		/// </summary>
		/// <param name="expectedSubstring">The sub-string expected not to be in the string</param>
		/// <param name="actualString">The string to be inspected</param>
		/// <exception cref="DoesNotContainException">Thrown when the sub-string is present inside the string</exception>
		public static void DoesNotContain(
			Memory<char> expectedSubstring,
			Memory<char> actualString) =>
				DoesNotContain((ReadOnlyMemory<char>)expectedSubstring, (ReadOnlyMemory<char>)actualString, StringComparison.CurrentCulture);

		/// <summary>
		/// Verifies that a string does not contain a given sub-string, using the current culture.
		/// </summary>
		/// <param name="expectedSubstring">The sub-string expected not to be in the string</param>
		/// <param name="actualString">The string to be inspected</param>
		/// <exception cref="DoesNotContainException">Thrown when the sub-string is present inside the string</exception>
		public static void DoesNotContain(
			Memory<char> expectedSubstring,
			ReadOnlyMemory<char> actualString) =>
				DoesNotContain((ReadOnlyMemory<char>)expectedSubstring, actualString, StringComparison.CurrentCulture);

		/// <summary>
		/// Verifies that a string does not contain a given sub-string, using the current culture.
		/// </summary>
		/// <param name="expectedSubstring">The sub-string expected not to be in the string</param>
		/// <param name="actualString">The string to be inspected</param>
		/// <exception cref="DoesNotContainException">Thrown when the sub-string is present inside the string</exception>
		public static void DoesNotContain(
			ReadOnlyMemory<char> expectedSubstring,
			Memory<char> actualString) =>
				DoesNotContain(expectedSubstring, (ReadOnlyMemory<char>)actualString, StringComparison.CurrentCulture);

		/// <summary>
		/// Verifies that a string does not contain a given sub-string, using the current culture.
		/// </summary>
		/// <param name="expectedSubstring">The sub-string expected not to be in the string</param>
		/// <param name="actualString">The string to be inspected</param>
		/// <exception cref="DoesNotContainException">Thrown when the sub-string is present inside the string</exception>
		public static void DoesNotContain(
			ReadOnlyMemory<char> expectedSubstring,
			ReadOnlyMemory<char> actualString) =>
				DoesNotContain(expectedSubstring, actualString, StringComparison.CurrentCulture);

		/// <summary>
		/// Verifies that a string does not contain a given sub-string, using the given comparison type.
		/// </summary>
		/// <param name="expectedSubstring">The sub-string expected not to be in the string</param>
		/// <param name="actualString">The string to be inspected</param>
		/// <param name="comparisonType">The type of string comparison to perform</param>
		/// <exception cref="DoesNotContainException">Thrown when the sub-string is present inside the string</exception>
		public static void DoesNotContain(
			Memory<char> expectedSubstring,
			Memory<char> actualString,
			StringComparison comparisonType = StringComparison.CurrentCulture) =>
				DoesNotContain((ReadOnlyMemory<char>)expectedSubstring, (ReadOnlyMemory<char>)actualString, comparisonType);

		/// <summary>
		/// Verifies that a string does not contain a given sub-string, using the given comparison type.
		/// </summary>
		/// <param name="expectedSubstring">The sub-string expected not to be in the string</param>
		/// <param name="actualString">The string to be inspected</param>
		/// <param name="comparisonType">The type of string comparison to perform</param>
		/// <exception cref="DoesNotContainException">Thrown when the sub-string is present inside the string</exception>
		public static void DoesNotContain(
			Memory<char> expectedSubstring,
			ReadOnlyMemory<char> actualString,
			StringComparison comparisonType = StringComparison.CurrentCulture) =>
				DoesNotContain((ReadOnlyMemory<char>)expectedSubstring, actualString, comparisonType);

		/// <summary>
		/// Verifies that a string does not contain a given sub-string, using the given comparison type.
		/// </summary>
		/// <param name="expectedSubstring">The sub-string expected not to be in the string</param>
		/// <param name="actualString">The string to be inspected</param>
		/// <param name="comparisonType">The type of string comparison to perform</param>
		/// <exception cref="DoesNotContainException">Thrown when the sub-string is present inside the string</exception>
		public static void DoesNotContain(
			ReadOnlyMemory<char> expectedSubstring,
			Memory<char> actualString,
			StringComparison comparisonType = StringComparison.CurrentCulture) =>
				DoesNotContain(expectedSubstring, (ReadOnlyMemory<char>)actualString, comparisonType);

		/// <summary>
		/// Verifies that a string does not contain a given sub-string, using the given comparison type.
		/// </summary>
		/// <param name="expectedSubstring">The sub-string expected not to be in the string</param>
		/// <param name="actualString">The string to be inspected</param>
		/// <param name="comparisonType">The type of string comparison to perform</param>
		/// <exception cref="DoesNotContainException">Thrown when the sub-string is present inside the string</exception>
		public static void DoesNotContain(
			ReadOnlyMemory<char> expectedSubstring,
			ReadOnlyMemory<char> actualString,
			StringComparison comparisonType = StringComparison.CurrentCulture)
		{
			GuardArgumentNotNull(nameof(expectedSubstring), expectedSubstring);

			DoesNotContain(expectedSubstring.Span, actualString.Span, comparisonType);
		}

		/// <summary>
		/// Verifies that a string does not contain a given sub-string, using the given comparison type.
		/// </summary>
		/// <param name="expectedSubstring">The sub-string expected not to be in the string</param>
		/// <param name="actualString">The string to be inspected</param>
		/// <param name="comparisonType">The type of string comparison to perform</param>
		/// <exception cref="DoesNotContainException">Thrown when the sub-string is present inside the string</exception>
		public static void DoesNotContain(
			Span<char> expectedSubstring,
			Span<char> actualString,
			StringComparison comparisonType = StringComparison.CurrentCulture) =>
				DoesNotContain((ReadOnlySpan<char>)expectedSubstring, (ReadOnlySpan<char>)actualString, comparisonType);

		/// <summary>
		/// Verifies that a string does not contain a given sub-string, using the given comparison type.
		/// </summary>
		/// <param name="expectedSubstring">The sub-string expected not to be in the string</param>
		/// <param name="actualString">The string to be inspected</param>
		/// <param name="comparisonType">The type of string comparison to perform</param>
		/// <exception cref="DoesNotContainException">Thrown when the sub-string is present inside the string</exception>
		public static void DoesNotContain(
			Span<char> expectedSubstring,
			ReadOnlySpan<char> actualString,
			StringComparison comparisonType = StringComparison.CurrentCulture) =>
				DoesNotContain((ReadOnlySpan<char>)expectedSubstring, actualString, comparisonType);

		/// <summary>
		/// Verifies that a string does not contain a given sub-string, using the given comparison type.
		/// </summary>
		/// <param name="expectedSubstring">The sub-string expected not to be in the string</param>
		/// <param name="actualString">The string to be inspected</param>
		/// <param name="comparisonType">The type of string comparison to perform</param>
		/// <exception cref="DoesNotContainException">Thrown when the sub-string is present inside the string</exception>
		public static void DoesNotContain(
			ReadOnlySpan<char> expectedSubstring,
			Span<char> actualString,
			StringComparison comparisonType = StringComparison.CurrentCulture) =>
				DoesNotContain(expectedSubstring, (ReadOnlySpan<char>)actualString, comparisonType);

		/// <summary>
		/// Verifies that a string does not contain a given sub-string, using the given comparison type.
		/// </summary>
		/// <param name="expectedSubstring">The sub-string expected not to be in the string</param>
		/// <param name="actualString">The string to be inspected</param>
		/// <param name="comparisonType">The type of string comparison to perform</param>
		/// <exception cref="DoesNotContainException">Thrown when the sub-string is present inside the string</exception>
		public static void DoesNotContain(
			ReadOnlySpan<char> expectedSubstring,
			ReadOnlySpan<char> actualString,
			StringComparison comparisonType = StringComparison.CurrentCulture)
		{
			var idx = actualString.IndexOf(expectedSubstring, comparisonType);
			if (idx > -1)
				throw DoesNotContainException.ForSubStringFound(expectedSubstring.ToString(), idx, actualString.ToString());
		}

		/// <summary>
		/// Verifies that a string does not contain a given sub-string, using the current culture.
		/// </summary>
		/// <param name="expectedSubstring">The sub-string expected not to be in the string</param>
		/// <param name="actualString">The string to be inspected</param>
		/// <exception cref="DoesNotContainException">Thrown when the sub-string is present inside the string</exception>
		public static void DoesNotContain(
			Span<char> expectedSubstring,
			Span<char> actualString) =>
				DoesNotContain((ReadOnlySpan<char>)expectedSubstring, (ReadOnlySpan<char>)actualString, StringComparison.CurrentCulture);

		/// <summary>
		/// Verifies that a string does not contain a given sub-string, using the current culture.
		/// </summary>
		/// <param name="expectedSubstring">The sub-string expected not to be in the string</param>
		/// <param name="actualString">The string to be inspected</param>
		/// <exception cref="DoesNotContainException">Thrown when the sub-string is present inside the string</exception>
		public static void DoesNotContain(
			Span<char> expectedSubstring,
			ReadOnlySpan<char> actualString) =>
				DoesNotContain((ReadOnlySpan<char>)expectedSubstring, actualString, StringComparison.CurrentCulture);

		/// <summary>
		/// Verifies that a string does not contain a given sub-string, using the current culture.
		/// </summary>
		/// <param name="expectedSubstring">The sub-string expected not to be in the string</param>
		/// <param name="actualString">The string to be inspected</param>
		/// <exception cref="DoesNotContainException">Thrown when the sub-string is present inside the string</exception>
		public static void DoesNotContain(
			ReadOnlySpan<char> expectedSubstring,
			Span<char> actualString) =>
				DoesNotContain(expectedSubstring, (ReadOnlySpan<char>)actualString, StringComparison.CurrentCulture);

		/// <summary>
		/// Verifies that a string does not contain a given sub-string, using the current culture.
		/// </summary>
		/// <param name="expectedSubstring">The sub-string expected not to be in the string</param>
		/// <param name="actualString">The string to be inspected</param>
		/// <exception cref="DoesNotContainException">Thrown when the sub-string is present inside the string</exception>
		public static void DoesNotContain(
			ReadOnlySpan<char> expectedSubstring,
			ReadOnlySpan<char> actualString) =>
				DoesNotContain(expectedSubstring, actualString, StringComparison.CurrentCulture);

#endif

		/// <summary>
		/// Verifies that a string does not match a regular expression.
		/// </summary>
		/// <param name="expectedRegexPattern">The regex pattern expected not to match</param>
		/// <param name="actualString">The string to be inspected</param>
		/// <exception cref="DoesNotMatchException">Thrown when the string matches the regex pattern</exception>
		public static void DoesNotMatch(
			[StringSyntax(StringSyntaxAttribute.Regex)]
			string expectedRegexPattern,
#if XUNIT_NULLABLE
			string? actualString)
#else
			string actualString)
#endif
		{
			GuardArgumentNotNull(nameof(expectedRegexPattern), expectedRegexPattern);

			if (actualString != null)
			{
				var match = Regex.Match(actualString, expectedRegexPattern);
				if (match.Success)
				{
					int pointerIndent;
					var formattedExpected = AssertHelper.ShortenAndEncodeString(expectedRegexPattern);
					var formattedActual = AssertHelper.ShortenAndEncodeString(actualString, match.Index, out pointerIndent);

					throw DoesNotMatchException.ForMatch(formattedExpected, match.Index, pointerIndent, formattedActual);
				}
			}
		}

		/// <summary>
		/// Verifies that a string does not match a regular expression.
		/// </summary>
		/// <param name="expectedRegex">The regex expected not to match</param>
		/// <param name="actualString">The string to be inspected</param>
		/// <exception cref="DoesNotMatchException">Thrown when the string matches the regex</exception>
		public static void DoesNotMatch(
			Regex expectedRegex,
#if XUNIT_NULLABLE
			string? actualString)
#else
			string actualString)
#endif
		{
			GuardArgumentNotNull(nameof(expectedRegex), expectedRegex);

			if (actualString != null)
			{
				var match = expectedRegex.Match(actualString);
				if (match.Success)
				{
					int pointerIndent;
					var formattedExpected = AssertHelper.ShortenAndEncodeString(expectedRegex.ToString());
					var formattedActual = AssertHelper.ShortenAndEncodeString(actualString, match.Index, out pointerIndent);

					throw DoesNotMatchException.ForMatch(formattedExpected, match.Index, pointerIndent, formattedActual);
				}
			}
		}

		/// <summary>
		/// Verifies that a string is empty.
		/// </summary>
		/// <param name="value">The string value to be inspected</param>
		/// <exception cref="ArgumentNullException">Thrown when the string is null</exception>
		/// <exception cref="EmptyException">Thrown when the string is not empty</exception>
		public static void Empty(string value)
		{
			GuardArgumentNotNull(nameof(value), value);

			if (value.Length != 0)
				throw EmptyException.ForNonEmptyString(value);
		}

		/// <summary>
		/// Verifies that a string ends with a given sub-string, using the current culture.
		/// </summary>
		/// <param name="expectedEndString">The sub-string expected to be at the end of the string</param>
		/// <param name="actualString">The string to be inspected</param>
		/// <exception cref="ContainsException">Thrown when the string does not end with the expected sub-string</exception>
		public static void EndsWith(
#if XUNIT_NULLABLE
			string? expectedEndString,
			string? actualString) =>
#else
			string expectedEndString,
			string actualString) =>
#endif
				EndsWith(expectedEndString, actualString, StringComparison.CurrentCulture);

		/// <summary>
		/// Verifies that a string ends with a given sub-string, using the given comparison type.
		/// </summary>
		/// <param name="expectedEndString">The sub-string expected to be at the end of the string</param>
		/// <param name="actualString">The string to be inspected</param>
		/// <param name="comparisonType">The type of string comparison to perform</param>
		/// <exception cref="ContainsException">Thrown when the string does not end with the expected sub-string</exception>
		public static void EndsWith(
#if XUNIT_NULLABLE
			string? expectedEndString,
			string? actualString,
#else
			string expectedEndString,
			string actualString,
#endif
			StringComparison comparisonType)
		{
			if (expectedEndString == null || actualString == null || !actualString.EndsWith(expectedEndString, comparisonType))
				throw EndsWithException.ForStringNotFound(expectedEndString, actualString);
		}

#if XUNIT_SPAN

		/// <summary>
		/// Verifies that a string ends with a given sub-string, using the current culture.
		/// </summary>
		/// <param name="expectedEndString">The sub-string expected to be at the end of the string</param>
		/// <param name="actualString">The string to be inspected</param>
		/// <exception cref="EndsWithException">Thrown when the string does not end with the expected sub-string</exception>
		public static void EndsWith(
			Memory<char> expectedEndString,
			Memory<char> actualString) =>
				EndsWith((ReadOnlyMemory<char>)expectedEndString, (ReadOnlyMemory<char>)actualString, StringComparison.CurrentCulture);

		/// <summary>
		/// Verifies that a string ends with a given sub-string, using the current culture.
		/// </summary>
		/// <param name="expectedEndString">The sub-string expected to be at the end of the string</param>
		/// <param name="actualString">The string to be inspected</param>
		/// <exception cref="EndsWithException">Thrown when the string does not end with the expected sub-string</exception>
		public static void EndsWith(
			Memory<char> expectedEndString,
			ReadOnlyMemory<char> actualString) =>
				EndsWith((ReadOnlyMemory<char>)expectedEndString, actualString, StringComparison.CurrentCulture);

		/// <summary>
		/// Verifies that a string ends with a given sub-string, using the current culture.
		/// </summary>
		/// <param name="expectedEndString">The sub-string expected to be at the end of the string</param>
		/// <param name="actualString">The string to be inspected</param>
		/// <exception cref="EndsWithException">Thrown when the string does not end with the expected sub-string</exception>
		public static void EndsWith(
			ReadOnlyMemory<char> expectedEndString,
			Memory<char> actualString) =>
				EndsWith(expectedEndString, (ReadOnlyMemory<char>)actualString, StringComparison.CurrentCulture);

		/// <summary>
		/// Verifies that a string ends with a given sub-string, using the current culture.
		/// </summary>
		/// <param name="expectedEndString">The sub-string expected to be at the end of the string</param>
		/// <param name="actualString">The string to be inspected</param>
		/// <exception cref="EndsWithException">Thrown when the string does not end with the expected sub-string</exception>
		public static void EndsWith(
			ReadOnlyMemory<char> expectedEndString,
			ReadOnlyMemory<char> actualString) =>
				EndsWith(expectedEndString, actualString, StringComparison.CurrentCulture);

		/// <summary>
		/// Verifies that a string ends with a given sub-string, using the given comparison type.
		/// </summary>
		/// <param name="expectedEndString">The sub-string expected to be at the end of the string</param>
		/// <param name="actualString">The string to be inspected</param>
		/// <param name="comparisonType">The type of string comparison to perform</param>
		/// <exception cref="EndsWithException">Thrown when the string does not end with the expected sub-string</exception>
		public static void EndsWith(
			Memory<char> expectedEndString,
			Memory<char> actualString,
			StringComparison comparisonType = StringComparison.CurrentCulture) =>
				EndsWith((ReadOnlyMemory<char>)expectedEndString, (ReadOnlyMemory<char>)actualString, comparisonType);

		/// <summary>
		/// Verifies that a string ends with a given sub-string, using the given comparison type.
		/// </summary>
		/// <param name="expectedEndString">The sub-string expected to be at the end of the string</param>
		/// <param name="actualString">The string to be inspected</param>
		/// <param name="comparisonType">The type of string comparison to perform</param>
		/// <exception cref="EndsWithException">Thrown when the string does not end with the expected sub-string</exception>
		public static void EndsWith(
			Memory<char> expectedEndString,
			ReadOnlyMemory<char> actualString,
			StringComparison comparisonType = StringComparison.CurrentCulture) =>
				EndsWith((ReadOnlyMemory<char>)expectedEndString, actualString, comparisonType);

		/// <summary>
		/// Verifies that a string ends with a given sub-string, using the given comparison type.
		/// </summary>
		/// <param name="expectedEndString">The sub-string expected to be at the end of the string</param>
		/// <param name="actualString">The string to be inspected</param>
		/// <param name="comparisonType">The type of string comparison to perform</param>
		/// <exception cref="EndsWithException">Thrown when the string does not end with the expected sub-string</exception>
		public static void EndsWith(
			ReadOnlyMemory<char> expectedEndString,
			Memory<char> actualString,
			StringComparison comparisonType = StringComparison.CurrentCulture) =>
				EndsWith(expectedEndString, (ReadOnlyMemory<char>)actualString, comparisonType);

		/// <summary>
		/// Verifies that a string ends with a given sub-string, using the given comparison type.
		/// </summary>
		/// <param name="expectedEndString">The sub-string expected to be at the end of the string</param>
		/// <param name="actualString">The string to be inspected</param>
		/// <param name="comparisonType">The type of string comparison to perform</param>
		/// <exception cref="EndsWithException">Thrown when the string does not end with the expected sub-string</exception>
		public static void EndsWith(
			ReadOnlyMemory<char> expectedEndString,
			ReadOnlyMemory<char> actualString,
			StringComparison comparisonType = StringComparison.CurrentCulture)
		{
			GuardArgumentNotNull(nameof(expectedEndString), expectedEndString);

			EndsWith(expectedEndString.Span, actualString.Span, comparisonType);
		}

		/// <summary>
		/// Verifies that a string ends with a given sub-string, using the current culture.
		/// </summary>
		/// <param name="expectedEndString">The sub-string expected to be at the end of the string</param>
		/// <param name="actualString">The string to be inspected</param>
		/// <exception cref="EndsWithException">Thrown when the string does not end with the expected sub-string</exception>
		public static void EndsWith(
			Span<char> expectedEndString,
			Span<char> actualString) =>
				EndsWith((ReadOnlySpan<char>)expectedEndString, (ReadOnlySpan<char>)actualString, StringComparison.CurrentCulture);

		/// <summary>
		/// Verifies that a string ends with a given sub-string, using the current culture.
		/// </summary>
		/// <param name="expectedEndString">The sub-string expected to be at the end of the string</param>
		/// <param name="actualString">The string to be inspected</param>
		/// <exception cref="EndsWithException">Thrown when the string does not end with the expected sub-string</exception>
		public static void EndsWith(
			Span<char> expectedEndString,
			ReadOnlySpan<char> actualString) =>
				EndsWith((ReadOnlySpan<char>)expectedEndString, actualString, StringComparison.CurrentCulture);

		/// <summary>
		/// Verifies that a string ends with a given sub-string, using the current culture.
		/// </summary>
		/// <param name="expectedEndString">The sub-string expected to be at the end of the string</param>
		/// <param name="actualString">The string to be inspected</param>
		/// <exception cref="EndsWithException">Thrown when the string does not end with the expected sub-string</exception>
		public static void EndsWith(
			ReadOnlySpan<char> expectedEndString,
			Span<char> actualString) =>
				EndsWith(expectedEndString, (ReadOnlySpan<char>)actualString, StringComparison.CurrentCulture);

		/// <summary>
		/// Verifies that a string ends with a given sub-string, using the current culture.
		/// </summary>
		/// <param name="expectedEndString">The sub-string expected to be at the end of the string</param>
		/// <param name="actualString">The string to be inspected</param>
		/// <exception cref="EndsWithException">Thrown when the string does not end with the expected sub-string</exception>
		public static void EndsWith(
			ReadOnlySpan<char> expectedEndString,
			ReadOnlySpan<char> actualString) =>
				EndsWith(expectedEndString, actualString, StringComparison.CurrentCulture);

		/// <summary>
		/// Verifies that a string ends with a given sub-string, using the given comparison type.
		/// </summary>
		/// <param name="expectedEndString">The sub-string expected to be at the end of the string</param>
		/// <param name="actualString">The string to be inspected</param>
		/// <param name="comparisonType">The type of string comparison to perform</param>
		/// <exception cref="EndsWithException">Thrown when the string does not end with the expected sub-string</exception>
		public static void EndsWith(
			Span<char> expectedEndString,
			Span<char> actualString,
			StringComparison comparisonType = StringComparison.CurrentCulture) =>
				EndsWith((ReadOnlySpan<char>)expectedEndString, (ReadOnlySpan<char>)actualString, comparisonType);

		/// <summary>
		/// Verifies that a string ends with a given sub-string, using the given comparison type.
		/// </summary>
		/// <param name="expectedEndString">The sub-string expected to be at the end of the string</param>
		/// <param name="actualString">The string to be inspected</param>
		/// <param name="comparisonType">The type of string comparison to perform</param>
		/// <exception cref="EndsWithException">Thrown when the string does not end with the expected sub-string</exception>
		public static void EndsWith(
			Span<char> expectedEndString,
			ReadOnlySpan<char> actualString,
			StringComparison comparisonType = StringComparison.CurrentCulture) =>
				EndsWith((ReadOnlySpan<char>)expectedEndString, actualString, comparisonType);

		/// <summary>
		/// Verifies that a string ends with a given sub-string, using the given comparison type.
		/// </summary>
		/// <param name="expectedEndString">The sub-string expected to be at the end of the string</param>
		/// <param name="actualString">The string to be inspected</param>
		/// <param name="comparisonType">The type of string comparison to perform</param>
		/// <exception cref="EndsWithException">Thrown when the string does not end with the expected sub-string</exception>
		public static void EndsWith(
			ReadOnlySpan<char> expectedEndString,
			Span<char> actualString,
			StringComparison comparisonType = StringComparison.CurrentCulture) =>
				EndsWith(expectedEndString, (ReadOnlySpan<char>)actualString, comparisonType);

		/// <summary>
		/// Verifies that a string ends with a given sub-string, using the given comparison type.
		/// </summary>
		/// <param name="expectedEndString">The sub-string expected to be at the end of the string</param>
		/// <param name="actualString">The string to be inspected</param>
		/// <param name="comparisonType">The type of string comparison to perform</param>
		/// <exception cref="EndsWithException">Thrown when the string does not end with the expected sub-string</exception>
		public static void EndsWith(
			ReadOnlySpan<char> expectedEndString,
			ReadOnlySpan<char> actualString,
			StringComparison comparisonType = StringComparison.CurrentCulture)
		{
			if (!actualString.EndsWith(expectedEndString, comparisonType))
				throw EndsWithException.ForStringNotFound(expectedEndString.ToString(), actualString.ToString());
		}

#endif

		/// <summary>
		/// Verifies that two strings are equivalent.
		/// </summary>
		/// <param name="expected">The expected string value.</param>
		/// <param name="actual">The actual string value.</param>
		/// <exception cref="EqualException">Thrown when the strings are not equivalent.</exception>
		public static void Equal(
#if XUNIT_NULLABLE
			string? expected,
			string? actual) =>
#else
			string expected,
			string actual) =>
#endif
				Equal(expected, actual, false, false, false, false);

		/// <summary>
		/// Verifies that two strings are equivalent.
		/// </summary>
		/// <param name="expected">The expected string value.</param>
		/// <param name="actual">The actual string value.</param>
		/// <param name="ignoreCase">If set to <c>true</c>, ignores cases differences. The invariant culture is used.</param>
		/// <param name="ignoreLineEndingDifferences">If set to <c>true</c>, treats \r\n, \r, and \n as equivalent.</param>
		/// <param name="ignoreWhiteSpaceDifferences">If set to <c>true</c>, treats horizontal white-space (i.e. spaces, tabs, and others; see remarks) in any non-zero quantity as equivalent.</param>
		/// <param name="ignoreAllWhiteSpace">If set to <c>true</c>, treats horizontal white-space (i.e. spaces, tabs, and others; see remarks), including zero quantities, as equivalent.</param>
		/// <exception cref="EqualException">Thrown when the strings are not equivalent.</exception>
		/// <remarks>
		/// The <paramref name="ignoreWhiteSpaceDifferences"/> and <paramref name="ignoreAllWhiteSpace"/> flags consider
		/// the following characters to be white-space:
		/// <see href="https://unicode-explorer.com/c/0009">Tab</see> (\t),
		/// <see href="https://unicode-explorer.com/c/0020">Space</see> (\u0020),
		/// <see href="https://unicode-explorer.com/c/00A0">No-Break Space</see> (\u00A0),
		/// <see href="https://unicode-explorer.com/c/1680">Ogham Space Mark</see> (\u1680),
		/// <see href="https://unicode-explorer.com/c/180E">Mongolian Vowel Separator</see> (\u180E),
		/// <see href="https://unicode-explorer.com/c/2000">En Quad</see> (\u2000),
		/// <see href="https://unicode-explorer.com/c/2001">Em Quad</see> (\u2001),
		/// <see href="https://unicode-explorer.com/c/2002">En Space</see> (\u2002),
		/// <see href="https://unicode-explorer.com/c/2003">Em Space</see> (\u2003),
		/// <see href="https://unicode-explorer.com/c/2004">Three-Per-Em Space</see> (\u2004),
		/// <see href="https://unicode-explorer.com/c/2005">Four-Per-Em Space</see> (\u2004),
		/// <see href="https://unicode-explorer.com/c/2006">Six-Per-Em Space</see> (\u2006),
		/// <see href="https://unicode-explorer.com/c/2007">Figure Space</see> (\u2007),
		/// <see href="https://unicode-explorer.com/c/2008">Punctuation Space</see> (\u2008),
		/// <see href="https://unicode-explorer.com/c/2009">Thin Space</see> (\u2009),
		/// <see href="https://unicode-explorer.com/c/200A">Hair Space</see> (\u200A),
		/// <see href="https://unicode-explorer.com/c/200B">Zero Width Space</see> (\u200B),
		/// <see href="https://unicode-explorer.com/c/202F">Narrow No-Break Space</see> (\u202F),
		/// <see href="https://unicode-explorer.com/c/205F">Medium Mathematical Space</see> (\u205F),
		/// <see href="https://unicode-explorer.com/c/3000">Ideographic Space</see> (\u3000),
		/// and <see href="https://unicode-explorer.com/c/FEFF">Zero Width No-Break Space</see> (\uFEFF).
		/// In particular, it does not include carriage return (\r) or line feed (\n), which are covered by
		/// <paramref name="ignoreLineEndingDifferences"/>.
		/// </remarks>

		public static void Equal(
#if XUNIT_SPAN
			ReadOnlySpan<char> expected,
			ReadOnlySpan<char> actual,
#elif XUNIT_NULLABLE
			string? expected,
			string? actual,
#else
			string expected,
			string actual,
#endif
			bool ignoreCase = false,
			bool ignoreLineEndingDifferences = false,
			bool ignoreWhiteSpaceDifferences = false,
			bool ignoreAllWhiteSpace = false)
		{
#if !XUNIT_SPAN
			if (expected == null && actual == null)
				return;
			if (expected == null || actual == null)
				throw EqualException.ForMismatchedStrings(expected, actual, -1, -1);
#endif

			// Walk the string, keeping separate indices since we can skip variable amounts of
			// data based on ignoreLineEndingDifferences and ignoreWhiteSpaceDifferences.
			var expectedIndex = 0;
			var actualIndex = 0;
			var expectedLength = expected.Length;
			var actualLength = actual.Length;

			// Block used to fix edge case of Equal("", " ") when ignoreAllWhiteSpace enabled.
			if (ignoreAllWhiteSpace)
			{
				if (expectedLength == 0 && SkipWhitespace(actual, 0) == actualLength)
					return;
				if (actualLength == 0 && SkipWhitespace(expected, 0) == expectedLength)
					return;
			}

			while (expectedIndex < expectedLength && actualIndex < actualLength)
			{
				var expectedChar = expected[expectedIndex];
				var actualChar = actual[actualIndex];

				if (ignoreLineEndingDifferences && charsLineEndings.Contains(expectedChar) && charsLineEndings.Contains(actualChar))
				{
					expectedIndex = SkipLineEnding(expected, expectedIndex);
					actualIndex = SkipLineEnding(actual, actualIndex);
				}
				else if (ignoreAllWhiteSpace && (charsWhitespace.Contains(expectedChar) || charsWhitespace.Contains(actualChar)))
				{
					expectedIndex = SkipWhitespace(expected, expectedIndex);
					actualIndex = SkipWhitespace(actual, actualIndex);
				}
				else if (ignoreWhiteSpaceDifferences && charsWhitespace.Contains(expectedChar) && charsWhitespace.Contains(actualChar))
				{
					expectedIndex = SkipWhitespace(expected, expectedIndex);
					actualIndex = SkipWhitespace(actual, actualIndex);
				}
				else
				{
					if (ignoreCase)
					{
						expectedChar = char.ToUpperInvariant(expectedChar);
						actualChar = char.ToUpperInvariant(actualChar);
					}

					if (expectedChar != actualChar)
						break;

					expectedIndex++;
					actualIndex++;
				}
			}

			if (expectedIndex < expectedLength || actualIndex < actualLength)
				throw EqualException.ForMismatchedStrings(expected.ToString(), actual.ToString(), expectedIndex, actualIndex);
		}

#if XUNIT_SPAN

		/// <summary>
		/// Verifies that two strings are equivalent.
		/// </summary>
		/// <param name="expected">The expected string value.</param>
		/// <param name="actual">The actual string value.</param>
		/// <exception cref="EqualException">Thrown when the strings are not equivalent.</exception>
		public static void Equal(
			Memory<char> expected,
			Memory<char> actual) =>
				Equal((ReadOnlyMemory<char>)expected, (ReadOnlyMemory<char>)actual, false, false, false, false);

		/// <summary>
		/// Verifies that two strings are equivalent.
		/// </summary>
		/// <param name="expected">The expected string value.</param>
		/// <param name="actual">The actual string value.</param>
		/// <exception cref="EqualException">Thrown when the strings are not equivalent.</exception>
		public static void Equal(
			Memory<char> expected,
			ReadOnlyMemory<char> actual) =>
				Equal((ReadOnlyMemory<char>)expected, actual, false, false, false, false);

		/// <summary>
		/// Verifies that two strings are equivalent.
		/// </summary>
		/// <param name="expected">The expected string value.</param>
		/// <param name="actual">The actual string value.</param>
		/// <exception cref="EqualException">Thrown when the strings are not equivalent.</exception>
		public static void Equal(
			ReadOnlyMemory<char> expected,
			Memory<char> actual) =>
				Equal(expected, (ReadOnlyMemory<char>)actual, false, false, false, false);

		/// <summary>
		/// Verifies that two strings are equivalent.
		/// </summary>
		/// <param name="expected">The expected string value.</param>
		/// <param name="actual">The actual string value.</param>
		/// <exception cref="EqualException">Thrown when the strings are not equivalent.</exception>
		public static void Equal(
			ReadOnlyMemory<char> expected,
			ReadOnlyMemory<char> actual) =>
				Equal(expected, actual, false, false, false, false);

		/// <summary>
		/// Verifies that two strings are equivalent.
		/// </summary>
		/// <param name="expected">The expected string value.</param>
		/// <param name="actual">The actual string value.</param>
		/// <param name="ignoreCase">If set to <c>true</c>, ignores cases differences. The invariant culture is used.</param>
		/// <param name="ignoreLineEndingDifferences">If set to <c>true</c>, treats \r\n, \r, and \n as equivalent.</param>
		/// <param name="ignoreWhiteSpaceDifferences">If set to <c>true</c>, treats horizontal white-space (i.e. spaces, tabs, and others; see remarks) in any non-zero quantity as equivalent.</param>
		/// <param name="ignoreAllWhiteSpace">If set to <c>true</c>, treats horizontal white-space (i.e. spaces, tabs, and others; see remarks), including zero quantities, as equivalent.</param>
		/// <exception cref="EqualException">Thrown when the strings are not equivalent.</exception>
		public static void Equal(
			Memory<char> expected,
			Memory<char> actual,
			bool ignoreCase = false,
			bool ignoreLineEndingDifferences = false,
			bool ignoreWhiteSpaceDifferences = false,
			bool ignoreAllWhiteSpace = false) =>
				Equal((ReadOnlyMemory<char>)expected, (ReadOnlyMemory<char>)actual, ignoreCase, ignoreLineEndingDifferences, ignoreWhiteSpaceDifferences, ignoreAllWhiteSpace);

		/// <summary>
		/// Verifies that two strings are equivalent.
		/// </summary>
		/// <param name="expected">The expected string value.</param>
		/// <param name="actual">The actual string value.</param>
		/// <param name="ignoreCase">If set to <c>true</c>, ignores cases differences. The invariant culture is used.</param>
		/// <param name="ignoreLineEndingDifferences">If set to <c>true</c>, treats \r\n, \r, and \n as equivalent.</param>
		/// <param name="ignoreWhiteSpaceDifferences">If set to <c>true</c>, treats horizontal white-space (i.e. spaces, tabs, and others; see remarks) in any non-zero quantity as equivalent.</param>
		/// <param name="ignoreAllWhiteSpace">If set to <c>true</c>, treats horizontal white-space (i.e. spaces, tabs, and others; see remarks), including zero quantities, as equivalent.</param>
		/// <exception cref="EqualException">Thrown when the strings are not equivalent.</exception>
		public static void Equal(
			Memory<char> expected,
			ReadOnlyMemory<char> actual,
			bool ignoreCase = false,
			bool ignoreLineEndingDifferences = false,
			bool ignoreWhiteSpaceDifferences = false,
			bool ignoreAllWhiteSpace = false) =>
				Equal((ReadOnlyMemory<char>)expected, actual, ignoreCase, ignoreLineEndingDifferences, ignoreWhiteSpaceDifferences, ignoreAllWhiteSpace);

		/// <summary>
		/// Verifies that two strings are equivalent.
		/// </summary>
		/// <param name="expected">The expected string value.</param>
		/// <param name="actual">The actual string value.</param>
		/// <param name="ignoreCase">If set to <c>true</c>, ignores cases differences. The invariant culture is used.</param>
		/// <param name="ignoreLineEndingDifferences">If set to <c>true</c>, treats \r\n, \r, and \n as equivalent.</param>
		/// <param name="ignoreWhiteSpaceDifferences">If set to <c>true</c>, treats horizontal white-space (i.e. spaces, tabs, and others; see remarks) in any non-zero quantity as equivalent.</param>
		/// <param name="ignoreAllWhiteSpace">If set to <c>true</c>, treats horizontal white-space (i.e. spaces, tabs, and others; see remarks), including zero quantities, as equivalent.</param>
		/// <exception cref="EqualException">Thrown when the strings are not equivalent.</exception>
		public static void Equal(
			ReadOnlyMemory<char> expected,
			Memory<char> actual,
			bool ignoreCase = false,
			bool ignoreLineEndingDifferences = false,
			bool ignoreWhiteSpaceDifferences = false,
			bool ignoreAllWhiteSpace = false) =>
				Equal(expected, (ReadOnlyMemory<char>)actual, ignoreCase, ignoreLineEndingDifferences, ignoreWhiteSpaceDifferences, ignoreAllWhiteSpace);

		/// <summary>
		/// Verifies that two strings are equivalent.
		/// </summary>
		/// <param name="expected">The expected string value.</param>
		/// <param name="actual">The actual string value.</param>
		/// <param name="ignoreCase">If set to <c>true</c>, ignores cases differences. The invariant culture is used.</param>
		/// <param name="ignoreLineEndingDifferences">If set to <c>true</c>, treats \r\n, \r, and \n as equivalent.</param>
		/// <param name="ignoreWhiteSpaceDifferences">If set to <c>true</c>, treats horizontal white-space (i.e. spaces, tabs, and others; see remarks) in any non-zero quantity as equivalent.</param>
		/// <param name="ignoreAllWhiteSpace">If set to <c>true</c>, treats horizontal white-space (i.e. spaces, tabs, and others; see remarks), including zero quantities, as equivalent.</param>
		/// <exception cref="EqualException">Thrown when the strings are not equivalent.</exception>
		public static void Equal(
			ReadOnlyMemory<char> expected,
			ReadOnlyMemory<char> actual,
			bool ignoreCase = false,
			bool ignoreLineEndingDifferences = false,
			bool ignoreWhiteSpaceDifferences = false,
			bool ignoreAllWhiteSpace = false)
		{
			GuardArgumentNotNull(nameof(expected), expected);

			Equal(
				expected.Span,
				actual.Span,
				ignoreCase,
				ignoreLineEndingDifferences,
				ignoreWhiteSpaceDifferences,
				ignoreAllWhiteSpace
			);
		}

		/// <summary>
		/// Verifies that two strings are equivalent.
		/// </summary>
		/// <param name="expected">The expected string value.</param>
		/// <param name="actual">The actual string value.</param>
		/// <exception cref="EqualException">Thrown when the strings are not equivalent.</exception>
		public static void Equal(
			Span<char> expected,
			Span<char> actual) =>
				Equal((ReadOnlySpan<char>)expected, (ReadOnlySpan<char>)actual, false, false, false, false);

		/// <summary>
		/// Verifies that two strings are equivalent.
		/// </summary>
		/// <param name="expected">The expected string value.</param>
		/// <param name="actual">The actual string value.</param>
		/// <exception cref="EqualException">Thrown when the strings are not equivalent.</exception>
		public static void Equal(
			Span<char> expected,
			ReadOnlySpan<char> actual) =>
				Equal((ReadOnlySpan<char>)expected, actual, false, false, false, false);

		/// <summary>
		/// Verifies that two strings are equivalent.
		/// </summary>
		/// <param name="expected">The expected string value.</param>
		/// <param name="actual">The actual string value.</param>
		/// <exception cref="EqualException">Thrown when the strings are not equivalent.</exception>
		public static void Equal(
			ReadOnlySpan<char> expected,
			Span<char> actual) =>
				Equal(expected, (ReadOnlySpan<char>)actual, false, false, false, false);

		/// <summary>
		/// Verifies that two strings are equivalent.
		/// </summary>
		/// <param name="expected">The expected string value.</param>
		/// <param name="actual">The actual string value.</param>
		/// <exception cref="EqualException">Thrown when the strings are not equivalent.</exception>
		public static void Equal(
			ReadOnlySpan<char> expected,
			ReadOnlySpan<char> actual) =>
				Equal(expected, actual, false, false, false, false);

		/// <summary>
		/// Verifies that two strings are equivalent.
		/// </summary>
		/// <param name="expected">The expected string value.</param>
		/// <param name="actual">The actual string value.</param>
		/// <param name="ignoreCase">If set to <c>true</c>, ignores cases differences. The invariant culture is used.</param>
		/// <param name="ignoreLineEndingDifferences">If set to <c>true</c>, treats \r\n, \r, and \n as equivalent.</param>
		/// <param name="ignoreWhiteSpaceDifferences">If set to <c>true</c>, treats spaces and tabs (in any non-zero quantity) as equivalent.</param>
		/// <param name="ignoreAllWhiteSpace">If set to <c>true</c>, ignores all white space differences during comparison.</param>
		/// <exception cref="EqualException">Thrown when the strings are not equivalent.</exception>
		/// <remarks>
		/// The <paramref name="ignoreWhiteSpaceDifferences"/> and <paramref name="ignoreAllWhiteSpace"/> flags consider
		/// the following characters to be white-space:
		/// <see href="https://unicode-explorer.com/c/0009">Tab</see> (\t),
		/// <see href="https://unicode-explorer.com/c/0020">Space</see> (\u0020),
		/// <see href="https://unicode-explorer.com/c/00A0">No-Break Space</see> (\u00A0),
		/// <see href="https://unicode-explorer.com/c/1680">Ogham Space Mark</see> (\u1680),
		/// <see href="https://unicode-explorer.com/c/180E">Mongolian Vowel Separator</see> (\u180E),
		/// <see href="https://unicode-explorer.com/c/2000">En Quad</see> (\u2000),
		/// <see href="https://unicode-explorer.com/c/2001">Em Quad</see> (\u2001),
		/// <see href="https://unicode-explorer.com/c/2002">En Space</see> (\u2002),
		/// <see href="https://unicode-explorer.com/c/2003">Em Space</see> (\u2003),
		/// <see href="https://unicode-explorer.com/c/2004">Three-Per-Em Space</see> (\u2004),
		/// <see href="https://unicode-explorer.com/c/2005">Four-Per-Em Space</see> (\u2004),
		/// <see href="https://unicode-explorer.com/c/2006">Six-Per-Em Space</see> (\u2006),
		/// <see href="https://unicode-explorer.com/c/2007">Figure Space</see> (\u2007),
		/// <see href="https://unicode-explorer.com/c/2008">Punctuation Space</see> (\u2008),
		/// <see href="https://unicode-explorer.com/c/2009">Thin Space</see> (\u2009),
		/// <see href="https://unicode-explorer.com/c/200A">Hair Space</see> (\u200A),
		/// <see href="https://unicode-explorer.com/c/200B">Zero Width Space</see> (\u200B),
		/// <see href="https://unicode-explorer.com/c/202F">Narrow No-Break Space</see> (\u202F),
		/// <see href="https://unicode-explorer.com/c/205F">Medium Mathematical Space</see> (\u205F),
		/// <see href="https://unicode-explorer.com/c/3000">Ideographic Space</see> (\u3000),
		/// and <see href="https://unicode-explorer.com/c/FEFF">Zero Width No-Break Space</see> (\uFEFF).
		/// In particular, it does not include carriage return (\r) or line feed (\n), which are covered by
		/// <paramref name="ignoreLineEndingDifferences"/>.
		/// </remarks>
		public static void Equal(
			Span<char> expected,
			Span<char> actual,
			bool ignoreCase = false,
			bool ignoreLineEndingDifferences = false,
			bool ignoreWhiteSpaceDifferences = false,
			bool ignoreAllWhiteSpace = false) =>
				Equal((ReadOnlySpan<char>)expected, (ReadOnlySpan<char>)actual, ignoreCase, ignoreLineEndingDifferences, ignoreWhiteSpaceDifferences, ignoreAllWhiteSpace);

		/// <summary>
		/// Verifies that two strings are equivalent.
		/// </summary>
		/// <param name="expected">The expected string value.</param>
		/// <param name="actual">The actual string value.</param>
		/// <param name="ignoreCase">If set to <c>true</c>, ignores cases differences. The invariant culture is used.</param>
		/// <param name="ignoreLineEndingDifferences">If set to <c>true</c>, treats \r\n, \r, and \n as equivalent.</param>
		/// <param name="ignoreWhiteSpaceDifferences">If set to <c>true</c>, treats spaces and tabs (in any non-zero quantity) as equivalent.</param>
		/// <param name="ignoreAllWhiteSpace">If set to <c>true</c>, ignores all white space differences during comparison.</param>
		/// <exception cref="EqualException">Thrown when the strings are not equivalent.</exception>
		/// <remarks>
		/// The <paramref name="ignoreWhiteSpaceDifferences"/> and <paramref name="ignoreAllWhiteSpace"/> flags consider
		/// the following characters to be white-space:
		/// <see href="https://unicode-explorer.com/c/0009">Tab</see> (\t),
		/// <see href="https://unicode-explorer.com/c/0020">Space</see> (\u0020),
		/// <see href="https://unicode-explorer.com/c/00A0">No-Break Space</see> (\u00A0),
		/// <see href="https://unicode-explorer.com/c/1680">Ogham Space Mark</see> (\u1680),
		/// <see href="https://unicode-explorer.com/c/180E">Mongolian Vowel Separator</see> (\u180E),
		/// <see href="https://unicode-explorer.com/c/2000">En Quad</see> (\u2000),
		/// <see href="https://unicode-explorer.com/c/2001">Em Quad</see> (\u2001),
		/// <see href="https://unicode-explorer.com/c/2002">En Space</see> (\u2002),
		/// <see href="https://unicode-explorer.com/c/2003">Em Space</see> (\u2003),
		/// <see href="https://unicode-explorer.com/c/2004">Three-Per-Em Space</see> (\u2004),
		/// <see href="https://unicode-explorer.com/c/2005">Four-Per-Em Space</see> (\u2004),
		/// <see href="https://unicode-explorer.com/c/2006">Six-Per-Em Space</see> (\u2006),
		/// <see href="https://unicode-explorer.com/c/2007">Figure Space</see> (\u2007),
		/// <see href="https://unicode-explorer.com/c/2008">Punctuation Space</see> (\u2008),
		/// <see href="https://unicode-explorer.com/c/2009">Thin Space</see> (\u2009),
		/// <see href="https://unicode-explorer.com/c/200A">Hair Space</see> (\u200A),
		/// <see href="https://unicode-explorer.com/c/200B">Zero Width Space</see> (\u200B),
		/// <see href="https://unicode-explorer.com/c/202F">Narrow No-Break Space</see> (\u202F),
		/// <see href="https://unicode-explorer.com/c/205F">Medium Mathematical Space</see> (\u205F),
		/// <see href="https://unicode-explorer.com/c/3000">Ideographic Space</see> (\u3000),
		/// and <see href="https://unicode-explorer.com/c/FEFF">Zero Width No-Break Space</see> (\uFEFF).
		/// In particular, it does not include carriage return (\r) or line feed (\n), which are covered by
		/// <paramref name="ignoreLineEndingDifferences"/>.
		/// </remarks>
		public static void Equal(
			Span<char> expected,
			ReadOnlySpan<char> actual,
			bool ignoreCase = false,
			bool ignoreLineEndingDifferences = false,
			bool ignoreWhiteSpaceDifferences = false,
			bool ignoreAllWhiteSpace = false) =>
				Equal((ReadOnlySpan<char>)expected, actual, ignoreCase, ignoreLineEndingDifferences, ignoreWhiteSpaceDifferences, ignoreAllWhiteSpace);

		/// <summary>
		/// Verifies that two strings are equivalent.
		/// </summary>
		/// <param name="expected">The expected string value.</param>
		/// <param name="actual">The actual string value.</param>
		/// <param name="ignoreCase">If set to <c>true</c>, ignores cases differences. The invariant culture is used.</param>
		/// <param name="ignoreLineEndingDifferences">If set to <c>true</c>, treats \r\n, \r, and \n as equivalent.</param>
		/// <param name="ignoreWhiteSpaceDifferences">If set to <c>true</c>, treats spaces and tabs (in any non-zero quantity) as equivalent.</param>
		/// <param name="ignoreAllWhiteSpace">If set to <c>true</c>, removes all whitespaces and tabs before comparing.</param>
		/// <exception cref="EqualException">Thrown when the strings are not equivalent.</exception>
		/// <remarks>
		/// The <paramref name="ignoreWhiteSpaceDifferences"/> and <paramref name="ignoreAllWhiteSpace"/> flags consider
		/// the following characters to be white-space:
		/// <see href="https://unicode-explorer.com/c/0009">Tab</see> (\t),
		/// <see href="https://unicode-explorer.com/c/0020">Space</see> (\u0020),
		/// <see href="https://unicode-explorer.com/c/00A0">No-Break Space</see> (\u00A0),
		/// <see href="https://unicode-explorer.com/c/1680">Ogham Space Mark</see> (\u1680),
		/// <see href="https://unicode-explorer.com/c/180E">Mongolian Vowel Separator</see> (\u180E),
		/// <see href="https://unicode-explorer.com/c/2000">En Quad</see> (\u2000),
		/// <see href="https://unicode-explorer.com/c/2001">Em Quad</see> (\u2001),
		/// <see href="https://unicode-explorer.com/c/2002">En Space</see> (\u2002),
		/// <see href="https://unicode-explorer.com/c/2003">Em Space</see> (\u2003),
		/// <see href="https://unicode-explorer.com/c/2004">Three-Per-Em Space</see> (\u2004),
		/// <see href="https://unicode-explorer.com/c/2005">Four-Per-Em Space</see> (\u2004),
		/// <see href="https://unicode-explorer.com/c/2006">Six-Per-Em Space</see> (\u2006),
		/// <see href="https://unicode-explorer.com/c/2007">Figure Space</see> (\u2007),
		/// <see href="https://unicode-explorer.com/c/2008">Punctuation Space</see> (\u2008),
		/// <see href="https://unicode-explorer.com/c/2009">Thin Space</see> (\u2009),
		/// <see href="https://unicode-explorer.com/c/200A">Hair Space</see> (\u200A),
		/// <see href="https://unicode-explorer.com/c/200B">Zero Width Space</see> (\u200B),
		/// <see href="https://unicode-explorer.com/c/202F">Narrow No-Break Space</see> (\u202F),
		/// <see href="https://unicode-explorer.com/c/205F">Medium Mathematical Space</see> (\u205F),
		/// <see href="https://unicode-explorer.com/c/3000">Ideographic Space</see> (\u3000),
		/// and <see href="https://unicode-explorer.com/c/FEFF">Zero Width No-Break Space</see> (\uFEFF).
		/// In particular, it does not include carriage return (\r) or line feed (\n), which are covered by
		/// <paramref name="ignoreLineEndingDifferences"/>.
		/// </remarks>
		public static void Equal(
			ReadOnlySpan<char> expected,
			Span<char> actual,
			bool ignoreCase = false,
			bool ignoreLineEndingDifferences = false,
			bool ignoreWhiteSpaceDifferences = false,
			bool ignoreAllWhiteSpace = false) =>
				Equal(expected, (ReadOnlySpan<char>)actual, ignoreCase, ignoreLineEndingDifferences, ignoreWhiteSpaceDifferences, ignoreAllWhiteSpace);

		/// <summary>
		/// Verifies that two strings are equivalent.
		/// </summary>
		/// <param name="expected">The expected string value.</param>
		/// <param name="actual">The actual string value.</param>
		/// <param name="ignoreCase">If set to <c>true</c>, ignores cases differences. The invariant culture is used.</param>
		/// <param name="ignoreLineEndingDifferences">If set to <c>true</c>, treats \r\n, \r, and \n as equivalent.</param>
		/// <param name="ignoreWhiteSpaceDifferences">If set to <c>true</c>, treats horizontal white-space (i.e. spaces, tabs, and others; see remarks) in any non-zero quantity as equivalent.</param>
		/// <param name="ignoreAllWhiteSpace">If set to <c>true</c>, treats horizontal white-space (i.e. spaces, tabs, and others; see remarks), including zero quantities, as equivalent.</param>
		/// <exception cref="EqualException">Thrown when the strings are not equivalent.</exception>
		/// <remarks>
		/// The <paramref name="ignoreWhiteSpaceDifferences"/> and <paramref name="ignoreAllWhiteSpace"/> flags consider
		/// the following characters to be white-space:
		/// <see href="https://unicode-explorer.com/c/0009">Tab</see> (\t),
		/// <see href="https://unicode-explorer.com/c/0020">Space</see> (\u0020),
		/// <see href="https://unicode-explorer.com/c/00A0">No-Break Space</see> (\u00A0),
		/// <see href="https://unicode-explorer.com/c/1680">Ogham Space Mark</see> (\u1680),
		/// <see href="https://unicode-explorer.com/c/180E">Mongolian Vowel Separator</see> (\u180E),
		/// <see href="https://unicode-explorer.com/c/2000">En Quad</see> (\u2000),
		/// <see href="https://unicode-explorer.com/c/2001">Em Quad</see> (\u2001),
		/// <see href="https://unicode-explorer.com/c/2002">En Space</see> (\u2002),
		/// <see href="https://unicode-explorer.com/c/2003">Em Space</see> (\u2003),
		/// <see href="https://unicode-explorer.com/c/2004">Three-Per-Em Space</see> (\u2004),
		/// <see href="https://unicode-explorer.com/c/2005">Four-Per-Em Space</see> (\u2004),
		/// <see href="https://unicode-explorer.com/c/2006">Six-Per-Em Space</see> (\u2006),
		/// <see href="https://unicode-explorer.com/c/2007">Figure Space</see> (\u2007),
		/// <see href="https://unicode-explorer.com/c/2008">Punctuation Space</see> (\u2008),
		/// <see href="https://unicode-explorer.com/c/2009">Thin Space</see> (\u2009),
		/// <see href="https://unicode-explorer.com/c/200A">Hair Space</see> (\u200A),
		/// <see href="https://unicode-explorer.com/c/200B">Zero Width Space</see> (\u200B),
		/// <see href="https://unicode-explorer.com/c/202F">Narrow No-Break Space</see> (\u202F),
		/// <see href="https://unicode-explorer.com/c/205F">Medium Mathematical Space</see> (\u205F),
		/// <see href="https://unicode-explorer.com/c/3000">Ideographic Space</see> (\u3000),
		/// and <see href="https://unicode-explorer.com/c/FEFF">Zero Width No-Break Space</see> (\uFEFF).
		/// In particular, it does not include carriage return (\r) or line feed (\n), which are covered by
		/// <paramref name="ignoreLineEndingDifferences"/>.
		/// </remarks>

		public static void Equal(
#if XUNIT_NULLABLE
			string? expected,
			string? actual,
#else
			string expected,
			string actual,
#endif
			bool ignoreCase = false,
			bool ignoreLineEndingDifferences = false,
			bool ignoreWhiteSpaceDifferences = false,
			bool ignoreAllWhiteSpace = false)
		{
			// This overload is inside #if XUNIT_SPAN because the string version is dynamically converted
			// to a span version, so this string version is a backup that then delegates to the span version.

			if (expected == null && actual == null)
				return;
			if (expected == null || actual == null)
				throw EqualException.ForMismatchedStrings(expected, actual, -1, -1);

			Equal(expected.AsSpan(), actual.AsSpan(), ignoreCase, ignoreLineEndingDifferences, ignoreWhiteSpaceDifferences, ignoreAllWhiteSpace);
		}

#endif

		/// <summary>
		/// Verifies that a string matches a regular expression.
		/// </summary>
		/// <param name="expectedRegexPattern">The regex pattern expected to match</param>
		/// <param name="actualString">The string to be inspected</param>
		/// <exception cref="MatchesException">Thrown when the string does not match the regex pattern</exception>
		public static void Matches(
			[StringSyntax(StringSyntaxAttribute.Regex)]
			string expectedRegexPattern,
#if XUNIT_NULLABLE
			string? actualString)
#else
			string actualString)
#endif
		{
			GuardArgumentNotNull(nameof(expectedRegexPattern), expectedRegexPattern);

			if (actualString == null || !Regex.IsMatch(actualString, expectedRegexPattern))
				throw MatchesException.ForMatchNotFound(expectedRegexPattern, actualString);
		}

		/// <summary>
		/// Verifies that a string matches a regular expression.
		/// </summary>
		/// <param name="expectedRegex">The regex expected to match</param>
		/// <param name="actualString">The string to be inspected</param>
		/// <exception cref="MatchesException">Thrown when the string does not match the regex</exception>
		public static void Matches(
			Regex expectedRegex,
#if XUNIT_NULLABLE
			string? actualString)
#else
			string actualString)
#endif
		{
			GuardArgumentNotNull(nameof(expectedRegex), expectedRegex);

			if (actualString == null || !expectedRegex.IsMatch(actualString))
				throw MatchesException.ForMatchNotFound(expectedRegex.ToString(), actualString);
		}

		/// <summary>
		/// Verifies that a string starts with a given string, using the current culture.
		/// </summary>
		/// <param name="expectedStartString">The string expected to be at the start of the string</param>
		/// <param name="actualString">The string to be inspected</param>
		/// <exception cref="ContainsException">Thrown when the string does not start with the expected sub-string</exception>
		public static void StartsWith(
#if XUNIT_NULLABLE
			string? expectedStartString,
			string? actualString) =>
#else
			string expectedStartString,
			string actualString) =>
#endif
				StartsWith(expectedStartString, actualString, StringComparison.CurrentCulture);

		/// <summary>
		/// Verifies that a string starts with a given sub-string, using the given comparison type.
		/// </summary>
		/// <param name="expectedStartString">The sub-string expected to be at the start of the string</param>
		/// <param name="actualString">The string to be inspected</param>
		/// <param name="comparisonType">The type of string comparison to perform</param>
		/// <exception cref="ContainsException">Thrown when the string does not start with the expected sub-string</exception>
		public static void StartsWith(
#if XUNIT_NULLABLE
			string? expectedStartString,
			string? actualString,
#else
			string expectedStartString,
			string actualString,
#endif
			StringComparison comparisonType)
		{
			if (expectedStartString == null || actualString == null || !actualString.StartsWith(expectedStartString, comparisonType))
				throw StartsWithException.ForStringNotFound(expectedStartString, actualString);
		}

#if XUNIT_SPAN

		/// <summary>
		/// Verifies that a string starts with a given sub-string, using the current culture.
		/// </summary>
		/// <param name="expectedStartString">The sub-string expected to be at the start of the string</param>
		/// <param name="actualString">The string to be inspected</param>
		/// <exception cref="StartsWithException">Thrown when the string does not start with the expected sub-string</exception>
		public static void StartsWith(
			Memory<char> expectedStartString,
			Memory<char> actualString) =>
				StartsWith((ReadOnlyMemory<char>)expectedStartString, (ReadOnlyMemory<char>)actualString, StringComparison.CurrentCulture);

		/// <summary>
		/// Verifies that a string starts with a given sub-string, using the current culture.
		/// </summary>
		/// <param name="expectedStartString">The sub-string expected to be at the start of the string</param>
		/// <param name="actualString">The string to be inspected</param>
		/// <exception cref="StartsWithException">Thrown when the string does not start with the expected sub-string</exception>
		public static void StartsWith(
			Memory<char> expectedStartString,
			ReadOnlyMemory<char> actualString) =>
				StartsWith((ReadOnlyMemory<char>)expectedStartString, actualString, StringComparison.CurrentCulture);

		/// <summary>
		/// Verifies that a string starts with a given sub-string, using the current culture.
		/// </summary>
		/// <param name="expectedStartString">The sub-string expected to be at the start of the string</param>
		/// <param name="actualString">The string to be inspected</param>
		/// <exception cref="StartsWithException">Thrown when the string does not start with the expected sub-string</exception>
		public static void StartsWith(
			ReadOnlyMemory<char> expectedStartString,
			Memory<char> actualString) =>
				StartsWith(expectedStartString, (ReadOnlyMemory<char>)actualString, StringComparison.CurrentCulture);

		/// <summary>
		/// Verifies that a string starts with a given sub-string, using the default StringComparison.CurrentCulture comparison type.
		/// </summary>
		/// <param name="expectedStartString">The sub-string expected to be at the start of the string</param>
		/// <param name="actualString">The string to be inspected</param>
		/// <exception cref="StartsWithException">Thrown when the string does not start with the expected sub-string</exception>
		public static void StartsWith(
			ReadOnlyMemory<char> expectedStartString,
			ReadOnlyMemory<char> actualString) =>
				StartsWith(expectedStartString, actualString, StringComparison.CurrentCulture);

		/// <summary>
		/// Verifies that a string starts with a given sub-string, using the given comparison type.
		/// </summary>
		/// <param name="expectedStartString">The sub-string expected to be at the start of the string</param>
		/// <param name="actualString">The string to be inspected</param>
		/// <param name="comparisonType">The type of string comparison to perform</param>
		/// <exception cref="StartsWithException">Thrown when the string does not start with the expected sub-string</exception>
		public static void StartsWith(
			Memory<char> expectedStartString,
			Memory<char> actualString,
			StringComparison comparisonType = StringComparison.CurrentCulture) =>
				StartsWith((ReadOnlyMemory<char>)expectedStartString, (ReadOnlyMemory<char>)actualString, comparisonType);

		/// <summary>
		/// Verifies that a string starts with a given sub-string, using the given comparison type.
		/// </summary>
		/// <param name="expectedStartString">The sub-string expected to be at the start of the string</param>
		/// <param name="actualString">The string to be inspected</param>
		/// <param name="comparisonType">The type of string comparison to perform</param>
		/// <exception cref="StartsWithException">Thrown when the string does not start with the expected sub-string</exception>
		public static void StartsWith(
			Memory<char> expectedStartString,
			ReadOnlyMemory<char> actualString,
			StringComparison comparisonType = StringComparison.CurrentCulture) =>
				StartsWith((ReadOnlyMemory<char>)expectedStartString, actualString, comparisonType);

		/// <summary>
		/// Verifies that a string starts with a given sub-string, using the given comparison type.
		/// </summary>
		/// <param name="expectedStartString">The sub-string expected to be at the start of the string</param>
		/// <param name="actualString">The string to be inspected</param>
		/// <param name="comparisonType">The type of string comparison to perform</param>
		/// <exception cref="StartsWithException">Thrown when the string does not start with the expected sub-string</exception>
		public static void StartsWith(
			ReadOnlyMemory<char> expectedStartString,
			Memory<char> actualString,
			StringComparison comparisonType = StringComparison.CurrentCulture) =>
				StartsWith(expectedStartString, (ReadOnlyMemory<char>)actualString, comparisonType);

		/// <summary>
		/// Verifies that a string starts with a given sub-string, using the given comparison type.
		/// </summary>
		/// <param name="expectedStartString">The sub-string expected to be at the start of the string</param>
		/// <param name="actualString">The string to be inspected</param>
		/// <param name="comparisonType">The type of string comparison to perform</param>
		/// <exception cref="StartsWithException">Thrown when the string does not start with the expected sub-string</exception>
		public static void StartsWith(
			ReadOnlyMemory<char> expectedStartString,
			ReadOnlyMemory<char> actualString,
			StringComparison comparisonType = StringComparison.CurrentCulture)
		{
			GuardArgumentNotNull(nameof(expectedStartString), expectedStartString);

			StartsWith(expectedStartString.Span, actualString.Span, comparisonType);
		}

		/// <summary>
		/// Verifies that a string starts with a given sub-string, using the current culture.
		/// </summary>
		/// <param name="expectedStartString">The sub-string expected to be at the start of the string</param>
		/// <param name="actualString">The string to be inspected</param>
		/// <exception cref="StartsWithException">Thrown when the string does not start with the expected sub-string</exception>
		public static void StartsWith(
			Span<char> expectedStartString,
			Span<char> actualString) =>
				StartsWith((ReadOnlySpan<char>)expectedStartString, (ReadOnlySpan<char>)actualString, StringComparison.CurrentCulture);

		/// <summary>
		/// Verifies that a string starts with a given sub-string, using the current culture.
		/// </summary>
		/// <param name="expectedStartString">The sub-string expected to be at the start of the string</param>
		/// <param name="actualString">The string to be inspected</param>
		/// <exception cref="StartsWithException">Thrown when the string does not start with the expected sub-string</exception>
		public static void StartsWith(
			Span<char> expectedStartString,
			ReadOnlySpan<char> actualString) =>
				StartsWith((ReadOnlySpan<char>)expectedStartString, actualString, StringComparison.CurrentCulture);

		/// <summary>
		/// Verifies that a string starts with a given sub-string, using the current culture.
		/// </summary>
		/// <param name="expectedStartString">The sub-string expected to be at the start of the string</param>
		/// <param name="actualString">The string to be inspected</param>
		/// <exception cref="StartsWithException">Thrown when the string does not start with the expected sub-string</exception>
		public static void StartsWith(
			ReadOnlySpan<char> expectedStartString,
			Span<char> actualString) =>
				StartsWith(expectedStartString, (ReadOnlySpan<char>)actualString, StringComparison.CurrentCulture);

		/// <summary>
		/// Verifies that a string starts with a given sub-string, using the current culture.
		/// </summary>
		/// <param name="expectedStartString">The sub-string expected to be at the start of the string</param>
		/// <param name="actualString">The string to be inspected</param>
		/// <exception cref="StartsWithException">Thrown when the string does not start with the expected sub-string</exception>
		public static void StartsWith(
			ReadOnlySpan<char> expectedStartString,
			ReadOnlySpan<char> actualString) =>
				StartsWith(expectedStartString, actualString, StringComparison.CurrentCulture);

		/// <summary>
		/// Verifies that a string starts with a given sub-string, using the given comparison type.
		/// </summary>
		/// <param name="expectedStartString">The sub-string expected to be at the start of the string</param>
		/// <param name="actualString">The string to be inspected</param>
		/// <param name="comparisonType">The type of string comparison to perform</param>
		/// <exception cref="StartsWithException">Thrown when the string does not start with the expected sub-string</exception>
		public static void StartsWith(
			Span<char> expectedStartString,
			Span<char> actualString,
			StringComparison comparisonType = StringComparison.CurrentCulture) =>
				StartsWith((ReadOnlySpan<char>)expectedStartString, (ReadOnlySpan<char>)actualString, comparisonType);

		/// <summary>
		/// Verifies that a string starts with a given sub-string, using the given comparison type.
		/// </summary>
		/// <param name="expectedStartString">The sub-string expected to be at the start of the string</param>
		/// <param name="actualString">The string to be inspected</param>
		/// <param name="comparisonType">The type of string comparison to perform</param>
		/// <exception cref="StartsWithException">Thrown when the string does not start with the expected sub-string</exception>
		public static void StartsWith(
			Span<char> expectedStartString,
			ReadOnlySpan<char> actualString,
			StringComparison comparisonType = StringComparison.CurrentCulture) =>
				StartsWith((ReadOnlySpan<char>)expectedStartString, actualString, comparisonType);

		/// <summary>
		/// Verifies that a string starts with a given sub-string, using the given comparison type.
		/// </summary>
		/// <param name="expectedStartString">The sub-string expected to be at the start of the string</param>
		/// <param name="actualString">The string to be inspected</param>
		/// <param name="comparisonType">The type of string comparison to perform</param>
		/// <exception cref="StartsWithException">Thrown when the string does not start with the expected sub-string</exception>
		public static void StartsWith(
			ReadOnlySpan<char> expectedStartString,
			Span<char> actualString,
			StringComparison comparisonType = StringComparison.CurrentCulture) =>
				StartsWith(expectedStartString, (ReadOnlySpan<char>)actualString, comparisonType);

		/// <summary>
		/// Verifies that a string starts with a given sub-string, using the given comparison type.
		/// </summary>
		/// <param name="expectedStartString">The sub-string expected to be at the start of the string</param>
		/// <param name="actualString">The string to be inspected</param>
		/// <param name="comparisonType">The type of string comparison to perform</param>
		/// <exception cref="StartsWithException">Thrown when the string does not start with the expected sub-string</exception>
		public static void StartsWith(
			ReadOnlySpan<char> expectedStartString,
			ReadOnlySpan<char> actualString,
			StringComparison comparisonType = StringComparison.CurrentCulture)
		{
			if (!actualString.StartsWith(expectedStartString, comparisonType))
				throw StartsWithException.ForStringNotFound(expectedStartString.ToString(), actualString.ToString());
		}

#endif

		static readonly HashSet<char> charsLineEndings = new HashSet<char>()
		{
			'\r',  // Carriage Return
			'\n',  // Line feed
		};
		static readonly HashSet<char> charsWhitespace = new HashSet<char>()
		{
			'\t',      // Tab
			' ',       // Space
			'\u00A0',  // No-Break Space
			'\u1680',  // Ogham Space Mark
			'\u180E',  // Mongolian Vowel Separator
			'\u2000',  // En Quad
			'\u2001',  // Em Quad
			'\u2002',  // En Space
			'\u2003',  // Em Space
			'\u2004',  // Three-Per-Em Space
			'\u2005',  // Four-Per-Em Space
			'\u2006',  // Six-Per-Em Space
			'\u2007',  // Figure Space
			'\u2008',  // Punctuation Space
			'\u2009',  // Thin Space
			'\u200A',  // Hair Space
			'\u200B',  // Zero Width Space
			'\u202F',  // Narrow No-Break Space
			'\u205F',  // Medium Mathematical Space
			'\u3000',  // Ideographic Space
			'\uFEFF',  // Zero Width No-Break Space
		};

		static int SkipLineEnding(
#if XUNIT_SPAN
			ReadOnlySpan<char> value,
#else
			string value,
#endif
			int index)
		{
			if (value[index] == '\r')
				++index;

			if (index < value.Length && value[index] == '\n')
				++index;

			return index;
		}

		static int SkipWhitespace(
#if XUNIT_SPAN
			ReadOnlySpan<char> value,
#else
			string value,
#endif
			int index)
		{
			while (index < value.Length)
			{
				if (charsWhitespace.Contains(value[index]))
					index++;
				else
					return index;
			}

			return index;
		}
	}
}
