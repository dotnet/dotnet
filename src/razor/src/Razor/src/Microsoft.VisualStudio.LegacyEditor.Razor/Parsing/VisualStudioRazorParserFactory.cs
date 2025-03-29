// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.ComponentModel.Composition;
using Microsoft.AspNetCore.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Logging;

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
