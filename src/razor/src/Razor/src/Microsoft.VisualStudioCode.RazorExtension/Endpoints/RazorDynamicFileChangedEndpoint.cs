// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Composition;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.ExternalAccess.Razor.Features;
using Microsoft.CodeAnalysis.Razor;

namespace Microsoft.VisualStudioCode.RazorExtension.Endpoints;

[Shared]
[ExportRazorStatelessLspService(typeof(RazorDynamicFileChangedEndpoint))]
[RazorEndpoint("razor/dynamicFileInfoChanged")]
internal class RazorDynamicFileChangedEndpoint : AbstractRazorNotificationHandler<RazorDynamicFileChangedParams>
{
    public override bool MutatesSolutionState => false;

    public override bool RequiresLSPSolution => false;

    protected override Task HandleNotificationAsync(RazorDynamicFileChangedParams request, RazorRequestContext context, CancellationToken cancellationToken)
    {
        var dynamicFileInfoProvider = context.GetRequiredService<RazorLspDynamicFileInfoProvider>();
        dynamicFileInfoProvider.Update(request.RazorDocument.DocumentUri.GetRequiredParsedUri());

        return Task.CompletedTask;
    }
}
