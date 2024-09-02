// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Composition;
using Microsoft.CodeAnalysis.Host;
using Microsoft.CodeAnalysis.Razor.Workspaces;

namespace Microsoft.CodeAnalysis.Remote.Razor;

[Export(typeof(IAdhocWorkspaceFactory)), Shared]
internal sealed class RemoteAdhocWorkspaceFactory() : IAdhocWorkspaceFactory
{
    public AdhocWorkspace Create(params IWorkspaceService[] workspaceServices)
    {
        workspaceServices ??= [];
        var hostServices = RemoteWorkspaceAccessor.GetWorkspace().Services.HostServices;

        var services = AdhocServices.Create(
            workspaceServices: workspaceServices.ToImmutableArray(),
            languageServices: [],
            fallbackServices: hostServices);

        return new AdhocWorkspace(services);
    }
}
