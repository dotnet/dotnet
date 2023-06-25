// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NuGet.Configuration;
using NuGet.Protocol.Core.Types;
using Xunit;

namespace NuGet.Protocol.Plugins.Tests
{
    public class PluginResourceTests
    {
        private static readonly PackageSource _packageSource = new PackageSource("https://unit.test");

        [Fact]
        public void Constructor_ThrowsForNullPluginCreationResults()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new PluginResource(
                    pluginCreationResults: null,
                    packageSource: _packageSource,
                    credentialService: Mock.Of<ICredentialService>()));

            Assert.Equal("pluginCreationResults", exception.ParamName);
        }

        [Fact]
        public void Constructor_ThrowsForNullPackageSource()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new PluginResource(
                    Enumerable.Empty<PluginCreationResult>(),
                    packageSource: null,
                    credentialService: Mock.Of<ICredentialService>()));

            Assert.Equal("packageSource", exception.ParamName);
        }

        [Fact]
        public void Constructor_AcceptsNullCredentialService()
        {
            new PluginResource(
                Enumerable.Empty<PluginCreationResult>(),
                _packageSource,
                credentialService: null);
        }

        [Fact]
        public async Task GetPluginAsync_ThrowsIfCancelled()
        {
            var resource = new PluginResource(
                Enumerable.Empty<PluginCreationResult>(),
                _packageSource,
                Mock.Of<ICredentialService>());

            await Assert.ThrowsAsync<OperationCanceledException>(
                () => resource.GetPluginAsync(OperationClaim.DownloadPackage, new CancellationToken(canceled: true)));
        }

        [Fact]
        public async Task GetPluginAsync_ReturnsNullIfNoPlugins()
        {
            var resource = new PluginResource(
                Enumerable.Empty<PluginCreationResult>(),
                _packageSource,
                Mock.Of<ICredentialService>());

            var result = await resource.GetPluginAsync(OperationClaim.DownloadPackage, CancellationToken.None);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetPluginAsync_ReturnsNullIfNoPluginWithMatchingOperationClaim()
        {
            var resource = new PluginResource(
                new[] { new PluginCreationResult(
                    Mock.Of<IPlugin>(),
                    Mock.Of<IPluginMulticlientUtilities>(),
                    new[] { OperationClaim.DownloadPackage }) },
                _packageSource,
                Mock.Of<ICredentialService>());

            var result = await resource.GetPluginAsync((OperationClaim)int.MaxValue, CancellationToken.None);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetPluginAsync_ThrowsIfFirstPluginDiscoveryFailed()
        {
            var pluginCreationResults = new List<PluginCreationResult>()
                {
                    new PluginCreationResult(message: "test"),
                    new PluginCreationResult(
                        Mock.Of<IPlugin>(),
                        Mock.Of<IPluginMulticlientUtilities>(),
                        new[] { OperationClaim.DownloadPackage })
                };
            var resource = new PluginResource(
                pluginCreationResults,
                _packageSource,
                Mock.Of<ICredentialService>());

            var exception = await Assert.ThrowsAsync<PluginException>(
                () => resource.GetPluginAsync(OperationClaim.DownloadPackage, CancellationToken.None));

            Assert.Equal("test", exception.Message);
        }

        [Fact]
        public async Task GetPluginAsync_ReturnsFirstPluginWithMatchingOperationClaim()
        {
            var plugin = new Mock<IPlugin>(MockBehavior.Strict);
            var utilities = new Mock<IPluginMulticlientUtilities>(MockBehavior.Strict);
            var connection = new Mock<IConnection>(MockBehavior.Strict);

            connection.Setup(x => x.SendRequestAndReceiveResponseAsync<SetCredentialsRequest, SetCredentialsResponse>(
                    It.Is<MessageMethod>(m => m == MessageMethod.SetCredentials),
                    It.Is<SetCredentialsRequest>(s => s.PackageSourceRepository == _packageSource.Source
                        && s.ProxyUsername == null && s.ProxyPassword == null
                        && s.Username == null && s.Password == null),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new SetCredentialsResponse(MessageResponseCode.Success));

            plugin.SetupGet(x => x.Connection)
                .Returns(connection.Object);

            utilities.Setup(x => x.DoOncePerPluginLifetimeAsync(
                    It.IsNotNull<string>(),
                    It.IsNotNull<Func<Task>>(),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var pluginCreationResults = new List<PluginCreationResult>()
                {
                    new PluginCreationResult(
                        plugin.Object,
                        utilities.Object,
                        new[] { OperationClaim.DownloadPackage }),
                    new PluginCreationResult(
                        Mock.Of<IPlugin>(),
                        Mock.Of<IPluginMulticlientUtilities>(),
                        new[] { OperationClaim.DownloadPackage })
                };
            var resource = new PluginResource(
                pluginCreationResults,
                _packageSource,
                credentialService: null);

            var result = await resource.GetPluginAsync(OperationClaim.DownloadPackage, CancellationToken.None);

            Assert.Same(pluginCreationResults[0].Plugin, result.Plugin);
            Assert.Same(pluginCreationResults[0].PluginMulticlientUtilities, result.PluginMulticlientUtilities);

            connection.Verify();
            plugin.Verify();
            utilities.Verify();
        }

        [Fact]
        public async Task GetPluginAsync_SendsLastKnownGoodCredentialsFromCredentialsCache()
        {
            var plugin = new Mock<IPlugin>(MockBehavior.Strict);
            var utilities = new Mock<IPluginMulticlientUtilities>(MockBehavior.Strict);
            var connection = new Mock<IConnection>(MockBehavior.Strict);
            var credentialService = new Mock<ICredentialService>(MockBehavior.Strict);
            var proxyCredentials = new NetworkCredential(userName: "a", password: "b");
            ICredentials proxyCredentialsOutResult = proxyCredentials;
            var packageSourceCredentials = new NetworkCredential(userName: "c", password: "d");
            ICredentials packageSourceCredentialsOutResult = packageSourceCredentials;

            credentialService.Setup(x => x.TryGetLastKnownGoodCredentialsFromCache(
                    It.Is<Uri>(u => u == _packageSource.SourceUri),
                    It.Is<bool>(i => i),
                    out proxyCredentialsOutResult))
                .Returns(true);

            credentialService.Setup(x => x.TryGetLastKnownGoodCredentialsFromCache(
                    It.Is<Uri>(u => u == _packageSource.SourceUri),
                    It.Is<bool>(i => !i),
                    out packageSourceCredentialsOutResult))
                .Returns(true);

            connection.Setup(x => x.SendRequestAndReceiveResponseAsync<SetCredentialsRequest, SetCredentialsResponse>(
                    It.Is<MessageMethod>(m => m == MessageMethod.SetCredentials),
                    It.Is<SetCredentialsRequest>(s => s.PackageSourceRepository == _packageSource.Source
                        && s.ProxyUsername == proxyCredentials.UserName
                        && s.ProxyPassword == proxyCredentials.Password
                        && s.Username == packageSourceCredentials.UserName
                        && s.Password == packageSourceCredentials.Password),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new SetCredentialsResponse(MessageResponseCode.Success));

            plugin.SetupGet(x => x.Connection)
                .Returns(connection.Object);

            utilities.Setup(x => x.DoOncePerPluginLifetimeAsync(
                    It.IsNotNull<string>(),
                    It.IsNotNull<Func<Task>>(),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var pluginCreationResults = new List<PluginCreationResult>()
                {
                    new PluginCreationResult(
                        plugin.Object,
                        utilities.Object,
                        new[] { OperationClaim.DownloadPackage }),
                };
            var resource = new PluginResource(
                pluginCreationResults,
                _packageSource,
                credentialService.Object);

            var result = await resource.GetPluginAsync(OperationClaim.DownloadPackage, CancellationToken.None);

            Assert.Same(pluginCreationResults[0].Plugin, result.Plugin);
            Assert.Same(pluginCreationResults[0].PluginMulticlientUtilities, result.PluginMulticlientUtilities);

            connection.Verify();
            plugin.Verify();
            utilities.Verify();
            credentialService.Verify();
        }
    }
}
