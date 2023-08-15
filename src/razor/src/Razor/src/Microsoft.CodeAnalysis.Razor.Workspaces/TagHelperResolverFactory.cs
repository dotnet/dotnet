// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Composition;
using Microsoft.AspNetCore.Razor.Telemetry;
using Microsoft.CodeAnalysis.Host;
using Microsoft.CodeAnalysis.Host.Mef;
using Microsoft.CodeAnalysis.Razor.Workspaces;

namespace Microsoft.CodeAnalysis.Razor;

[Shared]
[ExportWorkspaceServiceFactory(typeof(ITagHelperResolver), ServiceLayer.Default)]
[method: ImportingConstructor]
internal partial class TagHelperResolverFactory(ITelemetryReporter telemetryReporter) : IWorkspaceServiceFactory
{
    public IWorkspaceService CreateService(HostWorkspaceServices workspaceServices)
        => new Resolver(telemetryReporter);
}
