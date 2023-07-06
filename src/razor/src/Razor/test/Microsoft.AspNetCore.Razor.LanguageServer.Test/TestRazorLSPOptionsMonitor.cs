// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal class TestRazorLSPOptionsMonitor : RazorLSPOptionsMonitor
{
    private readonly IConfigurationSyncService _configuruationSyncService;

    private TestRazorLSPOptionsMonitor(
        IConfigurationSyncService configurationService,
        IOptionsMonitorCache<RazorLSPOptions> cache)
        : base(configurationService, cache, RazorLSPOptions.Default)
    {
        _configuruationSyncService = configurationService;
    }

    public bool Called { get; private set; }

    public override Task UpdateAsync(CancellationToken cancellationToken)
    {
        Called = true;
        return base.UpdateAsync();
    }

    public Task UpdateAsync(RazorLSPOptions options, CancellationToken cancellationToken)
    {
        if (_configuruationSyncService is not ConfigurationSyncService configurationSyncService)
        {
            throw new InvalidOperationException();
        }

        configurationSyncService.Options = options;
        return UpdateAsync(cancellationToken);
    }

    public static TestRazorLSPOptionsMonitor Create(
        IConfigurationSyncService? configurationService = null,
        IOptionsMonitorCache<RazorLSPOptions>? cache = null)
    {
        configurationService ??= new ConfigurationSyncService();
        cache ??= new ServiceCollection().AddOptions().BuildServiceProvider().GetRequiredService<IOptionsMonitorCache<RazorLSPOptions>>();

        return new(configurationService, cache);
    }

    private class ConfigurationSyncService : IConfigurationSyncService
    {
        public RazorLSPOptions? Options { get; set; }

        public Task<RazorLSPOptions?> GetLatestOptionsAsync(CancellationToken cancellationToken)
            => Task.FromResult(Options);
    }
}
