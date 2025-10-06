// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Microsoft.Css.Parser.Text
{
    /// <summary>
    /// Remembers the length of a decoded Unicode or escaped character
    /// (an int is used because the max Unicode character supported is 0xFFFFFF)
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1815:OverrideEqualsAndOperatorEqualsOnValueTypes")]
    internal struct DecodedChar
    {
        internal DecodedChar(int character, int encodedLength)
        {
            CharUtf32 = character;
            EncodedLength = encodedLength;
        }

        internal bool RequiresUtf32
        {
            get { return CharUtf32 > 0xFFFF; }
        }

        /// <summary>
        /// Return the UTF16 char if possible (NULL otherwise)
        /// </summary>
        internal char Char
        {
            get { return RequiresUtf32 ? '\0' : (char)CharUtf32; }
        }

        /// <summary>
        /// Returns all the bits for the UTF32 char
        /// </summary>
        internal int CharUtf32 { get; }

        internal int EncodedLength { get; }
    }
}
