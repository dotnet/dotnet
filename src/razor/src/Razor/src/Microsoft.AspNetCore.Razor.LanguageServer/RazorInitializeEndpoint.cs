// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.Telemetry;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CommonLanguageServerProtocol.Framework;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

[RazorLanguageServerEndpoint(Methods.InitializeName)]
internal class RazorInitializeEndpoint(
    LanguageServerFeatureOptions options,
    ITelemetryReporter telemetryReporter) : IRazorDocumentlessRequestHandler<InitializeParams, InitializeResult>
{
    private static bool s_reportedFeatureFlagState = false;

    private readonly LanguageServerFeatureOptions _options = options;
    private readonly ITelemetryReporter _telemetryReporter = telemetryReporter;

    public bool MutatesSolutionState { get; } = true;

    public Task<InitializeResult> HandleRequestAsync(InitializeParams request, RazorRequestContext requestContext, CancellationToken cancellationToken)
    {
        var capabilitiesManager = requestContext.GetRequiredService<IInitializeManager<InitializeParams, InitializeResult>>();

        capabilitiesManager.SetInitializeParams(request);
        var serverCapabilities = capabilitiesManager.GetInitializeResult();

        // Initialize can be called multiple times in a VS session, but the feature flag can't change in that time, so we only
        // need to report once. In VS Code things could change between solution loads, but each solution load starts a new rzls
        // process, so the static field gets reset anyway.
        if (!s_reportedFeatureFlagState)
        {
            s_reportedFeatureFlagState = true;
            _telemetryReporter.ReportEvent("initialize", Severity.Normal,
                new Property(nameof(LanguageServerFeatureOptions.ForceRuntimeCodeGeneration), _options.ForceRuntimeCodeGeneration),
                new Property(nameof(LanguageServerFeatureOptions.UseNewFormattingEngine), _options.UseNewFormattingEngine));
        }

        return Task.FromResult(serverCapabilities);
    }
}
