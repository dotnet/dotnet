// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal class TestRazorLSPOptionsMonitor : RazorLSPOptionsMonitor
{
    private readonly IConfigurationSyncService _configurationSyncService;

    private TestRazorLSPOptionsMonitor(
        IConfigurationSyncService configurationService)
        : base(configurationService, RazorLSPOptions.Default)
    {
        _configurationSyncService = configurationService;
    }

    public bool Called { get; private set; }

    public override Task UpdateAsync(CancellationToken cancellationToken)
    {
        Called = true;
        return base.UpdateAsync();
    }

    public Task UpdateAsync(RazorLSPOptions options, CancellationToken cancellationToken)
    {
        if (_configurationSyncService is not ConfigurationSyncService configurationSyncService)
        {
            throw new InvalidOperationException();
        }

        configurationSyncService.Options = options;
        return UpdateAsync(cancellationToken);
    }

    public static TestRazorLSPOptionsMonitor Create(
        IConfigurationSyncService? configurationService = null)
    {
        configurationService ??= new ConfigurationSyncService();

        return new(configurationService);
    }

    private class ConfigurationSyncService : IConfigurationSyncService
    {
        public RazorLSPOptions? Options { get; set; }

        public Task<RazorLSPOptions?> GetLatestOptionsAsync(CancellationToken cancellationToken)
            => Task.FromResult(Options);
    }
}
