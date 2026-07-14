// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Protocols.Configuration;
using Microsoft.IdentityModel.Telemetry;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.IdentityModel.Protocols
{
    /// <summary>
    /// Manages the retrieval of Configuration data.
    /// </summary>
    /// <typeparam name="T">The type of <see cref="IDocumentRetriever"/>.</typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable")]
    public partial class ConfigurationManager<T> : BaseConfigurationManager, IConfigurationManager<T> where T : class
    {
        internal Action _onBackgroundTaskFinish;

        private DateTimeOffset _syncAfter = DateTimeOffset.MinValue;
        private DateTimeOffset _lastRequestRefresh = DateTimeOffset.MinValue;
        private bool _isFirstRefreshRequest = true;
        private readonly SemaphoreSlim _configurationNullLock = new SemaphoreSlim(1);

        private readonly IDocumentRetriever _docRetriever;
        private readonly IConfigurationRetriever<T> _configRetriever;
        private readonly IConfigurationValidator<T> _configValidator;
        private T _currentConfiguration;

        // task states are used to ensure the call to 'update config' (UpdateCurrentConfiguration) is a singleton. Uses Interlocked.CompareExchange.
        // metadata is not being obtained
        private const int ConfigurationRetrieverIdle = 0;
        // metadata is being retrieved
        private const int ConfigurationRetrieverRunning = 1;
        private int _configurationRetrieverState = ConfigurationRetrieverIdle;

        private readonly Func<Task> _updateCurrentConfigurationWithBypassAsync;
        private readonly Func<Task> _updateCurrentConfigurationWithoutBypassAsync;

        internal TimeProvider TimeProvider = TimeProvider.System;
        internal ITelemetryClient TelemetryClient = new TelemetryClient();

        /// <summary>
        /// Gets or sets the optional configuration event handler.
        /// </summary>
        public IConfigurationEventHandler<T> ConfigurationEventHandler { get; set; }

        /// <summary>
        /// Instantiates a new <see cref="ConfigurationManager{T}"/> that manages automatic and controls refreshing on configuration data.
        /// </summary>
        /// <param name="metadataAddress">The address to obtain configuration.</param>
        /// <param name="configRetriever">The <see cref="IConfigurationRetriever{T}"/></param>
        public ConfigurationManager(string metadataAddress, IConfigurationRetriever<T> configRetriever)
            : this(metadataAddress, configRetriever, new HttpDocumentRetriever(), new LastKnownGoodConfigurationCacheOptions())
        {
        }

        /// <summary>
        /// Instantiates a new <see cref="ConfigurationManager{T}"/> that manages automatic and controls refreshing on configuration data.
        /// </summary>
        /// <param name="metadataAddress">The address to obtain configuration.</param>
        /// <param name="configRetriever">The <see cref="IConfigurationRetriever{T}"/></param>
        /// <param name="httpClient">The client to use when obtaining configuration.</param>
        public ConfigurationManager(string metadataAddress, IConfigurationRetriever<T> configRetriever, HttpClient httpClient)
            : this(metadataAddress, configRetriever, new HttpDocumentRetriever(httpClient), new LastKnownGoodConfigurationCacheOptions())
        {
        }

        /// <summary>
        /// Instantiates a new <see cref="ConfigurationManager{T}"/> that manages automatic and controls refreshing on configuration data.
        /// </summary>
        /// <param name="metadataAddress">The address to obtain configuration.</param>
        /// <param name="configRetriever">The <see cref="IConfigurationRetriever{T}"/></param>
        /// <param name="docRetriever">The <see cref="IDocumentRetriever"/> that reaches out to obtain the configuration.</param>
        /// <exception cref="ArgumentNullException">If 'metadataAddress' is null or empty.</exception>
        /// <exception cref="ArgumentNullException">If 'configRetriever' is null.</exception>
        /// <exception cref="ArgumentNullException">If 'docRetriever' is null.</exception>
        public ConfigurationManager(string metadataAddress, IConfigurationRetriever<T> configRetriever, IDocumentRetriever docRetriever)
            : this(metadataAddress, configRetriever, docRetriever, new LastKnownGoodConfigurationCacheOptions())
        {
        }

        /// <summary>
        /// Instantiates a new <see cref="ConfigurationManager{T}"/> that manages automatic and controls refreshing on configuration data.
        /// </summary>
        /// <param name="metadataAddress">The address to obtain configuration.</param>
        /// <param name="configRetriever">The <see cref="IConfigurationRetriever{T}"/></param>
        /// <param name="docRetriever">The <see cref="IDocumentRetriever"/> that reaches out to obtain the configuration.</param>
        /// <param name="lkgCacheOptions">The <see cref="LastKnownGoodConfigurationCacheOptions"/></param>
        /// <exception cref="ArgumentNullException">If 'metadataAddress' is null or empty.</exception>
        /// <exception cref="ArgumentNullException">If 'configRetriever' is null.</exception>
        /// <exception cref="ArgumentNullException">If 'docRetriever' is null.</exception>
        /// <exception cref="ArgumentNullException">If 'lkgCacheOptions' is null.</exception>
        public ConfigurationManager(string metadataAddress, IConfigurationRetriever<T> configRetriever, IDocumentRetriever docRetriever, LastKnownGoodConfigurationCacheOptions lkgCacheOptions)
            : base(lkgCacheOptions)
        {
            if (string.IsNullOrWhiteSpace(metadataAddress))
                throw LogHelper.LogArgumentNullException(nameof(metadataAddress));

            if (configRetriever == null)
                throw LogHelper.LogArgumentNullException(nameof(configRetriever));

            if (docRetriever == null)
                throw LogHelper.LogArgumentNullException(nameof(docRetriever));

            MetadataAddress = metadataAddress;
            _docRetriever = docRetriever;
            _configRetriever = configRetriever;

            _updateCurrentConfigurationWithBypassAsync = () => UpdateCurrentConfigurationAsync(bypassCache: true);
            _updateCurrentConfigurationWithoutBypassAsync = () => UpdateCurrentConfigurationAsync(bypassCache: false);
        }

        /// <summary>
        /// Instantiates a new <see cref="ConfigurationManager{T}"/> with configuration validator that manages automatic and controls refreshing on configuration data.
        /// </summary>
        /// <param name="metadataAddress">The address to obtain configuration.</param>
        /// <param name="configRetriever">The <see cref="IConfigurationRetriever{T}"/></param>
        /// <param name="docRetriever">The <see cref="IDocumentRetriever"/> that reaches out to obtain the configuration.</param>
        /// <param name="configValidator">The <see cref="IConfigurationValidator{T}"/></param>
        /// <exception cref="ArgumentNullException">If 'configValidator' is null.</exception>
        public ConfigurationManager(string metadataAddress, IConfigurationRetriever<T> configRetriever, IDocumentRetriever docRetriever, IConfigurationValidator<T> configValidator)
            : this(metadataAddress, configRetriever, docRetriever, configValidator, new LastKnownGoodConfigurationCacheOptions())
        {
        }

        /// <summary>
        /// Instantiates a new <see cref="ConfigurationManager{T}"/> with configuration validator that manages automatic and controls refreshing on configuration data.
        /// </summary>
        /// <param name="metadataAddress">The address to obtain configuration.</param>
        /// <param name="configRetriever">The <see cref="IConfigurationRetriever{T}"/></param>
        /// <param name="docRetriever">The <see cref="IDocumentRetriever"/> that reaches out to obtain the configuration.</param>
        /// <param name="configValidator">The <see cref="IConfigurationValidator{T}"/></param>
        /// <param name="lkgCacheOptions">The <see cref="LastKnownGoodConfigurationCacheOptions"/></param>
        /// <exception cref="ArgumentNullException">If 'configValidator' is null.</exception>
        public ConfigurationManager(string metadataAddress, IConfigurationRetriever<T> configRetriever, IDocumentRetriever docRetriever, IConfigurationValidator<T> configValidator, LastKnownGoodConfigurationCacheOptions lkgCacheOptions)
            : this(metadataAddress, configRetriever, docRetriever, lkgCacheOptions)
        {
            if (configValidator == null)
                throw LogHelper.LogArgumentNullException(nameof(configValidator));

            _configValidator = configValidator;
        }

        /// <summary>
        /// Instantiates a new <see cref="ConfigurationManager{T}"/> with configuration validator that manages automatic and controls refreshing on configuration data.
        /// </summary>
        /// <param name="metadataAddress">The address to obtain configuration.</param>
        /// <param name="configRetriever">The <see cref="IConfigurationRetriever{T}"/>.</param>
        /// <param name="docRetriever">The <see cref="IDocumentRetriever"/> that reaches out to obtain the configuration.</param>
        /// <param name="configValidator">The <see cref="IConfigurationValidator{T}"/>.</param>
        /// <param name="lkgCacheOptions">The <see cref="LastKnownGoodConfigurationCacheOptions"/>.</param>
        /// <param name="configurationEventHandler">The <see cref="IConfigurationEventHandler{T}"/> that handles configuration events.</param>
        /// <exception cref="ArgumentNullException">If 'configValidator' is null.</exception>
        public ConfigurationManager(string metadataAddress, IConfigurationRetriever<T> configRetriever, IDocumentRetriever docRetriever, IConfigurationValidator<T> configValidator, LastKnownGoodConfigurationCacheOptions lkgCacheOptions, IConfigurationEventHandler<T> configurationEventHandler)
            : this(metadataAddress, configRetriever, docRetriever, configValidator, lkgCacheOptions)
        {
            if (configurationEventHandler == null)
                throw LogHelper.LogArgumentNullException(nameof(configurationEventHandler));

            ConfigurationEventHandler = configurationEventHandler;
        }

        /// <summary>
        /// Obtains an updated version of Configuration.
        /// </summary>
        /// <returns>Configuration of type <typeparamref name="T"/>.</returns>
        /// <remarks>If the time since the last call is less than <see cref="BaseConfigurationManager.AutomaticRefreshInterval"/> then <see cref="IConfigurationRetriever{T}.GetConfigurationAsync"/> is not called and the current Configuration is returned.</remarks>
        public async Task<T> GetConfigurationAsync()
        {
            return await GetConfigurationAsync(CancellationToken.None).ConfigureAwait(false);
        }

        /// <summary>
        /// Obtains an updated version of Configuration.
        /// </summary>
        /// <param name="cancel">CancellationToken</param>
        /// <returns>Configuration of type <typeparamref name="T"/>.</returns>
        /// <remarks>
        /// <para>
        /// If the time since the last call is less than <see cref="BaseConfigurationManager.AutomaticRefreshInterval"/>
        /// then <see cref="IConfigurationRetriever{T}.GetConfigurationAsync"/> is not called and the current Configuration is returned.
        /// By default, this method blocks until the configuration is retrieved the first time. After the configuration was retrieved once,
        /// updates will happen in the background. Failures to retrieve the configuration on the background thread will be logged.
        /// </para>
        /// <para>
        /// If this operation is configured to be blocking through the switch 'Switch.Microsoft.IdentityModel.UpdateConfigAsBlocking'
        /// then this method will block each time the configuration needs to be updated or hasn't been retrieved. If the configuration
        /// cannot be initially retrieved an exception will be thrown. If the configuration has been retrieved, but cannot be updated,
        /// then the exception will be logged and the current configuration will be returned.
        /// </para>
        /// <para>
        /// By using the app context switch you choose what works best for you when there is a signing key update:
        /// either block requests from being validated until the new key is retrieved, or allow requests to be validated
        /// with the current key until the new key is retrieved. If blocking, a service receiving high concurrent request
        /// may experience thread starvation.
        /// </para>
        /// </remarks>
        public virtual async Task<T> GetConfigurationAsync(CancellationToken cancel)
        {
            if (_currentConfiguration != null && _syncAfter > TimeProvider.GetUtcNow())
                return _currentConfiguration;

            if (AppContextSwitches.UpdateConfigAsBlocking)
                return await GetConfigurationWithBlockingAsync(cancel).ConfigureAwait(false);
            else
                return await GetConfigurationNonBlockingAsync(cancel).ConfigureAwait(false);
        }

        private async Task<T> GetConfigurationNonBlockingAsync(CancellationToken cancel)
        {
            Exception fetchMetadataFailure = null;

            // LOGIC
            // if configuration == null => configuration has never been retrieved.
            //   reach out to the metadata endpoint. Since multiple threads could be calling this method
            //   we need to ensure that only one thread is actually fetching the metadata.
            // else
            //   if task is running, return the current configuration
            //   else kick off task to update current configuration
            if (_currentConfiguration == null)
            {
                await _configurationNullLock.WaitAsync(cancel).ConfigureAwait(false);
                if (_currentConfiguration != null)
                {
                    _configurationNullLock.Release();
                    return _currentConfiguration;
                }

                try
                {
                    var retrievalContext = new ConfigurationRetrievalContext { BypassCache = false };

                    // Check if event handler can provide configuration.
                    // If provided configuration is valid, skip regular retriaval process and update current configuration.
                    if (ConfigurationEventHandler != null)
                    {
                        var configurationRetrieved = await HandleBeforeRetrieveAsync(retrievalContext, cancel).ConfigureAwait(false);

                        // replicate the behavior of successful retrieval from endpoint
                        if (configurationRetrieved != null && configurationRetrieved.Configuration != null)
                        {
                            TelemetryClient.IncrementConfigurationRefreshRequestCounter(
                                MetadataAddress,
                                TelemetryConstants.Protocols.FirstRefresh,
                                TelemetryConstants.Protocols.ConfigurationSourceHandler);

                            UpdateConfiguration(configurationRetrieved.Configuration, configurationRetrieved.RetrievalTime, retrievalContext);
                            return _currentConfiguration;
                        }
                    }

                    // Don't use the individual CT here, this is a shared operation that shouldn't be affected by an individual's cancellation.
                    // The transport should have its own timeouts, etc.
                    T configuration = await _configRetriever.GetConfigurationAsync(
                        MetadataAddress,
                        _docRetriever,
                        CancellationToken.None).ConfigureAwait(false);

                    if (_configValidator != null)
                    {
                        ConfigurationValidationResult result = _configValidator.Validate(configuration);
                        // in this case we have never had a valid configuration, so we will throw an exception if the validation fails
                        if (!result.Succeeded)
                        {
                            var ex = new InvalidConfigurationException(
                                LogHelper.FormatInvariant(
                                    LogMessages.IDX20810,
                                    result.ErrorMessage));

                            throw LogHelper.LogExceptionMessage(ex);
                        }
                    }

                    TelemetryClient.IncrementConfigurationRefreshRequestCounter(
                        MetadataAddress,
                        TelemetryConstants.Protocols.FirstRefresh,
                        TelemetryConstants.Protocols.ConfigurationSourceRetriever);

                    UpdateConfiguration(configuration, TimeProvider.GetUtcNow(), retrievalContext);
                }
#pragma warning disable CA1031 // Do not catch general exception types
                catch (Exception ex)
                {
                    fetchMetadataFailure = ex;
                    TelemetryClient.IncrementConfigurationRefreshRequestCounter(
                        MetadataAddress,
                        TelemetryConstants.Protocols.FirstRefresh,
                        TelemetryConstants.Protocols.ConfigurationSourceRetriever,
                        ex);

                    LogHelper.LogExceptionMessage(
                        new InvalidOperationException(
                             LogHelper.FormatInvariant(
                                LogMessages.IDX20806,
                                LogHelper.MarkAsNonPII(MetadataAddress ?? "null"),
                                LogHelper.MarkAsNonPII(ex)),
                            ex));
                }
#pragma warning restore CA1031 // Do not catch general exception types
                finally
                {
                    _configurationNullLock.Release();
                }
            }
            else
            {
                if (Interlocked.CompareExchange(ref _configurationRetrieverState, ConfigurationRetrieverRunning, ConfigurationRetrieverIdle) == ConfigurationRetrieverIdle)
                {
                    TelemetryClient.IncrementConfigurationRefreshRequestCounter(
                        MetadataAddress,
                        TelemetryConstants.Protocols.Automatic,
                        TelemetryConstants.Protocols.ConfigurationSourceUnknown);

                    _ = Task.Run(_updateCurrentConfigurationWithoutBypassAsync, CancellationToken.None);
                }
            }

            // If metadata exists return it.
            if (_currentConfiguration != null)
                return _currentConfiguration;

            throw LogHelper.LogExceptionMessage(
                new InvalidOperationException(
                    LogHelper.FormatInvariant(
                        LogMessages.IDX20803,
                        LogHelper.MarkAsNonPII(MetadataAddress ?? "null"),
                        LogHelper.MarkAsNonPII(_syncAfter),
                        LogHelper.MarkAsNonPII(fetchMetadataFailure)),
                    fetchMetadataFailure));
        }

        /// <summary>
        /// This should be called when the configuration needs to be updated either from RequestRefresh or AutomaticRefresh
        /// The Caller should first check the state checking state using:
        ///   if (Interlocked.CompareExchange(ref _configurationRetrieverState, ConfigurationRetrieverRunning, ConfigurationRetrieverIdle) == ConfigurationRetrieverIdle).
        /// </summary>
        private async Task UpdateCurrentConfigurationAsync(bool bypassCache)
        {
            long startTimestamp = TimeProvider.GetTimestamp();
            var retrievalContext = new ConfigurationRetrievalContext { BypassCache = bypassCache };
            try
            {
                // Check if event handler can provide configuration
                // If provided configuration is valid, skip regular retriaval process and update current configuration.
                if (ConfigurationEventHandler != null)
                {
                    ConfigurationEventHandlerResult<T> configurationRetrieved = await HandleBeforeRetrieveAsync(retrievalContext).ConfigureAwait(false);

                    if (configurationRetrieved != null && configurationRetrieved.Configuration != null)
                    {
                        UpdateConfiguration(configurationRetrieved.Configuration, configurationRetrieved.RetrievalTime, retrievalContext);

                        _onBackgroundTaskFinish?.Invoke();
                        return;
                    }
                }

                T configuration = await _configRetriever.GetConfigurationAsync(
                    MetadataAddress,
                    _docRetriever,
                    CancellationToken.None).ConfigureAwait(false);

                var elapsedTime = TimeProvider.GetElapsedTime(startTimestamp);
                TelemetryClient.LogConfigurationRetrievalDuration(
                    MetadataAddress,
                    TelemetryConstants.Protocols.ConfigurationSourceRetriever,
                    elapsedTime);

                if (_configValidator == null)
                {
                    UpdateConfiguration(configuration, TimeProvider.GetUtcNow(), retrievalContext);
                }
                else
                {
                    ConfigurationValidationResult result = _configValidator.Validate(configuration);

                    if (!result.Succeeded)
                        LogHelper.LogExceptionMessage(
                            new InvalidConfigurationException(
                                LogHelper.FormatInvariant(
                                    LogMessages.IDX20810,
                                    result.ErrorMessage)));
                    else
                        UpdateConfiguration(configuration, TimeProvider.GetUtcNow(), retrievalContext);
                }
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception ex)
            {
                var elapsedTime = TimeProvider.GetElapsedTime(startTimestamp);
                TelemetryClient.LogConfigurationRetrievalDuration(
                    MetadataAddress,
                    TelemetryConstants.Protocols.ConfigurationSourceRetriever,
                    elapsedTime,
                    ex);

                LogHelper.LogExceptionMessage(
                    new InvalidOperationException(
                        LogHelper.FormatInvariant(
                            LogMessages.IDX20806,
                            LogHelper.MarkAsNonPII(MetadataAddress ?? "null"),
                            ex),
                        ex));
            }
#pragma warning restore CA1031 // Do not catch general exception types
            finally
            {
                Interlocked.Exchange(ref _configurationRetrieverState, ConfigurationRetrieverIdle);
            }

            _onBackgroundTaskFinish?.Invoke();
        }

        private void UpdateConfiguration(T configuration, DateTimeOffset retrievalTime, ConfigurationRetrievalContext context)
        {
            _currentConfiguration = configuration;
            _syncAfter = DateTimeUtil.Add(retrievalTime.UtcDateTime, AutomaticRefreshInterval +
                TimeSpan.FromSeconds(new Random().Next((int)AutomaticRefreshInterval.TotalSeconds / 20)));

            if (ConfigurationEventHandler != null)
            {
                // fire-and-forget an after update task
                _ = Task.Run(async () =>
                {
                    try
                    {
                        if (ConfigurationEventHandler is IConfigurationEventHandlerContextAware<T> contextAware)
                        {
                            await contextAware.AfterUpdateAsync(MetadataAddress, configuration, context).ConfigureAwait(false);
                        }
                        else
                        {
                            await ConfigurationEventHandler.AfterUpdateAsync(MetadataAddress, configuration).ConfigureAwait(false);
                        }
                    }
                    catch (Exception ex)
                    {
                        LogHelper.LogExceptionMessage(
                            new InvalidOperationException(
                                LogHelper.FormatInvariant(
                                   LogMessages.IDX20813,
                                    LogHelper.MarkAsNonPII(MetadataAddress ?? "null"),
                                    ex),
                                ex));
                    }
                });
            }
        }

        /// <summary>
        /// Obtains an updated version of Configuration.
        /// </summary>
        /// <param name="cancel">CancellationToken</param>
        /// <returns>Configuration of type BaseConfiguration.</returns>
        /// <remarks>If the time since the last call is less than <see cref="BaseConfigurationManager.AutomaticRefreshInterval"/> then <see cref="IConfigurationRetriever{T}.GetConfigurationAsync"/> is not called and the current Configuration is returned.</remarks>
        public override async Task<BaseConfiguration> GetBaseConfigurationAsync(CancellationToken cancel)
        {
            T obj = await GetConfigurationAsync(cancel).ConfigureAwait(false);
            return obj as BaseConfiguration;
        }

        /// <summary>
        /// Triggers updating metadata when:
        /// <para>1. Called the first time.</para>
        /// <para>2. The time between when this method was called and DateTimeOffset.Now is greater than <see cref="BaseConfigurationManager.RefreshInterval"/>.</para>
        /// <para>If <see cref="BaseConfigurationManager.RefreshInterval"/> == <see cref="TimeSpan.MaxValue"/> then this method does nothing.</para>
        /// </summary>
        /// <remarks>
        /// If the strategy is configured to be blocking through the switch 'Switch.Microsoft.IdentityModel.UpdateConfigAsBlocking',
        /// then this method will not update the configuration, instead it will request the next call to <see cref="GetConfigurationAsync()"/>
        /// should request new configuration.
        /// </remarks>
        public override void RequestRefresh()
        {
            if (AppContextSwitches.UpdateConfigAsBlocking)
                RequestRefreshBlocking();
            else
                RequestRefreshBackgroundThread();
        }

        private void RequestRefreshBackgroundThread()
        {
            DateTimeOffset now = TimeProvider.GetUtcNow();

            if (now >= DateTimeUtil.Add(_lastRequestRefresh.UtcDateTime, RefreshInterval) || _isFirstRefreshRequest)
            {
                TelemetryClient.IncrementConfigurationRefreshRequestCounter(
                    MetadataAddress,
                    TelemetryConstants.Protocols.Manual,
                    TelemetryConstants.Protocols.ConfigurationSourceUnknown);

                _isFirstRefreshRequest = false;
                if (Interlocked.CompareExchange(ref _configurationRetrieverState, ConfigurationRetrieverRunning, ConfigurationRetrieverIdle) == ConfigurationRetrieverIdle)
                {
                    _ = Task.Run(_updateCurrentConfigurationWithBypassAsync, CancellationToken.None);
                    _lastRequestRefresh = now;
                }
            }
        }

        private async Task<ConfigurationEventHandlerResult<T>> HandleBeforeRetrieveAsync(ConfigurationRetrievalContext context, CancellationToken cancellationToken = default)
        {
            long beforeHandlerTimestamp = TimeProvider.GetTimestamp();

            try
            {
                ConfigurationEventHandlerResult<T> handlerResult;
                if (ConfigurationEventHandler is IConfigurationEventHandlerContextAware<T> contextAware)
                {
                    handlerResult = await contextAware.BeforeRetrieveAsync(
                        MetadataAddress, context, cancellationToken).ConfigureAwait(false);
                }
                else
                {
                    handlerResult = await ConfigurationEventHandler.BeforeRetrieveAsync(
                        MetadataAddress, cancellationToken).ConfigureAwait(false);
                }

                if (handlerResult != null && handlerResult.Configuration != null)
                {
                    var handlerElapsedTime = TimeProvider.GetElapsedTime(beforeHandlerTimestamp);
                    TelemetryClient.LogConfigurationRetrievalDuration(
                        MetadataAddress,
                        TelemetryConstants.Protocols.ConfigurationSourceHandler,
                        handlerElapsedTime);

                    // Validate configuration from handler
                    if (_configValidator != null)
                    {
                        ConfigurationValidationResult result = _configValidator.Validate(handlerResult.Configuration);
                        if (!result.Succeeded)
                        {
                            // Just log the error and proceed to fetch from endpoint
                            LogHelper.LogExceptionMessage(
                                new InvalidConfigurationException(
                                    LogHelper.FormatInvariant(
                                        LogMessages.IDX20812,
                                        result.ErrorMessage)));

                            return ConfigurationEventHandlerResult<T>.NoResult;
                        }
                    }

                    // No validator configured, return configuration
                    return handlerResult;
                }
            }
            catch (Exception ex)
            {
                var handlerErrorElapsedTime = TimeProvider.GetElapsedTime(beforeHandlerTimestamp);
                TelemetryClient.LogConfigurationRetrievalDuration(
                    MetadataAddress,
                    TelemetryConstants.Protocols.ConfigurationSourceHandler,
                    handlerErrorElapsedTime,
                    ex);

                LogHelper.LogExceptionMessage(
                    new InvalidOperationException(
                        LogHelper.FormatInvariant(
                            LogMessages.IDX20811,
                            LogHelper.MarkAsNonPII(MetadataAddress ?? "null"),
                            ex),
                        ex));
            }

            return ConfigurationEventHandlerResult<T>.NoResult;
        }

        /// <summary>
        /// 12 hours is the default time interval that afterwards, <see cref="GetBaseConfigurationAsync(CancellationToken)"/> will obtain new configuration.
        /// </summary>
        public new static readonly TimeSpan DefaultAutomaticRefreshInterval = BaseConfigurationManager.DefaultAutomaticRefreshInterval;

        /// <summary>
        /// 5 minutes is the default time interval that must pass for <see cref="RequestRefresh"/> to obtain a new configuration.
        /// </summary>
        public new static readonly TimeSpan DefaultRefreshInterval = BaseConfigurationManager.DefaultRefreshInterval;

        /// <summary>
        /// 5 minutes is the minimum value for automatic refresh. <see cref="MinimumAutomaticRefreshInterval"/> can not be set less than this value.
        /// </summary>
        public new static readonly TimeSpan MinimumAutomaticRefreshInterval = BaseConfigurationManager.MinimumAutomaticRefreshInterval;

        /// <summary>
        /// 1 second is the minimum time interval that must pass for <see cref="MinimumRefreshInterval"/> to  obtain new configuration.
        /// </summary>
        public new static readonly TimeSpan MinimumRefreshInterval = BaseConfigurationManager.MinimumRefreshInterval;
    }
}
