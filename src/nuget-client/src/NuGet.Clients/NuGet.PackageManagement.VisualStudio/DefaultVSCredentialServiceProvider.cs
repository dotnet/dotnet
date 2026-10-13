// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using NuGet.Common;
using NuGet.Credentials;
using NuGet.Protocol.Plugins;
using IAsyncServiceProvider = Microsoft.VisualStudio.Shell.IAsyncServiceProvider;

namespace NuGet.PackageManagement.VisualStudio
{
    [Export(typeof(ICredentialServiceProvider))]
    public class DefaultVSCredentialServiceProvider : ICredentialServiceProvider
    {

        private readonly Lazy<INuGetUILogger> _outputConsoleLogger;
        private readonly IAsyncServiceProvider _asyncServiceProvider;
        private Func<Task<IEnumerable<ICredentialProvider>>> _vsCredentialProvidersFactory;
        private Func<Task<IEnumerable<ICredentialProvider>>> _pluginCredentialProvidersFactory;
        private Func<Task<IEnumerable<ICredentialProvider>>> _credentialPromptFactory;

        [ImportingConstructor]
        internal DefaultVSCredentialServiceProvider(Lazy<INuGetUILogger> outputConsoleLogger)
            : this(AsyncServiceProvider.GlobalProvider, outputConsoleLogger)
        { }

        internal DefaultVSCredentialServiceProvider(
            IAsyncServiceProvider asyncServiceProvider,
            Lazy<INuGetUILogger> outputConsoleLogger
            )
        {
            _asyncServiceProvider = asyncServiceProvider ?? throw new ArgumentNullException(nameof(asyncServiceProvider));
            _outputConsoleLogger = outputConsoleLogger ?? throw new ArgumentNullException(nameof(outputConsoleLogger));
        }

        /// <param name="vsCredentialProvidersFactory">Creates the MEF imported Visual Studio credential providers.</param>
        /// <param name="pluginCredentialProvidersFactory">Creates the child process plugin credential providers.</param>
        /// <param name="credentialPromptFactory">Creates the credential prompt provider.</param>
        internal DefaultVSCredentialServiceProvider(
            IAsyncServiceProvider asyncServiceProvider,
            Lazy<INuGetUILogger> outputConsoleLogger,
            Func<Task<IEnumerable<ICredentialProvider>>> vsCredentialProvidersFactory,
            Func<Task<IEnumerable<ICredentialProvider>>> pluginCredentialProvidersFactory,
            Func<Task<IEnumerable<ICredentialProvider>>> credentialPromptFactory
            )
            : this(asyncServiceProvider, outputConsoleLogger)
        {
            _vsCredentialProvidersFactory = vsCredentialProvidersFactory ?? throw new ArgumentNullException(nameof(vsCredentialProvidersFactory));
            _pluginCredentialProvidersFactory = pluginCredentialProvidersFactory ?? throw new ArgumentNullException(nameof(pluginCredentialProvidersFactory));
            _credentialPromptFactory = credentialPromptFactory ?? throw new ArgumentNullException(nameof(credentialPromptFactory));
        }

