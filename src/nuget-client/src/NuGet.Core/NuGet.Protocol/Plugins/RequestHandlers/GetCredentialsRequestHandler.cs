// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.Globalization;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Protocol.Core.Types;

namespace NuGet.Protocol.Plugins
{
    /// <summary>
    /// A request handler for get credentials requests.
    /// </summary>
    public sealed class GetCredentialsRequestHandler : IRequestHandler, IDisposable
    {
        private const string _basicAuthenticationType = "Basic";

        private readonly ICredentialService _credentialService;
        private bool _isDisposed;
        private readonly IPlugin _plugin;
        private readonly IWebProxy _proxy;
        private readonly ConcurrentDictionary<string, SourceRepository> _repositories;

        /// <summary>
        /// Gets the <see cref="CancellationToken" /> for a request.
        /// </summary>
        public CancellationToken CancellationToken => CancellationToken.None;

        /// <summary>
        /// Initializes a new <see cref="GetCredentialsRequestHandler" /> class.
        /// </summary>
        /// <param name="plugin">A plugin.</param>
        /// <param name="proxy">A web proxy.</param>
        /// <param name="credentialService">An optional credential service.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="plugin" />
        /// is <see langword="null" />.</exception>
        public GetCredentialsRequestHandler(
            IPlugin plugin,
            IWebProxy proxy,
            ICredentialService credentialService)
        {
            if (plugin == null)
            {
                throw new ArgumentNullException(nameof(plugin));
            }

            _plugin = plugin;
            _proxy = proxy;
            _credentialService = credentialService;
            _repositories = new ConcurrentDictionary<string, SourceRepository>();
        }

        /// <summary>
        /// Disposes of this instance.
        /// </summary>
        public void Dispose()
        {
            if (!_isDisposed)
            {
                _plugin.Dispose();

                GC.SuppressFinalize(this);

                _isDisposed = true;
            }
        }

        /// <summary>
        /// Adds or updates a source repository in a source repository cache.
        /// </summary>
        /// <param name="sourceRepository">A source repository.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="sourceRepository" />
        /// is <see langword="null" />.</exception>
        public void AddOrUpdateSourceRepository(SourceRepository sourceRepository)
        {
            if (sourceRepository == null)
            {
                throw new ArgumentNullException(nameof(sourceRepository));
            }

            if (sourceRepository.PackageSource != null && sourceRepository.PackageSource.IsHttp)
            {
                _repositories.AddOrUpdate(
                    sourceRepository.PackageSource.Source,
                    sourceRepository,
                    (source, repo) => sourceRepository);
            }
        }

        /// <summary>
        /// Asynchronously handles responding to a request.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="request">A request message.</param>
        /// <param name="responseHandler">A response handler.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="connection" />
        /// is <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="request" /> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="responseHandler" />
        /// is <see langword="null" />.</exception>
        /// <exception cref="OperationCanceledException">Thrown if <paramref name="cancellationToken" />
        /// is cancelled.</exception>
        public async Task HandleResponseAsync(
            IConnection connection,
            Message request,
            IResponseHandler responseHandler,
            CancellationToken cancellationToken)
        {
            if (connection == null)
            {
                throw new ArgumentNullException(nameof(connection));
            }

            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (responseHandler == null)
            {
                throw new ArgumentNullException(nameof(responseHandler));
            }

            cancellationToken.ThrowIfCancellationRequested();

            var requestPayload = MessageUtilities.DeserializePayload<GetCredentialsRequest>(request);
            var packageSource = GetPackageSource(requestPayload.PackageSourceRepository);

            GetCredentialsResponse responsePayload = null;

            if (packageSource.IsHttp &&
                string.Equals(
                    requestPayload.PackageSourceRepository,
                    packageSource.Source,
                    StringComparison.OrdinalIgnoreCase))
            {
                ICredentials credential = null;

                using (var progressReporter = AutomaticProgressReporter.Create(
                    _plugin.Connection,
                    request,
                    PluginConstants.ProgressInterval,
                    cancellationToken))
                {
                    credential = await GetCredentialAsync(
                        packageSource,
                        requestPayload.StatusCode,
                        cancellationToken);
                }

                if (credential is AuthTypeFilteredCredentials filteredCredentials)
                {
                    responsePayload = new GetCredentialsResponse(
                        MessageResponseCode.Success,
                        filteredCredentials.InnerCredential.UserName,
                        filteredCredentials.InnerCredential.Password,
                        filteredCredentials.AuthTypes);
                }
                else if (credential is NetworkCredential networkCredential)
                {
                    responsePayload = new GetCredentialsResponse(
                        MessageResponseCode.Success,
                        networkCredential.UserName,
                        networkCredential.Password);
                }
                else
                {
                    networkCredential = credential?.GetCredential(packageSource.SourceUri, null);

                    responsePayload = new GetCredentialsResponse(
                        networkCredential != null ? MessageResponseCode.Success : MessageResponseCode.NotFound,
                        networkCredential?.UserName,
                        networkCredential?.Password);
                }
            }
            else
            {
                responsePayload = new GetCredentialsResponse(
                    MessageResponseCode.NotFound,
                    username: null,
                    password: null);
            }

            await responseHandler.SendResponseAsync(request, responsePayload, cancellationToken);
        }

