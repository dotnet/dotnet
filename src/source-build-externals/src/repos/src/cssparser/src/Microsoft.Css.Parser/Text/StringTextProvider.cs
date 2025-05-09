// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Microsoft.Css.Parser.Text
{
    /// <summary>
    /// Implements <seealso cref="ITextProvider"/> on a string
    /// </summary>
    public class StringTextProvider : ITextProvider
    {
        private readonly string _text;
        private List<int> _lineStarts;

        // Array access (i.e. converting string to an array)
        // is faster, but takes more memory.

        [DebuggerStepThrough]
        public StringTextProvider(string text)
        {
            _text = text;
            _lineStarts = null;
        }

        [DebuggerStepThrough]
        public override string ToString()
        {
            return _text;
        }

        #region ITextProvider

        /// <summary>
        /// Text length
        /// </summary>
        public int Length
        {
            get { return _text.Length; }
        }

        /// <summary>
        /// Retrieves character at a given position
        /// </summary>
        public char this[int position]
        {
            get
            {
                if (position < 0 || position >= _text.Length)
                {
                    return '\0';
                }

                return _text[position];
            }
        }

        /// <summary>
        /// Retrieves a substring given start position and length
        /// </summary>
        public string GetText(int position, int length)
        {
            if (length == 0)
            {
                return string.Empty;
            }

            Debug.Assert(position >= 0 && length >= 0 && position + length <= _text.Length);
            return _text.Substring(position, length);
        }

        /// <summary>
        /// Searches text for a givne string starting at specified position
        /// </summary>
        /// <param name="stringToFind">String to find</param>
        /// <param name="position">Starting position</param>
        /// <param name="length">number of characters of the text provider to search through</param>
        /// <param name="ignoreCase">True if search should be case-insensitive</param>
        /// <returns>Character index of the first string appearance or -1 if string was not found</returns>
        public int IndexOf(string stringToFind, int position, int length, bool ignoreCase)
        {
            if (position + stringToFind.Length > _text.Length)
            {
                return -1;
            }

            if (position + length > _text.Length)
            {
                return -1;
            }

            return _text.IndexOf(stringToFind, position, length, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
        }

        public bool CompareTo(int position, string otherText, bool ignoreCase)
        {
            return CompareTo(position, otherText, 0, otherText.Length, ignoreCase);
        }

        public bool CompareTo(int position, string otherText, int otherPosition, int length, bool ignoreCase)
        {
            StringComparison comparison = ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;

            return 0 == string.Compare(_text, position, otherText, otherPosition, length, comparison);
        }

        public bool CompareTo(int position, ITextProvider otherProvider, int otherPosition, int length, bool ignoreCase)
        {
            return otherProvider.CompareTo(otherPosition, _text, position, length, ignoreCase);
        }

        /// <summary>
        /// Retrieves line number and column for the given position
        /// </summary>
        /// <param name="pos">The position</param>
        /// <param name="line">Output parameter for line number</param>
        /// <param name="column">Output parameter for column</param>
        public void GetLineAndColumnFromPosition(int pos, out int line, out int column)
        {
            EnsureLineStarts();

            line = _lineStarts.BinarySearch(pos);
            if (line < 0)
            {
                // Between lines, move to the previous line
                line = ~line - 1;
            }

            column = pos - _lineStarts[line];
        }

        public int GetPositionFromLineAndColumn(int line, int column)
        {
            EnsureLineStarts();

            int position = _lineStarts[line] + column;

            return position;
        }

        private void EnsureLineStarts()
        {
            if (_lineStarts == null)
            {
                // Initial guess of 20 chars / line
                _lineStarts = new List<int>(1 + _text.Length / 20)
                {
                    0
                };

                for (int i = 0; i < _text.Length; ++i)
                {
                    char curChar = _text[i];
                    if (curChar == '\r')
                    {
                        if ((i + 1 < _text.Length) && (_text[i + 1] == '\n'))
                        {
                            i++;
                        }

                        _lineStarts.Add(i + 1);
                    }
                    else if (curChar == '\n')
                    {
                        _lineStarts.Add(i + 1);
                    }
                }
            }
        }

        public int Version { get { return 0; } }

        // static string text provider does map between text providers
        public (int, int) GetAdjustedRange(int position, int length, ITextProvider other)
        {
            int adjustedPosition = position;
            int adjustedLength = length;

            if (position + length > other.Length)
            {
                if (position > other.Length)
                {
                    adjustedPosition = other.Length;
                    adjustedLength = 0;
                }
                else
                {
                    adjustedLength = other.Length - position;
                }
            }

            return (adjustedPosition, adjustedLength);
        }

        #endregion

    }
}
