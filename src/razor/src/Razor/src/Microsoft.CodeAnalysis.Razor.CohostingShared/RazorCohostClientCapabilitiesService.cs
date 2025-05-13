// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.ComponentModel.Composition;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.ExternalAccess.Razor.Cohost;
using Microsoft.CodeAnalysis.Razor.Protocol;

namespace Microsoft.VisualStudio.Razor.LanguageClient.Cohost;

[Export(typeof(IRazorCohostStartupService))]
[Export(typeof(IClientCapabilitiesService))]
internal sealed class RazorCohostClientCapabilitiesService : AbstractClientCapabilitiesService, IRazorCohostStartupService
{
    public int Order => WellKnownStartupOrder.ClientCapabilities;

    public Task StartupAsync(VSInternalClientCapabilities clientCapabilities, RazorCohostRequestContext requestContext, CancellationToken cancellationToken)
    {
        SetCapabilities(clientCapabilities);

        return Task.CompletedTask;
    }
}
