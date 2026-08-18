// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Protocol.Plugins;

namespace NuGet.Credentials
{
    /// <summary>
    /// Acquires credentials from a plugin that supports the secure NuGet plugin protocol.
    /// </summary>
    public sealed class SecurePluginCredentialProvider : ICredentialProvider
    {
        private const string _basicAuthenticationType = "Basic";

        /// <summary>
        /// Plugin that this provider will use to acquire credentials
        /// </summary>
        private readonly PluginDiscoveryResult _discoveredPlugin;

        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// pluginManager
        /// </summary>
        private readonly IPluginManager _pluginManager;

        /// <summary>
        /// canShowDialog, whether the plugin can prompt or it should use device flow. This is a host decision not a user one. 
        /// </summary>
        private readonly bool _canShowDialog;

        // We use this to avoid needlessly instantiating plugins if they don't support authentication.
        private bool _isAnAuthenticationPlugin = true;

        /// <summary>
        /// Initializes a credential provider for a discovered secure plugin.
        /// </summary>
        /// <param name="pluginManager">The plugin manager used to create and communicate with the plugin.</param>
        /// <param name="pluginDiscoveryResult">The discovered plugin used to acquire credentials.</param>
        /// <param name="canShowDialog">
        /// <see langword="true"/> if the plugin may display an authentication dialog;
        /// otherwise, <see langword="false"/>.
        /// </param>
        /// <param name="logger">The logger used for plugin diagnostics.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="pluginDiscoveryResult"/> is null</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="logger"/> is null</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="pluginManager"/> is null</exception>
        /// <exception cref="ArgumentException">if plugin file is not valid</exception>
        public SecurePluginCredentialProvider(IPluginManager pluginManager, PluginDiscoveryResult pluginDiscoveryResult, bool canShowDialog, ILogger logger)
        {
            _pluginManager = pluginManager ?? throw new ArgumentNullException(nameof(pluginManager));
            _discoveredPlugin = pluginDiscoveryResult ?? throw new ArgumentNullException(nameof(pluginDiscoveryResult));
            _canShowDialog = canShowDialog;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            Id = $"{nameof(SecurePluginCredentialProvider)}_{pluginDiscoveryResult.PluginFile.Path}";
        }

        /// <summary>
        /// Gets the unique identifier of this credential provider.
        /// </summary>
        public string Id { get; }

        /// <param name="uri">The uri of a web resource for which credentials are needed.</param>
        /// <param name="proxy">Ignored.  Proxy information will not be passed to plugins.</param>
        /// <param name="type">
        /// The type of credential request that is being made. Note that this implementation of
        /// <see cref="ICredentialProvider"/> does not support providing proxy credenitials and treats
        /// all other types the same.
        /// </param>
        /// <param name="isRetry">If true, credentials were previously supplied by this
        /// provider for the same uri.</param>
        /// <param name="message">A message provided by NuGet to show to the user when prompting.</param>
        /// <param name="nonInteractive">If true, the plugin must not prompt for credentials.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A credential object.</returns>
        public async Task<CredentialResponse> GetAsync(
            Uri uri,
            IWebProxy? proxy,
            CredentialRequestType type,
            string? message,
            bool isRetry,
            bool nonInteractive,
            CancellationToken cancellationToken)
        {
            if (uri == null)
            {
                throw new ArgumentNullException(nameof(uri));
            }

            CredentialResponse? taskResponse = null;
            if (type == CredentialRequestType.Proxy || !_isAnAuthenticationPlugin)
            {
                taskResponse = new CredentialResponse(CredentialStatus.ProviderNotApplicable);
                return taskResponse;
            }

            Tuple<bool, PluginCreationResult?> result = await _pluginManager.TryGetSourceAgnosticPluginAsync(
                _discoveredPlugin,
                OperationClaim.Authentication,
                cancellationToken);

            if (result.Item2 is PluginCreationResult creationResult)
            {
                if (creationResult.Message is string creationMessage)
                {
                    // There is a potential here for double logging as the CredentialService itself catches the exceptions and tries to log it.
                    // In reality the logger in the Credential Service will be null because the first request always comes from a resource provider (ServiceIndex provider).
                    _logger.LogError(creationMessage);

                    if (creationResult.Exception != null)
                    {
                        _logger.LogDebug(creationResult.Exception.ToString());
                    }
                    _isAnAuthenticationPlugin = false;
                    throw new PluginException(creationMessage, creationResult.Exception); // Throwing here will block authentication and ensure that the complete operation fails.
                }

                // The successful PluginCreationResult constructor initializes all three values.
                var claims = creationResult.Claims!;
                var plugin = creationResult.Plugin!;
                var pluginMulticlientUtilities = creationResult.PluginMulticlientUtilities!;

                _isAnAuthenticationPlugin = claims.Contains(OperationClaim.Authentication);

                if (_isAnAuthenticationPlugin)
                {
                    AddOrUpdateLogger(plugin);
                    await SetPluginLogLevelAsync(
                        plugin,
                        pluginMulticlientUtilities,
                        _logger,
                        cancellationToken);

                    if (proxy != null)
                    {
                        await SetProxyCredentialsToPlugin(
                            uri,
                            proxy,
                            plugin,
                            pluginMulticlientUtilities,
                            cancellationToken);
                    }

                    var request = new GetAuthenticationCredentialsRequest(uri, isRetry, nonInteractive, _canShowDialog);
                    GetAuthenticationCredentialsResponse? credentialResponse = await plugin.Connection.SendRequestAndReceiveResponseAsync<GetAuthenticationCredentialsRequest, GetAuthenticationCredentialsResponse>(
                        MessageMethod.GetAuthenticationCredentials,
                        request,
                        cancellationToken);

                    if (credentialResponse == null)
                    {
                        // The connection returns no response once it is closing or closed - for example because the
                        // plugin process was torn down while this request was in flight.
                        throw new PluginException(
                            string.Format(
                                CultureInfo.CurrentCulture,
                                Resources.SecurePluginException_ConnectionClosed,
                                _discoveredPlugin.PluginFile.Path));
                    }

                    if (credentialResponse.ResponseCode == MessageResponseCode.NotFound && nonInteractive)
                    {
                        _logger.LogWarning(
                            string.Format(
                                CultureInfo.CurrentCulture,
                                Resources.SecurePluginWarning_UseInteractiveOption));
                    }

                    taskResponse = GetAuthenticationCredentialsResponseToCredentialResponse(credentialResponse);
                }
            }
            else
            {
                _isAnAuthenticationPlugin = false;
            }

            return taskResponse ?? new CredentialResponse(CredentialStatus.ProviderNotApplicable);
        }

