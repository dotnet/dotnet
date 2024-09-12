// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Protocol;
using NuGet.Protocol.Plugins;

namespace NuGet.Credentials
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public static class DefaultCredentialServiceUtility
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
        /// <summary>
        /// Sets-up the CredentialService and all of its providers.
        /// It always updates the logger the CredentialService and its children own,
        /// because the lifetime of the logging infrastructure is not guaranteed. 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="nonInteractive"></param>
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
        /// Update the delegating logger for the credential service.
        /// </summary>
        /// <param name="log"></param>
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

        private static DelegatingLogger DelegatingLogger;

        // Add only the secure plugin. This will be done when there's nothing set
        // By default the plugins cannot prompt. Currently this is only used to setup from MSBuild/dotnet.exe code paths
        private static async Task<IEnumerable<ICredentialProvider>> GetCredentialProvidersAsync(ILogger logger)
        {
            var providers = new List<ICredentialProvider>();

            var securePluginProviders = await new SecurePluginCredentialProviderBuilder(pluginManager: PluginManager.Instance, canShowDialog: false, logger: logger).BuildAllAsync();
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
