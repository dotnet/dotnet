// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Razor;
using Microsoft.VisualStudio.Razor.Documents;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Utilities;

namespace Microsoft.VisualStudio.RazorExtension.SyntaxVisualizer;

[Export(typeof(IViewTaggerProvider))]
[ContentType(RazorConstants.RazorLSPContentTypeName)]
[TagType(typeof(IntraTextAdornmentTag))]
internal sealed class SourceMappingAdornmentTaggerProvider : IViewTaggerProvider
{
    private readonly IBufferTagAggregatorFactoryService _bufferTagAggregatorFactoryService;
    private readonly Lazy<RazorCodeDocumentProvidingSnapshotChangeTrigger> _sourceMappingProjectChangeTrigger;

    [ImportingConstructor]
    public SourceMappingAdornmentTaggerProvider(IBufferTagAggregatorFactoryService bufferTagAggregatorFactoryService, Lazy<RazorCodeDocumentProvidingSnapshotChangeTrigger> sourceMappingProjectChangeTrigger)
    {
        _bufferTagAggregatorFactoryService = bufferTagAggregatorFactoryService;
        _sourceMappingProjectChangeTrigger = sourceMappingProjectChangeTrigger;
    }

    public ITagger<T>? CreateTagger<T>(ITextView textView, ITextBuffer buffer) where T : ITag
    {
        if (textView is null)
        {
            throw new ArgumentNullException(nameof(textView));
        }

        if (buffer is null)
        {
            throw new ArgumentNullException(nameof(buffer));
        }

        if (buffer != textView.TextBuffer)
        {
            return null;
        }

        return SourceMappingAdornmentTagger.GetTagger(
            (IWpfTextView)textView,
            new Lazy<ITagAggregator<SourceMappingTag>>(
                () => _bufferTagAggregatorFactoryService.CreateTagAggregator<SourceMappingTag>(textView.TextBuffer)),
            _sourceMappingProjectChangeTrigger)
            as ITagger<T>;
    }
}
