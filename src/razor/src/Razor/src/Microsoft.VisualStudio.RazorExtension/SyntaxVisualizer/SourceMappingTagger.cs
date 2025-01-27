// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.VisualStudio.Razor.Documents;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Tagging;

namespace Microsoft.VisualStudio.RazorExtension.SyntaxVisualizer;

internal sealed class SourceMappingTagger : ITagger<SourceMappingTag>
{
    internal static SourceMappingTagger GetOrCreateTagger(ITextBuffer buffer, Func<SourceMappingTagger> creator)
    {
        return buffer.Properties.GetOrCreateSingletonProperty(creator);
    }

    private readonly ITextBuffer _buffer;
    private readonly Lazy<RazorCodeDocumentProvidingSnapshotChangeTrigger> _sourceMappingProjectChangeTrigger;
    private readonly ITextDocumentFactoryService _textDocumentFactoryService;

    public event EventHandler<SnapshotSpanEventArgs>? TagsChanged;

    public static bool Enabled { get; set; }

    internal SourceMappingTagger(ITextBuffer buffer, Lazy<RazorCodeDocumentProvidingSnapshotChangeTrigger> sourceMappingProjectChangeTrigger, ITextDocumentFactoryService textDocumentFactoryService)
    {
        _buffer = buffer;
        _sourceMappingProjectChangeTrigger = sourceMappingProjectChangeTrigger;
        _textDocumentFactoryService = textDocumentFactoryService;
        _buffer.Changed += (sender, args) => HandleBufferChanged(args);
    }

    public IEnumerable<ITagSpan<SourceMappingTag>> GetTags(NormalizedSnapshotSpanCollection spans)
    {
        if (!Enabled || spans.Count == 0)
        {
            return [];
        }

        var snapshot = spans[0].Snapshot;

        if (!_textDocumentFactoryService.TryGetTextDocument(_buffer, out var textDocument))
        {
            return [];
        }

        var codeDocument = ThreadHelper.JoinableTaskFactory.Run(
            () => _sourceMappingProjectChangeTrigger.Value.GetRazorCodeDocumentAsync(textDocument.FilePath, CancellationToken.None));

        if (codeDocument is null)
        {
            return [];
        }

        return GetTagsWorker(codeDocument, snapshot);

        static IEnumerable<ITagSpan<SourceMappingTag>> GetTagsWorker(RazorCodeDocument codeDocument, ITextSnapshot snapshot)
        {
            var csharpDocument = codeDocument.GetCSharpDocument();
            var generatedCode = csharpDocument.Text.ToString();
            foreach (var mapping in csharpDocument.SourceMappings)
            {
                var generatedText = GetGeneratedCodeSnippet(generatedCode, mapping.GeneratedSpan.AbsoluteIndex);

                var position = Math.Min(mapping.OriginalSpan.AbsoluteIndex, snapshot.Length);
                var point = new SnapshotPoint(snapshot, position);
                var tag = new SourceMappingTag(isStart: true, generatedText);
                var span = new SnapshotSpan(point, 0);
                yield return new TagSpan<SourceMappingTag>(span, tag);

                position = Math.Min(mapping.OriginalSpan.AbsoluteIndex + mapping.OriginalSpan.Length, snapshot.Length);
                point = new SnapshotPoint(snapshot, position);
                tag = new SourceMappingTag(isStart: false);
                span = new SnapshotSpan(point, 0);
                yield return new TagSpan<SourceMappingTag>(span, tag);
            }

            static string GetGeneratedCodeSnippet(string code, int position)
            {
                // We want to show from the previous line directive
                var start = code.LastIndexOf("#line", position);
                if (start < 0)
                {
                    return "";
                }

                // To 50 chars past the position (or document length maximum)
                var end = Math.Min(position + 50, code.Length);
                return code[start..end];
            }
        }
    }

    /// <summary>
    /// Handle buffer changes. The default implementation expands changes to full lines and sends out
    /// a <see cref="TagsChanged"/> event for these lines.
    /// </summary>
    /// <param name="args">The buffer change arguments.</param>
    private void HandleBufferChanged(TextContentChangedEventArgs args)
    {
        if (args.Changes.Count == 0)
        {
            return;
        }

        var tagsChanged = TagsChanged;
        if (tagsChanged == null)
        {
            return;
        }

        // Combine all changes into a single span so that
        // the ITagger<>.TagsChanged event can be raised just once for a compound edit
        // with many parts.

        var snapshot = args.After;

        var start = args.Changes[0].NewPosition;
        var end = args.Changes[args.Changes.Count - 1].NewEnd;

        var totalAffectedSpan = new SnapshotSpan(
            snapshot.GetLineFromPosition(start).Start,
            snapshot.GetLineFromPosition(end).End);

        tagsChanged(this, new SnapshotSpanEventArgs(totalAffectedSpan));
    }
}
