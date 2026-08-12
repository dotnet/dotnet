// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Protocol;
using NuGet.Protocol.Plugins;

namespace NuGet.Credentials
{
    /// <summary>
    /// Provides helpers for configuring the process-wide default credential service.
    /// Use in CLI or self-hosted scenarios. See NuGet.PackageManagement.VisualStudio.DefaultVSCredentialServiceProvider for Visual Studio scenarios.
    /// </summary>
    public static class DefaultCredentialServiceUtility
    {
        static DefaultCredentialServiceUtility()
        {
            StaticState.StartMSBuildRestoreTasks += ResetCredentialService;
        }

        /// <summary>
        /// Discards the process-wide credential service and its delegating logger so a process reused across builds
        /// rebuilds them for the next restore (with the current interactivity, settings, and credential providers)
        /// instead of reusing the first build's instance and its cached credentials.
        /// </summary>
        internal static void ResetCredentialService()
        {
            HttpHandlerResourceV3.CredentialService = null;
            DelegatingLogger = null;
        }

        /// <summary>
        /// Sets up the credential service and all of its providers.
        /// It always updates the logger that the credential service and its children own,
        /// because the lifetime of the logging infrastructure is not guaranteed. 
        /// </summary>
        /// <param name="logger">The logger used by the credential service and its providers.</param>
        /// <param name="nonInteractive">
        /// <see langword="true"/> to prevent credential providers from prompting the user;
        /// otherwise, <see langword="false"/>.
        /// </param>
        public static void SetupDefaultCredentialService(ILogger logger, bool nonInteractive)
        {
            // Always update the delegating logger.
            UpdateCredentialServiceDelegatingLogger(logger);

            if (HttpHandlerResourceV3.CredentialService == null)
            {
                var providers = new AsyncLazy<IEnumerable<ICredentialProvider>>(async () => await GetCredentialProvidersAsync(DelegatingLogger));
                HttpHandlerResourceV3.CredentialService = new Lazy<ICredentialService>(
                    () => new CredentialService(
                        providers: providers,
                        nonInteractive: nonInteractive,
                        handlesDefaultCredentials: PreviewFeatureSettings.DefaultCredentialsAfterCredentialProviders));
            }
        }

        /// <summary>
        /// Updates the delegating logger used by the credential service.
        /// </summary>
        /// <param name="log">The logger to which credential service messages are delegated.</param>
        [MemberNotNull(nameof(DelegatingLogger))]
        public static void UpdateCredentialServiceDelegatingLogger(ILogger log)
        {
            if (DelegatingLogger == null)
            {
                DelegatingLogger = new DelegatingLogger(log);
            }
            else
            {
                DelegatingLogger.UpdateDelegate(log);
            }
        }

        private static DelegatingLogger? DelegatingLogger;

        // Add only the secure plugin. This will be done when there's nothing set
        // By default the plugins cannot prompt. Currently this is only used to setup from MSBuild/dotnet.exe code paths
        private static async Task<IEnumerable<ICredentialProvider>> GetCredentialProvidersAsync(ILogger logger)
        {
            var providers = new List<ICredentialProvider>();

            var securePluginProviders = await new SecurePluginCredentialProviderBuilder(pluginManager: PluginManager.Instance, canShowDialog: true, logger: logger).BuildAllAsync();
            providers.AddRange(securePluginProviders);

            if (providers.Any())
            {
                if (PreviewFeatureSettings.DefaultCredentialsAfterCredentialProviders)
                {
                    providers.Add(new DefaultNetworkCredentialsCredentialProvider());
                }
            }
            return providers;
        }
    }
}
