// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Text;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.AspNetCore.Razor.TextDifferencing;

internal partial class SourceTextDiffer
{
    private class LineDiffer : SourceTextDiffer
    {
        private readonly TextLineCollection _oldLines;
        private readonly TextLineCollection _newLines;

        private char[] _oldLineBuffer;
        private char[] _newLineBuffer;
        private char[] _appendBuffer;

        protected override int OldSourceLength { get; }
        protected override int NewSourceLength { get; }

        public LineDiffer(SourceText oldText, SourceText newText)
            : base(oldText, newText)
        {
            _oldLineBuffer = RentArray(1024);
            _newLineBuffer = RentArray(1024);
            _appendBuffer = RentArray(1024);

            _oldLines = oldText.Lines;
            _newLines = newText.Lines;

            OldSourceLength = _oldLines.Count;
            NewSourceLength = _newLines.Count;
        }

        public override void Dispose()
        {
            ReturnArray(_oldLineBuffer);
            ReturnArray(_newLineBuffer);
            ReturnArray(_appendBuffer);
        }

        protected override bool SourceEqual(int oldSourceIndex, int newSourceIndex)
        {
            var oldLine = _oldLines[oldSourceIndex];
            var newLine = _newLines[newSourceIndex];

            var oldSpan = oldLine.SpanIncludingLineBreak;
            var newSpan = newLine.SpanIncludingLineBreak;

            if (oldSpan.Length != newSpan.Length)
            {
                return false;
            }

            var length = oldSpan.Length;

            // Simple case: Both lines are empty.
            if (length == 0)
            {
                return true;
            }

            // Copy the text into char arrays for comparison. Note: To avoid allocation,
            // we try to reuse the same char buffers and only grow them when a longer
            // line is encountered.
            var oldChars = EnsureBuffer(ref _oldLineBuffer, length);
            var newChars = EnsureBuffer(ref _newLineBuffer, length);

            OldText.CopyTo(oldSpan.Start, oldChars, 0, length);
            NewText.CopyTo(newSpan.Start, newChars, 0, length);

            for (var i = 0; i < length; i++)
            {
                if (oldChars[i] != newChars[i])
                {
                    return false;
                }
            }

            return true;
        }

        protected override int GetEditPosition(DiffEdit edit)
            => _oldLines[edit.Position].Start;

        protected override int AppendEdit(DiffEdit edit, StringBuilder builder)
        {
            if (edit.Kind == DiffEditKind.Insert)
            {
                Assumes.NotNull(edit.NewTextPosition);
                var newTextPosition = edit.NewTextPosition.GetValueOrDefault();

                for (var i = 0; i < edit.Length; i++)
                {
                    var newLine = _newLines[newTextPosition + i];

                    var newSpan = newLine.SpanIncludingLineBreak;
                    if (newSpan.Length > 0)
                    {
                        var buffer = EnsureBuffer(ref _appendBuffer, newSpan.Length);
                        NewText.CopyTo(newSpan.Start, buffer, 0, newSpan.Length);

                        builder.Append(buffer, 0, newSpan.Length);
                    }
                }

                return _oldLines[edit.Position].Start;
            }

            return _oldLines[edit.Position + edit.Length - 1].EndIncludingLineBreak;
        }
    }
}
