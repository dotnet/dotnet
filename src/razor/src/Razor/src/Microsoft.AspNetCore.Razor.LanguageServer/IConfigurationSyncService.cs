// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

/// <summary>
/// Service that syncs settings from the client to the LSP server
/// </summary>
internal interface IConfigurationSyncService
{
    Task<RazorLSPOptions?> GetLatestOptionsAsync(CancellationToken cancellationToken);
}
