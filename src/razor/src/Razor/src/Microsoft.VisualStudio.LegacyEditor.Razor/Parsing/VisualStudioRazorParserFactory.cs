// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel.Composition;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.ProjectEngineHost;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Razor.Extensions;
using Microsoft.VisualStudio.Threading;

namespace Microsoft.VisualStudio.LegacyEditor.Razor.Parsing;

[Export(typeof(IVisualStudioRazorParserFactory))]
[method: ImportingConstructor]
internal sealed class VisualStudioRazorParserFactory(
    IProjectEngineFactoryProvider projectEngineFactoryProvider,
    ICompletionBroker completionBroker,
    ILoggerFactory loggerFactory,
    JoinableTaskContext joinableTaskContext) : IVisualStudioRazorParserFactory
{
    private readonly IProjectEngineFactoryProvider _projectEngineFactoryProvider = projectEngineFactoryProvider;
    private readonly ICompletionBroker _completionBroker = completionBroker;
    private readonly ILoggerFactory _loggerFactory = loggerFactory;
    private readonly JoinableTaskContext _joinableTaskContext = joinableTaskContext;

    public IVisualStudioRazorParser Create(IVisualStudioDocumentTracker documentTracker)
    {
        _joinableTaskContext.AssertUIThread();

        return new VisualStudioRazorParser(
            documentTracker,
            _projectEngineFactoryProvider,
            _completionBroker,
            _loggerFactory,
            _joinableTaskContext);
    }
}
