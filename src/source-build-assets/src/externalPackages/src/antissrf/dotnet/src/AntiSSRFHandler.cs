// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Threading;
using System.Threading.Tasks;

#if NET5_0_OR_GREATER
#else
using System.Security.Cryptography.X509Certificates;
using System.Security.Authentication;
#endif

#if NET5_0_OR_GREATER
using InnerHandlerType = System.Net.Http.SocketsHttpHandler;
#else
using InnerHandlerType = System.Net.Http.HttpClientHandler;
#endif

namespace Microsoft.Security.AntiSSRF
{
    public sealed class AntiSSRFHandler : HttpMessageHandler
    {
        private readonly InnerHandlerType _innerHandler;
        private readonly RedirectHandler _redirectHandler;
        private volatile bool _disposed;
        private volatile bool _editLock;

        internal AntiSSRFHandler(AntiSSRFPolicy policy)
        {
            if (policy is null)
                throw new ArgumentNullException(nameof(policy));

#if NET5_0_OR_GREATER
            _innerHandler = InnerHandler.GetHandler(policy);       
#else
            _innerHandler = new InnerHandler(policy);
#endif
            _redirectHandler = new RedirectHandler(_innerHandler, policy);
        }

        private void CheckDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(AntiSSRFHandler));
        }

        private void CheckDisposedOrLocked()
        {
            CheckDisposed();
            if (_editLock)
                throw new InvalidOperationException("Cannot edit properties after a request has been sent.");
        }

#if NET5_0_OR_GREATER
        protected override HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            CheckDisposed();
            _editLock = true;
            return _redirectHandler.SendWrapper(request, cancellationToken);
        }
#endif

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            CheckDisposed();
            _editLock = true;
            return _redirectHandler.SendAsyncWrapper(request, cancellationToken);
        }

        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                _redirectHandler.Dispose();
                _disposed = true;
            }
        }

        // ----- Properties handled directly -----

        public bool AllowAutoRedirect
        {
            get => _redirectHandler.AllowAutoRedirect;
            set
            {
                CheckDisposedOrLocked();

                _redirectHandler.AllowAutoRedirect = value;
            }
        }

        public int MaxAutomaticRedirections
        {
            get => _redirectHandler.MaxAutomaticRedirections;
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(MaxAutomaticRedirections));
                }
                CheckDisposedOrLocked();

                _redirectHandler.MaxAutomaticRedirections = value;
            }
        }

        // ----- Properties that pass through to the underlying handler -----

#if NET5_0_OR_GREATER
        public CookieContainer CookieContainer
        {
            get => _innerHandler.CookieContainer;
            set
            {
                CheckDisposedOrLocked();
                _innerHandler.CookieContainer = value;
            }
        }
#endif

        public ICredentials? Credentials
        {
            get => _innerHandler.Credentials;
            set
            {
                CheckDisposedOrLocked();
                _innerHandler.Credentials = value;
            }
        }

        public int MaxConnectionsPerServer
        {
            get => _innerHandler.MaxConnectionsPerServer;
            set
            {
                CheckDisposedOrLocked();
                _innerHandler.MaxConnectionsPerServer = value;
            }
        }

        public int MaxResponseHeadersLength
        {
            get => _innerHandler.MaxResponseHeadersLength;
            set
            {
                CheckDisposedOrLocked();
                _innerHandler.MaxResponseHeadersLength = value;
            }
        }

#if NET5_0_OR_GREATER
        public bool UseCookies
        {
            get => _innerHandler.UseCookies;
            set
            {
                CheckDisposedOrLocked();
                _innerHandler.UseCookies = value;
            }
        }
#endif

#if NET5_0_OR_GREATER
        public TimeSpan ConnectTimeout
        {
            get => _innerHandler.ConnectTimeout;
            set
            {
                CheckDisposedOrLocked();
                _innerHandler.ConnectTimeout = value;
            }
        }

        public TimeSpan ResponseDrainTimeout
        {
            get => _innerHandler.ResponseDrainTimeout;
            set
            {
                CheckDisposedOrLocked();
                _innerHandler.ResponseDrainTimeout = value;
            }
        }

        public TimeSpan PooledConnectionIdleTimeout
        {
            get => _innerHandler.PooledConnectionIdleTimeout;
            set
            {
                CheckDisposedOrLocked();
                _innerHandler.PooledConnectionIdleTimeout = value;
            }
        }

        public TimeSpan PooledConnectionLifetime
        {
            get => _innerHandler.PooledConnectionLifetime;
            set
            {
                CheckDisposedOrLocked();
                _innerHandler.PooledConnectionLifetime = value;
            }
        }

        public SslClientAuthenticationOptions SslOptions
        {
            get => _innerHandler.SslOptions;
            set
            {
                CheckDisposedOrLocked();
                _innerHandler.SslOptions = value;
            }
        }
#else
        // Maps to SslOptions.CertificateRevocationCheckMode
        public bool CheckCertificateRevocationList
        {
            get => _innerHandler.CheckCertificateRevocationList;
            set
            {
                CheckDisposedOrLocked();
                _innerHandler.CheckCertificateRevocationList = value;
            }
        }

        // Maps to SslOptions.RemoteCertificateValidationCallback
        public Func<HttpRequestMessage, X509Certificate2?, X509Chain?, SslPolicyErrors, bool>? ServerCertificateCustomValidationCallback
        {
            get => _innerHandler.ServerCertificateCustomValidationCallback;
            set
            {
                CheckDisposedOrLocked();
                _innerHandler.ServerCertificateCustomValidationCallback = value;
            }
        }

        // Maps to SslOptions.EnabledSslProtocols
        public SslProtocols SslProtocols
        {
            get => _innerHandler.SslProtocols;
            set
            {
                CheckDisposedOrLocked();
                _innerHandler.SslProtocols = value;
            }
        }
#endif
    }
}