        private async Task<ICredentials> GetCredentialAsync(
            PackageSource packageSource,
            HttpStatusCode statusCode,
            CancellationToken cancellationToken)
        {
            var requestType = GetCredentialRequestType(statusCode);

            if (requestType == CredentialRequestType.Proxy)
            {
                return await GetProxyCredentialAsync(packageSource, cancellationToken);
            }

            return await GetPackageSourceCredential(requestType, packageSource, cancellationToken);
        }

        private async Task<ICredentials> GetPackageSourceCredential(
            CredentialRequestType requestType,
            PackageSource packageSource,
            CancellationToken cancellationToken)
        {
            if (packageSource.Credentials != null && packageSource.Credentials.IsValid())
            {
                return packageSource.Credentials.ToICredentials();
            }

            if (_credentialService == null)
            {
                return null;
            }

            string message;
            if (requestType == CredentialRequestType.Unauthorized)
            {
                message = string.Format(
                    CultureInfo.CurrentCulture,
                    Strings.Http_CredentialsForUnauthorized,
                    packageSource.Source);
            }
            else
            {
                message = string.Format(
                    CultureInfo.CurrentCulture,
                    Strings.Http_CredentialsForForbidden,
                    packageSource.Source);
            }

            var sourceUri = packageSource.SourceUri;
            var credentials = await _credentialService.GetCredentialsAsync(
                sourceUri,
                _proxy,
                requestType,
                message,
                cancellationToken);

            return credentials;
        }

        private async Task<ICredentials> GetProxyCredentialAsync(
            PackageSource packageSource,
            CancellationToken cancellationToken)
        {
            if (_proxy != null && _credentialService != null)
            {
                var sourceUri = packageSource.SourceUri;
                var proxyUri = _proxy.GetProxy(sourceUri);
                var message = string.Format(
                    CultureInfo.CurrentCulture,
                    Strings.Http_CredentialsForProxy,
                    proxyUri);
                var proxyCredentials = await _credentialService.GetCredentialsAsync(
                    sourceUri,
                    _proxy,
                    CredentialRequestType.Proxy,
                    message,
                    cancellationToken);

                return proxyCredentials?.GetCredential(proxyUri, _basicAuthenticationType);
            }

            return null;
        }

        private static CredentialRequestType GetCredentialRequestType(HttpStatusCode statusCode)
        {
            switch (statusCode)
            {
                case HttpStatusCode.ProxyAuthenticationRequired:
                    return CredentialRequestType.Proxy;

                case HttpStatusCode.Unauthorized:
                    return CredentialRequestType.Unauthorized;

                case HttpStatusCode.Forbidden:
                default:
                    return CredentialRequestType.Forbidden;
            }
        }

        private PackageSource GetPackageSource(string packageSourceRepository)
        {
            SourceRepository sourceRepository;

            if (_repositories.TryGetValue(packageSourceRepository, out sourceRepository))
            {
                return sourceRepository.PackageSource;
            }

            return new PackageSource(packageSourceRepository);
        }
    }
}
