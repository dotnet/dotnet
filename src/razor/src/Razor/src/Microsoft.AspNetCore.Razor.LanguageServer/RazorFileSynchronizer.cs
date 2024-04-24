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
            RazorFileChangeKind.Added => _projectService.AddDocumentAsync(filePath, cancellationToken),
            RazorFileChangeKind.Removed => _projectService.RemoveDocumentAsync(filePath, cancellationToken),
            _ => Task.CompletedTask
        };
}
