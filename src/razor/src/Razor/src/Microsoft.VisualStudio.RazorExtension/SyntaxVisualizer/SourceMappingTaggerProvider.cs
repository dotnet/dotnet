// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Razor;
using Microsoft.VisualStudio.Razor.Documents;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Utilities;

namespace Microsoft.VisualStudio.RazorExtension.SyntaxVisualizer;

[Export(typeof(ITaggerProvider))]
[ContentType(RazorConstants.RazorLSPContentTypeName)]
[TagType(typeof(SourceMappingTag))]
internal sealed class SourceMappingTaggerProvider : ITaggerProvider
{
    private readonly Lazy<RazorCodeDocumentProvidingSnapshotChangeTrigger> _sourceMappingProjectChangeTrigger;
    private readonly ITextDocumentFactoryService _textDocumentFactoryService;

    [ImportingConstructor]
    public SourceMappingTaggerProvider(Lazy<RazorCodeDocumentProvidingSnapshotChangeTrigger> sourceMappingProjectChangeTrigger, ITextDocumentFactoryService textDocumentFactoryService)
    {
        _sourceMappingProjectChangeTrigger = sourceMappingProjectChangeTrigger;
        _textDocumentFactoryService = textDocumentFactoryService;
    }

    public ITagger<T>? CreateTagger<T>(ITextBuffer buffer) where T : ITag
    {
        if (buffer is null)
        {
            throw new ArgumentNullException(nameof(buffer));
        }

        return SourceMappingTagger.GetOrCreateTagger(
            buffer,
            () => new SourceMappingTagger(buffer, _sourceMappingProjectChangeTrigger, _textDocumentFactoryService))
            as ITagger<T>;
    }
}
