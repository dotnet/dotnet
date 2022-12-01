// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Razor.Telemetry;
using Microsoft.CodeAnalysis.Host;
using Microsoft.CodeAnalysis.Host.Mef;
using Microsoft.CodeAnalysis.Razor;

namespace Microsoft.CodeAnalysis.Remote.Razor;

[System.Composition.Shared]
[ExportWorkspaceServiceFactory(typeof(TagHelperResolver), ServiceLayer.Host)]
internal class OOPTagHelperResolverFactory : IWorkspaceServiceFactory
{
    private readonly ITelemetryReporter _telemetryReporter;

    [System.Composition.ImportingConstructor]
    public OOPTagHelperResolverFactory(ITelemetryReporter telemetryReporter)
    {
        _telemetryReporter = telemetryReporter;
    }

    public IWorkspaceService CreateService(HostWorkspaceServices workspaceServices)
    {
        return new OOPTagHelperResolver(
            workspaceServices.GetRequiredService<ProjectSnapshotProjectEngineFactory>(),
            workspaceServices.GetRequiredService<ErrorReporter>(),
            workspaceServices.Workspace,
            _telemetryReporter);
    }
}
