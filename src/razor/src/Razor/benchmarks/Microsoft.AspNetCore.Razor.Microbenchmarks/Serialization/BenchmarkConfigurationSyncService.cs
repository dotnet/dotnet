// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting;
using Microsoft.AspNetCore.Razor.Threading;

namespace Microsoft.AspNetCore.Razor.Microbenchmarks.Serialization;

internal class BenchmarkConfigurationSyncService : IConfigurationSyncService
{
    public Task<RazorLSPOptions?> GetLatestOptionsAsync(CancellationToken cancellationToken)
    {
        return SpecializedTasks.Null<RazorLSPOptions>();
    }
}
