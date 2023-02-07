// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Moq;
using Newtonsoft.Json.Linq;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Protocol;
using NuGet.Protocol.Plugins;
using NuGet.Test.Utility;

namespace NuGet.Credentials.Test
{
    using SemanticVersion = Versioning.SemanticVersion;

    internal sealed class TestExpectation
    {
        internal IEnumerable<OperationClaim> OperationClaims { get; }
        public string OperationClaimsSourceRepository { get; }
        public JObject ServiceIndex { get; }
        public ConnectionOptions ClientConnectionOptions { get; }
        public SemanticVersion PluginVersion { get; }
        public Uri Uri { get; }
        public string AuthenticationUsername { get; }
        public string AuthenticationPassword { get; }
        public bool Success { get; }
        public string ProxyUsername { get; }
        public string ProxyPassword { get; }
        public bool PluginLaunched { get; }
        public bool CanShowDialog { get; }

        internal TestExpectation(
            IEnumerable<OperationClaim> operationClaims,
            ConnectionOptions connectionOptions,
            SemanticVersion pluginVersion,
            Uri uri,
            string authenticationUsername,
            string authenticationPassword,
            bool success,
            string proxyUsername = null,
            string proxyPassword = null,
            bool pluginLaunched = true,
            bool canShowDialog = true)
        {
            OperationClaims = operationClaims;
            ClientConnectionOptions = connectionOptions;
            PluginVersion = pluginVersion;
            Uri = uri;
            AuthenticationUsername = authenticationUsername;
            AuthenticationPassword = authenticationPassword;
            Success = success;
            ProxyPassword = proxyPassword;
            ProxyUsername = proxyUsername;
            PluginLaunched = pluginLaunched;
            CanShowDialog = canShowDialog;
        }
    }

    internal sealed class PluginManagerMock : IDisposable
    {
        private readonly Mock<IConnection> _connection;
        private readonly TestExpectation _expectations;
        private readonly Mock<IPluginFactory> _factory;
        private readonly Mock<IPlugin> _plugin;
        private readonly Mock<IPluginDiscoverer> _pluginDiscoverer;
        private readonly Mock<IEnvironmentVariableReader> _reader;

        internal PluginManager PluginManager { get; }

        private readonly string _pluginFilePath;
        private readonly TestDirectory _testDirectory;

