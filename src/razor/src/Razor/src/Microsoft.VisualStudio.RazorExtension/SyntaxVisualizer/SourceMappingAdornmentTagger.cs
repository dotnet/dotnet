// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.Editor.Razor.Documents;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Tagging;

namespace Microsoft.VisualStudio.RazorExtension.SyntaxVisualizer;

internal sealed class SourceMappingAdornmentTagger : IntraTextAdornmentTagger<SourceMappingTag, SourceMappingAdornment>
{
    internal static ITagger<IntraTextAdornmentTag> GetTagger(IWpfTextView view, Lazy<ITagAggregator<SourceMappingTag>> sourceMappingTagger, Lazy<RazorCodeDocumentProvidingSnapshotChangeTrigger> sourceMappingProjectChangeTrigger)
    {
        return view.Properties.GetOrCreateSingletonProperty(() => new SourceMappingAdornmentTagger(view, sourceMappingTagger.Value, sourceMappingProjectChangeTrigger.Value));
    }

    private readonly ITagAggregator<SourceMappingTag> _sourceMappingTagger;

    private SourceMappingAdornmentTagger(IWpfTextView view, ITagAggregator<SourceMappingTag> sourceMappingTagger, RazorCodeDocumentProvidingSnapshotChangeTrigger sourceMappingProjectChangeTrigger)
        : base(view)
    {
        _sourceMappingTagger = sourceMappingTagger;
        sourceMappingProjectChangeTrigger.DocumentReady += SourceMappingProjectChangeTrigger_DocumentReady;
    }

    private void SourceMappingProjectChangeTrigger_DocumentReady(object sender, string e)
    {
        Refresh();
    }

    internal void Refresh()
    {
        if (_sourceMappingTagger.BufferGraph.TopBuffer == view.TextBuffer)
        {
            var snapshot = view.TextSnapshot;

            RaiseTagsChanged(new SnapshotSpan(snapshot, new Span(0, snapshot.Length)));
        }
    }

    public void Dispose()
    {
        _sourceMappingTagger.Dispose();

        view.Properties.RemoveProperty(typeof(SourceMappingAdornmentTagger));
    }

    // To produce adornments that don't obscure the text, the adornment tags
    // should have zero length spans. Overriding this method allows control
    // over the tag spans.
    protected override IEnumerable<Tuple<SnapshotSpan, PositionAffinity?, SourceMappingTag>> GetAdornmentData(NormalizedSnapshotSpanCollection spans)
    {
        if (spans.Count == 0)
            yield break;

        var snapshot = spans[0].Snapshot;

        var sourceMappingTags = _sourceMappingTagger.GetTags(spans);

        foreach (var dataTagSpan in sourceMappingTags)
        {
            var sourceMappingSpans = dataTagSpan.Span.GetSpans(snapshot);

            // Ignore data tags that are split by projection.
            // This is theoretically possible but unlikely in current scenarios.
            if (sourceMappingSpans.Count != 1)
                continue;

            var adornmentSpan = new SnapshotSpan(sourceMappingSpans[0].Start, 0);

            yield return Tuple.Create(adornmentSpan, (PositionAffinity?)PositionAffinity.Successor, dataTagSpan.Tag);
        }
    }

    protected override SourceMappingAdornment CreateAdornment(SourceMappingTag dataTag, SnapshotSpan span)
    {
        return new SourceMappingAdornment(dataTag.IsStart, dataTag.ToolTipText);
    }

    protected override bool UpdateAdornment(SourceMappingAdornment adornment, SourceMappingTag dataTag)
    {
        return true;
    }
}
