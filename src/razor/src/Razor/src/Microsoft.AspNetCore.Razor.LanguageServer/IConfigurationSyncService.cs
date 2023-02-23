// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

/// <summary>
/// Service that syncs settings from the client to the LSP server
/// </summary>
internal interface IConfigurationSyncService
{
    Task<RazorLSPOptions?> GetLatestOptionsAsync(CancellationToken cancellationToken);
}
