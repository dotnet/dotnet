// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Razor.LanguageServer.Hosting;

internal interface IClientConnection
{
    Task<TResponse> SendRequestAsync<TParams, TResponse>(string method, TParams @params, CancellationToken cancellationToken);

    Task SendNotificationAsync<TParams>(string method, TParams @params, CancellationToken cancellationToken);

    Task SendNotificationAsync(string method, CancellationToken cancellationToken);
}
