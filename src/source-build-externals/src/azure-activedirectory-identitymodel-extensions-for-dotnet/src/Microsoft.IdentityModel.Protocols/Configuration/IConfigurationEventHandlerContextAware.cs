// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.IdentityModel.Protocols.Configuration;

/// <summary>
/// Defines event handlers for configuration retrieval and update operations.
/// </summary>
/// <typeparam name="T">The type of configuration.</typeparam>
public interface IConfigurationEventHandlerContextAware<T> : IConfigurationEventHandler<T> where T : class
{
    /// <summary>
    /// Called before retrieving configuration from the metadata endpoint.
    /// </summary>
    /// <param name="metadataAddress">The metadata endpoint address.</param>
    /// <param name="context">The context for the configuration retrieval operation, providing additional information and control.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>
    /// A <see cref="ConfigurationEventHandlerResult{T}"/> if valid and available, or <see cref="ConfigurationEventHandlerResult{T}.NoResult"/> to proceed with normal retrieval.
    /// </returns>
    Task<ConfigurationEventHandlerResult<T>> BeforeRetrieveAsync(string metadataAddress, ConfigurationRetrievalContext context, CancellationToken cancellationToken = default);

    /// <summary>
    /// Called in a fire-and-forget manner after a configuration has been successfully retrieved.
    /// </summary>
    /// <param name="metadataAddress">The metadata endpoint address.</param>
    /// <param name="configuration">The retrieved configuration.</param>
    /// <param name="context">The context for the configuration retrieval operation, providing additional information and control.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    Task AfterUpdateAsync(string metadataAddress, T configuration, ConfigurationRetrievalContext context, CancellationToken cancellationToken = default);
}
