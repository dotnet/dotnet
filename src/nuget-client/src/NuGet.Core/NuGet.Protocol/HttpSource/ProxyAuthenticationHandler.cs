// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Configuration;

namespace NuGet.Protocol
{
    /// <summary>
    /// A message handler responsible for retrying request for authenticated proxies
    /// with missing credentials.
    /// </summary>
    public class ProxyAuthenticationHandler : DelegatingHandler
    {
        public static readonly int MaxAuthRetries = 3;
        private const string BasicAuthenticationType = "Basic";

        // Only one source may prompt at a time
        private static readonly SemaphoreSlim _credentialPromptLock = new SemaphoreSlim(1, 1);

        private readonly HttpClientHandler _clientHandler;
        private readonly ICredentialService _credentialService;
        private readonly IProxyCredentialCache _credentialCache;

        private int _authRetries;

        public ProxyAuthenticationHandler(
            HttpClientHandler clientHandler,
            ICredentialService credentialService,
            IProxyCredentialCache credentialCache)
            : base(clientHandler)
        {
            if (clientHandler == null)
            {
                throw new ArgumentNullException(nameof(clientHandler));
            }

            _clientHandler = clientHandler;

            // credential service is optional
            _credentialService = credentialService;

            if (credentialCache == null)
            {
                throw new ArgumentNullException(nameof(credentialCache));
            }

            _credentialCache = credentialCache;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            HttpResponseMessage response = null;
            var logger = request.GetOrCreateConfiguration().Logger;

            while (true)
            {
                // Clean up any previous responses
                if (response != null)
                {
                    response.Dispose();
                }

                // Store the auth start before sending the request
                var cacheVersion = _credentialCache.Version;

                try
                {
                    using (var req = request.Clone())
                    {
                        response = await base.SendAsync(req, cancellationToken);
                    }

                    if (response.StatusCode != HttpStatusCode.ProxyAuthenticationRequired)
                    {
                        return response;
                    }

                    if (_clientHandler.Proxy == null)
                    {
                        return response;
                    }

                    if (_credentialService == null)
                    {
                        return response;
                    }

                    if (!await AcquireCredentialsAsync(request.RequestUri, cacheVersion, logger, cancellationToken))
                    {
                        return response;
                    }
                }
                catch (Exception ex)
                when (ProxyAuthenticationRequired(ex) && _clientHandler.Proxy != null && _credentialService != null)
                {
                    if (!await AcquireCredentialsAsync(request.RequestUri, cacheVersion, logger, cancellationToken))
                    {
                        throw;
                    }
                }
            }
        }

#if !IS_CORECLR
        // Returns true if the cause of the exception is proxy authentication failure
        private static bool ProxyAuthenticationRequired(Exception ex)
        {
            // HACK!!! : This is a hack to workaround Xamarin Bug 19594
            var webException = ex as WebException;
            if (RuntimeEnvironmentHelper.IsMono && webException != null)
            {
                return IsMonoProxyAuthenticationRequiredError(webException);
            }

            var response = ExtractResponse(ex);
            return response?.StatusCode == HttpStatusCode.ProxyAuthenticationRequired;
        }

        private static HttpWebResponse ExtractResponse(Exception ex)
        {
            var webException = ex.InnerException as WebException;
            var response = webException?.Response as HttpWebResponse;
            return response;
        }

        private static bool IsMonoProxyAuthenticationRequiredError(WebException ex)
        {
            return ex.Status == WebExceptionStatus.SecureChannelFailure &&
                ex.Message != null &&
                ex.Message.Contains("The remote server returned a 407 status code.");
        }
#else
        private static bool ProxyAuthenticationRequired(Exception ex)
        {
            return false;
        }
#endif

        private async Task<bool> AcquireCredentialsAsync(Uri requestUri, Guid cacheVersion, ILogger log, CancellationToken cancellationToken)
        {
            try
            {
                await _credentialPromptLock.WaitAsync(cancellationToken);

                cancellationToken.ThrowIfCancellationRequested();

                // Check if the credentials have already changed
                if (cacheVersion != _credentialCache.Version)
                {
                    // retry the request with updated credentials
                    return true;
                }

                // Limit the number of retries
                _authRetries++;
                if (_authRetries >= MaxAuthRetries)
                {
                    // user prompting no more
                    return false;
                }

                var proxyAddress = _clientHandler.Proxy.GetProxy(requestUri);

                // prompt user for proxy credentials.
                var credentials = await PromptForProxyCredentialsAsync(proxyAddress, _clientHandler.Proxy, log, cancellationToken);

                cancellationToken.ThrowIfCancellationRequested();

                if (credentials == null)
                {
                    // user cancelled or error occured
                    return false;
                }

                _credentialCache.UpdateCredential(proxyAddress, credentials);

                // use the user provided credential to send the request again.
                return true;
            }
            finally
            {
                _credentialPromptLock.Release();
            }
        }

        private async Task<NetworkCredential> PromptForProxyCredentialsAsync(Uri proxyAddress, IWebProxy proxy, ILogger log, CancellationToken cancellationToken)
        {
            ICredentials promptCredentials;

            try
            {
                var message = string.Format(
                    CultureInfo.CurrentCulture,
                    Strings.Http_CredentialsForProxy,
                    proxyAddress);

                promptCredentials = await _credentialService.GetCredentialsAsync(
                    proxyAddress,
                    proxy,
                    type: CredentialRequestType.Proxy,
                    message: message,
                    cancellationToken: cancellationToken);
            }
            catch (OperationCanceledException)
            {
                throw; // pass-thru
            }
            catch (Exception e)
            {
                // Fatal credential service failure
                log.LogError(ExceptionUtilities.DisplayMessage(e));
                promptCredentials = null;
            }

            return promptCredentials?.GetCredential(proxyAddress, BasicAuthenticationType);
        }
    }
}
