// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Protocols.Configuration;
using Microsoft.IdentityModel.Telemetry;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.IdentityModel.Protocols
{
    public partial class ConfigurationManager<T> where T : class
    {
        private readonly SemaphoreSlim _refreshLock = new(1, 1);
        private TimeSpan _bootstrapRefreshInterval = TimeSpan.FromSeconds(1);

        /// <summary>
        /// Used to track the type of request for signaling the event handler and for telemetry.
        /// </summary>
        private bool _refreshRequested;

        private async Task<T> GetConfigurationWithBlockingAsync(CancellationToken cancel)
        {
            Exception _fetchMetadataFailure = null;
            await _refreshLock.WaitAsync(cancel).ConfigureAwait(false);

            long startTimestamp = TimeProvider.GetTimestamp();

            try
            {
                if (_syncAfter <= TimeProvider.GetUtcNow())
                {
                    var retrievalContext = new ConfigurationRetrievalContext { BypassCache = _refreshRequested };
                    try
                    {
                        // Check if event handler can provide configuration
                        // If provided configuration is valid, skip regular retriaval process and update current configuration.
                        if (ConfigurationEventHandler != null)
                        {
                            ConfigurationEventHandlerResult<T> configurationRetrieved =
                                await HandleBeforeRetrieveAsync(retrievalContext, cancel).ConfigureAwait(false);

                            // replicate the behavior of successful retrieval from endpoint
                            if (configurationRetrieved != null && configurationRetrieved.Configuration != null)
                            {
                                TelemetryForUpdateBlocking(TelemetryConstants.Protocols.ConfigurationSourceHandler);

                                if (_refreshRequested)
                                    _refreshRequested = false;

                                UpdateConfiguration(configurationRetrieved.Configuration, configurationRetrieved.RetrievalTime, retrievalContext);

                                return _currentConfiguration;
                            }
                        }

                        // Don't use the individual CT here, this is a shared operation that shouldn't be affected by an individual's cancellation.
                        // The transport should have it's own timeouts, etc..
                        var configuration = await _configRetriever.GetConfigurationAsync(MetadataAddress, _docRetriever, CancellationToken.None).ConfigureAwait(false);

                        var elapsedTime = TimeProvider.GetElapsedTime(startTimestamp);
                        TelemetryClient.LogConfigurationRetrievalDuration(
                            MetadataAddress,
                            TelemetryConstants.Protocols.ConfigurationSourceRetriever,
                            elapsedTime);

                        if (_configValidator != null)
                        {
                            ConfigurationValidationResult result = _configValidator.Validate(configuration);
                            if (!result.Succeeded)
                                throw LogHelper.LogExceptionMessage(new InvalidConfigurationException(LogHelper.FormatInvariant(LogMessages.IDX20810, result.ErrorMessage)));
                        }

                        _lastRequestRefresh = TimeProvider.GetUtcNow().UtcDateTime;

                        TelemetryForUpdateBlocking(TelemetryConstants.Protocols.ConfigurationSourceRetriever);

                        if (_refreshRequested)
                            _refreshRequested = false;

                        UpdateConfiguration(configuration, TimeProvider.GetUtcNow(), retrievalContext);
                    }
                    catch (Exception ex)
                    {
                        _fetchMetadataFailure = ex;

                        if (_currentConfiguration == null)
                        {
                            if (_bootstrapRefreshInterval < RefreshInterval)
                            {
                                // Adopt exponential backoff for bootstrap refresh interval with a decorrelated jitter if it is not longer than the refresh interval.
                                TimeSpan _bootstrapRefreshIntervalWithJitter = TimeSpan.FromSeconds(new Random().Next((int)_bootstrapRefreshInterval.TotalSeconds));
                                _bootstrapRefreshInterval += _bootstrapRefreshInterval;
                                _syncAfter = DateTimeUtil.Add(DateTime.UtcNow, _bootstrapRefreshIntervalWithJitter);
                            }
                            else
                            {
                                _syncAfter = DateTimeUtil.Add(
                                    TimeProvider.GetUtcNow().UtcDateTime,
                                    AutomaticRefreshInterval < RefreshInterval ? AutomaticRefreshInterval : RefreshInterval);
                            }

                            TelemetryClient.IncrementConfigurationRefreshRequestCounter(
                                MetadataAddress,
                                TelemetryConstants.Protocols.FirstRefresh,
                                TelemetryConstants.Protocols.ConfigurationSourceRetriever,
                                ex);

                            throw LogHelper.LogExceptionMessage(
                                new InvalidOperationException(
                                    LogHelper.FormatInvariant(LogMessages.IDX20803, LogHelper.MarkAsNonPII(MetadataAddress ?? "null"), LogHelper.MarkAsNonPII(_syncAfter), LogHelper.MarkAsNonPII(ex)), ex));
                        }
                        else
                        {
                            _syncAfter = DateTimeUtil.Add(
                                TimeProvider.GetUtcNow().UtcDateTime,
                                AutomaticRefreshInterval < RefreshInterval ? AutomaticRefreshInterval : RefreshInterval);

                            var elapsedTime = TimeProvider.GetElapsedTime(startTimestamp);

                            TelemetryClient.LogConfigurationRetrievalDuration(
                                MetadataAddress,
                                TelemetryConstants.Protocols.ConfigurationSourceRetriever,
                                elapsedTime,
                                ex);

                            LogHelper.LogExceptionMessage(
                                new InvalidOperationException(
                                    LogHelper.FormatInvariant(LogMessages.IDX20806, LogHelper.MarkAsNonPII(MetadataAddress ?? "null"), LogHelper.MarkAsNonPII(ex)), ex));
                        }
                    }
                }

                // Stale metadata is better than no metadata
                if (_currentConfiguration != null)
                    return _currentConfiguration;
                else
                    throw LogHelper.LogExceptionMessage(
                        new InvalidOperationException(
                            LogHelper.FormatInvariant(
                                LogMessages.IDX20803,
                                LogHelper.MarkAsNonPII(MetadataAddress ?? "null"),
                                LogHelper.MarkAsNonPII(_syncAfter),
                                LogHelper.MarkAsNonPII(_fetchMetadataFailure)),
                            _fetchMetadataFailure));
            }
            finally
            {
                _refreshLock.Release();
            }
        }

        private void RequestRefreshBlocking()
        {
            DateTime now = TimeProvider.GetUtcNow().UtcDateTime;

            if (now >= DateTimeUtil.Add(_lastRequestRefresh.UtcDateTime, RefreshInterval) || _isFirstRefreshRequest)
            {
                _refreshRequested = true;
                _syncAfter = now;
                _isFirstRefreshRequest = false;
            }
        }

        private void TelemetryForUpdateBlocking(string configurationSource)
        {
            string updateMode;

            if (_currentConfiguration is null)
                updateMode = TelemetryConstants.Protocols.FirstRefresh;
            else
                updateMode = _refreshRequested ? TelemetryConstants.Protocols.Manual : TelemetryConstants.Protocols.Automatic;

            try
            {
                TelemetryClient.IncrementConfigurationRefreshRequestCounter(
                    MetadataAddress,
                    updateMode,
                    configurationSource);
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch
            { }
#pragma warning restore CA1031 // Do not catch general exception types
        }
    }
}
