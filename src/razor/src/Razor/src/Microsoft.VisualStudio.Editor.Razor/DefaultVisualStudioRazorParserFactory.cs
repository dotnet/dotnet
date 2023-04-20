// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.VisualStudio.Threading;

namespace Microsoft.VisualStudio.Editor.Razor;

internal class DefaultVisualStudioRazorParserFactory : VisualStudioRazorParserFactory
{
    private readonly JoinableTaskContext _joinableTaskContext;
    private readonly ProjectSnapshotProjectEngineFactory _projectEngineFactory;
    private readonly VisualStudioCompletionBroker _completionBroker;
    private readonly IErrorReporter _errorReporter;

    public DefaultVisualStudioRazorParserFactory(
        JoinableTaskContext joinableTaskContext,
        IErrorReporter errorReporter,
        VisualStudioCompletionBroker completionBroker,
        ProjectSnapshotProjectEngineFactory projectEngineFactory)
    {
        if (joinableTaskContext is null)
        {
            throw new ArgumentNullException(nameof(joinableTaskContext));
        }

        if (errorReporter is null)
        {
            throw new ArgumentNullException(nameof(errorReporter));
        }

        if (completionBroker is null)
        {
            throw new ArgumentNullException(nameof(completionBroker));
        }

        if (projectEngineFactory is null)
        {
            throw new ArgumentNullException(nameof(projectEngineFactory));
        }

        _joinableTaskContext = joinableTaskContext;
        _errorReporter = errorReporter;
        _completionBroker = completionBroker;
        _projectEngineFactory = projectEngineFactory;
    }

    public override VisualStudioRazorParser Create(VisualStudioDocumentTracker documentTracker)
    {
        if (documentTracker is null)
        {
            throw new ArgumentNullException(nameof(documentTracker));
        }

        _joinableTaskContext.AssertUIThread();

        var parser = new DefaultVisualStudioRazorParser(
            _joinableTaskContext,
            documentTracker,
            _projectEngineFactory,
            _errorReporter,
            _completionBroker);
        return parser;
    }
}
