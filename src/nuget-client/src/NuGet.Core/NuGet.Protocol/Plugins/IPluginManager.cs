// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.


using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Protocol.Core.Types;

namespace NuGet.Protocol.Plugins
{
    public interface IPluginManager
    {
        /// <summary>
        /// Create plugins appropriate for the given source
        /// </summary>
        /// <param name="source"></param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="ArgumentNullException">Throw if <paramref name="source"/> is null </exception>
        /// <returns>PluginCreationResults</returns>
        Task<IEnumerable<PluginCreationResult>> CreatePluginsAsync(
            SourceRepository source,
            CancellationToken cancellationToken);

        /// <summary>
        /// Find all available plugins on the machine
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>PluginDiscoveryResults</returns>
        Task<IEnumerable<PluginDiscoveryResult>> FindAvailablePluginsAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Creates a plugin from the given pluginDiscoveryResult.
        /// This plugin's operations will be source agnostic ones
        /// </summary>
        /// <param name="pluginDiscoveryResult"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>A PluginCreationResult</returns>
        Task<Tuple<bool, PluginCreationResult>> TryGetSourceAgnosticPluginAsync(PluginDiscoveryResult pluginDiscoveryResult, OperationClaim requestedOperationClaim, CancellationToken cancellationToken);
    }
}
