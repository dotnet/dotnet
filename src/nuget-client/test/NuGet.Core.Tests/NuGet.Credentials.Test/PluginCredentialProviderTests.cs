using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NuGet.Configuration;
using Xunit;

namespace NuGet.Credentials.Test
{
    public class PluginCredentialProviderTests
    {
        private const string DefaultTestStdOut = @"{""username"":""u"", ""password"":""p"", ""Message"":""""}";
        private const string DefaultVerbosity = "Detailed";
        private string _testStdOut;

        public Mock<PluginCredentialProvider> CreateMockProvider(
            int testStatusCode = 0,
            string testStdOut = DefaultTestStdOut,
            string verbosity = DefaultVerbosity)
        {
            _testStdOut = testStdOut;
            var mockLogger = new Mock<Common.ILogger>();
            var mockProvider = new Mock<PluginCredentialProvider>(
                mockLogger.Object,
                @"c:\path\plugin.exe",
                10,
                verbosity)
            { CallBase = true };

            mockProvider
                .Setup(x => x.Execute(It.IsAny<ProcessStartInfo>(), It.IsAny<CancellationToken>(), out testStdOut))
                .Returns(testStatusCode);
            return mockProvider;
        }

        [Fact]
        public async Task WhenProxyRequest_ReturnNull()
        {
            // Arrange
            var mockProvider = CreateMockProvider();
            var proxy = null as IWebProxy;
            var uri = new Uri("http://host/");
            var type = CredentialRequestType.Proxy;
            var message = null as string;
            var isRetry = false;
            var nonInteractive = false;

            // Act
            var result = await mockProvider.Object.GetAsync(
                uri,
                proxy,
                type,
                message,
                isRetry,
                nonInteractive,
                CancellationToken.None);

            // Assert
            Assert.Equal(CredentialStatus.ProviderNotApplicable, result.Status);
            Assert.Null(result.Credentials);
            mockProvider.Verify(
                x => x.Execute(It.IsAny<ProcessStartInfo>(), It.IsAny<CancellationToken>(), out _testStdOut),
                Times.Never());
        }

        [Fact]
        public async Task CreatesExpectedCredentialRequestWithUnauthorized()
        {
            // Arrange
            var mockProvider = CreateMockProvider();
            var uri = new Uri("http://host/");
            var proxy = null as IWebProxy;
            var type = CredentialRequestType.Unauthorized;
            var message = null as string;
            var isRetry = true;
            var nonInteractive = true;

            // Act
            var result = await mockProvider.Object.GetAsync(
                uri,
                proxy,
                type,
                message,
                isRetry,
                nonInteractive,
                CancellationToken.None);

            // Assert
            mockProvider.Verify(x => x.Execute(
                It.Is<ProcessStartInfo>(p =>
                    p.Arguments == "-uri http://host/ -isRetry -nonInteractive -verbosity detailed"),
                It.IsAny<CancellationToken>(),
                out _testStdOut));
        }

        [Theory, InlineData("Silent"), InlineData("Detailed")]
        public async Task PassVerbosityParameter(string verbosity)
        {
            // Arrange
            var mockProvider = CreateMockProvider(verbosity: verbosity);
            var uri = new Uri("http://host/");
            var proxy = null as IWebProxy;
            var type = CredentialRequestType.Unauthorized;
            var message = null as string;
            var isRetry = true;
            var nonInteractive = true;

            // Act
            var result = await mockProvider.Object.GetAsync(
                uri,
                proxy,
                type,
                message,
                isRetry,
                nonInteractive,
                CancellationToken.None);

            // Assert
            mockProvider.Verify(x => x.Execute(
                It.Is<ProcessStartInfo>(p =>
                    p.Arguments == $"-uri http://host/ -isRetry -nonInteractive -verbosity {verbosity.ToLower()}"),
                It.IsAny<CancellationToken>(),
                out _testStdOut));
        }

        [Fact]
        public async Task WhenVerbosityNormal_DoNotPassVerbosityParameter()
        {
            // Arrange
            var mockProvider = CreateMockProvider(verbosity: "Normal");
            var uri = new Uri("http://host/");
            var proxy = null as IWebProxy;
            var type = CredentialRequestType.Unauthorized;
            var message = null as string;
            var isRetry = true;
            var nonInteractive = true;

            // Act
            var result = await mockProvider.Object.GetAsync(
                uri,
                proxy,
                type,
                message,
                isRetry,
                nonInteractive,
                CancellationToken.None);

            // Assert
            mockProvider.Verify(x => x.Execute(
                It.Is<ProcessStartInfo>(p => p.Arguments == "-uri http://host/ -isRetry -nonInteractive"),
                It.IsAny<CancellationToken>(),
                out _testStdOut));
        }

        [Fact]
        public async Task CreatesExpectedCredentialRequestWithForbidden()
        {
            // Arrange
            var mockProvider = CreateMockProvider();
            var uri = new Uri("http://host/");
            var proxy = null as IWebProxy;
            var type = CredentialRequestType.Forbidden;
            var message = null as string;
            var isRetry = true;
            var nonInteractive = true;

            // Act
            var result = await mockProvider.Object.GetAsync(
                uri,
                proxy,
                type,
                message,
                isRetry,
                nonInteractive,
                CancellationToken.None);

            // Assert
            mockProvider.Verify(x => x.Execute(
                It.Is<ProcessStartInfo>(p =>
                    p.Arguments == "-uri http://host/ -isRetry -nonInteractive -verbosity detailed"),
                It.IsAny<CancellationToken>(),
                out _testStdOut));
        }

        [Fact]
        public async Task WhenResponseContainsAbort_ThenThrow()
        {
            // Arrange
            var mockProvider = CreateMockProvider(
                testStatusCode: (int)PluginCredentialResponseExitCode.Failure,
                testStdOut: string.Empty);
            var uri = new Uri("http://host/");
            var proxy = null as IWebProxy;
            var type = CredentialRequestType.Unauthorized;
            var message = null as string;
            var isRetry = true;
            var nonInteractive = true;

            // Act & Assert
            var exception = await Record.ExceptionAsync(async () => await mockProvider.Object.GetAsync(
                uri,
                proxy,
                type,
                message,
                isRetry,
                nonInteractive,
                CancellationToken.None));

            Assert.IsAssignableFrom<PluginException>(exception);
            Assert.Contains(
                @"Credential plugin c:\path\plugin.exe handles this request, but is unable to provide credentials.",
                exception.Message);
        }

        [Fact]
        public async Task WhenResponseContainsAbortAndAbortMessage_ThenThrow()
        {
            // Arrange
            var mockProvider = CreateMockProvider(
                testStatusCode: (int)PluginCredentialResponseExitCode.Failure,
                testStdOut: @"{""Message"":""Extra message.""}");
            var uri = new Uri("http://host/");
            var proxy = null as IWebProxy;
            var type = CredentialRequestType.Unauthorized;
            var message = null as string;
            var isRetry = true;
            var nonInteractive = true;

            // Act & Assert
            var exception = await Record.ExceptionAsync(async () => await mockProvider.Object.GetAsync(
               uri,
               proxy,
               type,
               message,
               isRetry,
               nonInteractive,
               CancellationToken.None));

            Assert.IsAssignableFrom<PluginException>(exception);
            Assert.Contains(
                @"Credential plugin c:\path\plugin.exe handles this request, but is unable to provide credentials. Extra message.",
                exception.Message);
        }

        [Fact]
        public async Task WhenResponseContainsUsername_ReturnCredential()
        {
            // Arrange
            var mockProvider = CreateMockProvider(testStdOut: @"{""username"":""u"", ""password"":""p"", ""Message"":""""}");
            var uri = new Uri("http://host/");
            var proxy = null as IWebProxy;
            var type = CredentialRequestType.Unauthorized;
            var message = null as string;
            var isRetry = true;
            var nonInteractive = true;

