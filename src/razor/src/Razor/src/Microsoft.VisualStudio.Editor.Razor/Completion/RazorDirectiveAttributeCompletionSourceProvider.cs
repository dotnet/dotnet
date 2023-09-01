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
    private readonly IRazorCompletionFactsService _completionFactsService;
    private readonly ICompletionBroker _completionBroker;
    private readonly IVisualStudioDescriptionFactory _descriptionFactory;
    private readonly JoinableTaskContext _joinableTaskContext;

    [ImportingConstructor]
    public RazorDirectiveAttributeCompletionSourceProvider(
        ProjectSnapshotManagerDispatcher projectSnapshotManagerDispatcher,
        IRazorCompletionFactsService completionFactsService,
        ICompletionBroker completionBroker,
        IVisualStudioDescriptionFactory descriptionFactory,
        JoinableTaskContext joinableTaskContext)
    {
        _projectSnapshotManagerDispatcher = projectSnapshotManagerDispatcher ?? throw new ArgumentNullException(nameof(projectSnapshotManagerDispatcher));
        _completionFactsService = completionFactsService ?? throw new ArgumentNullException(nameof(completionFactsService));
        _completionBroker = completionBroker ?? throw new ArgumentNullException(nameof(completionBroker));
        _descriptionFactory = descriptionFactory ?? throw new ArgumentNullException(nameof(descriptionFactory));
        _joinableTaskContext = joinableTaskContext ?? throw new ArgumentNullException(nameof(joinableTaskContext));
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

        return new RazorDirectiveAttributeCompletionSource(
            _projectSnapshotManagerDispatcher,
            parser,
            _completionFactsService,
            _completionBroker,
            _descriptionFactory,
            _joinableTaskContext.Factory);
    }
}
