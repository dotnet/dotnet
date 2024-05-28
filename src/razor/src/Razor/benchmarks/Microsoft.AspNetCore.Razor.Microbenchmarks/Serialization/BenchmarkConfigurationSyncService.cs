// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

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
