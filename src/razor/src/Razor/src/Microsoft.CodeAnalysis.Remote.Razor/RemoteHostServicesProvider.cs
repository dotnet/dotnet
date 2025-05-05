// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Composition;
using Microsoft.AspNetCore.Razor;
using Microsoft.CodeAnalysis.Host;
using Microsoft.CodeAnalysis.Razor.Workspaces;

namespace Microsoft.CodeAnalysis.Remote.Razor;

[Shared]
[Export(typeof(IHostServicesProvider))]
[Export(typeof(RemoteHostServicesProvider))]
internal sealed class RemoteHostServicesProvider : IHostServicesProvider
{
    private IWorkspaceProvider? _workspaceProvider;

    public void SetWorkspaceProvider(IWorkspaceProvider workspaceProvider)
    {
        _workspaceProvider = workspaceProvider;
    }

    public HostServices GetServices()
    {
        return _workspaceProvider.AssumeNotNull().GetWorkspace().Services.HostServices;
    }
}
