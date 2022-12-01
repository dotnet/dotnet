// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.ComponentModel.Composition;
using System.Linq;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.Completion;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Language.Intellisense.AsyncCompletion;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Threading;
using Microsoft.VisualStudio.Utilities;

namespace Microsoft.VisualStudio.Editor.Razor.Completion;

[System.Composition.Shared]
[Export(typeof(IAsyncCompletionSourceProvider))]
[Name("Razor directive attribute completion provider.")]
[ContentType(RazorLanguage.CoreContentType)]
[ContentType(RazorConstants.LegacyCoreContentType)]
internal class RazorDirectiveAttributeCompletionSourceProvider : IAsyncCompletionSourceProvider
{
    private readonly ProjectSnapshotManagerDispatcher _projectSnapshotManagerDispatcher;
    private readonly RazorCompletionFactsService _completionFactsService;
    private readonly ICompletionBroker _completionBroker;
    private readonly VisualStudioDescriptionFactory _descriptionFactory;
    private readonly JoinableTaskContext _joinableTaskContext;

    [ImportingConstructor]
    public RazorDirectiveAttributeCompletionSourceProvider(
        ProjectSnapshotManagerDispatcher projectSnapshotManagerDispatcher,
        RazorCompletionFactsService completionFactsService,
        IAsyncCompletionBroker asyncCoompletionBroker,
        ICompletionBroker completionBroker,
        VisualStudioDescriptionFactory descriptionFactory,
        JoinableTaskContext joinableTaskContext)
    {
        if (projectSnapshotManagerDispatcher is null)
        {
            throw new ArgumentNullException(nameof(projectSnapshotManagerDispatcher));
        }

        if (completionFactsService is null)
        {
            throw new ArgumentNullException(nameof(completionFactsService));
        }

        if (asyncCoompletionBroker is null)
        {
            throw new ArgumentNullException(nameof(asyncCoompletionBroker));
        }

        if (descriptionFactory is null)
        {
            throw new ArgumentNullException(nameof(descriptionFactory));
        }

        if (joinableTaskContext is null)
        {
            throw new ArgumentNullException(nameof(joinableTaskContext));
        }

        _projectSnapshotManagerDispatcher = projectSnapshotManagerDispatcher;
        _completionFactsService = completionFactsService;
        _completionBroker = completionBroker;
        _descriptionFactory = descriptionFactory;
        _joinableTaskContext = joinableTaskContext;
    }

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
        if (!razorBuffer.Properties.TryGetProperty(typeof(VisualStudioRazorParser), out VisualStudioRazorParser parser))
        {
            // Parser hasn't been associated with the text buffer yet.
            return null;
        }

        var completionSource = new RazorDirectiveAttributeCompletionSource(
            _projectSnapshotManagerDispatcher,
            parser,
            _completionFactsService,
            _completionBroker,
            _descriptionFactory,
            _joinableTaskContext.Factory);
        return completionSource;
    }
}
