// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Packaging;
using NuGet.Protocol.Core.Types;
using NuGet.Shared;

namespace NuGet.Protocol.Plugins
{
    /// <summary>
    /// A plugin manager. This manages all the live plugins and their operation claims.
    /// Invoked in by both the credential provider and the PluginResourceProvider
    /// </summary>
    public sealed class PluginManager : IPluginManager, IDisposable
    {
        private static readonly Lazy<IPluginManager> _lazy = new Lazy<IPluginManager>(() => new PluginManager());
        public static IPluginManager Instance => _lazy.Value;

        private ConnectionOptions _connectionOptions;
        private Lazy<IPluginDiscoverer> _discoverer;
        private bool _isDisposed;
        private IPluginFactory _pluginFactory;
        private ConcurrentDictionary<PluginRequestKey, Lazy<Task<IReadOnlyList<OperationClaim>>>> _pluginOperationClaims;
        private ConcurrentDictionary<string, Lazy<IPluginMulticlientUtilities>> _pluginUtilities;
        private string _rawPluginPaths;

        private static Lazy<int> _currentProcessId = new Lazy<int>(GetCurrentProcessId);
        private Lazy<string> _pluginsCacheDirectoryPath;

        /// <summary>
        /// Gets an environment variable reader.
        /// </summary>
        /// <remarks>This is non-private only to facilitate testing.</remarks>
        public IEnvironmentVariableReader EnvironmentVariableReader { get; private set; }

        private PluginManager()
        {
            Initialize(
                EnvironmentVariableWrapper.Instance,
                new Lazy<IPluginDiscoverer>(InitializeDiscoverer),
                (TimeSpan idleTimeout) => new PluginFactory(idleTimeout),
                new Lazy<string>(() => SettingsUtility.GetPluginsCacheFolder()));
        }

        public PluginManager(
            IEnvironmentVariableReader reader,
            Lazy<IPluginDiscoverer> pluginDiscoverer,
            Func<TimeSpan, IPluginFactory> pluginFactoryCreator,
            Lazy<string> pluginsCacheDirectoryPath)
        {
            Initialize(
                reader,
                pluginDiscoverer,
                pluginFactoryCreator,
                pluginsCacheDirectoryPath);
        }

        /// <summary>
        /// Disposes of this instance.
        /// This should not be called in production code as this is a singleton instance.
        /// The pattern is implemented because the plugin manager transitively owns objects
        /// that need to implement IDisposable because they potentially have managed and unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (!_isDisposed)
            {
                if (_discoverer.IsValueCreated)
                {
                    _discoverer.Value.Dispose();
                }

                _pluginFactory.Dispose();

                GC.SuppressFinalize(this);

                _isDisposed = true;
            }
        }

        /// <summary>
        /// Find all available plugins on the machine
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>PluginDiscoveryResults</returns>
        public async Task<IEnumerable<PluginDiscoveryResult>> FindAvailablePluginsAsync(CancellationToken cancellationToken)
        {
            return await _discoverer.Value.DiscoverAsync(cancellationToken);
        }

        /// <summary>
        /// Create plugins appropriate for the given source
        /// </summary>
        /// <param name="source"></param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="ArgumentNullException">Throw if <paramref name="source"/> is null </exception>
        /// <returns>PluginCreationResults</returns>
        public async Task<IEnumerable<PluginCreationResult>> CreatePluginsAsync(
            SourceRepository source,
            CancellationToken cancellationToken)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            cancellationToken.ThrowIfCancellationRequested();
            var pluginCreationResults = new List<PluginCreationResult>();

            // Fast path
            if (source.PackageSource.IsHttp && IsPluginPossiblyAvailable())
            {
                var serviceIndex = await source.GetResourceAsync<ServiceIndexResourceV3>(cancellationToken);

                if (serviceIndex != null)
                {
                    var serviceIndexJson = JObject.Parse(serviceIndex.Json);

                    foreach (var result in await FindAvailablePluginsAsync(cancellationToken))
                    {
                        var pluginCreationResult = await TryCreatePluginAsync(
                            result,
                            OperationClaim.DownloadPackage,
                            new PluginRequestKey(result.PluginFile.Path, source.PackageSource.Source),
                            source.PackageSource.Source,
                            serviceIndexJson,
                            cancellationToken);

                        if (pluginCreationResult.Item1)
                        {
                            pluginCreationResults.Add(pluginCreationResult.Item2);
                        }
                    }
                }
            }

