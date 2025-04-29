// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Composition;
using System.Text.Json.Serialization;
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
