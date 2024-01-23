// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Composition;
using Microsoft.AspNetCore.Razor.Telemetry;
using Microsoft.CodeAnalysis.Host;
using Microsoft.CodeAnalysis.Host.Mef;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.Workspaces;

namespace Microsoft.CodeAnalysis.Remote.Razor;

[Shared]
[ExportWorkspaceServiceFactory(typeof(ITagHelperResolver), ServiceLayer.Host)]
[method: ImportingConstructor]
internal class OOPTagHelperResolverFactory(
    IErrorReporter errorReporter,
    ITelemetryReporter telemetryReporter) : IWorkspaceServiceFactory
{
    public IWorkspaceService CreateService(HostWorkspaceServices workspaceServices)
        => new OOPTagHelperResolver(
            workspaceServices.Workspace,
            errorReporter,
            telemetryReporter);
}
