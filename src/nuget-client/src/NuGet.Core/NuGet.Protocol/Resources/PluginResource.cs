// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Configuration;
using NuGet.Protocol.Plugins;

namespace NuGet.Protocol.Core.Types
{
    /// <summary>
    /// Represents a plugin resource.
    /// </summary>
    public sealed class PluginResource : INuGetResource
    {
        private const string _basicAuthenticationType = "Basic";

        private readonly ICredentialService _credentialService;
        private readonly PackageSource _packageSource;
        private readonly IReadOnlyList<PluginCreationResult> _pluginCreationResults;

        /// <summary>
        /// Instantiates a new <see cref="PluginResource" /> class.
        /// </summary>
        /// <param name="pluginCreationResults">Plugin creation results.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="pluginCreationResults" />
        /// is <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="packageSource" />
        /// is <see langword="null" />.</exception>
        public PluginResource(
            IEnumerable<PluginCreationResult> pluginCreationResults,
            PackageSource packageSource,
            ICredentialService credentialService)
        {
            if (pluginCreationResults == null)
            {
                throw new ArgumentNullException(nameof(pluginCreationResults));
            }

            if (packageSource == null)
            {
                throw new ArgumentNullException(nameof(packageSource));
            }

            _pluginCreationResults = pluginCreationResults.ToArray();
            _packageSource = packageSource;
            _credentialService = credentialService;
        }

        /// <summary>
        /// Gets the first plugin satisfying the required operation claims for the current package source.
        /// </summary>
        /// <param name="requiredClaim">The required operation claim.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result (<see cref="Task{TResult}.Result" />) returns a <see cref="GetPluginResult" />.</returns>
        /// <exception cref="OperationCanceledException">Thrown if <paramref name="cancellationToken" />
        /// is cancelled.</exception>
        public async Task<GetPluginResult> GetPluginAsync(
            OperationClaim requiredClaim,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var messages = new List<string>();

            foreach (var result in _pluginCreationResults)
            {
                if (!string.IsNullOrEmpty(result.Message))
                {
                    throw new PluginException(result.Message);
                }

                if (result.Claims.Contains(requiredClaim))
                {
                    var key = $"{MessageMethod.SetCredentials}.{_packageSource.SourceUri}";

                    await result.PluginMulticlientUtilities.DoOncePerPluginLifetimeAsync(
                        key,
                        () => SetPackageSourceCredentialsAsync(result.Plugin, cancellationToken),
                        cancellationToken);

                    return new GetPluginResult(result.Plugin, result.PluginMulticlientUtilities);
                }
            }

            return null;
        }

        private async Task SetPackageSourceCredentialsAsync(IPlugin plugin, CancellationToken cancellationToken)
        {
            var payload = CreateRequest();

            await plugin.Connection.SendRequestAndReceiveResponseAsync<SetCredentialsRequest, SetCredentialsResponse>(
                MessageMethod.SetCredentials,
                payload,
                cancellationToken);
        }

        private SetCredentialsRequest CreateRequest()
        {
            var sourceUri = _packageSource.SourceUri;
            string proxyUsername = null;
            string proxyPassword = null;
            string username = null;
            string password = null;
            ICredentials credentials;

            if (TryGetCachedCredentials(sourceUri, isProxy: true, credentials: out credentials))
            {
                var proxyCredential = credentials.GetCredential(sourceUri, _basicAuthenticationType);

                if (proxyCredential != null)
                {
                    proxyUsername = proxyCredential.UserName;
                    proxyPassword = proxyCredential.Password;
                }
            }

            if (TryGetCachedCredentials(sourceUri, isProxy: false, credentials: out credentials))
            {
                var packageSourceCredential = credentials.GetCredential(sourceUri, authType: null);

                if (packageSourceCredential != null)
                {
                    username = packageSourceCredential.UserName;
                    password = packageSourceCredential.Password;
                }
            }

            return new SetCredentialsRequest(
                _packageSource.Source,
                proxyUsername,
                proxyPassword,
                username,
                password);
        }

        private bool TryGetCachedCredentials(Uri uri, bool isProxy, out ICredentials credentials)
        {
            credentials = null;

            if (_credentialService == null)
            {
                return false;
            }

            return _credentialService.TryGetLastKnownGoodCredentialsFromCache(uri, isProxy, out credentials);
        }

        public sealed class GetPluginResult
        {
            public IPlugin Plugin { get; }
            public IPluginMulticlientUtilities PluginMulticlientUtilities { get; }

            internal GetPluginResult(IPlugin plugin, IPluginMulticlientUtilities utilities)
            {
                Plugin = plugin;
                PluginMulticlientUtilities = utilities;
            }
        }
    }
}
