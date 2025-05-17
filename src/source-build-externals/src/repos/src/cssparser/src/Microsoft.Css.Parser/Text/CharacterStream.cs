// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Microsoft.Css.Parser.Text
{
    /// <summary>
    /// Helper class that represents stream of characters for a parser or tokenizer
    /// </summary>
    internal class CharacterStream
    {
        private int _position;
        private readonly int _start;

        internal CharacterStream(string text) :
            this(new StringTextProvider(text))
        {
        }

        internal CharacterStream(ITextProvider textProvider) :
            this(textProvider, 0, textProvider.Length)
        {
        }

        internal CharacterStream(ITextProvider textProvider, int start, int length)
        {
            Debug.Assert(textProvider != null);

            TextProvider = textProvider;
            _start = Math.Max(start, 0);
            End = Math.Min(start + length, TextProvider.Length);

            Position = _start;
        }

        internal ITextProvider TextProvider { get; }
        internal char CurrentChar { get; private set; }
        internal int End { get; }

        internal int Position
        {
            get { return _position; }
            set
            {
                if (value < _start)
                {
                    _position = 0;
                    CurrentChar = (TextProvider.Length > _start ? TextProvider[_start] : '\0');
                }
                else if (value >= End)
                {
                    _position = End;
                    CurrentChar = '\0';
                }
                else
                {
                    _position = value;
                    CurrentChar = TextProvider[_position];
                }
            }
        }

        internal char Peek(int offset = 1)
        {
            int index = _position + offset;

            return (index >= 0 && index < End) ? TextProvider[index] : '\0';
        }

        internal bool Advance(int offset = 1)
        {
            int newPosition = _position + offset;
            Position = newPosition;

            return _position == newPosition;
        }

        internal bool IsAtEnd
        {
            get { return _position >= End; }
        }

        [ExcludeFromCodeCoverage]
        [DebuggerStepThrough]
        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "@{0} ({1})", Position, TextProvider[Position]);
        }
    }
}
