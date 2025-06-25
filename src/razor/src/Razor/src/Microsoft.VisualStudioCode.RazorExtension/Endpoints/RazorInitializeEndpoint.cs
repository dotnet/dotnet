// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Composition;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor;
using Microsoft.CodeAnalysis.ExternalAccess.Razor.Features;

namespace Microsoft.VisualStudioCode.RazorExtension.Endpoints;

[Shared]
[ExportRazorStatelessLspService(typeof(RazorInitializeEndpoint))]
[RazorEndpoint("razor/initialize")]
internal class RazorInitializeEndpoint : AbstractRazorNotificationHandler<RazorInitializeParams>
{
    public override bool MutatesSolutionState => false;

    public override bool RequiresLSPSolution => true;

    protected override Task HandleNotificationAsync(RazorInitializeParams request, RazorRequestContext requestContext, CancellationToken cancellationToken)
    {
        var workspaceService = requestContext.GetRequiredService<RazorWorkspaceService>();
        workspaceService.Initialize(requestContext.Workspace.AssumeNotNull(), request.PipeName);
        return Task.CompletedTask;
    }
}

internal class RazorInitializeParams
{
    [JsonPropertyName("pipeName")]
    public required string PipeName { get; set; }
}