        private static async Task SetPluginLogLevelAsync(
            IPlugin plugin,
            IPluginMulticlientUtilities pluginMulticlientUtilities,
            ILogger logger,
            CancellationToken cancellationToken)
        {
            var logLevel = LogRequestHandler.GetLogLevel(logger);

            await pluginMulticlientUtilities.DoOncePerPluginLifetimeAsync(
                MessageMethod.SetLogLevel.ToString(),
                () => plugin.Connection.SendRequestAndReceiveResponseAsync<SetLogLevelRequest, SetLogLevelResponse>(
                    MessageMethod.SetLogLevel,
                    new SetLogLevelRequest(logLevel),
                    cancellationToken),
                cancellationToken);
        }

        private void AddOrUpdateLogger(IPlugin plugin)
        {
            plugin.Connection.MessageDispatcher.RequestHandlers.AddOrUpdate(
                MessageMethod.Log,
                () => new LogRequestHandler(_logger),
                existingHandler =>
                {
                    ((LogRequestHandler)existingHandler).SetLogger(_logger);

                    return existingHandler;
                });
        }

        private async Task SetProxyCredentialsToPlugin(
            Uri uri,
            IWebProxy proxy,
            IPlugin plugin,
            IPluginMulticlientUtilities pluginMulticlientUtilities,
            CancellationToken cancellationToken)
        {
            NetworkCredential? proxyCredential = proxy.Credentials?.GetCredential(uri, _basicAuthenticationType);

            var key = $"{MessageMethod.SetCredentials}.{Id}";

            var proxyCredRequest = new SetCredentialsRequest(
                uri.AbsolutePath,
                proxyCredential?.UserName,
                proxyCredential?.Password,
                username: null,
                password: null);

            await pluginMulticlientUtilities.DoOncePerPluginLifetimeAsync(
                 key,
                 () =>
                     plugin.Connection.SendRequestAndReceiveResponseAsync<SetCredentialsRequest, SetCredentialsResponse>(
                     MessageMethod.SetCredentials,
                     proxyCredRequest,
                     cancellationToken),
                 cancellationToken);
        }

        /// <summary>
        /// Convert from Plugin CredentialResponse to the CredentialResponse model used by the ICredentialService
        /// </summary>
        /// <param name="credentialResponse"></param>
        /// <returns>credential response</returns>
        private static CredentialResponse GetAuthenticationCredentialsResponseToCredentialResponse(GetAuthenticationCredentialsResponse credentialResponse)
        {
            CredentialResponse taskResponse;
            if (credentialResponse.IsValid())
            {
                ICredentials result = new AuthTypeFilteredCredentials(
                    new NetworkCredential(credentialResponse.Username, credentialResponse.Password),
                    credentialResponse.AuthenticationTypes ?? Enumerable.Empty<string>());

                taskResponse = new CredentialResponse(result);
            }
            else if (credentialResponse.ResponseCode == MessageResponseCode.NotFound)
            {
                taskResponse = new CredentialResponse(CredentialStatus.UserCanceled);
            }
            else
            {
                taskResponse = new CredentialResponse(CredentialStatus.ProviderNotApplicable);
            }

            return taskResponse;
        }
    }
}
