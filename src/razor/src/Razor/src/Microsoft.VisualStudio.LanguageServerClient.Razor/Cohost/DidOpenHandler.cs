// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Composition;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;
using Microsoft.AspNetCore.Razor.Utilities;
using Microsoft.CodeAnalysis.ExternalAccess.Razor.Cohost;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor.Cohost;

[Export(typeof(IRazorCohostDidOpenHandler)), Shared]
[method: ImportingConstructor]
internal class DidOpenHandler(
    ProjectSnapshotManagerDispatcher projectSnapshotManagerDispatcher,
    IRazorProjectService razorProjectService,
    OpenDocumentGenerator openDocumentGenerator) : IRazorCohostDidOpenHandler
{
    private readonly ProjectSnapshotManagerDispatcher _projectSnapshotManagerDispatcher = projectSnapshotManagerDispatcher;
    private readonly IRazorProjectService _razorProjectService = razorProjectService;
    private readonly OpenDocumentGenerator _openDocumentGenerator = openDocumentGenerator;

    public async Task HandleAsync(Uri uri, int version, SourceText sourceText, CancellationToken cancellationToken)
    {
        await await _projectSnapshotManagerDispatcher.RunOnDispatcherThreadAsync(() =>
        {
            var textDocumentPath = FilePathNormalizer.Normalize(uri.GetAbsoluteOrUNCPath());
            _razorProjectService.OpenDocument(textDocumentPath, sourceText, version);

            return _openDocumentGenerator.DocumentOpenedOrChangedAsync(textDocumentPath, version, cancellationToken);
        }, cancellationToken).ConfigureAwait(false);
    }
}