            // Act
            var result = await mockProvider.Object.GetAsync(
                uri,
                proxy,
                type,
                message,
                isRetry,
                nonInteractive,
                CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Credentials);
            Assert.Equal("u", result.Credentials.GetCredential(uri, "basic")?.UserName);
        }

        [Fact]
        public async Task WhenResponseContainsAuthTypeFilter_AppliesFilter()
        {
            // Arrange
            var mockProvider = CreateMockProvider(
                testStdOut: @"{""username"":""u"", ""password"":""p"", ""AuthTypes"": [ ""basic"" ], ""Message"":""""}");
            var uri = new Uri("http://host/");
            var proxy = null as IWebProxy;
            var type = CredentialRequestType.Unauthorized;
            var message = null as string;
            var isRetry = true;
            var nonInteractive = true;

            // Act
            var result = await mockProvider.Object.GetAsync(
                uri,
                proxy,
                type,
                message,
                isRetry,
                nonInteractive,
                CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Credentials);
            Assert.Equal("p", result.Credentials.GetCredential(uri, "basic")?.Password);
            Assert.Null(result.Credentials.GetCredential(uri, "negotiate")?.Password);
        }

        [Fact]
        public async Task WhenResponseDoesNotContainAuthTypeFilter_DoesNotFilter()
        {
            // Arrange
            var mockProvider = CreateMockProvider(testStdOut: @"{""username"":""u"", ""password"":""p"", ""Message"":""""}");
            var uri = new Uri("http://host/");
            var proxy = null as IWebProxy;
            var type = CredentialRequestType.Unauthorized;
            var message = null as string;
            var isRetry = true;
            var nonInteractive = true;

            // Act
            var result = await mockProvider.Object.GetAsync(
                uri,
                proxy,
                type,
                message,
                isRetry,
                nonInteractive,
                CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Credentials);
            Assert.Equal("p", result.Credentials.GetCredential(uri, "basic")?.Password);
            Assert.Equal("p", result.Credentials.GetCredential(uri, "digest")?.Password);
            Assert.Equal("p", result.Credentials.GetCredential(uri, "negotiate")?.Password);
            Assert.Equal("p", result.Credentials.GetCredential(uri, "ntlm")?.Password);
        }

        [Fact]
        public async Task WhenResponseContainsPassword_ReturnCredential()
        {
            // Arrange
            var mockProvider = CreateMockProvider();
            var uri = new Uri("http://host/");
            var proxy = null as IWebProxy;
            var type = CredentialRequestType.Unauthorized;
            var message = null as string;
            var isRetry = true;
            var nonInteractive = true;

            // Act
            var result = await mockProvider.Object.GetAsync(
                uri,
                proxy,
                type,
                message,
                isRetry,
                nonInteractive,
                CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Credentials);
            Assert.Equal("p", result.Credentials.GetCredential(uri, "basic")?.Password);
        }

        [Fact]
        public async Task WhenCredentialProviderIsCanceled_Throws()
        {
            // Arrange
            var mockProvider = CreateMockProvider();
            var uri = new Uri("http://host/");
            var proxy = null as IWebProxy;
            var type = CredentialRequestType.Unauthorized;
            var message = null as string;
            var isRetry = true;
            var nonInteractive = true;
            var exception = new OperationCanceledException();
            mockProvider
                .Setup(x => x.Execute(It.IsAny<ProcessStartInfo>(), It.IsAny<CancellationToken>(), out _testStdOut))
                .Throws(exception);

            // Act & Assert
            var actual = await Assert.ThrowsAsync<OperationCanceledException>(() => mockProvider.Object.GetAsync(
                uri,
                proxy,
                type,
                message,
                isRetry,
                nonInteractive,
                CancellationToken.None));
            Assert.Same(exception, actual);
        }

        [Fact]
        public async Task WhenResponseEmptyUsernameAndPassword_Throws()
        {
            // Arrange
            var mockProvider = CreateMockProvider(testStdOut: @"{""username"":"""", ""password"":"""", ""Message"":""""}");
            var uri = new Uri("http://host/");
            var proxy = null as IWebProxy;
            var type = CredentialRequestType.Unauthorized;
            var message = null as string;
            var isRetry = true;
            var nonInteractive = true;

            // Act & Assert
            var exception = await Assert.ThrowsAsync<PluginException>(() => mockProvider.Object.GetAsync(
                uri,
                proxy,
                type,
                message,
                isRetry,
                nonInteractive,
                CancellationToken.None));

            Assert.IsAssignableFrom<PluginException>(exception);
            Assert.Contains(
                @"the payload was not valid",
                exception.Message);
        }

        [Fact]
        public async Task WhenInvalidResponse_DoesNotLeakPasswordData()
        {
            // Arrange
            // To be valid, either username or password must be supplied, and AuthTypes must be null or nonempty list
            var mockProvider = CreateMockProvider(
                testStdOut: @"{""username"":""user"", ""password"":""secret"", ""AuthTypes"":""{}"", ""Message"":""""}");
            var uri = new Uri("http://host/");
            var proxy = null as IWebProxy;
            var type = CredentialRequestType.Unauthorized;
            var message = null as string;
            var isRetry = true;
            var nonInteractive = true;

            // Act & Assert
            var exception = await Assert.ThrowsAsync<PluginException>(() => mockProvider.Object.GetAsync(
                uri,
                proxy,
                type,
                message,
                isRetry,
                nonInteractive,
                CancellationToken.None));

            Assert.IsAssignableFrom<PluginException>(exception);
            Assert.DoesNotContain(
                @"secret",
                exception.Message);
        }

        [Fact]
        public async Task WhenUnexpectedStatus_Throws()
        {
            // Arrange
            var mockProvider = CreateMockProvider(testStatusCode: (int)10);
            var uri = new Uri("http://host/");
            var proxy = null as IWebProxy;
            var type = CredentialRequestType.Unauthorized;
            var message = null as string;
            var isRetry = true;
            var nonInteractive = true;

            // Act & Assert
            var exception = await Assert.ThrowsAsync<PluginUnexpectedStatusException>(() => mockProvider.Object.GetAsync(
                uri,
                proxy,
                type,
                message,
                isRetry,
                nonInteractive,
                CancellationToken.None));

            Assert.Contains(
                @"Credential plugin c:\path\plugin.exe exited with unexpected error 10.",
                exception.Message);
        }

        [Fact]
        public async Task OnPluginUnexpectedStatusException_RetryWithoutVerbosityFlag()
        {
            // Arrange
            var mockLogger = new Mock<Common.ILogger>();
            var mockProvider = new Mock<PluginCredentialProvider>(
                mockLogger.Object,
                @"c:\path\plugin.exe",
                10,
                "Detailed")
            { CallBase = true };
            var stdout1 = @"{""Message"":""Unexpected Parameter""}";
            var stdout2 = @"{""username"":""u1"", ""password"":""p1"", ""Message"":""""}";
            mockProvider.Setup(x => x.Execute(
                    It.Is<ProcessStartInfo>(p => p.Arguments.Contains("-verbosity")),
                    It.IsAny<CancellationToken>(),
                    out stdout1))
                .Returns((int)-1)
                .Verifiable();
            mockProvider.Setup(x => x.Execute(
                    It.Is<ProcessStartInfo>(p => !p.Arguments.Contains("-verbosity")),
                    It.IsAny<CancellationToken>(),
                    out stdout2))
                .Returns((int)PluginCredentialResponseExitCode.Success)
                .Verifiable();

            var uri = new Uri("http://host/");
            var proxy = null as IWebProxy;
            var type = CredentialRequestType.Unauthorized;
            var message = null as string;
            var isRetry = true;
            var nonInteractive = true;

            // Act
            var result = await mockProvider.Object.GetAsync(
                uri,
                proxy,
                type,
                message,
                isRetry,
                nonInteractive,
                CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Credentials);
            Assert.Equal("u1", result.Credentials.GetCredential(uri, "basic")?.UserName);
            Assert.Equal("p1", result.Credentials.GetCredential(uri, "basic")?.Password);
            mockProvider.VerifyAll(); // ensure both calls to Execute occurred
        }

        [Fact]
        public async Task OnPluginUnexpectedStatusException_NoRetryIfVerbosityFlagWasNotSent()
        {
            // Arrange
            var mockLogger = new Mock<Common.ILogger>();
            // note that we do not pass the verbosity flag to the plugin for "Normal" verbosity.
            var mockProvider = new Mock<PluginCredentialProvider>(
                mockLogger.Object,
                @"c:\path\plugin.exe",
                10,
                "Normal")
            { CallBase = true };
            var stdout1 = @"{""Message"":""Unexpected Parameter""}";
            mockProvider.Setup(x => x.Execute(
                    It.Is<ProcessStartInfo>(p => !p.Arguments.Contains("-verbosity")),
                    It.IsAny<CancellationToken>(),
                    out stdout1))
                .Returns((int)-1)
                .Verifiable();

            var uri = new Uri("http://host/");
            var proxy = null as IWebProxy;
            var type = CredentialRequestType.Unauthorized;
            var message = null as string;
            var isRetry = true;
            var nonInteractive = true;

            // Act & Assert
            var exception = await Assert.ThrowsAsync<PluginUnexpectedStatusException>(() => mockProvider.Object.GetAsync(
                uri,
                proxy,
                type,
                message,
                isRetry,
                nonInteractive,
                CancellationToken.None));
            mockProvider.Verify(
                x => x.Execute(It.IsAny<ProcessStartInfo>(), It.IsAny<CancellationToken>(), out _testStdOut),
                Times.Once());
        }

        [Fact]
        public void SetsIdBasedOnTypeAndFilename()
        {
            FileInfo providerFileInfo = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                ? new FileInfo(@"c:\some\path\provider.exe")
                : new FileInfo("/some/path/provider");

            var mockLogger = new Mock<Common.ILogger>();
            var provider = new PluginCredentialProvider(mockLogger.Object, providerFileInfo.FullName, 5, "Normal");

            Assert.StartsWith($"PluginCredentialProvider_{providerFileInfo.Name}_", provider.Id);
        }

        [Fact]
        public async Task EncodesSpacesWhenUriWithSpaces()
        {
            // Arrange
            var mockProvider = CreateMockProvider(verbosity: "Normal");
            var uri = new Uri("http://host/path with spaces/index.json");
            var proxy = null as IWebProxy;
            var type = CredentialRequestType.Unauthorized;
            var message = null as string;
            var isRetry = false;
            var nonInteractive = false;

            // Act
            var result = await mockProvider.Object.GetAsync(
                uri,
                proxy,
                type,
                message,
                isRetry,
                nonInteractive,
                CancellationToken.None);

            // Assert
            mockProvider.Verify(x => x.Execute(
                It.Is<ProcessStartInfo>(p => p.Arguments == "-uri http://host/path%20with%20spaces/index.json"),
                It.IsAny<CancellationToken>(),
                out _testStdOut));
        }

        [Fact]
        public async Task EncodesSpacesWhenUriWithEncodedSpaces()
        {
            // Arrange
            var mockProvider = CreateMockProvider(verbosity: "Normal");
            var uri = new Uri("http://host/path%20with%20spaces/index.json");
            var proxy = null as IWebProxy;
            var type = CredentialRequestType.Unauthorized;
            var message = null as string;
            var isRetry = false;
            var nonInteractive = false;

            // Act
            var result = await mockProvider.Object.GetAsync(
                uri,
                proxy,
                type,
                message,
                isRetry,
                nonInteractive,
                CancellationToken.None);

            // Assert
            mockProvider.Verify(x => x.Execute(
                It.Is<ProcessStartInfo>(p => p.Arguments == "-uri http://host/path%20with%20spaces/index.json"),
                It.IsAny<CancellationToken>(),
                out _testStdOut));
        }
    }
}
