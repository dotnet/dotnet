// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Composition;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;
using Microsoft.CodeAnalysis.ExternalAccess.Razor.Cohost;
using Microsoft.CodeAnalysis.Razor;

namespace Microsoft.VisualStudio.LanguageServerClient.Razor.Cohost;

[Export(typeof(IRazorCohostDidCloseHandler)), Shared]
[method: ImportingConstructor]
internal class DidCloseHandler(ProjectSnapshotManagerDispatcher projectSnapshotManagerDispatcher, RazorProjectService razorProjectService) : IRazorCohostDidCloseHandler
{
    private readonly ProjectSnapshotManagerDispatcher _projectSnapshotManagerDispatcher = projectSnapshotManagerDispatcher;
    private readonly RazorProjectService _razorProjectService = razorProjectService;

    public Task HandleAsync(Uri uri, CancellationToken cancellationToken)
    {
        return _projectSnapshotManagerDispatcher.RunOnDispatcherThreadAsync(
            () => _razorProjectService.CloseDocument(uri.GetAbsoluteOrUNCPath()),
            cancellationToken);
    }
}
