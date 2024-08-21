// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using Microsoft.CodeAnalysis.Host;
using Microsoft.CodeAnalysis.Razor.Workspaces;

namespace Microsoft.CodeAnalysis.Remote.Razor;

internal sealed class RemoteAdhocWorkspaceFactory(HostServices hostServices) : IAdhocWorkspaceFactory
{
    public AdhocWorkspace Create(params IWorkspaceService[] workspaceServices)
    {
        workspaceServices ??= [];

        var services = AdhocServices.Create(
            workspaceServices: workspaceServices.ToImmutableArray(),
            languageServices: ImmutableArray<ILanguageService>.Empty,
            fallbackServices: hostServices);

        return new AdhocWorkspace(services);
    }
}
