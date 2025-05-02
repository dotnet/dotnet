// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Composition;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.ExternalAccess.Razor;
using Microsoft.CodeAnalysis.ExternalAccess.Razor.Cohost;
using Microsoft.VisualStudio.Razor.LanguageClient.Cohost;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

[Shared]
[Export(typeof(IRazorCohostStartupService))]
[Export(typeof(RazorClientServerManagerProvider))]
[method: ImportingConstructor]
internal class RazorClientServerManagerProvider() : IRazorCohostStartupService
{
    private IRazorClientLanguageServerManager? _razorClientLanguageServerManager;

    public IRazorClientLanguageServerManager? ClientLanguageServerManager => _razorClientLanguageServerManager;

    public int Order => WellKnownStartupOrder.ClientServerManager;

    public Task StartupAsync(VSInternalClientCapabilities clientCapabilities, RazorCohostRequestContext requestContext, CancellationToken cancellationToken)
    {
        _razorClientLanguageServerManager = requestContext.GetRequiredService<IRazorClientLanguageServerManager>();
        return Task.CompletedTask;
    }
}
