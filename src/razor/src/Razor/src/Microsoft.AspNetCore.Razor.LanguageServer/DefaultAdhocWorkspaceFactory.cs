// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Host;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal class DefaultAdhocWorkspaceFactory : AdhocWorkspaceFactory
{
    private readonly HostServicesProvider _hostServicesProvider;

    public DefaultAdhocWorkspaceFactory(HostServicesProvider hostWorkspaceServicesProvider)
    {
        if (hostWorkspaceServicesProvider is null)
        {
            throw new ArgumentNullException(nameof(hostWorkspaceServicesProvider));
        }

        _hostServicesProvider = hostWorkspaceServicesProvider;
    }

    public override AdhocWorkspace Create() => Create(Enumerable.Empty<IWorkspaceService>());

    public override AdhocWorkspace Create(IEnumerable<IWorkspaceService> workspaceServices)
    {
        if (workspaceServices is null)
        {
            throw new ArgumentNullException(nameof(workspaceServices));
        }

        var fallbackServices = _hostServicesProvider.GetServices();
        var services = AdhocServices.Create(
            workspaceServices,
            razorLanguageServices: Enumerable.Empty<ILanguageService>(),
            fallbackServices);
        var workspace = new AdhocWorkspace(services);
        return workspace;
    }
}
