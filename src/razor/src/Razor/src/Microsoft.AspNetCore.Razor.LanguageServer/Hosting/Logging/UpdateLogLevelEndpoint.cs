// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting.Logging;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.Protocol;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Hosting.Logging;

[RazorLanguageServerEndpoint(CustomMessageNames.RazorUpdateLogLevelName)]
internal class UpdateLogLevelEndpoint(LogLevelProvider logLevelProvider) : IRazorNotificationHandler<UpdateLogLevelParams>
{
    public bool MutatesSolutionState => false;

    public Task HandleNotificationAsync(UpdateLogLevelParams request, RazorRequestContext requestContext, CancellationToken cancellationToken)
    {
        logLevelProvider.Current = (LogLevel)request.LogLevel;
        return Task.CompletedTask;
    }
}
