// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Protocol.Plugins;

namespace NuGet.Credentials
{
    /// <summary>
    /// Builder for credential providers that are based on the secure plugin model (Version 2.0.0)
    /// </summary>
    public class SecurePluginCredentialProviderBuilder
    {
        private readonly Common.ILogger _logger;
        private readonly IPluginManager _pluginManager;
        private readonly bool _canShowDialog;

        /// <summary>
        /// Create a credential provider builders
        /// </summary>
        /// <param name="pluginManager">pluginManager</param>
        /// <param name="canShowDialog">canShowDialog - whether can pop up a dialog or it needs to use device flow</param>
        /// <param name="logger">logger</param>
        /// <exception cref="ArgumentNullException">if <paramref name="logger"/> is null</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="pluginManager"/> is null</exception>
        public SecurePluginCredentialProviderBuilder(IPluginManager pluginManager, bool canShowDialog, Common.ILogger logger)
        {
            _pluginManager = pluginManager ?? throw new ArgumentNullException(nameof(pluginManager));
            _canShowDialog = canShowDialog;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Creates credential providers for each valid plugin (regardless if it supports authentication or not)
        /// </summary>
        /// <returns>credential providers</returns>
        public async Task<IEnumerable<ICredentialProvider>> BuildAllAsync()
        {
            var availablePlugins = await _pluginManager.FindAvailablePluginsAsync(CancellationToken.None);

            var plugins = new List<ICredentialProvider>();
            foreach (var pluginDiscoveryResult in availablePlugins)
            {
                _logger.LogDebug(string.Format(CultureInfo.CurrentCulture, Resources.SecurePluginNotice_UsingPluginAsProvider, pluginDiscoveryResult.PluginFile.Path));
                plugins.Add(new SecurePluginCredentialProvider(_pluginManager, pluginDiscoveryResult, _canShowDialog, _logger));
            }

            return plugins;
        }
    }
}
