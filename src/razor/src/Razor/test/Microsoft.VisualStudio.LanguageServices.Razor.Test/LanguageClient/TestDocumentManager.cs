// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Razor.Telemetry;
using Microsoft.AspNetCore.Razor.Test.Common.Editor;
using Microsoft.AspNetCore.Razor.Test.Common.LanguageServer;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.LanguageServer.ContainedLanguage;
using Microsoft.VisualStudio.Text;

namespace Microsoft.VisualStudio.Razor.LanguageClient;

internal class TestDocumentManager(CSharpTestLspServer testLspServer = null) : TrackingLSPDocumentManager
{
    private readonly Dictionary<Uri, LSPDocumentSnapshot> _documents = [];
    private readonly CSharpTestLspServer _testLspServer = testLspServer;

    public int UpdateVirtualDocumentCallCount { get; private set; }

    public override bool TryGetDocument(Uri uri, out LSPDocumentSnapshot lspDocumentSnapshot)
    {
        return _documents.TryGetValue(uri, out lspDocumentSnapshot);
    }

    public void AddDocument(Uri uri, LSPDocumentSnapshot documentSnapshot)
    {
        _documents.Add(uri, documentSnapshot);
    }

    public override void TrackDocument(ITextBuffer buffer)
    {
        throw new NotImplementedException();
    }

    public override void UntrackDocument(ITextBuffer buffer)
    {
        throw new NotImplementedException();
    }

    public override void UpdateVirtualDocument<TVirtualDocument>(Uri hostDocumentUri, IReadOnlyList<ITextChange> changes, int hostDocumentVersion, object state)
    {
        if (!_documents.TryGetValue(hostDocumentUri, out var documentSnapshot))
        {
            return;
        }

        if (!documentSnapshot.TryGetVirtualDocument<CSharpVirtualDocumentSnapshot>(out var virtualDocumentSnapshot))
        {
            return;
        }

        using var virtualDocument = new CSharpVirtualDocument(projectKey: default, virtualDocumentSnapshot.Uri, virtualDocumentSnapshot.Snapshot.TextBuffer, NoOpTelemetryReporter.Instance);
        virtualDocument.Update(changes, hostDocumentVersion, state);
        _documents[hostDocumentUri] = new TestLSPDocumentSnapshot(hostDocumentUri, documentSnapshot.Version, new[] { virtualDocument.CurrentSnapshot });

        if (_testLspServer is not null)
        {
            UpdateCSharpServerDocument(changes, virtualDocumentSnapshot);
        }

        UpdateVirtualDocumentCallCount++;
    }

    private void UpdateCSharpServerDocument(IReadOnlyList<ITextChange> changes, VirtualDocumentSnapshot virtualDocumentSnapshot)
    {
        var virtualSourceText = SourceText.From(virtualDocumentSnapshot.Snapshot.GetText());
        var rangesAndTexts = changes.Select(c =>
        {
            GetLinesAndOffsets(virtualSourceText, c.OldSpan, out var startLine, out var startCharacter, out var endLine, out var endCharacter);
            var range = new Range
            {
                Start = new LanguageServer.Protocol.Position { Line = startLine, Character = startCharacter },
                End = new LanguageServer.Protocol.Position { Line = endLine, Character = endCharacter }
            };

            return (range, c.NewText);
        }).ToArray();

#pragma warning disable VSTHRD002 // Avoid problematic synchronous waits
        _testLspServer.ReplaceTextAsync(virtualDocumentSnapshot.Uri, rangesAndTexts).Wait();
#pragma warning restore VSTHRD002 // Avoid problematic synchronous waits
    }

    private static void GetLinesAndOffsets(
        SourceText sourceText,
        Span span,
        out int startLineNumber,
        out int startOffset,
        out int endLineNumber,
        out int endOffset)
    {
        (startLineNumber, startOffset) = GetLineAndOffset(sourceText, span.Start);
        (endLineNumber, endOffset) = GetLineAndOffset(sourceText, span.End);

        static (int lineNumber, int offset) GetLineAndOffset(SourceText source, int position)
        {
            var line = source.Lines.GetLineFromPosition(position);

            return (lineNumber: line.LineNumber, offset: position - line.Start);
        }
    }
}
