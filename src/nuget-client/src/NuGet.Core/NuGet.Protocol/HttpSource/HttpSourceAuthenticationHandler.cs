// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Configuration;

namespace NuGet.Protocol
{
    public class HttpSourceAuthenticationHandler : DelegatingHandler
    {
        public static readonly int MaxAuthRetries = AmbientAuthenticationState.MaxAuthRetries;

        // Only one source may prompt at a time
        private readonly static SemaphoreSlim _credentialPromptLock = new SemaphoreSlim(1, 1);

        private readonly PackageSource _packageSource;
        private readonly HttpClientHandler _clientHandler;
        private readonly ICredentialService _credentialService;

        private readonly SemaphoreSlim _httpClientLock = new SemaphoreSlim(1, 1);
        private Dictionary<string, AmbientAuthenticationState> _authStates = new Dictionary<string, AmbientAuthenticationState>();
        private HttpSourceCredentials _credentials;
        private bool _isDisposed = false;

        public HttpSourceAuthenticationHandler(
            PackageSource packageSource,
            HttpClientHandler clientHandler,
            ICredentialService credentialService)
            : base(clientHandler)
        {
            _packageSource = packageSource ?? throw new ArgumentNullException(nameof(packageSource));
            _clientHandler = clientHandler ?? throw new ArgumentNullException(nameof(clientHandler));

            // credential service is optional as credentials may be attached to a package source
            _credentialService = credentialService;

            // Create a new wrapper for ICredentials that can be modified

            if (_credentialService == null || !_credentialService.HandlesDefaultCredentials)
            {
                // This is used to match the value of HttpClientHandler.UseDefaultCredentials = true
                _credentials = new HttpSourceCredentials(CredentialCache.DefaultNetworkCredentials);
            }
            else
            {
                _credentials = new HttpSourceCredentials();
            }

            if (packageSource.Credentials != null &&
                packageSource.Credentials.IsValid())
            {
                _credentials.Credentials = packageSource.Credentials.ToICredentials();
            }

            _clientHandler.Credentials = _credentials;
            // Always take the credentials from the helper.
            _clientHandler.UseDefaultCredentials = false;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(objectName: null); // we don't know the caller name
            }

            HttpResponseMessage response = null;
            ICredentials promptCredentials = null;

            var configuration = request.GetOrCreateConfiguration();

            // Authorizing may take multiple attempts
            while (true)
            {
                // Clean up any previous responses
                if (response != null)
                {
                    response.Dispose();
                }

                // store the auth state before sending the request
                var beforeLockVersion = _credentials.Version;

                using (var req = request.Clone())
                {
                    response = await base.SendAsync(req, cancellationToken);
                }

                if (_credentialService == null)
                {
                    return response;
                }

                if (response.StatusCode == HttpStatusCode.Unauthorized ||
                    (configuration.PromptOn403 && response.StatusCode == HttpStatusCode.Forbidden))
                {
                    List<Stopwatch> stopwatches = null;

#if NET5_0_OR_GREATER
                    if (request.Options.TryGetValue(
                        new HttpRequestOptionsKey<List<Stopwatch>>(HttpRetryHandler.StopwatchPropertyName),
                        out stopwatches))
                    {
#else
                    if (request.Properties.TryGetValue(HttpRetryHandler.StopwatchPropertyName, out var value))
                    {
                        stopwatches = value as List<Stopwatch>;
#endif
                        if (stopwatches != null)
                        {
                            foreach (var stopwatch in stopwatches)
                            {
                                stopwatch.Stop();
                            }
                        }
                    }

                    promptCredentials = await AcquireCredentialsAsync(
                        response.StatusCode,
                        beforeLockVersion,
                        configuration.Logger,
                        cancellationToken);

                    if (stopwatches != null)
                    {
                        foreach (var stopwatch in stopwatches)
                        {
                            stopwatch.Start();
                        }
                    }

                    if (promptCredentials == null)
                    {
                        return response;
                    }

                    continue;
                }

                if (promptCredentials != null)
                {
                    CredentialsSuccessfullyUsed(_packageSource.SourceUri, promptCredentials);
                }

                return response;
            }
        }

