// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.ComponentModel.Composition;
using System.Linq;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.Completion;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Language.Intellisense.AsyncCompletion;
using Microsoft.VisualStudio.LegacyEditor.Razor.Parsing;
using Microsoft.VisualStudio.Razor;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Threading;
using Microsoft.VisualStudio.Utilities;

namespace Microsoft.VisualStudio.LegacyEditor.Razor.Completion;

[Export(typeof(IAsyncCompletionSourceProvider))]
[Name("Razor directive attribute completion provider.")]
[ContentType(RazorLanguage.CoreContentType)]
[ContentType(RazorConstants.LegacyCoreContentType)]
[method: ImportingConstructor]
internal sealed class RazorDirectiveAttributeCompletionSourceProvider(
    IRazorCompletionFactsService completionFactsService,
    ICompletionBroker completionBroker,
    IVisualStudioDescriptionFactory descriptionFactory,
    JoinableTaskContext joinableTaskContext) : IAsyncCompletionSourceProvider
{
    private readonly IRazorCompletionFactsService _completionFactsService = completionFactsService;
    private readonly ICompletionBroker _completionBroker = completionBroker;
    private readonly IVisualStudioDescriptionFactory _descriptionFactory = descriptionFactory;
    private readonly JoinableTaskContext _joinableTaskContext = joinableTaskContext;

    public IAsyncCompletionSource? GetOrCreate(ITextView textView)
    {
        if (textView is null)
        {
            throw new ArgumentNullException(nameof(textView));
        }

        var razorBuffer = textView.BufferGraph.GetRazorBuffers().FirstOrDefault();
        if (!razorBuffer.Properties.TryGetProperty(typeof(RazorDirectiveAttributeCompletionSource), out IAsyncCompletionSource? completionSource) ||
            completionSource is null)
        {
            completionSource = CreateCompletionSource(razorBuffer);
            razorBuffer.Properties.AddProperty(typeof(RazorDirectiveAttributeCompletionSource), completionSource);
        }

        return completionSource;
    }

    // Internal for testing
    internal IAsyncCompletionSource? CreateCompletionSource(ITextBuffer razorBuffer)
    {
        if (!razorBuffer.Properties.TryGetProperty(typeof(IVisualStudioRazorParser), out IVisualStudioRazorParser parser))
        {
            // Parser hasn't been associated with the text buffer yet.
            return null;
        }

        return new RazorDirectiveAttributeCompletionSource(
            parser,
            _completionFactsService,
            _completionBroker,
            _descriptionFactory,
            _joinableTaskContext.Factory);
    }
}
