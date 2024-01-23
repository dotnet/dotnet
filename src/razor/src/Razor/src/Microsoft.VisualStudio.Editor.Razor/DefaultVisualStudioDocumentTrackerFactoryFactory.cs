// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Composition;
using Microsoft.AspNetCore.Razor.ProjectEngineHost;
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
[method: ImportingConstructor]
internal class DefaultVisualStudioDocumentTrackerFactoryFactory(
    ProjectSnapshotManagerDispatcher dispatcher,
    JoinableTaskContext joinableTaskContext,
    ITextDocumentFactoryService textDocumentFactory,
    IProjectEngineFactoryProvider projectEngineFactoryProvider) : ILanguageServiceFactory
{
    public ILanguageService CreateLanguageService(HostLanguageServices languageServices)
    {
        var projectManager = languageServices.GetRequiredService<ProjectSnapshotManager>();
        var workspaceEditorSettings = languageServices.GetRequiredService<WorkspaceEditorSettings>();
        var importDocumentManager = languageServices.GetRequiredService<ImportDocumentManager>();

        var projectPathProvider = languageServices.WorkspaceServices.GetRequiredService<ProjectPathProvider>();

        return new DefaultVisualStudioDocumentTrackerFactory(
            dispatcher,
            joinableTaskContext,
            projectManager,
            workspaceEditorSettings,
            projectPathProvider,
            textDocumentFactory,
            importDocumentManager,
            projectEngineFactoryProvider);
    }
}
