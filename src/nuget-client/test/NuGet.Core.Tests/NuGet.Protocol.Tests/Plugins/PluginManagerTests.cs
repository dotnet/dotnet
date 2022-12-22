// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NuGet.Common;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.Protocol.Plugins.Tests
{
    public class PluginManagerTests
    {
        private const string PluginFilePath = "a";

        [Fact]
        public async Task TryGetSourceAgnosticPluginAsync_WhenExceptionIsThrownDuringPluginCreation_PropagatesException()
        {
            const string message = "b";

            var reader = Mock.Of<IEnvironmentVariableReader>();
            var pluginFactory = new Mock<IPluginFactory>(MockBehavior.Strict);
            var exception = new Exception(message);

            pluginFactory.Setup(x => x.GetOrCreateAsync(
                    It.Is<string>(filePath => string.Equals(filePath, PluginFilePath, StringComparison.Ordinal)),
                    It.Is<IEnumerable<string>>(arguments => arguments != null && arguments.Any()),
                    It.IsNotNull<IRequestHandlers>(),
                    It.IsNotNull<ConnectionOptions>(),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(exception);
            pluginFactory.Setup(x => x.Dispose());

            using (var directory = TestDirectory.Create())
            using (var pluginManager = new PluginManager(
                reader,
                new Lazy<IPluginDiscoverer>(() => Mock.Of<IPluginDiscoverer>()),
                (TimeSpan idleTimeout) => pluginFactory.Object,
                new Lazy<string>(() => directory.Path)))
            {
                var discoveryResult = new PluginDiscoveryResult(
                    new PluginFile(
                        PluginFilePath,
                        new Lazy<PluginFileState>(() => PluginFileState.Valid)));

                Tuple<bool, PluginCreationResult> result = await pluginManager.TryGetSourceAgnosticPluginAsync(
                    discoveryResult,
                    OperationClaim.Authentication,
                    CancellationToken.None);
                bool wasSomethingCreated = result.Item1;
                PluginCreationResult creationResult = result.Item2;

                Assert.True(wasSomethingCreated);
                Assert.NotNull(creationResult);

                Assert.Equal($"Problem starting the plugin '{PluginFilePath}'. {message}", creationResult.Message);
                Assert.Same(exception, creationResult.Exception);
            }

            pluginFactory.Verify();
        }

        [Fact]
        public async Task TryGetSourceAgnosticPluginAsync_WhenSuccessfullyCreated_OperationClaimsAreCached()
        {
            var operationClaims = new[] { OperationClaim.Authentication };

            using (var test = new PluginManagerTest(PluginFilePath, PluginFileState.Valid, operationClaims))
            {
                Assert.False(File.Exists(test.PluginCacheEntry.CacheFileName));

                var discoveryResult = new PluginDiscoveryResult(
                    new PluginFile(
                        PluginFilePath,
                        new Lazy<PluginFileState>(() => PluginFileState.Valid)));

                Tuple<bool, PluginCreationResult> result = await test.PluginManager.TryGetSourceAgnosticPluginAsync(
                    discoveryResult,
                    OperationClaim.Authentication,
                    CancellationToken.None);
                bool wasSomethingCreated = result.Item1;
                PluginCreationResult creationResult = result.Item2;

                Assert.True(wasSomethingCreated);
                Assert.NotNull(creationResult);

                Assert.True(File.Exists(test.PluginCacheEntry.CacheFileName));

                Assert.Null(creationResult.Message);
                Assert.Null(creationResult.Exception);
                Assert.Same(test.Plugin, creationResult.Plugin);
                Assert.NotNull(creationResult.PluginMulticlientUtilities);
                Assert.Equal(operationClaims, creationResult.Claims);
            }
        }

        [Fact]
        public async Task TryGetSourceAgnosticPluginAsync_WhenCacheFileIndicatesIndicatesNoSupportedOperationClaims_PluginIsNotCreated()
        {
            var operationClaims = Array.Empty<OperationClaim>();

            using (var test = new PluginManagerTest(PluginFilePath, PluginFileState.Valid, operationClaims))
            {
                test.PluginCacheEntry.OperationClaims = operationClaims;

                await test.PluginCacheEntry.UpdateCacheFileAsync();

                Assert.True(File.Exists(test.PluginCacheEntry.CacheFileName));

                var discoveryResult = new PluginDiscoveryResult(
                    new PluginFile(
                        PluginFilePath,
                        new Lazy<PluginFileState>(() => PluginFileState.Valid)));

                Tuple<bool, PluginCreationResult> result = await test.PluginManager.TryGetSourceAgnosticPluginAsync(
                    discoveryResult,
                    OperationClaim.Authentication,
                    CancellationToken.None);
                bool wasSomethingCreated = result.Item1;
                PluginCreationResult creationResult = result.Item2;

                Assert.False(wasSomethingCreated);
                Assert.Null(creationResult);

                Assert.True(File.Exists(test.PluginCacheEntry.CacheFileName));
            }
        }

        [Theory]
        [InlineData(PluginFilePath)]
        public async Task PluginManager_CreatePlugin_PrefersFrameworkSpecificEnvironmentVariable(string pluginPath)
        {
            var operationClaims = new[] { OperationClaim.Authentication };
            var mockReader = new Mock<IEnvironmentVariableReader>(MockBehavior.Strict);
            mockReader.Setup(x => x.GetEnvironmentVariable(
                    It.Is<string>(value => value == EnvironmentVariableConstants.PluginPaths)))
                .Returns("badPluginPath");
#if IS_DESKTOP
            mockReader.Setup(x => x.GetEnvironmentVariable(
                    It.Is<string>(value => value == EnvironmentVariableConstants.DesktopPluginPaths)))
                .Returns(pluginPath);
#else
            mockReader.Setup(x => x.GetEnvironmentVariable(
                    It.Is<string>(value => value == EnvironmentVariableConstants.CorePluginPaths)))
                .Returns(pluginPath);
#endif
            mockReader.Setup(x => x.GetEnvironmentVariable(
                    It.Is<string>(value => value == EnvironmentVariableConstants.RequestTimeout)))
                .Returns("RequestTimeout");
            mockReader.Setup(x => x.GetEnvironmentVariable(
                    It.Is<string>(value => value == EnvironmentVariableConstants.IdleTimeout)))
                .Returns("IdleTimeout");
            mockReader.Setup(x => x.GetEnvironmentVariable(
                    It.Is<string>(value => value == EnvironmentVariableConstants.HandshakeTimeout)))
                .Returns("HandshakeTimeout");

            using (var test = new PluginManagerTest(pluginPath, PluginFileState.Valid, operationClaims, mockReader))
            {
                var discoveryResult = new PluginDiscoveryResult(
                    new PluginFile(
                        PluginFilePath,
                        new Lazy<PluginFileState>(() => PluginFileState.Valid)));

                Tuple<bool, PluginCreationResult> result = await test.PluginManager.TryGetSourceAgnosticPluginAsync(
                    discoveryResult,
                    OperationClaim.Authentication,
                    CancellationToken.None);
                bool wasSomethingCreated = result.Item1;
                PluginCreationResult creationResult = result.Item2;

                Assert.True(wasSomethingCreated);
                Assert.NotNull(creationResult);

                Assert.Null(creationResult.Message);
                Assert.Null(creationResult.Exception);
                Assert.Same(test.Plugin, creationResult.Plugin);
                Assert.NotNull(creationResult.PluginMulticlientUtilities);
                Assert.Equal(operationClaims, creationResult.Claims);
            }
        }

        [Theory]
        [InlineData(PluginFilePath)]
        public async Task PluginManager_CreatePlugin_EmptyFrameworkSpecificEnvironmentVariableFallsBackTo(string pluginPath)
        {
            var operationClaims = new[] { OperationClaim.Authentication };
            var mockReader = new Mock<IEnvironmentVariableReader>(MockBehavior.Strict);
            mockReader.Setup(x => x.GetEnvironmentVariable(
                    It.Is<string>(value => value == EnvironmentVariableConstants.PluginPaths)))
                .Returns(pluginPath);
            mockReader.Setup(x => x.GetEnvironmentVariable(
                    It.Is<string>(value => value == EnvironmentVariableConstants.DesktopPluginPaths)))
                .Returns("   ");
            mockReader.Setup(x => x.GetEnvironmentVariable(
                    It.Is<string>(value => value == EnvironmentVariableConstants.CorePluginPaths)))
                .Returns("   ");
            mockReader.Setup(x => x.GetEnvironmentVariable(
                    It.Is<string>(value => value == EnvironmentVariableConstants.RequestTimeout)))
                .Returns("RequestTimeout");
            mockReader.Setup(x => x.GetEnvironmentVariable(
                    It.Is<string>(value => value == EnvironmentVariableConstants.IdleTimeout)))
                .Returns("IdleTimeout");
            mockReader.Setup(x => x.GetEnvironmentVariable(
                    It.Is<string>(value => value == EnvironmentVariableConstants.HandshakeTimeout)))
                .Returns("HandshakeTimeout");

            using (var test = new PluginManagerTest(pluginPath, PluginFileState.Valid, operationClaims, mockReader))
            {
                var discoveryResult = new PluginDiscoveryResult(
                    new PluginFile(
                        PluginFilePath,
                        new Lazy<PluginFileState>(() => PluginFileState.Valid)));

                Tuple<bool, PluginCreationResult> result = await test.PluginManager.TryGetSourceAgnosticPluginAsync(
                    discoveryResult,
                    OperationClaim.Authentication,
                    CancellationToken.None);
                bool wasSomethingCreated = result.Item1;
                PluginCreationResult creationResult = result.Item2;

                Assert.True(wasSomethingCreated);
                Assert.NotNull(creationResult);

                Assert.Null(creationResult.Message);
                Assert.Null(creationResult.Exception);
                Assert.Same(test.Plugin, creationResult.Plugin);
                Assert.NotNull(creationResult.PluginMulticlientUtilities);
                Assert.Equal(operationClaims, creationResult.Claims);
            }
        }

        private sealed class PluginManagerTest : IDisposable
        {
            private readonly Mock<IConnection> _connection;
            private readonly Mock<IPluginFactory> _factory;
            private readonly Mock<IPlugin> _plugin;
            private readonly Mock<IPluginDiscoverer> _pluginDiscoverer;
            private readonly Mock<IEnvironmentVariableReader> _reader;
            private readonly TestDirectory _testDirectory;

            internal IPlugin Plugin { get; }
            internal PluginCacheEntry PluginCacheEntry { get; }
            internal PluginManager PluginManager { get; }

            internal PluginManagerTest(
                string pluginFilePath,
                PluginFileState pluginFileState,
                IReadOnlyList<OperationClaim> operationClaims,
                Mock<IEnvironmentVariableReader> mockEnvironmentVariableReader = null)
            {
                _testDirectory = TestDirectory.Create();

                if (mockEnvironmentVariableReader != null)
                {
                    _reader = mockEnvironmentVariableReader;
                }
                else
                {
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
                }

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
                    .Returns(ProtocolConstants.CurrentVersion);

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

                _connection.Setup(x => x.SendRequestAndReceiveResponseAsync<GetOperationClaimsRequest, GetOperationClaimsResponse>(
                        It.Is<MessageMethod>(m => m == MessageMethod.GetOperationClaims),
                        It.Is<GetOperationClaimsRequest>(g => g.PackageSourceRepository == null),
                        It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new GetOperationClaimsResponse(operationClaims));

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

                PluginManager = new PluginManager(
                    _reader.Object,
                    new Lazy<IPluginDiscoverer>(() => _pluginDiscoverer.Object),
                    (TimeSpan idleTimeout) => _factory.Object,
                    new Lazy<string>(() => _testDirectory.Path));

                PluginCacheEntry = new PluginCacheEntry(_testDirectory.Path, pluginFilePath, requestKey: "Source-Agnostic");
                Plugin = _plugin.Object;
            }

            public void Dispose()
            {
                PluginManager.Dispose();
                _testDirectory.Dispose();

                _reader.Verify();
                _pluginDiscoverer.Verify();
                _connection.Verify();
                _plugin.Verify();
                _factory.Verify();
            }
        }


    }
}