        internal PluginManagerMock(
            string pluginFilePath,
            PluginFileState pluginFileState,
            TestExpectation expectations)
        {
            _pluginFilePath = pluginFilePath;
            _expectations = expectations;
            _testDirectory = TestDirectory.Create();

            _reader = new Mock<IEnvironmentVariableReader>(MockBehavior.Strict);
            EnsureAllEnvironmentVariablesAreCalled(pluginFilePath);

            _pluginDiscoverer = new Mock<IPluginDiscoverer>(MockBehavior.Strict);
            EnsureDiscovererIsCalled(pluginFilePath, pluginFileState);

            _connection = new Mock<IConnection>(MockBehavior.Strict);
            EnsureBasicPluginSetupCalls();

            _plugin = new Mock<IPlugin>(MockBehavior.Strict);
            EnsurePluginSetupCalls();

            _factory = new Mock<IPluginFactory>(MockBehavior.Strict);
            EnsureFactorySetupCalls(pluginFilePath);

            // Setup connection
            _connection.SetupGet(x => x.Options)
                .Returns(expectations.ClientConnectionOptions);

            _connection.SetupGet(x => x.ProtocolVersion)
                .Returns(expectations.PluginVersion);

            // Setup expectations
            _connection.Setup(x => x.SendRequestAndReceiveResponseAsync<GetOperationClaimsRequest, GetOperationClaimsResponse>(
                    It.Is<MessageMethod>(m => m == MessageMethod.GetOperationClaims),
                    It.Is<GetOperationClaimsRequest>(
                        g => g.PackageSourceRepository == expectations.OperationClaimsSourceRepository),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GetOperationClaimsResponse(expectations.OperationClaims.ToArray()));

            if (_expectations.Success)
            {
                _connection.Setup(x => x.SendRequestAndReceiveResponseAsync<SetLogLevelRequest, SetLogLevelResponse>(
                        It.Is<MessageMethod>(m => m == MessageMethod.SetLogLevel),
                        It.IsAny<SetLogLevelRequest>(),
                        It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new SetLogLevelResponse(MessageResponseCode.Success));
            }

            if (expectations.ProxyUsername != null && expectations.ProxyPassword != null)
            {
                _connection.Setup(x => x.SendRequestAndReceiveResponseAsync<SetCredentialsRequest, SetCredentialsResponse>(
                        It.Is<MessageMethod>(m => m == MessageMethod.SetCredentials),
                        It.Is<SetCredentialsRequest>(e => e.PackageSourceRepository.Equals(expectations.Uri.AbsolutePath) && e.Password == null && e.Username == null && e.ProxyPassword.Equals(expectations.ProxyPassword) && e.ProxyUsername.Equals(expectations.ProxyUsername)),
                        It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new SetCredentialsResponse(MessageResponseCode.Success));
            }

            if (_expectations.Success)
            {
                _connection.Setup(x => x.SendRequestAndReceiveResponseAsync<GetAuthenticationCredentialsRequest, GetAuthenticationCredentialsResponse>(
                        It.Is<MessageMethod>(m => m == MessageMethod.GetAuthenticationCredentials),
                        It.Is<GetAuthenticationCredentialsRequest>(e => e.Uri.Equals(expectations.Uri) && e.CanShowDialog.Equals(expectations.CanShowDialog)),
                        It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new GetAuthenticationCredentialsResponse(expectations.AuthenticationUsername, expectations.AuthenticationPassword, message: null, authenticationTypes: null, responseCode: MessageResponseCode.Success));
            }

            PluginManager = new PluginManager(
                _reader.Object,
                new Lazy<IPluginDiscoverer>(() => _pluginDiscoverer.Object),
                (TimeSpan idleTimeout) => _factory.Object,
                new Lazy<string>(() => _testDirectory.Path));
        }

        public void Dispose()
        {
            LocalResourceUtils.DeleteDirectoryTree(
                Path.Combine(
                    SettingsUtility.GetPluginsCacheFolder(),
                    CachingUtility.RemoveInvalidFileNameChars(CachingUtility.ComputeHash(_pluginFilePath))),
                new List<string>());
            PluginManager.Dispose();

            _reader.Verify();
            _pluginDiscoverer.Verify();
            if (_expectations.PluginLaunched)
            {
                _connection.Verify(x => x.SendRequestAndReceiveResponseAsync<GetOperationClaimsRequest, GetOperationClaimsResponse>(
                    It.Is<MessageMethod>(m => m == MessageMethod.GetOperationClaims),
                    It.Is<GetOperationClaimsRequest>(
                        g => g.PackageSourceRepository == null), // The source repository should be null in the context of credential plugins
                    It.IsAny<CancellationToken>()), Times.Once());

                if (_expectations.Success)
                {
                    _connection.Verify(x => x.SendRequestAndReceiveResponseAsync<GetAuthenticationCredentialsRequest, GetAuthenticationCredentialsResponse>(
                        It.Is<MessageMethod>(m => m == MessageMethod.GetAuthenticationCredentials),
                        It.IsAny<GetAuthenticationCredentialsRequest>(),
                        It.IsAny<CancellationToken>()), Times.Once());
                }

                if (_expectations.ProxyUsername != null && _expectations.ProxyPassword != null)
                {
                    _connection.Verify(x => x.SendRequestAndReceiveResponseAsync<SetCredentialsRequest, SetCredentialsResponse>(
                            It.Is<MessageMethod>(m => m == MessageMethod.SetCredentials),
                            It.Is<SetCredentialsRequest>(e => e.PackageSourceRepository.Equals(_expectations.Uri.AbsolutePath) && e.Password == null && e.Username == null && e.ProxyPassword.Equals(_expectations.ProxyPassword) && e.ProxyUsername.Equals(_expectations.ProxyUsername)),
                            It.IsAny<CancellationToken>()),
                        Times.Once());
                }
            }
            _connection.Verify();

            _plugin.Verify();
            _factory.Verify();

            _testDirectory.Dispose();
        }

        private void EnsureAllEnvironmentVariablesAreCalled(string pluginFilePath)
        {
            _reader.Setup(x => x.GetEnvironmentVariable(
                    It.Is<string>(value => value == EnvironmentVariableConstants.PluginPaths)))
                .Returns(pluginFilePath);
            _reader.Setup(x => x.GetEnvironmentVariable(
                    It.Is<string>(value => value == EnvironmentVariableConstants.CorePluginPaths)))
                .Returns((string)null);
            _reader.Setup(x => x.GetEnvironmentVariable(
                    It.Is<string>(value => value == EnvironmentVariableConstants.DesktopPluginPaths)))
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

        private void EnsureDiscovererIsCalled(string pluginFilePath, PluginFileState pluginFileState)
        {
            _pluginDiscoverer.Setup(x => x.Dispose());
            _pluginDiscoverer.Setup(x => x.DiscoverAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new[]
                    {
                        new PluginDiscoveryResult(new PluginFile(pluginFilePath, new Lazy<PluginFileState>(() => pluginFileState)))
                    });
        }

        private void EnsureBasicPluginSetupCalls()
        {
            _connection.Setup(x => x.Dispose());

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

            //An Authentication claim triggers AddOrUpdateLogger.
            if (_expectations.OperationClaims.Contains(OperationClaim.Authentication))
            {
                _connection.Setup(x => x.MessageDispatcher.RequestHandlers.AddOrUpdate(
                    It.Is<MessageMethod>(m => m == MessageMethod.Log),
                    It.IsAny<Func<IRequestHandler>>(),
                    It.IsAny<Func<IRequestHandler, IRequestHandler>>()));
            }
        }

        private void EnsurePluginSetupCalls()
        {
            _plugin.Setup(x => x.Dispose());
            _plugin.SetupGet(x => x.Connection)
                .Returns(_connection.Object);
            _plugin.SetupGet(x => x.Id)
                .Returns("id");
        }

        private void EnsureFactorySetupCalls(string pluginFilePath)
        {
            _factory.Setup(x => x.Dispose());
            _factory.Setup(x => x.GetOrCreateAsync(
                    It.Is<string>(p => p == pluginFilePath),
                    It.IsNotNull<IEnumerable<string>>(),
                    It.IsNotNull<IRequestHandlers>(),
                    It.IsNotNull<ConnectionOptions>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(_plugin.Object);
        }
    }
}
