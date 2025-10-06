// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Microsoft.Css.Parser.Text
{
    /// <summary>
    /// Text provider abstraction.
    /// Can be implemented on a string <seealso cref="StringTextProvider"/> or
    /// on a Visual Studio ITextSnapshot
    /// </summary>
    public interface ITextProvider
    {
        /// <summary>Text length</summary>
        int Length { get; }

        /// <summary>
        /// Retrieves character at a given position.
        /// Returns 0 if index is out of range. Must not throw.
        /// </summary>
        char this[int position] { get; }

        /// <summary>Retrieves a substring from given position and length</summary>
        string GetText(int start, int length);

        /// <summary>Finds first index of a text sequence. Returns -1 if not found.</summary>
        int IndexOf(string text, int position, int length, bool ignoreCase);

        /// <summary>Compares text range to a given string.</summary>
        bool CompareTo(int position, string otherText, bool ignoreCase);

        /// <summary>Compares text range to a given string.</summary>
        bool CompareTo(int position, string otherText, int otherPosition, int length, bool ignoreCase);

        /// <summary>Compares this text provider's content at specified range to another text provider's content at a given range.</summary>
        bool CompareTo(int position, ITextProvider otherProvider, int otherPosition, int length, bool ignoreCase);

        /// <summary>Snapshot version.</summary>
        int Version { get; }

        /// <summary>
        ///  Adjusts the position and length from the current text provider's coordinate space to another text provider's
        ///   coordinate space
        /// </summary>
        /// <param name="position">position to adjust</param>
        /// <param name="length">length to adjust</param>
        /// <param name="other">text provider with desired coordinate space</param>
        /// <returns></returns>
        (int adjustedPosition, int adjustedLength) GetAdjustedRange(int position, int length, ITextProvider other);

        /// <summary>
        /// Retrieves line number and column for the given position
        /// </summary>
        /// <param name="pos">The position</param>
        /// <param name="line">Output parameter for line number</param>
        /// <param name="column">Output parameter for column</param>
        void GetLineAndColumnFromPosition(int pos, out int line, out int column);

        /// <summary>
        /// Retrieves position for the given line number and column 
        /// </summary>
        /// <param name="line"Line number</param>
        /// <param name="column">Column</param>
        /// <returns>The position</returns>
        int GetPositionFromLineAndColumn(int line, int column);
    }
}
