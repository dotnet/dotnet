// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Composition;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceHub.Framework;
using Microsoft.VisualStudio.Editor.Razor.Logging;
using Microsoft.VisualStudio.RpcContracts.Logging;
using VSShell = Microsoft.VisualStudio.Shell;

namespace Microsoft.VisualStudio.LanguageServices.Razor.Logging;

[Shared]
[Export(typeof(RazorLogHubTraceProvider))]
internal class VisualStudioWindowsLogHubTraceProvider : RazorLogHubTraceProvider
{
    private static readonly LoggerOptions s_logOptions = new(
        requestedLoggingLevel: new LoggingLevelSettings(SourceLevels.Information | SourceLevels.ActivityTracing),
        privacySetting: PrivacyFlags.MayContainPersonallyIdentifibleInformation | PrivacyFlags.MayContainPrivateInformation);

    private readonly SemaphoreSlim _initializationSemaphore;
    private IServiceBroker? _serviceBroker = null;
    private TraceSource? _traceSource;

    public VisualStudioWindowsLogHubTraceProvider()
    {
        _initializationSemaphore = new SemaphoreSlim(initialCount: 1, maxCount: 1);
    }

    public override async Task InitializeTraceAsync(string logIdentifier, int logHubSessionId, CancellationToken cancellationToken)
    {
        if ((await TryInitializeServiceBrokerAsync(cancellationToken).ConfigureAwait(false)) is false)
        {
            return;
        }

        var logId = new LogId(
            logName: $"{logIdentifier}.{logHubSessionId}",
            serviceId: new ServiceMoniker($"Razor.{logIdentifier}"));

        using var traceConfig = await LogHub.TraceConfiguration.CreateTraceConfigurationInstanceAsync(_serviceBroker!, ownsServiceBroker: true, cancellationToken).ConfigureAwait(false);
        _traceSource = await traceConfig.RegisterLogSourceAsync(logId, s_logOptions, cancellationToken).ConfigureAwait(false);
    }

    public override TraceSource? TryGetTraceSource()
    {
        return _traceSource;
    }

    private async Task<bool> TryInitializeServiceBrokerAsync(CancellationToken cancellationToken)
    {
        await _initializationSemaphore.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            // Check if the service broker has already been initialized
            if (_serviceBroker is not null)
            {
                return true;
            }

            if (VSShell.Package.GetGlobalService(typeof(VSShell.Interop.SAsyncServiceProvider)) is not VSShell.IAsyncServiceProvider serviceProvider)
            {
                return false;
            }

            var serviceContainer = await VSShell.ServiceExtensions.GetServiceAsync<
                VSShell.ServiceBroker.SVsBrokeredServiceContainer,
                VSShell.ServiceBroker.IBrokeredServiceContainer>(serviceProvider).ConfigureAwait(false);
            if (serviceContainer is null)
            {
                return false;
            }

            _serviceBroker = serviceContainer.GetFullAccessServiceBroker();
            if (_serviceBroker is null)
            {
                return false;
            }

            return true;
        }
        finally
        {
            _initializationSemaphore.Release();
        }
    }
}
