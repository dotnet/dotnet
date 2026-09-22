// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Sdk.TestFramework;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Threading;
using Moq;
using NuGet.Configuration;
using NuGet.Credentials;
using NuGet.VisualStudio;
using Xunit;

namespace NuGet.PackageManagement.VisualStudio.Test
{
    [Collection(MockedVS.Collection)]
    public class VisualStudioCredentialProviderTests : MockedVSCollectionTests
    {
        private static readonly Uri _uri = new Uri("http://unit.test");

        private sealed class TestPackageSourceCredentialPrompt : IPackageSourceCredentialPrompt
        {
            internal required NetworkCredential? CredentialsToReturn { get; set; }
            internal bool IsRetry { get; private set; }
            internal IntPtr ParentWindow { get; private set; }
            internal Uri? PackageSourceUri { get; private set; }

            public NetworkCredential? PromptForCredentials(Uri packageSourceUri, bool isRetry, IntPtr parentWindow)
            {
                PackageSourceUri = packageSourceUri;
                IsRetry = isRetry;
                ParentWindow = parentWindow;

                return CredentialsToReturn;
            }
        }

        public VisualStudioCredentialProviderTests(GlobalServiceProvider globalServiceProvider)
            : base(globalServiceProvider)
        {
            globalServiceProvider.Reset();
        }

        [Fact]
        public void Constructor_WebProxyServiceIsNull_Throws()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new VisualStudioCredentialProvider(null));