        private async Task<ICredentials> AcquireCredentialsAsync(HttpStatusCode statusCode, Guid credentialsVersion, ILogger log, CancellationToken cancellationToken)
        {
            // Only one request may prompt and attempt to auth at a time
            await _httpClientLock.WaitAsync();

            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                // Auth may have happened on another thread, if so just continue
                if (credentialsVersion != _credentials.Version)
                {
                    return _credentials.Credentials;
                }

                var authState = GetAuthenticationState();

                if (authState.IsBlocked)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    return null;
                }

                // Construct a reasonable message for the prompt to use.
                CredentialRequestType type;
                string message;
                if (statusCode == HttpStatusCode.Unauthorized)
                {
                    type = CredentialRequestType.Unauthorized;
                    message = string.Format(
                        CultureInfo.CurrentCulture,
                        Strings.Http_CredentialsForUnauthorized,
                        _packageSource.Source);
                }
                else
                {
                    type = CredentialRequestType.Forbidden;
                    message = string.Format(
                        CultureInfo.CurrentCulture,
                        Strings.Http_CredentialsForForbidden,
                        _packageSource.Source);
                }

                var promptCredentials = await PromptForCredentialsAsync(
                    type,
                    message,
                    authState,
                    log,
                    cancellationToken);

                if (promptCredentials == null)
                {
                    return null;
                }

                _credentials.Credentials = promptCredentials;

                return promptCredentials;
            }
            finally
            {
                _httpClientLock.Release();
            }
        }

        private AmbientAuthenticationState GetAuthenticationState()
        {
            var correlationId = ActivityCorrelationId.Current;

            AmbientAuthenticationState authState;
            if (!_authStates.TryGetValue(correlationId, out authState))
            {
                authState = new AmbientAuthenticationState();
                _authStates[correlationId] = authState;
            }

            return authState;
        }

        private async Task<ICredentials> PromptForCredentialsAsync(
            CredentialRequestType type,
            string message,
            AmbientAuthenticationState authState,
            ILogger log,
            CancellationToken token)
        {
            ICredentials promptCredentials;

            // Only one prompt may display at a time.
            await _credentialPromptLock.WaitAsync();

            try
            {
                // Get the proxy for this URI so we can pass it to the credentialService methods
                // this lets them use the proxy if they have to hit the network.
                var proxyCache = ProxyCache.Instance;
                var proxy = proxyCache?.GetProxy(_packageSource.SourceUri);

                promptCredentials = await _credentialService
                    .GetCredentialsAsync(_packageSource.SourceUri, proxy, type, message, token);

                if (promptCredentials == null)
                {
                    // If this is the case, this means none of the credential providers were able to
                    // handle the credential request or no credentials were available for the
                    // endpoint.
                    authState.Block();
                }
                else
                {
                    authState.Increment();
                }
            }
            catch (OperationCanceledException)
            {
                // This indicates a non-human cancellation.
                throw;
            }
            catch (Exception e)
            {
                // If this is the case, this means there was a fatal exception when interacting
                // with the credential service (or its underlying credential providers). Either way,
                // block asking for credentials for the live of this operation.
                log.LogError(ExceptionUtilities.DisplayMessage(e));
                promptCredentials = null;
                authState.Block();
            }
            finally
            {
                _credentialPromptLock.Release();
            }

            return promptCredentials;
        }

        private void CredentialsSuccessfullyUsed(Uri uri, ICredentials credentials)
        {
            HttpHandlerResourceV3.CredentialsSuccessfullyUsed?.Invoke(uri, credentials);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (_isDisposed)
            {
                return;
            }

            if (disposing)
            {
                // free managed resources
                _httpClientLock.Dispose();
                _authStates = null;
                _credentials = null;
            }

            _isDisposed = true;
        }
    }
}
