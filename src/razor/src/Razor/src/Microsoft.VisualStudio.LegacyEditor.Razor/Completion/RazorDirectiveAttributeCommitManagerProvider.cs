// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.ComponentModel.Composition;
using System.Linq;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.VisualStudio.Language.Intellisense.AsyncCompletion;
using Microsoft.VisualStudio.Razor;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;

namespace Microsoft.VisualStudio.LegacyEditor.Razor.Completion;

[Export(typeof(IAsyncCompletionCommitManagerProvider))]
[Name("Razor directive attribute completion commit provider.")]
[ContentType(RazorLanguage.CoreContentType)]
[ContentType(RazorConstants.LegacyCoreContentType)]
internal sealed class RazorDirectiveAttributeCommitManagerProvider : IAsyncCompletionCommitManagerProvider
{
    public IAsyncCompletionCommitManager GetOrCreate(ITextView textView)
    {
        if (textView is null)
        {
            throw new ArgumentNullException(nameof(textView));
        }

        var razorBuffer = textView.BufferGraph.GetRazorBuffers().FirstOrDefault();
        if (!razorBuffer.Properties.TryGetProperty(typeof(RazorDirectiveAttributeCommitManager), out IAsyncCompletionCommitManager? completionSource) ||
            completionSource is null)
        {
            completionSource = new RazorDirectiveAttributeCommitManager();
            razorBuffer.Properties.AddProperty(typeof(RazorDirectiveAttributeCommitManager), completionSource);
        }

        return completionSource;
    }
}
