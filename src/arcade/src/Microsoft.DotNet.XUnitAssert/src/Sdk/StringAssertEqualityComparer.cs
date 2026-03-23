#pragma warning disable IDE0028 // Simplify collection initialization
#pragma warning disable IDE0090 // Use 'new(...)'
#pragma warning disable IDE0290 // Use primary constructor

#if XUNIT_NULLABLE
#nullable enable
#else
// In case this is source-imported with global nullable enabled but no XUNIT_NULLABLE
#pragma warning disable CS8604
#endif

using System;
using System.Collections.Generic;

namespace Xunit.Sdk
{
	/// <summary>
	/// This static class offers equivalence comparisons for string values
	/// </summary>
#if XUNIT_VISIBILITY_INTERNAL
	internal
#else
	public
#endif
	static class StringAssertEqualityComparer
	{
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

		/// <summary>
		/// Compare two string values for equalivalence.
		/// </summary>
		/// <param name="expected">The expected string value.</param>
		/// <param name="actual">The actual string value.</param>
		/// <param name="ignoreCase">If set to <see langword="true"/>, ignores cases differences. The invariant culture is used.</param>
		/// <param name="ignoreLineEndingDifferences">If set to <see langword="true"/>, treats \r\n, \r, and \n as equivalent.</param>
		/// <param name="ignoreWhiteSpaceDifferences">If set to <see langword="true"/>, treats horizontal white-space (i.e. spaces, tabs, and others; see remarks) in any non-zero quantity as equivalent.</param>
		/// <param name="ignoreAllWhiteSpace">If set to <see langword="true"/>, treats horizontal white-space (i.e. spaces, tabs, and others; see remarks), including zero quantities, as equivalent.</param>
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
		public static AssertEqualityResult Equivalent(
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
			if (expected == null && actual == null)
				return AssertEqualityResult.ForResult(true, expected, actual);
			if (expected == null || actual == null)
				return AssertEqualityResult.ForResult(false, expected, actual);

			return Equivalent(expected.AsSpan(), actual.AsSpan(), ignoreCase, ignoreLineEndingDifferences, ignoreWhiteSpaceDifferences, ignoreAllWhiteSpace);
		}

		/// <summary>
		/// Compare two string values for equalivalence.
		/// </summary>
		/// <param name="expected">The expected string value.</param>
		/// <param name="actual">The actual string value.</param>
		/// <param name="ignoreCase">If set to <see langword="true"/>, ignores cases differences. The invariant culture is used.</param>
		/// <param name="ignoreLineEndingDifferences">If set to <see langword="true"/>, treats \r\n, \r, and \n as equivalent.</param>
		/// <param name="ignoreWhiteSpaceDifferences">If set to <see langword="true"/>, treats horizontal white-space (i.e. spaces, tabs, and others; see remarks) in any non-zero quantity as equivalent.</param>
		/// <param name="ignoreAllWhiteSpace">If set to <see langword="true"/>, treats horizontal white-space (i.e. spaces, tabs, and others; see remarks), including zero quantities, as equivalent.</param>
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
		public static AssertEqualityResult Equivalent(
			ReadOnlySpan<char> expected,
			ReadOnlySpan<char> actual,
			bool ignoreCase = false,
			bool ignoreLineEndingDifferences = false,
			bool ignoreWhiteSpaceDifferences = false,
			bool ignoreAllWhiteSpace = false)
		{
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
					return AssertEqualityResult.ForResult(true, expected.ToString(), actual.ToString());
				if (actualLength == 0 && SkipWhitespace(expected, 0) == expectedLength)
					return AssertEqualityResult.ForResult(true, expected.ToString(), actual.ToString());
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
				return AssertEqualityResult.ForMismatch(expected.ToString(), actual.ToString(), expectedIndex, actualIndex);

			return AssertEqualityResult.ForResult(true, expected.ToString(), actual.ToString());
		}

		static int SkipLineEnding(
			ReadOnlySpan<char> value,
			int index)
		{
			if (value[index] == '\r')
				++index;

			if (index < value.Length && value[index] == '\n')
				++index;

			return index;
		}

		static int SkipWhitespace(
			ReadOnlySpan<char> value,
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
