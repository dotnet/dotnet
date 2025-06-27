// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Hosting;

internal interface IClientConnection
{
    Task<TResponse> SendRequestAsync<TParams, TResponse>(string method, TParams @params, CancellationToken cancellationToken);

    Task SendNotificationAsync<TParams>(string method, TParams @params, CancellationToken cancellationToken);

    Task SendNotificationAsync(string method, CancellationToken cancellationToken);
}
