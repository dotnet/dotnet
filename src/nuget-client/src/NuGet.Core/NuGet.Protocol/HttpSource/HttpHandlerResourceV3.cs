// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Net;
using System.Net.Http;
using NuGet.Configuration;
using NuGet.Protocol.Core.Types;

namespace NuGet.Protocol
{
    /// <summary>
    /// Resource wrapper for an HttpClient
    /// </summary>
    public class HttpHandlerResourceV3 : HttpHandlerResource
    {
        private readonly HttpClientHandler _clientHandler;
        private readonly HttpMessageHandler _messageHandler;

        public HttpHandlerResourceV3(HttpClientHandler clientHandler, HttpMessageHandler messageHandler)
        {
            if (clientHandler == null)
            {
                throw new ArgumentNullException(nameof(clientHandler));
            }

            if (messageHandler == null)
            {
                throw new ArgumentNullException(nameof(messageHandler));
            }

            _clientHandler = clientHandler;
            _messageHandler = messageHandler;
        }

        public override HttpClientHandler ClientHandler => _clientHandler;

        public override HttpMessageHandler MessageHandler => _messageHandler;

        public static Lazy<ICredentialService> CredentialService { get; set; }

        /// <summary>
        /// Gets or sets a delegate that is to be invoked when authenticated feed credentials are successfully
        /// used.
        /// </summary>
        public static Action<Uri, ICredentials> CredentialsSuccessfullyUsed { get; set; }
    }
}
