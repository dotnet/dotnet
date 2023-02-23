// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal class TestRazorLSPOptionsMonitor : RazorLSPOptionsMonitor
{
    public TestRazorLSPOptionsMonitor(
        IConfigurationSyncService configurationService,
        IOptionsMonitorCache<RazorLSPOptions> cache)
        : base(configurationService, cache, RazorLSPOptions.Default)
    {
    }

    public bool Called { get; private set; }

    public override Task UpdateAsync(CancellationToken cancellationToken)
    {
        Called = true;
        return base.UpdateAsync();
    }
}