        public async Task<NuGet.Configuration.ICredentialService> GetCredentialServiceAsync()
        {
            // Binding these delegates can load NuGet.Credentials, so defer it until credentials are needed.
            _vsCredentialProvidersFactory ??= CreateVsCredentialProvidersAsync;
            _pluginCredentialProvidersFactory ??= CreatePluginCredentialProvidersAsync;
            _credentialPromptFactory ??= CreateCredentialPromptAsync;

            // Initialize the credential providers. Providers that can acquire credentials without user
            // interaction must come before those that prompt, so that a user is only asked to type
            // credentials when nothing else was able to supply them.
            var credentialProviders = new List<ICredentialProvider>();

            // VS MEF-plugin credential providers
            await TryAddCredentialProvidersAsync(
                credentialProviders,
                Strings.CredentialProviderFailed_VisualStudioAccountProvider,
                _vsCredentialProvidersFactory);

            // child process plugin credential providers
            await TryAddCredentialProvidersAsync(
                credentialProviders,
                Strings.CredentialProviderFailed_PluginCredentialProvider,
                _pluginCredentialProvidersFactory);

            // the current user's ambient Windows credentials
            if (PreviewFeatureSettings.DefaultCredentialsAfterCredentialProviders)
            {
                TryAddCredentialProviders(
                credentialProviders,
                Strings.CredentialProviderFailed_DefaultCredentialsCredentialProvider,
                () =>
                {
                    return new ICredentialProvider[] {
                        new DefaultNetworkCredentialsCredentialProvider()
                    };
                });
            }

            // proxy credentials and username/password prompt windows
            await TryAddCredentialProvidersAsync(
                credentialProviders,
                Strings.CredentialProviderFailed_VisualStudioCredentialProvider,
                _credentialPromptFactory);

            var credentialService = new CredentialService(
                new AsyncLazy<IEnumerable<ICredentialProvider>>(() => Task.FromResult((IEnumerable<ICredentialProvider>)credentialProviders)),
                nonInteractive: false,
                handlesDefaultCredentials: PreviewFeatureSettings.DefaultCredentialsAfterCredentialProviders);

            return credentialService;
        }

        private async Task<IEnumerable<ICredentialProvider>> CreateVsCredentialProvidersAsync()
        {
            var importer = new VsCredentialProviderImporter(
                (exception, failureMessage) => LogCredentialProviderError(exception, failureMessage));

            return await importer.GetProvidersAsync();
        }

        private static async Task<IEnumerable<ICredentialProvider>> CreatePluginCredentialProvidersAsync()
        {
            return await new SecurePluginCredentialProviderBuilder(PluginManager.Instance, canShowDialog: true, logger: NullLogger.Instance).BuildAllAsync();
        }

        private async Task<IEnumerable<ICredentialProvider>> CreateCredentialPromptAsync()
        {
            var webProxy = await _asyncServiceProvider.GetServiceAsync<SVsWebProxy, IVsWebProxy>();
            var uiShell = await _asyncServiceProvider.GetServiceAsync<SVsUIShell, IVsUIShell>();

            Debug.Assert(webProxy != null);

            return new ICredentialProvider[] {
                new VisualStudioCredentialProvider(
                    webProxy,
                    uiShell)
            };
        }

        private async Task TryAddCredentialProvidersAsync(
            List<ICredentialProvider> credentialProviders,
            string failureMessage,
            Func<Task<IEnumerable<ICredentialProvider>>> factory)
        {
            try
            {
                foreach (var credentialProvider in await factory())
                {
                    credentialProviders.Add(credentialProvider);
                }
            }
            catch (Exception exception)
            {
                LogCredentialProviderError(exception, failureMessage);
            }
        }

        private void TryAddCredentialProviders(
            List<ICredentialProvider> credentialProviders,
            string failureMessage,
            Func<IEnumerable<ICredentialProvider>> factory)
        {
            try
            {
                var providers = factory();

                if (providers != null)
                {
                    foreach (var credentialProvider in providers)
                    {
                        credentialProviders.Add(credentialProvider);
                    }
                }
            }
            catch (Exception exception)
            {
                LogCredentialProviderError(exception, failureMessage);
            }
        }

        private void LogCredentialProviderError(Exception exception, string failureMessage)
        {
            // Log the user-friendly message to the output console (no stack trace).
            _outputConsoleLogger.Value.Log(
                new LogMessage(
                    LogLevel.Error,
                    failureMessage +
                    Environment.NewLine +
                    ExceptionUtilities.DisplayMessage(exception)));

            // Write the stack trace to the activity log.
            ActivityLog.LogWarning(
                ExceptionHelper.LogEntrySource,
                failureMessage +
                Environment.NewLine +
                exception);
        }
    }
}
