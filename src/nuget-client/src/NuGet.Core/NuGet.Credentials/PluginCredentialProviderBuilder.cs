// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NuGet.Common;
using NuGet.Configuration;

namespace NuGet.Credentials
{
    /// <summary>
    /// Discovers plugin providers.
    /// </summary>
    public class PluginCredentialProviderBuilder
    {
        private readonly Configuration.ISettings _settings;
        private readonly Common.IEnvironmentVariableReader _envarReader;
        private readonly IExtensionLocator _extensionLocator;
        private readonly Common.ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginCredentialProviderBuilder"/> class.
        /// </summary>
        /// <param name="extensionLocator">The extension locator.</param>
        /// <param name="settings">The settings.</param>
        /// <param name="logger">The logger.</param>
        public PluginCredentialProviderBuilder(
            IExtensionLocator extensionLocator,
            Configuration.ISettings settings,
            Common.ILogger logger)
            : this(extensionLocator, settings, logger, EnvironmentVariableWrapper.Instance)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginCredentialProviderBuilder"/> class.
        /// </summary>
        /// <param name="extensionLocator">The extension locator.</param>
        /// <param name="settings">The settings.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="envarReader">The environment variable reader.</param>
        public PluginCredentialProviderBuilder(
            IExtensionLocator extensionLocator,
            Configuration.ISettings settings,
            Common.ILogger logger,
            Common.IEnvironmentVariableReader envarReader)
        {
            _extensionLocator = extensionLocator ?? throw new ArgumentNullException(nameof(extensionLocator));
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _envarReader = envarReader ?? throw new ArgumentNullException(nameof(envarReader));

        }

        /// <summary>
        /// Plugin providers are entered loaded the same way as other nuget extensions,
        /// matching any extension named CredentialProvider.*.exe.
        /// </summary>
        /// <returns>An enumeration of plugin providers</returns>
        public IEnumerable<ICredentialProvider> BuildAll(string verbosity)
        {
            if (verbosity == null)
            {
                throw new ArgumentNullException(nameof(verbosity));
            }

            var timeout = TimeoutSeconds;
            var pluginPaths = _extensionLocator.FindCredentialProviders();

            // Sort the plugin providers by filename within each directory
            // so that we load them in a predictable order
            // but still respect the precedence of directories loaded by
            // ExtensionLocator
            var plugins = pluginPaths
                .GroupBy(Path.GetDirectoryName)
                .SelectMany(g => g.OrderBy(Path.GetFileName, StringComparer.OrdinalIgnoreCase))
                .Select(x => new PluginCredentialProvider(_logger, x, timeout, verbosity));

            return plugins;
        }

        private int TimeoutSeconds
        {
            get
            {
                var timeoutSetting = SettingsUtility.GetConfigValue(
                    _settings, CredentialsConstants.ProviderTimeoutSecondsSetting);

                var timeoutEnvar = _envarReader.GetEnvironmentVariable(
                    CredentialsConstants.ProviderTimeoutSecondsEnvar);

                int value;
                if (int.TryParse(timeoutSetting, out value)
                    || int.TryParse(timeoutEnvar, out value))
                {
                    return value;
                }

                return CredentialsConstants.ProviderTimeoutSecondsDefault;
            }
        }
    }
}
