// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

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
