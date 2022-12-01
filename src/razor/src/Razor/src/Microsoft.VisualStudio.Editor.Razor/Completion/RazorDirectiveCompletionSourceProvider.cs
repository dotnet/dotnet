// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.ComponentModel.Composition;
using System.Linq;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.Completion;
using Microsoft.VisualStudio.Language.Intellisense.AsyncCompletion;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;

namespace Microsoft.VisualStudio.Editor.Razor.Completion;

[System.Composition.Shared]
[Export(typeof(IAsyncCompletionSourceProvider))]
[Name("Razor directive completion provider.")]
[ContentType(RazorLanguage.CoreContentType)]
[ContentType(RazorConstants.LegacyCoreContentType)]
internal class RazorDirectiveCompletionSourceProvider : IAsyncCompletionSourceProvider
{
    private readonly RazorCompletionFactsService _completionFactsService;

    [ImportingConstructor]
    public RazorDirectiveCompletionSourceProvider(RazorCompletionFactsService completionFactsService)
    {
        if (completionFactsService is null)
        {
            throw new ArgumentNullException(nameof(completionFactsService));
        }

        _completionFactsService = completionFactsService;
    }

    public IAsyncCompletionSource? GetOrCreate(ITextView textView)
    {
        if (textView is null)
        {
            throw new ArgumentNullException(nameof(textView));
        }

        var razorBuffer = textView.BufferGraph.GetRazorBuffers().FirstOrDefault();
        if (!razorBuffer.Properties.TryGetProperty(typeof(RazorDirectiveCompletionSource), out IAsyncCompletionSource? completionSource) ||
            completionSource is null)
        {
            completionSource = CreateCompletionSource(razorBuffer);
            razorBuffer.Properties.AddProperty(typeof(RazorDirectiveCompletionSource), completionSource);
        }

        return completionSource;
    }

    // Internal for testing
    internal IAsyncCompletionSource? CreateCompletionSource(ITextBuffer razorBuffer)
    {
        if (!razorBuffer.Properties.TryGetProperty(typeof(VisualStudioRazorParser), out VisualStudioRazorParser parser))
        {
            // Parser hasn't been associated with the text buffer yet.
            return null;
        }

        var completionSource = new RazorDirectiveCompletionSource(parser, _completionFactsService);
        return completionSource;
    }
}