            return pluginCreationResults;
        }

        /// <summary>
        /// Creates a plugin from the given pluginDiscoveryResult.
        /// This plugin's operations will be source agnostic ones (Authentication)
        /// </summary>
        /// <param name="pluginDiscoveryResult">plugin discovery result</param>
        /// <param name="requestedOperationClaim">The requested operation claim</param>
        /// <param name="cancellationToken">cancellation token</param>
        /// <returns>A plugin creation result, null if the requested plugin cannot handle the given operation claim</returns>
        public Task<Tuple<bool, PluginCreationResult>> TryGetSourceAgnosticPluginAsync(PluginDiscoveryResult pluginDiscoveryResult, OperationClaim requestedOperationClaim, CancellationToken cancellationToken)
        {
            if (pluginDiscoveryResult == null)
            {
                throw new ArgumentNullException(nameof(pluginDiscoveryResult));
            }

            return TryCreatePluginAsync(
                pluginDiscoveryResult,
                requestedOperationClaim,
                new PluginRequestKey(pluginDiscoveryResult.PluginFile.Path, "Source-Agnostic"),
                packageSourceRepository: null,
                serviceIndex: null,
                cancellationToken: cancellationToken);
        }

        /// <summary>
        /// Creates a plugin from the discovered plugin.
        /// We firstly check the cache for the operation claims for the given request key.
        /// If there is a valid cache entry, and it does contain the requested operation claim, then we start the plugin, and if need be update the cache value itself.
        /// If there is a valid cache entry, and it does NOT contain the requested operation claim, then we return a null.
        /// If there is no valid cache entry or an invalid one, we start the plugin as normally, return an active plugin even if the requested claim is not available, and write a cache entry.
        /// </summary>
        /// <param name="result">plugin discovery result</param>
        /// <param name="requestedOperationClaim">The requested operation claim</param>
        /// <param name="requestKey">plugin request key</param>
        /// <param name="packageSourceRepository">package source repository</param>
        /// <param name="serviceIndex">service index</param>
        /// <param name="cancellationToken">cancellation token</param>
        /// <returns>A plugin creation result, null if the requested plugin cannot handle the given operation claim</returns>
        private async Task<Tuple<bool, PluginCreationResult>> TryCreatePluginAsync(
            PluginDiscoveryResult result,
            OperationClaim requestedOperationClaim,
            PluginRequestKey requestKey,
            string packageSourceRepository,
            JObject serviceIndex,
            CancellationToken cancellationToken)
        {
            // This is a non cancellable task.
            // We should only honor cancellation requests we can recover from. 
            // Once we have reached this part of the code, we do the plugin initialization
            // handshake, operation claims, and shut down set up. 
            // If either one of these tasks fails then the plugin itself is not usable for the rest of the process. 
            // We could consider handling each of this operations more cleverly, 
            // but simplicity and readability is prioritized           
            cancellationToken = CancellationToken.None;
            PluginCreationResult pluginCreationResult = null;
            var cacheEntry = new PluginCacheEntry(_pluginsCacheDirectoryPath.Value, result.PluginFile.Path, requestKey.PackageSourceRepository);

            ConcurrencyUtilities.ExecuteWithFileLocked(cacheEntry.CacheFileName, cacheEntry.LoadFromFile);

            if (cacheEntry.OperationClaims == null || cacheEntry.OperationClaims.Contains(requestedOperationClaim))
            {
                try
                {
                    if (result.PluginFile.State.Value == PluginFileState.Valid)
                    {
                        var plugin = await _pluginFactory.GetOrCreateAsync(
                            result.PluginFile.Path,
                            PluginConstants.PluginArguments,
                            new RequestHandlers(),
                            _connectionOptions,
                            cancellationToken);

                        var utilities = await PerformOneTimePluginInitializationAsync(plugin, cancellationToken);

                        // We still make the GetOperationClaims call even if we have the operation claims cached. This is a way to self-update the cache.
                        var operationClaims = await _pluginOperationClaims.GetOrAdd(
                            requestKey,
                            key => new Lazy<Task<IReadOnlyList<OperationClaim>>>(() =>
                            GetPluginOperationClaimsAsync(
                                plugin,
                                packageSourceRepository,
                                serviceIndex,
                                cancellationToken))).Value;

                        if (!EqualityUtility.SequenceEqualWithNullCheck(operationClaims, cacheEntry.OperationClaims))
                        {
                            cacheEntry.OperationClaims = operationClaims;

                            await utilities.Value.DoOncePerPluginLifetimeAsync(
                                nameof(PluginCacheEntry),
                                () => ConcurrencyUtilities.ExecuteWithFileLockedAsync(
                                    cacheEntry.CacheFileName,
                                    action: async lockedToken =>
                                    {
                                        await cacheEntry.UpdateCacheFileAsync();

                                        return TaskResult.Null<object>();
                                    },
                                    token: cancellationToken),
                                cancellationToken);
                        }

                        pluginCreationResult = new PluginCreationResult(
                            plugin,
                            utilities.Value,
                            operationClaims);
                    }
                    else
                    {
                        pluginCreationResult = new PluginCreationResult(result.Message);
                    }
                }
                catch (Exception e)
                {
                    pluginCreationResult = new PluginCreationResult(
                        string.Format(CultureInfo.CurrentCulture,
                            Strings.Plugin_ProblemStartingPlugin,
                            result.PluginFile.Path,
                            e.Message),
                        e);
                }
            }

            return new Tuple<bool, PluginCreationResult>(pluginCreationResult != null, pluginCreationResult);
        }

        private async Task<Lazy<IPluginMulticlientUtilities>> PerformOneTimePluginInitializationAsync(IPlugin plugin, CancellationToken cancellationToken)
        {
            var utilities = _pluginUtilities.GetOrAdd(
                plugin.Id,
                path => new Lazy<IPluginMulticlientUtilities>(
                    () => new PluginMulticlientUtilities()));

            plugin.Closed += OnPluginClosed;

            await utilities.Value.DoOncePerPluginLifetimeAsync(
                MessageMethod.MonitorNuGetProcessExit.ToString(),
                () => plugin.Connection.SendRequestAndReceiveResponseAsync<MonitorNuGetProcessExitRequest, MonitorNuGetProcessExitResponse>(
                    MessageMethod.MonitorNuGetProcessExit,
                    new MonitorNuGetProcessExitRequest(_currentProcessId.Value),
                    cancellationToken),
                cancellationToken);

            await utilities.Value.DoOncePerPluginLifetimeAsync(
                MessageMethod.Initialize.ToString(),
                () => InitializePluginAsync(plugin, _connectionOptions.RequestTimeout, cancellationToken),
                cancellationToken);

            return utilities;
        }

        private void Initialize(IEnvironmentVariableReader reader,
            Lazy<IPluginDiscoverer> pluginDiscoverer,
            Func<TimeSpan, IPluginFactory> pluginFactoryCreator,
            Lazy<string> pluginsCacheDirectoryPath)
        {
            EnvironmentVariableReader = reader ?? throw new ArgumentNullException(nameof(reader));
            _discoverer = pluginDiscoverer ?? throw new ArgumentNullException(nameof(pluginDiscoverer));
            _pluginsCacheDirectoryPath = pluginsCacheDirectoryPath ?? throw new ArgumentNullException(nameof(pluginsCacheDirectoryPath));

            if (pluginFactoryCreator == null)
            {
                throw new ArgumentNullException(nameof(pluginFactoryCreator));
            }
#if IS_DESKTOP
            _rawPluginPaths = reader.GetEnvironmentVariable(EnvironmentVariableConstants.DesktopPluginPaths);
#else
            _rawPluginPaths = reader.GetEnvironmentVariable(EnvironmentVariableConstants.CorePluginPaths);
#endif
            if (string.IsNullOrEmpty(_rawPluginPaths))
            {
                _rawPluginPaths = reader.GetEnvironmentVariable(EnvironmentVariableConstants.PluginPaths);
            }

            _connectionOptions = ConnectionOptions.CreateDefault(reader);

            var idleTimeoutInSeconds = EnvironmentVariableReader.GetEnvironmentVariable(EnvironmentVariableConstants.IdleTimeout);
            var idleTimeout = TimeoutUtilities.GetTimeout(idleTimeoutInSeconds, PluginConstants.IdleTimeout);

            _pluginFactory = pluginFactoryCreator(idleTimeout);
            _pluginOperationClaims = new ConcurrentDictionary<PluginRequestKey, Lazy<Task<IReadOnlyList<OperationClaim>>>>();
            _pluginUtilities = new ConcurrentDictionary<string, Lazy<IPluginMulticlientUtilities>>(
                StringComparer.OrdinalIgnoreCase);
        }

        private async Task<IReadOnlyList<OperationClaim>> GetPluginOperationClaimsAsync(
            IPlugin plugin,
            string packageSourceRepository,
            JObject serviceIndex,
            CancellationToken cancellationToken)
        {
            if (plugin.Connection.ProtocolVersion.Equals(Plugins.ProtocolConstants.Version100) && (string.IsNullOrEmpty(packageSourceRepository) || serviceIndex == null))
            {
                throw new ArgumentException("Cannot invoke get operation claims with null arguments on a " + Plugins.ProtocolConstants.Version100 + " plugin");
            }

            var payload = new GetOperationClaimsRequest(packageSourceRepository, serviceIndex);

            var response = await plugin.Connection.SendRequestAndReceiveResponseAsync<GetOperationClaimsRequest, GetOperationClaimsResponse>(
                MessageMethod.GetOperationClaims,
                payload,
                cancellationToken);
            if (response == null)
            {
                return Array.Empty<OperationClaim>();
            }

            return response.Claims;
        }

        private PluginDiscoverer InitializeDiscoverer()
        {
            var verifier = EmbeddedSignatureVerifier.Create();

            return new PluginDiscoverer(_rawPluginPaths, verifier);
        }

        private bool IsPluginPossiblyAvailable()
        {
            return !string.IsNullOrEmpty(_rawPluginPaths);
        }

        private void OnPluginClosed(object sender, EventArgs e)
        {
            if (sender is IPlugin plugin)
            {
                plugin.Closed -= OnPluginClosed;

                _pluginUtilities.TryRemove(plugin.Id, out Lazy<IPluginMulticlientUtilities> utilities);
            }
        }

        private static int GetCurrentProcessId()
        {
            using (var process = Process.GetCurrentProcess())
            {
                return process.Id;
            }
        }

        /// <summary>
        /// Gets the current culture.
        /// An invariant culture's name will be "". Since InitializeRequest has a null or empty check, this can be a problem.
        /// Because the InitializeRequest message is part of a protocol, and the reason why we set the culture is allow the plugins to localize their messages,
        /// we can safely default to en. 
        /// </summary>
        /// <returns>CurrentCulture or an en default if the current culture is invariant</returns>
        private static string GetCurrentCultureName()
        {
            var currentCultureName = CultureInfo.CurrentCulture.Name;
            if (string.IsNullOrEmpty(currentCultureName))
            {
                currentCultureName = "en";
            }
            return currentCultureName;
        }

        private static async Task InitializePluginAsync(
            IPlugin plugin,
            TimeSpan requestTimeout,
            CancellationToken cancellationToken)
        {
            var clientVersion = MinClientVersionUtility.GetNuGetClientVersion().ToNormalizedString();
            var culture = GetCurrentCultureName();

            var payload = new InitializeRequest(
                clientVersion,
                culture,
                requestTimeout);

            var response = await plugin.Connection.SendRequestAndReceiveResponseAsync<InitializeRequest, InitializeResponse>(
                MessageMethod.Initialize,
                payload,
                cancellationToken);

            if (response != null && response.ResponseCode != MessageResponseCode.Success)
            {
                throw new PluginException(Strings.Plugin_InitializationFailed);
            }

            plugin.Connection.Options.SetRequestTimeout(requestTimeout);
        }

        private sealed class PluginRequestKey : IEquatable<PluginRequestKey>
        {
            internal string PluginFilePath { get; }
            internal string PackageSourceRepository { get; }

            internal PluginRequestKey(string pluginFilePath, string packageSourceRepository)
            {
                PluginFilePath = pluginFilePath;
                PackageSourceRepository = packageSourceRepository;
            }

            public override bool Equals(object obj)
            {
                return Equals(obj as PluginRequestKey);
            }

            public override int GetHashCode()
            {
                return HashCodeCombiner.GetHashCode(PluginFilePath, PackageSourceRepository);
            }

            public bool Equals(PluginRequestKey other)
            {
                if (ReferenceEquals(this, other))
                {
                    return true;
                }

                if (ReferenceEquals(null, other))
                {
                    return false;
                }

                return PathUtility.GetStringComparerBasedOnOS().Equals(PluginFilePath, other.PluginFilePath)
                    && string.Equals(
                        PackageSourceRepository,
                        other.PackageSourceRepository,
                        StringComparison.OrdinalIgnoreCase);
            }
        }
    }
}
