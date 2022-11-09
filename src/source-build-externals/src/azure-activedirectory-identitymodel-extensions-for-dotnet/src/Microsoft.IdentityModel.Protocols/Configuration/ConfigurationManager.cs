//------------------------------------------------------------------------------
//
// Copyright (c) Microsoft Corporation.
// All rights reserved.
//
// This code is licensed under the MIT License.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files(the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and / or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions :
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
//------------------------------------------------------------------------------

using System;
using System.Diagnostics.Contracts;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.IdentityModel.Protocols
{
    /// <summary>
    /// Manages the retrieval of Configuration data.
    /// </summary>
    /// <typeparam name="T">The type of <see cref="IDocumentRetriever"/>.</typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable")]
    public class ConfigurationManager<T> : BaseConfigurationManager, IConfigurationManager<T> where T : class
    {
        private DateTimeOffset _syncAfter = DateTimeOffset.MinValue;
        private DateTimeOffset _lastRefresh = DateTimeOffset.MinValue;
        private bool _isFirstRefreshRequest = true;

        private readonly SemaphoreSlim _refreshLock;
        private readonly IDocumentRetriever _docRetriever;
        private readonly IConfigurationRetriever<T> _configRetriever;
        private readonly IConfigurationValidator<T> _configValidator;
        private T _currentConfiguration;

        /// <summary>
        /// Static initializer for a new object. Static initializers run before the first instance of the type is created.
        /// </summary>
        static ConfigurationManager()
        {
        }

        /// <summary>
        /// Instantiates a new <see cref="ConfigurationManager{T}"/> that manages automatic and controls refreshing on configuration data.
        /// </summary>
        /// <param name="metadataAddress">The address to obtain configuration.</param>
        /// <param name="configRetriever">The <see cref="IConfigurationRetriever{T}"/></param>
        public ConfigurationManager(string metadataAddress, IConfigurationRetriever<T> configRetriever)
            : this(metadataAddress, configRetriever, new HttpDocumentRetriever())
        {
        }

        /// <summary>
        /// Instantiates a new <see cref="ConfigurationManager{T}"/> that manages automatic and controls refreshing on configuration data.
        /// </summary>
        /// <param name="metadataAddress">The address to obtain configuration.</param>
        /// <param name="configRetriever">The <see cref="IConfigurationRetriever{T}"/></param>
        /// <param name="httpClient">The client to use when obtaining configuration.</param>
        public ConfigurationManager(string metadataAddress, IConfigurationRetriever<T> configRetriever, HttpClient httpClient)
            : this(metadataAddress, configRetriever, new HttpDocumentRetriever(httpClient))
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
            _refreshLock = new SemaphoreSlim(1);
        }

        /// <summary>
        /// Instantiates a new <see cref="ConfigurationManager{T}"/> with cinfiguration validator that manages automatic and controls refreshing on configuration data.
        /// </summary>
        /// <param name="metadataAddress">The address to obtain configuration.</param>
        /// <param name="configRetriever">The <see cref="IConfigurationRetriever{T}"/></param>
        /// <param name="docRetriever">The <see cref="IDocumentRetriever"/> that reaches out to obtain the configuration.</param>
        /// <param name="configValidator">The <see cref="IConfigurationValidator{T}"/></param>
        /// <exception cref="ArgumentNullException">If 'configValidator' is null.</exception>
        public ConfigurationManager(string metadataAddress, IConfigurationRetriever<T> configRetriever, IDocumentRetriever docRetriever, IConfigurationValidator<T> configValidator)
            :this(metadataAddress, configRetriever, docRetriever)
        {
            if (configValidator == null)
                throw LogHelper.LogArgumentNullException(nameof(configValidator));

            _configValidator = configValidator;
        }

        /// <summary>
        /// Obtains an updated version of Configuration.
        /// </summary>
        /// <returns>Configuration of type T.</returns>
        /// <remarks>If the time since the last call is less than <see cref="BaseConfigurationManager.AutomaticRefreshInterval"/> then <see cref="IConfigurationRetriever{T}.GetConfigurationAsync"/> is not called and the current Configuration is returned.</remarks>
        public async Task<T> GetConfigurationAsync()
        {
            return await GetConfigurationAsync(CancellationToken.None).ConfigureAwait(false);
        }

        /// <summary>
        /// Obtains an updated version of Configuration.
        /// </summary>
        /// <param name="cancel">CancellationToken</param>
        /// <returns>Configuration of type T.</returns>
        /// <remarks>If the time since the last call is less than <see cref="BaseConfigurationManager.AutomaticRefreshInterval"/> then <see cref="IConfigurationRetriever{T}.GetConfigurationAsync"/> is not called and the current Configuration is returned.</remarks>
        public async Task<T> GetConfigurationAsync(CancellationToken cancel)
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            if (_currentConfiguration != null && _syncAfter > now)
            {
                return _currentConfiguration;
            }

            await _refreshLock.WaitAsync(cancel).ConfigureAwait(false);
            try
            {
                if (_syncAfter <= now)
                {
                    try
                    {
                        // Don't use the individual CT here, this is a shared operation that shouldn't be affected by an individual's cancellation.
                        // The transport should have it's own timeouts, etc..
                        var configuration = await _configRetriever.GetConfigurationAsync(MetadataAddress, _docRetriever, CancellationToken.None).ConfigureAwait(false);
                        _lastRefresh = now;
                        _syncAfter = DateTimeUtil.Add(now.UtcDateTime, AutomaticRefreshInterval);
                        if (_configValidator != null)
                        {
                            ConfigurationValidationResult result = _configValidator.Validate(configuration);
                            if (!result.Succeeded)
                                LogHelper.LogWarning(LogMessages.IDX20810, result.ErrorMessage);
                        }

                        _currentConfiguration = configuration;

                    }
                    catch (Exception ex)
                    {
                        _syncAfter = DateTimeUtil.Add(now.UtcDateTime, AutomaticRefreshInterval < RefreshInterval ? AutomaticRefreshInterval : RefreshInterval);
                        if (_currentConfiguration == null) // Throw an exception if there's no configuration to return.
                            throw LogHelper.LogExceptionMessage(new InvalidOperationException(LogHelper.FormatInvariant(LogMessages.IDX20803, (MetadataAddress ?? "null")), ex));
                        else
                            LogHelper.LogExceptionMessage(new InvalidOperationException(LogHelper.FormatInvariant(LogMessages.IDX20806, (MetadataAddress ?? "null")), ex));
                    }
                }

                // Stale metadata is better than no metadata
                if (_currentConfiguration != null)
                    return _currentConfiguration;
                else
                    throw LogHelper.LogExceptionMessage(new InvalidOperationException(LogHelper.FormatInvariant(LogMessages.IDX20803, (MetadataAddress ?? "null"))));
            }
            finally
            {
                _refreshLock.Release();
            }
        }

        /// <summary>
        /// Obtains an updated version of Configuration.
        /// </summary>
        /// <param name="cancel">CancellationToken</param>
        /// <returns>Configuration of type BaseConfiguration    .</returns>
        /// <remarks>If the time since the last call is less than <see cref="BaseConfigurationManager.AutomaticRefreshInterval"/> then <see cref="IConfigurationRetriever{T}.GetConfigurationAsync"/> is not called and the current Configuration is returned.</remarks>
        public override async Task<BaseConfiguration> GetBaseConfigurationAsync(CancellationToken cancel)
        {
            var obj = await GetConfigurationAsync(cancel).ConfigureAwait(false);
            if (obj is BaseConfiguration)
                return obj as BaseConfiguration;
            return null;
        }

        /// <summary>
        /// Requests that then next call to <see cref="GetConfigurationAsync()"/> obtain new configuration.
        /// <para>If it is a first force refresh or the last refresh was greater than <see cref="BaseConfigurationManager.RefreshInterval"/> then the next call to <see cref="GetConfigurationAsync()"/> will retrieve new configuration.</para>
        /// <para>If <see cref="BaseConfigurationManager.RefreshInterval"/> == <see cref="TimeSpan.MaxValue"/> then this method does nothing.</para>
        /// </summary>
        public override void RequestRefresh()
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            if (_isFirstRefreshRequest)
            {
                _syncAfter = now;
                _isFirstRefreshRequest = false;
            }
            else if (now >= DateTimeUtil.Add(_lastRefresh.UtcDateTime, RefreshInterval))
            {
                _syncAfter = now;
            }
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
