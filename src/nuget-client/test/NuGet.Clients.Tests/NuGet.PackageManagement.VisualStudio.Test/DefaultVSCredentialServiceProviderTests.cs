// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Sdk.TestFramework;
using Microsoft.VisualStudio.Shell.Interop;
using Moq;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Credentials;
using Xunit;
using IAsyncServiceProvider = Microsoft.VisualStudio.Shell.IAsyncServiceProvider;

namespace NuGet.PackageManagement.VisualStudio.Test
{
    [Collection(MockedVS.Collection)]
    public class DefaultVSCredentialServiceProviderTests : IDisposable
    {
        private static readonly Uri PackageSourceUri = new Uri("https://test.nuget.local/v3/index.json");

        private readonly bool _originalDefaultCredentialsAfterCredentialProviders;

        public DefaultVSCredentialServiceProviderTests(GlobalServiceProvider globalServiceProvider)
        {
            globalServiceProvider.Reset();

            // LogCredentialProviderError writes to the Visual Studio activity log.
            globalServiceProvider.AddService(typeof(SVsActivityLog), Mock.Of<IVsActivityLog>());

            _originalDefaultCredentialsAfterCredentialProviders = PreviewFeatureSettings.DefaultCredentialsAfterCredentialProviders;
        }

        public void Dispose()
        {
            PreviewFeatureSettings.DefaultCredentialsAfterCredentialProviders = _originalDefaultCredentialsAfterCredentialProviders;
        }

        [Fact]
        public async Task GetCredentialServiceAsync_WhenNotUsingDefaultNetworkCredentials_AsksPluginsBeforePrompting()
        {
            PreviewFeatureSettings.DefaultCredentialsAfterCredentialProviders = false;
            var callLog = new List<string>();
            DefaultVSCredentialServiceProvider target = CreateTarget(callLog);

            ICredentialService credentialService = await target.GetCredentialServiceAsync();
            await credentialService.GetCredentialsAsync(PackageSourceUri, proxy: null, CredentialRequestType.Unauthorized, message: "test", CancellationToken.None);

            Assert.Equal(["vs", "plugin", "prompt"], callLog);
            Assert.False(credentialService.HandlesDefaultCredentials);
        }

        [Fact]
        public async Task GetCredentialServiceAsync_WhenUsingDefaultNetworkCredentials_AsksThemAfterPluginsButBeforePrompting()
        {
            PreviewFeatureSettings.DefaultCredentialsAfterCredentialProviders = true;
            var callLog = new List<string>();
            DefaultVSCredentialServiceProvider target = CreateTarget(callLog);

            ICredentialService credentialService = await target.GetCredentialServiceAsync();
            ICredentials? credentials = await credentialService.GetCredentialsAsync(PackageSourceUri, proxy: null, CredentialRequestType.Unauthorized, message: "test", CancellationToken.None);

            // DefaultNetworkCredentialsCredentialProvider supplies credentials, so the prompt is never shown.
            Assert.Same(CredentialCache.DefaultNetworkCredentials, credentials);
            Assert.Equal(["vs", "plugin"], callLog);
            Assert.True(credentialService.HandlesDefaultCredentials);
        }

        [Fact]
        public async Task GetCredentialServiceAsync_WhenUsingDefaultNetworkCredentialsForAProxy_PromptsOnlyOnRetry()
        {
            PreviewFeatureSettings.DefaultCredentialsAfterCredentialProviders = true;
            var callLog = new List<string>();
            DefaultVSCredentialServiceProvider target = CreateTarget(callLog);
            ICredentialService credentialService = await target.GetCredentialServiceAsync();

            // DefaultNetworkCredentialsCredentialProvider ignores the request type, so it answers proxy requests too.
            ICredentials? firstAttempt = await credentialService.GetCredentialsAsync(PackageSourceUri, proxy: null, CredentialRequestType.Proxy, message: "test", CancellationToken.None);

            Assert.Same(CredentialCache.DefaultNetworkCredentials, firstAttempt);
            Assert.Equal(["vs", "plugin"], callLog);

            // Once the default credentials were rejected, it declines, so the proxy prompt is finally shown.
            // The providers that already declined are not asked again, they are served from the credential service's cache.
            callLog.Clear();
            await credentialService.GetCredentialsAsync(PackageSourceUri, proxy: null, CredentialRequestType.Proxy, message: "test", CancellationToken.None);

            Assert.Equal(["prompt"], callLog);
        }

        [Fact]
        public async Task GetCredentialServiceAsync_WhenAFactoryThrows_LogsErrorAndAsksTheOtherProviders()
        {
            PreviewFeatureSettings.DefaultCredentialsAfterCredentialProviders = false;
            var callLog = new List<string>();
            var logger = new Mock<INuGetUILogger>();
            DefaultVSCredentialServiceProvider target = CreateTarget(
                callLog,
                pluginCredentialProvidersFactory: () => throw new InvalidOperationException("plugin discovery failed"),
                logger: logger);

            ICredentialService credentialService = await target.GetCredentialServiceAsync();
            await credentialService.GetCredentialsAsync(PackageSourceUri, proxy: null, CredentialRequestType.Unauthorized, message: "test", CancellationToken.None);

            Assert.Equal(["vs", "prompt"], callLog);
            logger.Verify(l => l.Log(It.Is<ILogMessage>(m => m.Level == LogLevel.Error)), Times.Once);
        }

        private static DefaultVSCredentialServiceProvider CreateTarget(
            List<string> callLog,
            Func<Task<IEnumerable<ICredentialProvider>>>? pluginCredentialProvidersFactory = null,
            Mock<INuGetUILogger>? logger = null)
        {
            return new DefaultVSCredentialServiceProvider(
                Mock.Of<IAsyncServiceProvider>(),
                new Lazy<INuGetUILogger>(() => (logger ?? new Mock<INuGetUILogger>()).Object),
                Factory(callLog, "vs"),
                pluginCredentialProvidersFactory ?? Factory(callLog, "plugin"),
                Factory(callLog, "prompt"));
        }

        private static Func<Task<IEnumerable<ICredentialProvider>>> Factory(List<string> callLog, params string[] ids)
        {
            return () => Task.FromResult<IEnumerable<ICredentialProvider>>(ids.Select(id => new RecordingCredentialProvider(id, callLog)).ToArray());
        }

        /// <summary>
        /// A credential provider that records that it was asked for credentials, but never supplies any, so that
        /// <see cref="CredentialService" /> continues on to every remaining provider.
        /// </summary>
        private sealed class RecordingCredentialProvider : ICredentialProvider
        {
            private readonly List<string> _callLog;

            public RecordingCredentialProvider(string id, List<string> callLog)
            {
                Id = id;
                _callLog = callLog;
            }

            public string Id { get; }

            public Task<CredentialResponse> GetAsync(Uri uri, IWebProxy? proxy, CredentialRequestType type, string? message, bool isRetry, bool nonInteractive, CancellationToken cancellationToken)
            {
                _callLog.Add(Id);

                return Task.FromResult(new CredentialResponse(CredentialStatus.ProviderNotApplicable));
            }
        }
    }
}
