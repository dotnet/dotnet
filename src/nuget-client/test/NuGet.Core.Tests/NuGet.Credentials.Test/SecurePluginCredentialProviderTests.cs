// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Protocol.Plugins;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.Credentials.Test
{
    public sealed class SecurePluginCredentialProviderTests : IDisposable
    {
        private static readonly Uri _uri = new Uri("https://unit.test");
        private const string _username = "username";
        private const string _password = "password";

        private readonly TestDirectory _testDirectory;

        public SecurePluginCredentialProviderTests()
        {
            _testDirectory = TestDirectory.Create();
        }

        public void Dispose()
        {
            _testDirectory.Dispose();
        }

        [Fact]
        public void Constructor_WhenPluginManagerIsNull_Throws()
        {
            IPluginManager pluginManager = null;

            var exception = Assert.Throws<ArgumentNullException>(
                () => new SecurePluginCredentialProvider(pluginManager, CreatePluginDiscoveryResult(), canShowDialog: true, logger: NullLogger.Instance));

            Assert.Equal("pluginManager", exception.ParamName);
        }

        [Fact]
        public void Constructor_WhenPluginDiscoveryResultIsNull_Throws()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new SecurePluginCredentialProvider(CreateDefaultPluginManager(), pluginDiscoveryResult: null, canShowDialog: true, logger: NullLogger.Instance));

            Assert.Equal("pluginDiscoveryResult", exception.ParamName);
        }

        [Fact]
        public void Constructor_WhenLoggerIsNull_Throws()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new SecurePluginCredentialProvider(CreateDefaultPluginManager(), CreatePluginDiscoveryResult(), canShowDialog: true, logger: null));

            Assert.Equal("logger", exception.ParamName);
        }

        [Fact]
        public void Type_IsICredentialProvider()
        {
            var provider = new SecurePluginCredentialProvider(CreateDefaultPluginManager(), CreatePluginDiscoveryResult(), canShowDialog: true, logger: NullLogger.Instance);
            Assert.True(provider is ICredentialProvider);
        }

        [Fact]
        public void Id_WithValidArguments_ContainsPluginFilePath()
        {
            var pluginResult = CreatePluginDiscoveryResult();
            var provider = new SecurePluginCredentialProvider(CreateDefaultPluginManager(), pluginResult, canShowDialog: true, logger: NullLogger.Instance);
            Assert.Contains(pluginResult.PluginFile.Path, provider.Id);
        }

        [PlatformFact(Platform.Windows)]
        public async Task GetAsync_WithValidArguments_ReturnsValidCredentials()
        {
            var expectation = new TestExpectation(
                operationClaims: new[] { OperationClaim.Authentication },
                connectionOptions: ConnectionOptions.CreateDefault(),
                pluginVersion: ProtocolConstants.CurrentVersion,
                uri: _uri,
                authenticationUsername: _username,
                authenticationPassword: _password,
                success: true);

            using (var test = new PluginManagerMock(
                pluginFilePath: "a",
                pluginFileState: PluginFileState.Valid,
                expectations: expectation))
            {
                var discoveryResult = new PluginDiscoveryResult(new PluginFile("a", new Lazy<PluginFileState>(() => PluginFileState.Valid)));
                var provider = new SecurePluginCredentialProvider(test.PluginManager, discoveryResult, canShowDialog: true, logger: NullLogger.Instance);

                IWebProxy proxy = null;
                var credType = CredentialRequestType.Unauthorized;
                var message = "nothing";
                var isRetry = false;
                var isInteractive = false;
                var token = CancellationToken.None;
                var credentialResponse = await provider.GetAsync(_uri, proxy, credType, message, isRetry, isInteractive, token);

                Assert.True(credentialResponse.Status == CredentialStatus.Success);
                Assert.NotNull(credentialResponse.Credentials);
                Assert.Equal(_username, credentialResponse.Credentials.GetCredential(_uri, authType: null).UserName);
                Assert.Equal(_password, credentialResponse.Credentials.GetCredential(_uri, authType: null).Password);
            }
        }

        [PlatformFact(Platform.Windows)]
        public async Task GetAsync_WhenCalledMultipleTimes_DoesNotCreateMultipleInstancesOfANonCredentialsPlugin()
        {
            var expectation = new TestExpectation(
                operationClaims: new[] { OperationClaim.DownloadPackage },
                connectionOptions: ConnectionOptions.CreateDefault(),
                pluginVersion: ProtocolConstants.CurrentVersion,
                uri: _uri,
                authenticationUsername: _username,
                authenticationPassword: _password,
                success: false);

            using (var test = new PluginManagerMock(
                pluginFilePath: "a",
                pluginFileState: PluginFileState.Valid,
                expectations: expectation))
            {
                var discoveryResult = new PluginDiscoveryResult(new PluginFile("a", new Lazy<PluginFileState>(() => PluginFileState.Valid)));
                var provider = new SecurePluginCredentialProvider(test.PluginManager, discoveryResult, canShowDialog: true, logger: NullLogger.Instance);

                IWebProxy proxy = null;
                var credType = CredentialRequestType.Unauthorized;
                var message = "nothing";
                var isRetry = false;
                var isInteractive = false;
                var token = CancellationToken.None;
                var credentialResponse = await provider.GetAsync(_uri, proxy, credType, message, isRetry, isInteractive, token);

                Assert.True(credentialResponse.Status == CredentialStatus.ProviderNotApplicable);
                Assert.Null(credentialResponse.Credentials);

                var credentialResponse2 = await provider.GetAsync(_uri, proxy, credType, message, isRetry, isInteractive, token);
                Assert.True(credentialResponse2.Status == CredentialStatus.ProviderNotApplicable);
                Assert.Null(credentialResponse2.Credentials);
            }
        }

        [PlatformFact(Platform.Windows)]
        public async Task GetAsync_WhenPluginClaimsMultipleOperations_ReturnsValidCredentials()
        {
            var expectation = new TestExpectation(
                operationClaims: new[] { OperationClaim.Authentication, OperationClaim.DownloadPackage },
                connectionOptions: ConnectionOptions.CreateDefault(),
                pluginVersion: ProtocolConstants.CurrentVersion,
                uri: _uri,
                authenticationUsername: _username,
                authenticationPassword: _password,
                success: true);

            using (var test = new PluginManagerMock(
                pluginFilePath: "a",
                pluginFileState: PluginFileState.Valid,
                expectations: expectation))
            {
                var discoveryResult = new PluginDiscoveryResult(new PluginFile("a", new Lazy<PluginFileState>(() => PluginFileState.Valid)));
                var provider = new SecurePluginCredentialProvider(test.PluginManager, discoveryResult, canShowDialog: true, logger: NullLogger.Instance);

                IWebProxy proxy = null;
                var credType = CredentialRequestType.Unauthorized;
                var message = "nothing";
                var isRetry = false;
                var isInteractive = false;
                var token = CancellationToken.None;
                var credentialResponse = await provider.GetAsync(_uri, proxy, credType, message, isRetry, isInteractive, token);

                Assert.True(credentialResponse.Status == CredentialStatus.Success);
                Assert.NotNull(credentialResponse.Credentials);
                Assert.Equal(_username, credentialResponse.Credentials.GetCredential(_uri, authType: null).UserName);
                Assert.Equal(_password, credentialResponse.Credentials.GetCredential(_uri, authType: null).Password);
            }
        }

        [PlatformFact(Platform.Windows)]
        public async Task GetAsync_WhenProxyIsUsed_SetsProxyCredentials()
        {
            var proxyUsername = "proxyUsername";
            var proxyPassword = "proxyPassword";

            var expectation = new TestExpectation(
                operationClaims: new[] { OperationClaim.Authentication },
                connectionOptions: ConnectionOptions.CreateDefault(),
                pluginVersion: ProtocolConstants.CurrentVersion,
                uri: _uri,
                authenticationUsername: _username,
                authenticationPassword: _password,
                success: true,
                proxyUsername: proxyUsername,
                proxyPassword: proxyPassword);

            using (var test = new PluginManagerMock(
                pluginFilePath: "a",
                pluginFileState: PluginFileState.Valid,
                expectations: expectation))
            {
                var discoveryResult = new PluginDiscoveryResult(new PluginFile("a", new Lazy<PluginFileState>(() => PluginFileState.Valid)));
                var provider = new SecurePluginCredentialProvider(test.PluginManager, discoveryResult, canShowDialog: true, logger: NullLogger.Instance);
                var proxy = new System.Net.WebProxy()
                {
                    Credentials = new NetworkCredential(proxyUsername, proxyPassword)
                };
                var credType = CredentialRequestType.Unauthorized;
                var message = "nothing";
                var isRetry = false;
                var isInteractive = false;
                var token = CancellationToken.None;
                var credentialResponse = await provider.GetAsync(_uri, proxy, credType, message, isRetry, isInteractive, token);

                Assert.True(credentialResponse.Status == CredentialStatus.Success);
                Assert.NotNull(credentialResponse.Credentials);
                Assert.Equal(_username, credentialResponse.Credentials.GetCredential(_uri, authType: null).UserName);
                Assert.Equal(_password, credentialResponse.Credentials.GetCredential(_uri, authType: null).Password);
            }
        }

        [PlatformFact(Platform.Windows)]
        public async Task GetAsync_WhenCalledMultipleTimes_CachesCapabilities()
        {
            var expectation = new TestExpectation(
                operationClaims: new[] { OperationClaim.DownloadPackage },
                connectionOptions: ConnectionOptions.CreateDefault(),
                pluginVersion: ProtocolConstants.CurrentVersion,
                uri: _uri,
                authenticationUsername: _username,
                authenticationPassword: _password,
                success: false);

            using (var test = new PluginManagerMock(
                pluginFilePath: "a",
                pluginFileState: PluginFileState.Valid,
                expectations: expectation))
            {
                var discoveryResult = new PluginDiscoveryResult(new PluginFile("a", new Lazy<PluginFileState>(() => PluginFileState.Valid)));
                var provider = new SecurePluginCredentialProvider(test.PluginManager, discoveryResult, canShowDialog: true, logger: NullLogger.Instance);

                IWebProxy proxy = null;
                var credType = CredentialRequestType.Unauthorized;
                var message = "nothing";
                var isRetry = false;
                var isInteractive = false;
                var token = CancellationToken.None;
                var credentialResponse = await provider.GetAsync(_uri, proxy, credType, message, isRetry, isInteractive, token);

                Assert.True(credentialResponse.Status == CredentialStatus.ProviderNotApplicable);
                Assert.Null(credentialResponse.Credentials);
            }

            var expectations2 = new TestExpectation(
                operationClaims: new[] { OperationClaim.DownloadPackage },
                connectionOptions: ConnectionOptions.CreateDefault(),
                pluginVersion: ProtocolConstants.CurrentVersion,
                uri: _uri,
                authenticationUsername: _username,
                authenticationPassword: _password,
                success: false,
                pluginLaunched: false);

            using (var test = new PluginManagerMock(
                pluginFilePath: "a",
                pluginFileState: PluginFileState.Valid,
                expectations: expectations2))
            {
                var discoveryResult = new PluginDiscoveryResult(new PluginFile("a", new Lazy<PluginFileState>(() => PluginFileState.Valid)));
                var provider = new SecurePluginCredentialProvider(test.PluginManager, discoveryResult, canShowDialog: true, logger: NullLogger.Instance);

                IWebProxy proxy = null;
                var credType = CredentialRequestType.Unauthorized;
                var message = "nothing";
                var isRetry = false;
                var isInteractive = false;
                var token = CancellationToken.None;
                var credentialResponse = await provider.GetAsync(_uri, proxy, credType, message, isRetry, isInteractive, token);

                Assert.True(credentialResponse.Status == CredentialStatus.ProviderNotApplicable);
                Assert.Null(credentialResponse.Credentials);
            }
        }

        [PlatformFact(Platform.Windows)]
        public async Task GetAsync_SendsCorrectCanShowDialogValue()
        {
            var canShowDialog = false;
            var expectation = new TestExpectation(
                operationClaims: new[] { OperationClaim.Authentication },
                connectionOptions: ConnectionOptions.CreateDefault(),
                pluginVersion: ProtocolConstants.CurrentVersion,
                uri: _uri,
                authenticationUsername: _username,
                authenticationPassword: _password,
                success: true,
                proxyUsername: null,
                proxyPassword: null,
                pluginLaunched: true,
                canShowDialog: canShowDialog);

            using (var test = new PluginManagerMock(
                pluginFilePath: "a",
                pluginFileState: PluginFileState.Valid,
                expectations: expectation))
            {
                var discoveryResult = new PluginDiscoveryResult(new PluginFile("a", new Lazy<PluginFileState>(() => PluginFileState.Valid)));
                var provider = new SecurePluginCredentialProvider(test.PluginManager, discoveryResult, canShowDialog, logger: NullLogger.Instance);

                IWebProxy proxy = null;
                var credType = CredentialRequestType.Unauthorized;
                var message = "nothing";
                var isRetry = false;
                var isInteractive = false;
                var token = CancellationToken.None;
                var credentialResponse = await provider.GetAsync(_uri, proxy, credType, message, isRetry, isInteractive, token);

                Assert.True(credentialResponse.Status == CredentialStatus.Success);
                Assert.NotNull(credentialResponse.Credentials);
                Assert.Equal(_username, credentialResponse.Credentials.GetCredential(_uri, authType: null).UserName);
                Assert.Equal(_password, credentialResponse.Credentials.GetCredential(_uri, authType: null).Password);
            }
        }

        [PlatformFact(Platform.Windows)]
        public async Task GetAsync_WhenPluginManagerReturnsException_ExceptionIsPropagated()
        {
            var expectedMessage = "a";
            var expectedException = CreateExceptionWithCallstack("b");
            var pluginCreationResult = new PluginCreationResult(expectedMessage, expectedException);
            var result = new Tuple<bool, PluginCreationResult>(true, pluginCreationResult);
            var pluginManager = new Mock<IPluginManager>(MockBehavior.Strict);

            pluginManager.Setup(x => x.TryGetSourceAgnosticPluginAsync(
                    It.IsNotNull<PluginDiscoveryResult>(),
                    It.Is<OperationClaim>(claim => claim == OperationClaim.Authentication),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(result);

            var pluginDiscoveryResult = new PluginDiscoveryResult(new PluginFile("c", new Lazy<PluginFileState>(() => PluginFileState.Valid)));
            var logger = new Mock<ILogger>(MockBehavior.Strict);

            logger.Setup(x => x.LogError(It.Is<string>(data => data == expectedMessage)));
            logger.Setup(x => x.LogDebug(It.Is<string>(data => data == expectedException.ToString())));

            var provider = new SecurePluginCredentialProvider(pluginManager.Object, pluginDiscoveryResult, canShowDialog: false, logger: logger.Object);

            var exception = await Assert.ThrowsAsync<PluginException>(
                () => provider.GetAsync(_uri, proxy: null, type: CredentialRequestType.Forbidden, message: null, isRetry: false, nonInteractive: true, cancellationToken: CancellationToken.None));

            Assert.Same(expectedException, exception.InnerException);

            pluginManager.Verify();
            logger.Verify();
        }

        [Fact]
        public async Task GetAsync_WhenCredentialPluginIsUnableToAcquireCredentials_ReturnsNotFoundAsync()
        {
            var expectation = new TestExpectation(
                operationClaims: new[] { OperationClaim.Authentication },
                connectionOptions: ConnectionOptions.CreateDefault(),
                pluginVersion: ProtocolConstants.CurrentVersion,
                uri: _uri,
                authenticationUsername: null,
                authenticationPassword: null,
                success: true,
                messageCodeNotFound: true);

            using (var test = new PluginManagerMock(
                pluginFilePath: "a",
                pluginFileState: PluginFileState.Valid,
                expectations: expectation))
            {
                var discoveryResult = new PluginDiscoveryResult(new PluginFile("a", new Lazy<PluginFileState>(() => PluginFileState.Valid)));
                var provider = new SecurePluginCredentialProvider(test.PluginManager, discoveryResult, canShowDialog: true, logger: NullLogger.Instance);

                IWebProxy proxy = null;
                var credType = CredentialRequestType.Unauthorized;
                var message = "nothing";
                var isRetry = false;
                var isInteractive = false;
                var token = CancellationToken.None;
                var credentialResponse = await provider.GetAsync(_uri, proxy, credType, message, isRetry, isInteractive, token);

                Assert.True(credentialResponse.Status == CredentialStatus.UserCanceled);
                Assert.Null(credentialResponse.Credentials);
            }
        }

        private PluginDiscoveryResult CreatePluginDiscoveryResult(PluginFileState pluginState = PluginFileState.Valid)
        {
            return new PluginDiscoveryResult(new PluginFile(Path.Combine(_testDirectory.Path, "plugin.exe"), new Lazy<PluginFileState>(() => pluginState)));
        }

        private PluginManager CreateDefaultPluginManager()
        {
            return new PluginManager(
                Mock.Of<IEnvironmentVariableReader>(),
                new Lazy<IPluginDiscoverer>(),
                (TimeSpan idleTimeout) => Mock.Of<PluginFactory>(),
                new Lazy<string>(() => _testDirectory.Path));
        }

        private static Exception CreateExceptionWithCallstack(string message)
        {
            try
            {
                throw new Exception(message);
            }
            catch (Exception ex)
            {
                return ex;
            }

            throw new InvalidOperationException("This should never be hit.");
        }
    }
}
