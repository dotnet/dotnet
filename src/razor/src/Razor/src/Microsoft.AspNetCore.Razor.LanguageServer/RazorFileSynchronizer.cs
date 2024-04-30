// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;
using Microsoft.VisualStudio.Threading;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal class RazorFileSynchronizer(IRazorProjectService projectService) : IRazorFileChangeListener
{
    private readonly IRazorProjectService _projectService = projectService;

    public Task RazorFileChangedAsync(string filePath, RazorFileChangeKind kind, CancellationToken cancellationToken)
        => kind switch
        {
            // We put the new file in the misc files project, so we don't confuse the client by sending updates for
            // a razor file that we guess is going to be in a project, when the client might not have received that
            // info yet. When the client does find out, it will tell us by updating the project info, and we'll
            // migrate the file as necessary.
            RazorFileChangeKind.Added => _projectService.AddDocumentToMiscProjectAsync(filePath, cancellationToken),
            RazorFileChangeKind.Removed => _projectService.RemoveDocumentAsync(filePath, cancellationToken),
            _ => Task.CompletedTask
        };
}
