// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace NuGet.Configuration
{
    /// <summary>
    /// A credentials service.
    /// </summary>
    public interface ICredentialService
    {
        /// <summary>
        /// Asynchronously gets credentials.
        /// </summary>
        /// <param name="uri">The URI for which credentials should be retrieved.</param>
        /// <param name="proxy">A web proxy.</param>
        /// <param name="type">The credential request type.</param>
        /// <param name="message">A message to display when prompting for credentials.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result (<see cref="Task{TResult}.Result" />) returns a <see cref="ICredentials" />.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="uri" /> is <see langword="null" />.</exception>
        /// <exception cref="OperationCanceledException">Thrown if <paramref name="cancellationToken" />
        /// is cancelled.</exception>
        Task<ICredentials?> GetCredentialsAsync(
            Uri uri,
            IWebProxy? proxy,
            CredentialRequestType type,
            string message,
            CancellationToken cancellationToken);

        /// <summary>
        /// Attempts to retrieve last known good credentials for a URI from a credentials cache.
        /// </summary>
        /// <remarks>
        /// When the return value is <see langword="true" />, <paramref name="credentials" /> will have last known
        /// good credentials from the credentials cache.  These credentials may have become invalid
        /// since their last use, so there is no guarantee that the credentials are currently valid.
        /// </remarks>
        /// <param name="uri">The URI for which cached credentials should be retrieved.</param>
        /// <param name="isProxy"><see langword="true" /> for proxy credentials; otherwise, <see langword="false" />.</param>
        /// <param name="credentials">Cached credentials or <see langword="null" />.</param>
        /// <returns><see langword="true" /> if a result is returned from the cache; otherwise, false.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="uri" /> is <see langword="null" />.</exception>
        bool TryGetLastKnownGoodCredentialsFromCache(
            Uri uri,
            bool isProxy,
            out ICredentials? credentials);

        /// <summary>
        /// Gets a value indicating whether this credential service wants to handle "default credentials" specially,
        /// instead of relying on DefaultNetworkCredentials
        /// </summary>
        bool HandlesDefaultCredentials { get; }
    }
}
