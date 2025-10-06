// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Configuration;

namespace NuGet.Credentials
{
    /// <summary>
    /// Interface for providing credentials.
    /// </summary>
    public interface ICredentialProvider
    {
        /// <summary>
        /// Gets the identifier of the credential provider.
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Asynchronously gets the credentials.
        /// </summary>
        /// <param name="uri">The URI for which the credentials are requested.</param>
        /// <param name="proxy">The proxy to use.</param>
        /// <param name="type">The type of credential request.</param>
        /// <param name="message">The message to display.</param>
        /// <param name="isRetry">Indicates if this is a retry attempt.</param>
        /// <param name="nonInteractive">Indicates if the request should be non-interactive.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the credential response.</returns>
        Task<CredentialResponse> GetAsync(
            Uri uri,
            IWebProxy proxy,
            CredentialRequestType type,
            string message,
            bool isRetry,
            bool nonInteractive,
            CancellationToken cancellationToken);
    }
}
