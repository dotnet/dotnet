// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Composition;
using Microsoft.CodeAnalysis.Host;
using Microsoft.CodeAnalysis.Razor.Workspaces;

namespace Microsoft.CodeAnalysis.Remote.Razor;

[Export(typeof(IHostServicesProvider)), Shared]
internal sealed class RemoteHostServicesProvider : IHostServicesProvider
{
    public HostServices GetServices()
    {
        return RemoteWorkspaceAccessor.GetWorkspace().Services.HostServices;
    }
}
