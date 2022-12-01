// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Razor.Telemetry;

namespace Microsoft.CodeAnalysis.Razor;

// Provides access to Razor language and workspace services that are avialable in the OOP host.
//
// Since we don't have access to the workspace we only have access to some specific things
// that we can construct directly.
internal sealed class RazorServices
{
    public RazorServices(ITelemetryReporter telemetryReporter)
    {
        FallbackProjectEngineFactory = new FallbackProjectEngineFactory();
        TagHelperResolver = new RemoteTagHelperResolver(FallbackProjectEngineFactory, telemetryReporter);
    }

    public IFallbackProjectEngineFactory FallbackProjectEngineFactory { get; }

    public RemoteTagHelperResolver TagHelperResolver { get; }
}
