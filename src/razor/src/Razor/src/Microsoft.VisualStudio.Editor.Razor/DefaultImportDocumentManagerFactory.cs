// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Composition;
using Microsoft.CodeAnalysis.Host;
using Microsoft.CodeAnalysis.Host.Mef;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.VisualStudio.Editor.Razor.Documents;

namespace Microsoft.VisualStudio.Editor.Razor;

[Shared]
[ExportLanguageServiceFactory(typeof(ImportDocumentManager), RazorLanguage.Name, ServiceLayer.Default)]
[method: ImportingConstructor]
internal class DefaultImportDocumentManagerFactory(
    ProjectSnapshotManagerDispatcher dispatcher,
    IErrorReporter errorReporter) : ILanguageServiceFactory
{
    public ILanguageService CreateLanguageService(HostLanguageServices languageServices)
    {
        var fileChangeTrackerFactory = languageServices.WorkspaceServices.GetRequiredService<FileChangeTrackerFactory>();

        return new DefaultImportDocumentManager(dispatcher, errorReporter, fileChangeTrackerFactory);
    }
}
