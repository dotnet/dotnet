﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Razor.LanguageServer.EndpointContracts;
using Microsoft.CodeAnalysis.Razor.Logging;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.CodeAnalysis.Razor.Protocol;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Hosting.NamedPipes;

[RazorLanguageServerEndpoint(CustomMessageNames.RazorNamedPipeConnectEndpointName)]
internal sealed class RazorNamedPipeConnectEndpoint(IRazorProjectInfoDriver infoDriver, ILoggerFactory loggerFactory) : IRazorNotificationHandler<RazorNamedPipeConnectParams>
{
    private readonly IRazorProjectInfoDriver _infoDriver = infoDriver;
    private readonly ILogger _logger = loggerFactory.GetOrCreateLogger<RazorNamedPipeConnectEndpoint>();

    public bool MutatesSolutionState => false;

    public Task HandleNotificationAsync(RazorNamedPipeConnectParams request, RazorRequestContext requestContext, CancellationToken cancellationToken)
    {
        if (_infoDriver is not INamedPipeProjectInfoDriver namedPipeDriver)
        {
            _logger.LogError($"Named pipe communication is attempting to be set up when a valid driver is not available.");
            return Task.CompletedTask;
        }

        return namedPipeDriver.CreateNamedPipeAsync(request.PipeName, cancellationToken);
    }
}
