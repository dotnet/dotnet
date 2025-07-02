// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.IdentityModel.Protocols
{
    /// <summary>
    /// Interface that defines a model for retrieving configuration data.
    /// </summary>
    /// <typeparam name="T">The type of <see cref="IDocumentRetriever"/>.</typeparam>
    public interface IConfigurationManager<T> where T : class
    {
        /// <summary>
        /// Retrieve the current configuration, refreshing and/or caching as needed.
        /// This method will throw if the configuration cannot be retrieved, instead of returning null.
        /// </summary>
        /// <param name="cancel"><see cref="CancellationToken"/></param>
        /// <returns><see cref="Task{T}"/></returns>
        Task<T> GetConfigurationAsync(CancellationToken cancel);

        /// <summary>
        /// Indicate that the configuration may be stale (as indicated by failing to process incoming tokens).
        /// </summary>
        void RequestRefresh();
    }
}
