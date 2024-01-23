// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Host;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal sealed class AdhocWorkspaceFactory(HostServicesProvider hostServicesProvider) : IAdhocWorkspaceFactory
{
    public AdhocWorkspace Create(params IWorkspaceService[] workspaceServices)
    {
        workspaceServices ??= [];

        var fallbackServices = hostServicesProvider.GetServices();
        var services = AdhocServices.Create(
            workspaceServices: workspaceServices.ToImmutableArray(),
            languageServices: ImmutableArray<ILanguageService>.Empty,
            fallbackServices);

        return new AdhocWorkspace(services);
    }
}
