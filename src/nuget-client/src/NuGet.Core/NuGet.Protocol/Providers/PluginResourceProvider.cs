// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Protocol.Plugins;

namespace NuGet.Protocol.Core.Types
{
    /// <summary>
    /// A plugin resource provider.
    /// </summary>
    /// <remarks>This is unsealed only to facilitate testing.</remarks>
    public class PluginResourceProvider : ResourceProvider
    {
        private readonly IPluginManager _pluginManager;

        public PluginResourceProvider() : this(PluginManager.Instance)
        {
        }

        // To be used for testing purposes only
        public PluginResourceProvider(IPluginManager pluginManager)
            : base(typeof(PluginResource), nameof(PluginResourceProvider))
        {
            _pluginManager = pluginManager ?? throw new ArgumentNullException(nameof(pluginManager));
        }

        /// <summary>
        /// Asynchronously attempts to create a resource for the specified source repository.
        /// </summary>
        /// <param name="source">A source repository.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result (<see cref="Task{TResult}.Result" />) returns a Tuple&lt;bool, INuGetResource&gt;</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="source"/> is <see langword="null" />.</exception>
        /// <exception cref="OperationCanceledException">Thrown if <paramref name="cancellationToken"/>
        /// is cancelled.</exception>
        public override async Task<Tuple<bool, INuGetResource>> TryCreate(
            SourceRepository source,
            CancellationToken cancellationToken)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            cancellationToken.ThrowIfCancellationRequested();

            PluginResource resource = null;

            var pluginCreationResults = await _pluginManager.CreatePluginsAsync(source, cancellationToken);

            if (pluginCreationResults != null && pluginCreationResults.Any())
            {
                resource = new PluginResource(
                    pluginCreationResults,
                    source.PackageSource,
                    HttpHandlerResourceV3.CredentialService?.Value);
            }

            return new Tuple<bool, INuGetResource>(resource != null, resource);
        }
    }
}
