// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Microsoft.VisualStudio.Text;

namespace Microsoft.VisualStudio.LanguageServer.ContainedLanguage;

internal class VisualStudioTextChange : ITextChange
{
    public VisualStudioTextChange(int oldStart, int oldLength, string newText)
    {
        OldSpan = new Span(oldStart, oldLength);
        NewText = newText;
    }

    public VisualStudioTextChange(TextEdit textEdit, ITextSnapshot textSnapshot)
    {
        var startRange = textEdit.Range.Start;
        var startLine = textSnapshot.GetLineFromLineNumber(startRange.Line);
        var startAbsoluteIndex = startLine.Start + startRange.Character;
        var endRange = textEdit.Range.End;
        var endLine = textSnapshot.GetLineFromLineNumber(endRange.Line);
        var endAbsoluteIndex = endLine.Start + endRange.Character;
        var length = endAbsoluteIndex - startAbsoluteIndex;
        OldSpan = new Span(startAbsoluteIndex, length);
        NewText = textEdit.NewText;
    }

    public Span OldSpan { get; }
    public int OldPosition => OldSpan.Start;
    public int OldEnd => OldSpan.End;
    public int OldLength => OldSpan.Length;
    public string NewText { get; }
    public int NewLength => NewText.Length;

    public Span NewSpan => throw new NotImplementedException();

    public int NewPosition => throw new NotImplementedException();
    public int Delta => throw new NotImplementedException();
    public int NewEnd => throw new NotImplementedException();
    public string OldText => throw new NotImplementedException();
    public int LineCountDelta => throw new NotImplementedException();

    public override string ToString()
    {
        return OldSpan.ToString() + "->" + NewText;
    }
}
