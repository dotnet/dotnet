// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Razor.ProjectEngineHost;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.VisualStudio.Threading;

namespace Microsoft.VisualStudio.Editor.Razor;

internal class DefaultVisualStudioRazorParserFactory(
    JoinableTaskContext joinableTaskContext,
    IErrorReporter errorReporter,
    VisualStudioCompletionBroker completionBroker,
    IProjectEngineFactoryProvider projectEngineFactoryProvider) : VisualStudioRazorParserFactory
{
    private readonly JoinableTaskContext _joinableTaskContext = joinableTaskContext;
    private readonly IProjectEngineFactoryProvider _projectEngineFactoryProvider = projectEngineFactoryProvider;
    private readonly VisualStudioCompletionBroker _completionBroker = completionBroker;
    private readonly IErrorReporter _errorReporter = errorReporter;

    public override VisualStudioRazorParser Create(VisualStudioDocumentTracker documentTracker)
    {
        if (documentTracker is null)
        {
            throw new ArgumentNullException(nameof(documentTracker));
        }

        _joinableTaskContext.AssertUIThread();

        return new DefaultVisualStudioRazorParser(
            _joinableTaskContext,
            documentTracker,
            _projectEngineFactoryProvider,
            _errorReporter,
            _completionBroker);
    }
}
