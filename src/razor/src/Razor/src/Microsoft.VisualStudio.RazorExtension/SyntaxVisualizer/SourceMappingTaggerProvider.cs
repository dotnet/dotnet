// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Editor.Razor;
using Microsoft.VisualStudio.Editor.Razor.Documents;
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
