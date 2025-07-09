// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.CodeAnalysis.Razor.Telemetry;
using Microsoft.CodeAnalysis.Razor.Workspaces;
using Microsoft.CommonLanguageServerProtocol.Framework;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

[RazorLanguageServerEndpoint(Methods.InitializeName)]
internal class RazorInitializeEndpoint : IRazorDocumentlessRequestHandler<InitializeParams, InitializeResult>
{
    public bool MutatesSolutionState { get; } = true;

    public Task<InitializeResult> HandleRequestAsync(InitializeParams request, RazorRequestContext requestContext, CancellationToken cancellationToken)
    {
        var capabilitiesManager = requestContext.GetRequiredService<IInitializeManager<InitializeParams, InitializeResult>>();

        capabilitiesManager.SetInitializeParams(request);
        var serverCapabilities = capabilitiesManager.GetInitializeResult();

        return Task.FromResult(serverCapabilities);
    }
}
