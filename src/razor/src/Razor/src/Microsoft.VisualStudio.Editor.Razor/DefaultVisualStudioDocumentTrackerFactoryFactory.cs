// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Composition;
using Microsoft.CodeAnalysis.Host;
using Microsoft.CodeAnalysis.Host.Mef;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.Editor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Threading;

namespace Microsoft.VisualStudio.Editor.Razor;

[Shared]
[ExportLanguageServiceFactory(typeof(VisualStudioDocumentTrackerFactory), RazorLanguage.Name, ServiceLayer.Default)]
internal class DefaultVisualStudioDocumentTrackerFactoryFactory : ILanguageServiceFactory
{
    private readonly ProjectSnapshotManagerDispatcher _projectSnapshotManagerDispatcher;
    private readonly JoinableTaskContext _joinableTaskContext;
    private readonly ITextDocumentFactoryService _textDocumentFactory;

    [ImportingConstructor]
    public DefaultVisualStudioDocumentTrackerFactoryFactory(
        ProjectSnapshotManagerDispatcher projectSnapshotManagerDispatcher,
        JoinableTaskContext joinableTaskContext,
        ITextDocumentFactoryService textDocumentFactory)
    {
        if (projectSnapshotManagerDispatcher is null)
        {
            throw new ArgumentNullException(nameof(projectSnapshotManagerDispatcher));
        }

        if (joinableTaskContext is null)
        {
            throw new ArgumentNullException(nameof(joinableTaskContext));
        }

        if (textDocumentFactory is null)
        {
            throw new ArgumentNullException(nameof(textDocumentFactory));
        }

        _projectSnapshotManagerDispatcher = projectSnapshotManagerDispatcher;
        _joinableTaskContext = joinableTaskContext;
        _textDocumentFactory = textDocumentFactory;
    }

    public ILanguageService CreateLanguageService(HostLanguageServices languageServices)
    {
        if (languageServices is null)
        {
            throw new ArgumentNullException(nameof(languageServices));
        }

        var projectManager = languageServices.GetRequiredService<ProjectSnapshotManager>();
        var workspaceEditorSettings = languageServices.GetRequiredService<WorkspaceEditorSettings>();
        var importDocumentManager = languageServices.GetRequiredService<ImportDocumentManager>();

        var projectPathProvider = languageServices.WorkspaceServices.GetRequiredService<ProjectPathProvider>();

        return new DefaultVisualStudioDocumentTrackerFactory(
            _projectSnapshotManagerDispatcher,
            _joinableTaskContext,
            projectManager,
            workspaceEditorSettings,
            projectPathProvider,
            _textDocumentFactory,
            importDocumentManager,
            languageServices.WorkspaceServices.Workspace);
    }
}
