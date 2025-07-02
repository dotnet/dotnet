// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.IdentityModel.Protocols
{
    /// <summary>
    /// Interface that defines methods to retrieve configuration.
    /// </summary>
    /// <typeparam name="T">The type of the configuration metadata.</typeparam>
    public interface IConfigurationRetriever<T>
    {
        /// <summary>
        /// Retrieves a populated configuration given an address and an <see cref="IDocumentRetriever"/>.
        /// </summary>
        /// <param name="address">Address of the discovery document.</param>
        /// <param name="retriever">The <see cref="IDocumentRetriever"/> to use to read the discovery document.</param>
        /// <param name="cancel">A cancellation token that can be used by other objects or threads to receive notice of cancellation. <see cref="CancellationToken"/>.</param>
        Task<T> GetConfigurationAsync(string address, IDocumentRetriever retriever, CancellationToken cancel);
    }
}
