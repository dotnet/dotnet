// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.LanguageServer.Hosting;

namespace Microsoft.AspNetCore.Razor.LanguageServer;

internal class RazorLSPOptionsMonitor
{
    private readonly IConfigurationSyncService _configurationService;
    private event Action<RazorLSPOptions>? OnChangeEvent;
    private RazorLSPOptions _currentValue;

    public RazorLSPOptionsMonitor(IConfigurationSyncService configurationService, RazorLSPOptions currentOptions)
    {
        if (configurationService is null)
        {
            throw new ArgumentNullException(nameof(configurationService));
        }

        _configurationService = configurationService;
        _currentValue = currentOptions;
    }

    public RazorLSPOptions CurrentValue => _currentValue;

    public IDisposable OnChange(Action<RazorLSPOptions> listener)
    {
        var disposable = new ChangeTrackerDisposable(this, listener);
        OnChangeEvent += disposable.OnChange;
        return disposable;
    }

    public virtual async Task UpdateAsync(CancellationToken cancellationToken = default)
    {
        var latestOptions = await _configurationService.GetLatestOptionsAsync(cancellationToken).ConfigureAwait(false);
        if (latestOptions != null)
        {
            _currentValue = latestOptions;
            InvokeChanged();
        }
    }

    private void InvokeChanged()
    {
        OnChangeEvent?.Invoke(_currentValue);
    }

    internal class ChangeTrackerDisposable : IDisposable
    {
        private readonly Action<RazorLSPOptions> _listener;
        private readonly RazorLSPOptionsMonitor _monitor;

        public ChangeTrackerDisposable(RazorLSPOptionsMonitor monitor, Action<RazorLSPOptions> listener)
        {
            _listener = listener;
            _monitor = monitor;
        }

        public void OnChange(RazorLSPOptions options) => _listener.Invoke(options);

        public void Dispose() => _monitor.OnChangeEvent -= OnChange;
    }
}
