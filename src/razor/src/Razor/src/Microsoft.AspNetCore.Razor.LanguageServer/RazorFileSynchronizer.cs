// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.LanguageServer.ProjectSystem;
using Microsoft.VisualStudio.Threading;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal class RazorFileSynchronizer(IRazorProjectService projectService) : IRazorFileChangeListener
{
    private readonly IRazorProjectService _projectService = projectService;

    public void RazorFileChanged(string filePath, RazorFileChangeKind kind)
    {
        if (filePath is null)
        {
            throw new ArgumentNullException(nameof(filePath));
        }

        switch (kind)
        {
            case RazorFileChangeKind.Added:
                _projectService.AddDocumentAsync(filePath, CancellationToken.None).Forget();
                break;
            case RazorFileChangeKind.Removed:
                _projectService.RemoveDocumentAsync(filePath, CancellationToken.None).Forget();
                break;
        }
    }
}
