// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Composition;
using Microsoft.AspNetCore.Razor.Telemetry;
using Microsoft.CodeAnalysis.Host;
using Microsoft.CodeAnalysis.Host.Mef;

namespace Microsoft.CodeAnalysis.Razor;

[Shared]
[ExportWorkspaceServiceFactory(typeof(TagHelperResolver), ServiceLayer.Default)]
internal class DefaultTagHelperResolverFactory : IWorkspaceServiceFactory
{
    private readonly ITelemetryReporter _telemetryReporter;

    [ImportingConstructor]
    public DefaultTagHelperResolverFactory(ITelemetryReporter telemetryReporter)
    {
        _telemetryReporter = telemetryReporter ?? throw new ArgumentNullException(nameof(telemetryReporter));
    }

    public IWorkspaceService CreateService(HostWorkspaceServices workspaceServices)
    {
        return new DefaultTagHelperResolver(_telemetryReporter);
    }
}
