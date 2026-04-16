// This is a port of the Python version of wcwidth() written by Jeff Quast,
// which originally was written by Markus Kuhn.
//
// https://github.com/jquast/wcwidth
// http://www.cl.cam.ac.uk/~mgk25/ucs/wcwidth.c
//
// In fixed-width output devices, Latin characters all occupy a single "cell" position
// of equal width, whereas ideographic CJK characters occupy two such cells. Interoperability
// between terminal-line applications and (teletype-style) character terminals using the
// UTF-8 encoding requires agreement on which character should advance the cursor by how
// many cell positions. No established formal standards exist at present on which Unicode
// character shall occupy how many cell positions on character terminals. These routines
// are a first attempt of defining such behavior based on simple rules applied to data
// provided by the Unicode Consortium.
//
// For some graphical characters, the Unicode standard explicitly defines a character-cell
// width via the definition of the East Asian FullWidth (F), Wide (W), Half-width (H),
// and Narrow (Na) classes. In all these cases, there is no ambiguity about which width
// a terminal shall use. For characters in the East Asian Ambiguous (A) class, the width
// choice depends purely on a preference of backward compatibility with either historic
// CJK or Western practice. Choosing single-width for these characters is easy to justify
// as the appropriate long-term solution, as the CJK practice of displaying these characters
// as double-width comes from historic implementation simplicity (8-bit encoded characters
// were displayed single-width and 16-bit ones double-width, even for Greek, Cyrillic, etc.)
// and not any typographic considerations.
//
// Much less clear is the choice of width for the Not East Asian (Neutral) class.
// Existing practice does not dictate a width for any of these characters. It would
// nevertheless make sense typographically to allocate two character cells to characters
// such as for instance EM SPACE or VOLUME INTEGRAL, which cannot be represented adequately
// with a single-width glyph. The following routines at present merely assign a single-cell
// width to all neutral characters, in the interest of simplicity. This is not entirely
// satisfactory and should be reconsidered before establishing a formal standard in this area.
// At the moment, the decision which Not East Asian (Neutral) characters should be represented
// by double-width glyphs cannot yet be answered by applying a simple rule from the Unicode
// database content. Setting up a proper standard for the behavior of UTF-8 character
// terminals will require a careful analysis not only of each Unicode character, but also
// of each presentation form, something the author of these routines has avoided to do so far.
//
// http://www.unicode.org/unicode/reports/tr11/

#if WCWIDTH
#pragma warning restore
#else
#pragma warning disable
#endif

using System.Collections.Generic;
#if NET6_0_OR_GREATER
using System.Text;
#endif

namespace Wcwidth
{
    /// <summary>
    /// A utility for calculating the width of Unicode characters.
    /// </summary>
#if WCWIDTH_VISIBILITY_INTERNAL
    internal
#else
    public
#endif
    static class UnicodeCalculator
    {
        private const Unicode Latest = Unicode.Version_15_1_0;

        // NOTE: created by hand, there isn't anything identifiable other than
        // general Cf category code to identify these, and some characters in Cf
        // category code are of non-zero width.
        // Also includes some Cc, Mn, Zl, and Zp characters
        private static readonly HashSet<uint> _zeroWidthCf = new HashSet<uint>
        {
            0,       // Null (Cc)
            0x034F,  // Combining grapheme joiner (Mn)
            0x200B,  // Zero width space
            0x200C,  // Zero width non-joiner
            0x200D,  // Zero width joiner
            0x200E,  // Left-to-right mark
            0x200F,  // Right-to-left mark
            0x2028,  // Line separator (Zl)
            0x2029,  // Paragraph separator (Zp)
            0x202A,  // Left-to-right embedding
            0x202B,  // Right-to-left embedding
            0x202C,  // Pop directional formatting
            0x202D,  // Left-to-right override
            0x202E,  // Right-to-left override
            0x2060,  // Word joiner
            0x2061,  // Function application
            0x2062,  // Invisible times
            0x2063,  // Invisible separator
        };

        /// <summary>
        /// Gets the width of a Unicode code point.
        /// </summary>
        /// <param name="value">The Unicode code point to calculate the width of.</param>
        /// <param name="version">The Unicode version to use.</param>
        /// <returns>The width of the character (-1, 0, 1, 2).</returns>
        public static int GetWidth(int value, Unicode? version = null)
        {
            version ??= Latest;

            // NOTE: created by hand, there isn't anything identifiable other than
            // general Cf category code to identify these, and some characters in Cf
            // category code are of non-zero width.
            if (_zeroWidthCf.Contains((uint)value))
            {
                return 0;
            }

            // C0/C1 control characters
            if (value < 32 || (value >= 0x07F && value < 0x0A0))
            {
                return -1;
            }

            // Combining characters with zero width?
            var zeroTable = ZeroTable.GetTable(version.Value);
            if (zeroTable.Exist((uint)value))
            {
                return 0;
            }

            // Wide character?
            var wideTable = WideTable.GetTable(version.Value);
            return wideTable.Exist((uint)value) ? 2 : 1;
        }

#if NET6_0_OR_GREATER
        /// <summary>
        /// Gets the width of a Unicode scalar.
        /// </summary>
        /// <param name="value">The Unicode scalar to calculate the width of.</param>
        /// <param name="version">The Unicode version to use.</param>
        /// <returns>The width of the character (-1, 0, 1, 2).</returns>
        public static int GetWidth(Rune value, Unicode? version = null)
        {
            return GetWidth(value.Value, version);
        }
#endif

        /// <summary>
        /// Gets the width of a UTF-16 code unit.
        /// </summary>
        /// <param name="value">The UTF-16 code unit to calculate the width of.</param>
        /// <param name="version">The Unicode version to use.</param>
        /// <returns>The width of the character (-1, 0, 1, 2).</returns>
        public static int GetWidth(char value, Unicode? version = null)
        {
            return GetWidth((int)value, version);
        }
    }
}
