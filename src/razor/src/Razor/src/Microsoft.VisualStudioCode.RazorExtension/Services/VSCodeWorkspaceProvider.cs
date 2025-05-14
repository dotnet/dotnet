// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Composition;
using Microsoft.AspNetCore.Razor;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Razor.Workspaces;

namespace Microsoft.VisualStudioCode.RazorExtension.Services;

[Shared]
[Export(typeof(IWorkspaceProvider))]
[Export(typeof(VSCodeWorkspaceProvider))]
internal sealed class VSCodeWorkspaceProvider : IWorkspaceProvider
{
    private Workspace? _workspace;

    public void SetWorkspace(Workspace workspace)
    {
        _workspace = workspace;
    }

    public Workspace GetWorkspace()
    {
        return _workspace ?? Assumed.Unreachable<Workspace>("Accessing the workspace before it has been provided");
    }
}