            Assert.Equal("webProxyService", exception.ParamName);
        }

        [Fact]
        public void Constructor_JoinableTaskFactoryIsNull_Throws()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new VisualStudioCredentialProvider(
                    Mock.Of<IVsWebProxy>(),
                    joinableTaskFactory: null));

            Assert.Equal("joinableTaskFactory", exception.ParamName);
        }

        [Fact]
        public void Id_Initialized_IsRandomPerInstance()
        {
            const int trials = 3;
            var hashset = new HashSet<string>();

            for (var i = 0; i < trials; ++i)
            {
                hashset.Add(new VisualStudioCredentialProvider(Mock.Of<IVsWebProxy>()).Id);
            }

            Assert.Equal(trials, hashset.Count);
        }

        [Fact]
        public async Task GetAsync_UriIsNull_Throws()
        {
            var provider = new VisualStudioCredentialProvider(Mock.Of<IVsWebProxy>());

            var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                () => provider.GetAsync(
                    uri: null,
                    proxy: Mock.Of<IWebProxy>(),
                    type: CredentialRequestType.Unauthorized,
                    message: "a",
                    isRetry: false,
                    nonInteractive: true,
                    cancellationToken: CancellationToken.None));

            Assert.Equal("uri", exception.ParamName);
        }

        [Fact]
        public async Task GetAsync_IfCancelled_Throws()
        {
            var provider = new VisualStudioCredentialProvider(Mock.Of<IVsWebProxy>());

            await Assert.ThrowsAsync<OperationCanceledException>(
                () => provider.GetAsync(
                    _uri,
                    proxy: null,
                    type: CredentialRequestType.Unauthorized,
                    message: "a",
                    isRetry: false,
                    nonInteractive: true,
                    cancellationToken: new CancellationToken(canceled: true)));
        }

        [Theory]
        [InlineData(CredentialRequestType.Proxy)]
        [InlineData(CredentialRequestType.Unauthorized)]
        public async Task GetAsync_WhenNonInteractive_ReturnsProviderNotApplicableWithoutPrompting(
            CredentialRequestType requestType)
        {
            var vsWebProxy = new Mock<IVsWebProxy>(MockBehavior.Strict);
            var credentialPrompt = new TestPackageSourceCredentialPrompt
            {
                CredentialsToReturn = new NetworkCredential("user", "password"),
            };
            var provider = new VisualStudioCredentialProvider(
                vsWebProxy.Object,
                uiShell: null,
                new Lazy<JoinableTaskFactory>(() => NuGetUIThreadHelper.JoinableTaskFactory),
                credentialPrompt);

            CredentialResponse response = await provider.GetAsync(
                _uri,
                proxy: null,
                type: requestType,
                message: null,
                isRetry: false,
                nonInteractive: true,
                cancellationToken: CancellationToken.None);

            Assert.Equal(CredentialStatus.ProviderNotApplicable, response.Status);
            Assert.Null(credentialPrompt.PackageSourceUri);
            vsWebProxy.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task GetAsync_CallsWebProxy_PassesDefaultCredentialsState()
        {
            var vsWebProxy = new Mock<IVsWebProxy>(MockBehavior.Strict);
            var newState = (uint)__VsWebProxyState.VsWebProxyState_NoCredentials;
            var expectedCredentials = new NetworkCredential();

            vsWebProxy.Setup(x => x.PrepareWebProxy(
                    It.Is<string>(u => u == _uri.OriginalString),
                    It.Is<uint>(s => s == (uint)__VsWebProxyState.VsWebProxyState_DefaultCredentials),
                    out newState,
                    It.Is<int>(f => f == 1)))
                .Callback(() =>
                    {
                        newState = (uint)__VsWebProxyState.VsWebProxyState_PromptForCredentials;

                        WebRequest.DefaultWebProxy.Credentials = expectedCredentials;
                    })
                .Returns(0)
                .Verifiable();

            var provider = new VisualStudioCredentialProvider(
                vsWebProxy.Object,
                new Lazy<JoinableTaskFactory>(() => NuGetUIThreadHelper.JoinableTaskFactory));

            var response = await provider.GetAsync(
                _uri,
                proxy: null,
                type: CredentialRequestType.Proxy,
                message: "a",
                isRetry: false,
                nonInteractive: false,
                cancellationToken: CancellationToken.None);

            Assert.NotNull(response);
            Assert.Equal(CredentialStatus.Success, response.Status);
            Assert.Same(expectedCredentials, response.Credentials);

            vsWebProxy.Verify();
        }

        [Fact]
        public async Task GetAsync_ForPackageSource_PromptsWithPackageSourceDetails()
        {
            var vsWebProxy = new Mock<IVsWebProxy>(MockBehavior.Strict);
            var uiShell = new Mock<IVsUIShell>(MockBehavior.Strict);
            var parentWindow = new IntPtr(42);
            var expectedCredentials = new NetworkCredential("user", "password");
            var credentialPrompt = new TestPackageSourceCredentialPrompt
            {
                CredentialsToReturn = expectedCredentials,
            };
            var uri = new Uri("https://user:secret@unit.test/v3/index.json?token=secret");

            uiShell.Setup(x => x.GetDialogOwnerHwnd(out parentWindow))
                .Returns(0);

            var provider = new VisualStudioCredentialProvider(
                vsWebProxy.Object,
                uiShell.Object,
                new Lazy<JoinableTaskFactory>(() => NuGetUIThreadHelper.JoinableTaskFactory),
                credentialPrompt);

            CredentialResponse response = await provider.GetAsync(
                uri,
                proxy: null,
                type: CredentialRequestType.Unauthorized,
                message: null,
                isRetry: true,
                nonInteractive: false,
                cancellationToken: CancellationToken.None);

            Assert.Equal(CredentialStatus.Success, response.Status);
            Assert.Same(expectedCredentials, response.Credentials);
            Assert.Same(uri, credentialPrompt.PackageSourceUri);
            Assert.True(credentialPrompt.IsRetry);
            Assert.Equal(parentWindow, credentialPrompt.ParentWindow);
            vsWebProxy.VerifyNoOtherCalls();
            uiShell.VerifyAll();
        }

        [Fact]
        public async Task GetAsync_ForPackageSource_WhenUserCancels_ReturnsUserCanceled()
        {
            var credentialPrompt = new TestPackageSourceCredentialPrompt
            {
                CredentialsToReturn = null,
            };
            var provider = new VisualStudioCredentialProvider(
                Mock.Of<IVsWebProxy>(),
                uiShell: null,
                new Lazy<JoinableTaskFactory>(() => NuGetUIThreadHelper.JoinableTaskFactory),
                credentialPrompt);

            CredentialResponse response = await provider.GetAsync(
                _uri,
                proxy: null,
                type: CredentialRequestType.Forbidden,
                message: null,
                isRetry: false,
                nonInteractive: false,
                cancellationToken: CancellationToken.None);

            Assert.Equal(CredentialStatus.UserCanceled, response.Status);
            Assert.Same(_uri, credentialPrompt.PackageSourceUri);
            Assert.False(credentialPrompt.IsRetry);
            Assert.Equal(IntPtr.Zero, credentialPrompt.ParentWindow);
        }
    }
}
