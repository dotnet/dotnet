// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Newtonsoft.Json.Linq;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Protocol.Core.Types;
using NuGet.Protocol.Tests;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.Protocol.Plugins.Tests
{
    [Collection(nameof(NotThreadSafeResourceCollection))]
    public class PluginResourceProviderTests
    {
        private const string _sourceUri = "https://unit.test";

        [Fact]
        public void Before_IsEmpty()
        {
            var provider = new PluginResourceProvider();

            Assert.Empty(provider.Before);
        }

        [Fact]
        public void After_IsEmpty()
        {
            var provider = new PluginResourceProvider();

            Assert.Empty(provider.After);
        }

        [Fact]
        public void Name_IsPluginResourceProvider()
        {
            var provider = new PluginResourceProvider();

            Assert.Equal(nameof(PluginResourceProvider), provider.Name);
        }

        [Fact]
        public void ResourceType_IsPluginResource()
        {
            var provider = new PluginResourceProvider();

            Assert.Equal(typeof(PluginResource), provider.ResourceType);
        }

        [Fact]
        public async Task TryCreate_ThrowsForNullSource()
        {
            var provider = new PluginResourceProvider();

            var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                () => provider.TryCreate(source: null, cancellationToken: CancellationToken.None));

            Assert.Equal("source", exception.ParamName);
        }

        [Fact]
        public async Task TryCreate_ThrowsIfCancelled()
        {
            var sourceRepository = CreateSourceRepository(serviceIndexResource: null, sourceUri: _sourceUri);
            var provider = new PluginResourceProvider();

            await Assert.ThrowsAsync<OperationCanceledException>(
                () => provider.TryCreate(sourceRepository, new CancellationToken(canceled: true)));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task TryCreate_ReturnsFalseForNullOrEmptyEnvironmentVariable(string pluginsPath)
        {
            using (var test = new PluginResourceProviderNegativeTest(
                serviceIndexJson: "{}",
                sourceUri: _sourceUri,
                pluginsPath: pluginsPath))
            {
                var result = await test.Provider.TryCreate(test.SourceRepository, CancellationToken.None);

                Assert.False(result.Item1);
                Assert.Null(result.Item2);
            }
        }

        [Fact]
        public async Task TryCreate_ReturnsFalseIfNoServiceIndexResourceV3()
        {
            using (var test = new PluginResourceProviderNegativeTest(
                serviceIndexJson: null,
                sourceUri: _sourceUri))
            {
                var result = await test.Provider.TryCreate(test.SourceRepository, CancellationToken.None);

                Assert.False(result.Item1);
                Assert.Null(result.Item2);
            }
        }

        [Theory]
        [InlineData("\\unit\test")]
        [InlineData("file:///C:/unit/test")]
        public async Task TryCreate_ReturnsFalseIfPackageSourceIsNotHttpOrHttps(string sourceUri)
        {
            using (var test = new PluginResourceProviderNegativeTest(
                serviceIndexJson: "{}",
                sourceUri: sourceUri))
            {
                var result = await test.Provider.TryCreate(test.SourceRepository, CancellationToken.None);

                Assert.False(result.Item1);
                Assert.Null(result.Item2);
            }
        }

        [PlatformFact(Platform.Windows)]
        public async Task TryCreate_ReturnsPluginResource()
        {
            var expectations = new[]
                {
                    new PositiveTestExpectation("{}", "https://unit.test", new [] { OperationClaim.DownloadPackage })
                };

            using (var test = new PluginResourceProviderPositiveTest(
                pluginFilePath: "a",
                pluginFileState: PluginFileState.Valid,
                expectations: expectations))
            {
                var result = await test.Provider.TryCreate(expectations[0].SourceRepository, CancellationToken.None);

                Assert.True(result.Item1);
                Assert.IsType<PluginResource>(result.Item2);

                var pluginResource = (PluginResource)result.Item2;
                var pluginResult = await pluginResource.GetPluginAsync(
                    OperationClaim.DownloadPackage,
                    CancellationToken.None);

                Assert.NotNull(pluginResult);
                Assert.NotNull(pluginResult.Plugin);
                Assert.NotNull(pluginResult.PluginMulticlientUtilities);
            }
        }

        [PlatformFact(Platform.Windows)]
        public async Task TryCreate_QueriesPluginForEachPackageSourceRepository()
        {
            var expectations = new[]
                {
                    new PositiveTestExpectation("{\"serviceIndex\":1}", "https://1.unit.test", new [] { OperationClaim.DownloadPackage }),
                    new PositiveTestExpectation("{\"serviceIndex\":2}", "https://2.unit.test", Enumerable.Empty<OperationClaim>()),
                    new PositiveTestExpectation("{\"serviceIndex\":3}", "https://3.unit.test", new [] { OperationClaim.DownloadPackage })
                };

            using (var test = new PluginResourceProviderPositiveTest(
                pluginFilePath: "a",
                pluginFileState: PluginFileState.Valid,
                expectations: expectations))
            {
                IPlugin firstPluginResult = null;
                IPlugin thirdPluginResult = null;

                for (var i = 0; i < expectations.Length; ++i)
                {
                    var expectation = expectations[i];
                    var result = await test.Provider.TryCreate(expectation.SourceRepository, CancellationToken.None);

                    Assert.True(result.Item1);
                    Assert.IsType<PluginResource>(result.Item2);

                    var pluginResource = (PluginResource)result.Item2;
                    var pluginResult = await pluginResource.GetPluginAsync(
                        OperationClaim.DownloadPackage,
                        CancellationToken.None);

                    switch (i)
                    {
                        case 0:
                            firstPluginResult = pluginResult.Plugin;
                            break;

                        case 1:
                            Assert.Null(pluginResult);
                            break;

                        case 2:
                            thirdPluginResult = pluginResult.Plugin;
                            break;
                    }
                }

                Assert.NotNull(firstPluginResult);
                Assert.Same(firstPluginResult, thirdPluginResult);
            }
        }

        private static SourceRepository CreateSourceRepository(
            ServiceIndexResourceV3 serviceIndexResource,
            string sourceUri)
        {
            var packageSource = new PackageSource(sourceUri);
            var provider = new Mock<ServiceIndexResourceV3Provider>();

            provider.Setup(x => x.Name)
                .Returns(nameof(ServiceIndexResourceV3Provider));
            provider.Setup(x => x.ResourceType)
                .Returns(typeof(ServiceIndexResourceV3));

            var tryCreateResult = new Tuple<bool, INuGetResource>(serviceIndexResource != null, serviceIndexResource);

            provider.Setup(x => x.TryCreate(It.IsAny<SourceRepository>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(tryCreateResult));

            return new SourceRepository(packageSource, new[] { provider.Object });
        }

        private sealed class PluginResourceProviderNegativeTest : IDisposable
        {
            private readonly Mock<IPluginDiscoverer> _pluginDiscoverer;
            private readonly PluginManager _pluginManager;
            private readonly Mock<IEnvironmentVariableReader> _environmentVariableReader;
            private readonly TestDirectory _testDirectory;

            internal PluginResourceProvider Provider { get; }
            internal SourceRepository SourceRepository { get; }

            internal PluginResourceProviderNegativeTest(string serviceIndexJson, string sourceUri, string pluginsPath = "a")
            {
                var serviceIndex = string.IsNullOrEmpty(serviceIndexJson)
                    ? null : new ServiceIndexResourceV3(JObject.Parse(serviceIndexJson), DateTime.UtcNow);

                SourceRepository = CreateSourceRepository(serviceIndex, sourceUri);

                _environmentVariableReader = new Mock<IEnvironmentVariableReader>(MockBehavior.Strict);

                _environmentVariableReader.Setup(x => x.GetEnvironmentVariable(
                        It.Is<string>(value => value == EnvironmentVariableConstants.PluginPaths)))
                    .Returns(pluginsPath);
                _environmentVariableReader.Setup(x => x.GetEnvironmentVariable(
                        It.Is<string>(value => value == EnvironmentVariableConstants.DesktopPluginPaths)))
                    .Returns((string)null);
                _environmentVariableReader.Setup(x => x.GetEnvironmentVariable(
                        It.Is<string>(value => value == EnvironmentVariableConstants.CorePluginPaths)))
                    .Returns((string)null);
                _environmentVariableReader.Setup(x => x.GetEnvironmentVariable(
                        It.Is<string>(value => value == EnvironmentVariableConstants.RequestTimeout)))
                    .Returns("b");
                _environmentVariableReader.Setup(x => x.GetEnvironmentVariable(
                        It.Is<string>(value => value == EnvironmentVariableConstants.IdleTimeout)))
                    .Returns("c");
                _environmentVariableReader.Setup(x => x.GetEnvironmentVariable(
                        It.Is<string>(value => value == EnvironmentVariableConstants.HandshakeTimeout)))
                    .Returns("d");

                _pluginDiscoverer = new Mock<IPluginDiscoverer>(MockBehavior.Strict);
                var pluginDiscoveryResults = GetPluginDiscoveryResults(pluginsPath);

                _pluginDiscoverer.Setup(x => x.DiscoverAsync(It.IsAny<CancellationToken>()))
                    .ReturnsAsync(pluginDiscoveryResults);

                _testDirectory = TestDirectory.Create();

                _pluginManager = new PluginManager(
                    _environmentVariableReader.Object,
                    new Lazy<IPluginDiscoverer>(() => _pluginDiscoverer.Object),
                    (TimeSpan idleTimeout) => Mock.Of<IPluginFactory>(),
                    new Lazy<string>(() => _testDirectory.Path));
                Provider = new PluginResourceProvider(_pluginManager);
            }

            public void Dispose()
            {
                _pluginManager.Dispose();
                _testDirectory.Dispose();

                _environmentVariableReader.Verify();
                _pluginDiscoverer.Verify();
            }

            private static IEnumerable<PluginDiscoveryResult> GetPluginDiscoveryResults(string pluginPaths)
            {
                var results = new List<PluginDiscoveryResult>();

                if (string.IsNullOrEmpty(pluginPaths))
                {
                    return results;
                }

                foreach (var path in pluginPaths.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    var state = path == "a" ? PluginFileState.Valid : PluginFileState.InvalidEmbeddedSignature;
                    var file = new PluginFile(path, new Lazy<PluginFileState>(() => state));
                    results.Add(new PluginDiscoveryResult(file));
                }

                return results;
            }
        }

        private sealed class PluginResourceProviderPositiveTest : IDisposable
        {
            private readonly Mock<IConnection> _connection;
            private readonly IEnumerable<PositiveTestExpectation> _expectations;
            private readonly Mock<IPluginFactory> _factory;
            private readonly Mock<IPlugin> _plugin;
            private readonly Mock<IPluginDiscoverer> _pluginDiscoverer;
            private readonly Mock<IEnvironmentVariableReader> _reader;
            private readonly string _pluginFilePath;
            private readonly TestDirectory _testDirectory;

            internal PluginResourceProvider Provider { get; }
            internal PluginManager PluginManager { get; }

            internal PluginResourceProviderPositiveTest(
                string pluginFilePath,
                PluginFileState pluginFileState,
                IEnumerable<PositiveTestExpectation> expectations)
            {
                _expectations = expectations;
                _pluginFilePath = pluginFilePath;
                _reader = new Mock<IEnvironmentVariableReader>(MockBehavior.Strict);

                _reader.Setup(x => x.GetEnvironmentVariable(
                        It.Is<string>(value => value == EnvironmentVariableConstants.PluginPaths)))
                    .Returns(pluginFilePath);
                _reader.Setup(x => x.GetEnvironmentVariable(
                        It.Is<string>(value => value == EnvironmentVariableConstants.DesktopPluginPaths)))
                    .Returns((string)null);
                _reader.Setup(x => x.GetEnvironmentVariable(
                        It.Is<string>(value => value == EnvironmentVariableConstants.CorePluginPaths)))
                    .Returns((string)null);
                _reader.Setup(x => x.GetEnvironmentVariable(
                        It.Is<string>(value => value == EnvironmentVariableConstants.RequestTimeout)))
                    .Returns("RequestTimeout");
                _reader.Setup(x => x.GetEnvironmentVariable(
                        It.Is<string>(value => value == EnvironmentVariableConstants.IdleTimeout)))
                    .Returns("IdleTimeout");
                _reader.Setup(x => x.GetEnvironmentVariable(
                        It.Is<string>(value => value == EnvironmentVariableConstants.HandshakeTimeout)))
                    .Returns("HandshakeTimeout");

                _pluginDiscoverer = new Mock<IPluginDiscoverer>(MockBehavior.Strict);

                _pluginDiscoverer.Setup(x => x.Dispose());
                _pluginDiscoverer.Setup(x => x.DiscoverAsync(It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new[]
                        {
                            new PluginDiscoveryResult(new PluginFile(pluginFilePath, new Lazy<PluginFileState>(() => pluginFileState)))
                        });

                _connection = new Mock<IConnection>(MockBehavior.Strict);

                _connection.Setup(x => x.Dispose());
                _connection.SetupGet(x => x.Options)
                    .Returns(ConnectionOptions.CreateDefault());

                _connection.SetupGet(x => x.ProtocolVersion)
                    .Returns(ProtocolConstants.Version100);

                _connection.Setup(x => x.SendRequestAndReceiveResponseAsync<MonitorNuGetProcessExitRequest, MonitorNuGetProcessExitResponse>(
                        It.Is<MessageMethod>(m => m == MessageMethod.MonitorNuGetProcessExit),
                        It.IsNotNull<MonitorNuGetProcessExitRequest>(),
                        It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new MonitorNuGetProcessExitResponse(MessageResponseCode.Success));

                _connection.Setup(x => x.SendRequestAndReceiveResponseAsync<InitializeRequest, InitializeResponse>(
                        It.Is<MessageMethod>(m => m == MessageMethod.Initialize),
                        It.IsNotNull<InitializeRequest>(),
                        It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new InitializeResponse(MessageResponseCode.Success));

                foreach (var expectation in expectations)
                {
                    _connection.Setup(x => x.SendRequestAndReceiveResponseAsync<GetOperationClaimsRequest, GetOperationClaimsResponse>(
                            It.Is<MessageMethod>(m => m == MessageMethod.GetOperationClaims),
                            It.Is<GetOperationClaimsRequest>(
                                g => g.PackageSourceRepository == expectation.SourceRepository.PackageSource.Source),
                            It.IsAny<CancellationToken>()))
                        .ReturnsAsync(new GetOperationClaimsResponse(expectation.OperationClaims.ToArray()));

                    if (expectation.OperationClaims.Any())
                    {
                        _connection.Setup(x => x.SendRequestAndReceiveResponseAsync<SetCredentialsRequest, SetCredentialsResponse>(
                                It.Is<MessageMethod>(m => m == MessageMethod.SetCredentials),
                                It.Is<SetCredentialsRequest>(
                                    g => g.PackageSourceRepository == expectation.SourceRepository.PackageSource.Source),
                                It.IsAny<CancellationToken>()))
                            .ReturnsAsync(new SetCredentialsResponse(MessageResponseCode.Success));
                    }
                }

                _plugin = new Mock<IPlugin>(MockBehavior.Strict);

                _plugin.Setup(x => x.Dispose());
                _plugin.SetupGet(x => x.Connection)
                    .Returns(_connection.Object);
                _plugin.SetupGet(x => x.Id)
                    .Returns("id");

                _factory = new Mock<IPluginFactory>(MockBehavior.Strict);

                _factory.Setup(x => x.Dispose());
                _factory.Setup(x => x.GetOrCreateAsync(
                        It.Is<string>(p => p == pluginFilePath),
                        It.IsNotNull<IEnumerable<string>>(),
                        It.IsNotNull<IRequestHandlers>(),
                        It.IsNotNull<ConnectionOptions>(),
                        It.IsAny<CancellationToken>()))
                    .ReturnsAsync(_plugin.Object);

                _testDirectory = TestDirectory.Create();

                PluginManager = new PluginManager(
                    _reader.Object,
                    new Lazy<IPluginDiscoverer>(() => _pluginDiscoverer.Object),
                    (TimeSpan idleTimeout) => _factory.Object,
                    new Lazy<string>(() => _testDirectory.Path));
                Provider = new PluginResourceProvider(PluginManager);
            }

            public void Dispose()
            {
                LocalResourceUtils.DeleteDirectoryTree(
                    Path.Combine(
                        SettingsUtility.GetPluginsCacheFolder(),
                        CachingUtility.RemoveInvalidFileNameChars(CachingUtility.ComputeHash(_pluginFilePath))),
                    new List<string>());
                PluginManager.Dispose();
                _testDirectory.Dispose();

                _reader.Verify();
                _pluginDiscoverer.Verify();

                foreach (var expectation in _expectations)
                {
                    _connection.Verify(x => x.SendRequestAndReceiveResponseAsync<GetOperationClaimsRequest, GetOperationClaimsResponse>(
                        It.Is<MessageMethod>(m => m == MessageMethod.GetOperationClaims),
                        It.Is<GetOperationClaimsRequest>(
                            g => g.PackageSourceRepository == expectation.SourceRepository.PackageSource.Source),
                        It.IsAny<CancellationToken>()), Times.Once());

                    var expectedSetCredentialsRequestCalls = expectation.OperationClaims.Any()
                        ? Times.Once() : Times.Never();

                    _connection.Verify(x => x.SendRequestAndReceiveResponseAsync<SetCredentialsRequest, SetCredentialsResponse>(
                        It.Is<MessageMethod>(m => m == MessageMethod.SetCredentials),
                        It.Is<SetCredentialsRequest>(
                            g => g.PackageSourceRepository == expectation.SourceRepository.PackageSource.Source),
                        It.IsAny<CancellationToken>()), expectedSetCredentialsRequestCalls);
                }

                _plugin.Verify();
                _factory.Verify();
            }
        }

        private sealed class PositiveTestExpectation
        {
            internal IEnumerable<OperationClaim> OperationClaims { get; }
            internal SourceRepository SourceRepository { get; }

            internal PositiveTestExpectation(
                string serviceIndexJson,
                string sourceUri,
                IEnumerable<OperationClaim> operationClaims)
            {
                var serviceIndex = string.IsNullOrEmpty(serviceIndexJson)
                    ? null : new ServiceIndexResourceV3(JObject.Parse(serviceIndexJson), DateTime.UtcNow);

                OperationClaims = operationClaims;
                SourceRepository = CreateSourceRepository(serviceIndex, sourceUri);
            }
        }
    }
}
