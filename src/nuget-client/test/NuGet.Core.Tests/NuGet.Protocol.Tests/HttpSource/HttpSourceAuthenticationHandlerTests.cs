// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NuGet.Configuration;
using NuGet.Test.Utility;
using Xunit;

namespace NuGet.Protocol.Tests
{
    [Collection(nameof(NotThreadSafeResourceCollection))]
    public class HttpSourceAuthenticationHandlerTests
    {
        [Fact]
        public void Constructor_WithSourceCredentials_InitializesClientHandler()
        {
            var packageSource = new PackageSource("http://package.source.test", "source")
            {
                Credentials = new PackageSourceCredential("source", "user", "password", isPasswordClearText: true, validAuthenticationTypesText: null)
            };
            var clientHandler = new HttpClientHandler();
            var credentialService = Mock.Of<ICredentialService>();

            var handler = new HttpSourceAuthenticationHandler(packageSource, clientHandler, credentialService);

            Assert.NotNull(clientHandler.Credentials);

            var actualCredentials = clientHandler.Credentials.GetCredential(packageSource.SourceUri, "Basic");
            Assert.NotNull(actualCredentials);
            Assert.Equal("user", actualCredentials.UserName);
            Assert.Equal("password", actualCredentials.Password);
        }

        [Fact]
        public async Task SendAsync_WithUnauthenticatedSource_PassesThru()
        {
            var packageSource = new PackageSource("http://package.source.test");
            var clientHandler = new HttpClientHandler();

            var credentialService = new Mock<ICredentialService>(MockBehavior.Strict);
            credentialService.SetupGet(x => x.HandlesDefaultCredentials)
                .Returns(false);
            var handler = new HttpSourceAuthenticationHandler(packageSource, clientHandler, credentialService.Object)
            {
                InnerHandler = GetLambdaMessageHandler(HttpStatusCode.OK)
            };

            var response = await SendAsync(handler);

            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task SendAsync_WithAcquiredCredentialsOn401_RetriesRequest()
        {
            var packageSource = new PackageSource("http://package.source.test");
            var clientHandler = new HttpClientHandler();

            var credentialService = Mock.Of<ICredentialService>();
            Mock.Get(credentialService)
                .Setup(
                    x => x.GetCredentialsAsync(
                        packageSource.SourceUri,
                        It.IsAny<IWebProxy>(),
                        CredentialRequestType.Unauthorized,
                        It.IsAny<string>(),
                        It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult<ICredentials>(new NetworkCredential()));

            var handler = new HttpSourceAuthenticationHandler(packageSource, clientHandler, credentialService)
            {
                InnerHandler = GetLambdaMessageHandler(
                    HttpStatusCode.Unauthorized, HttpStatusCode.OK)
            };

            var response = await SendAsync(handler);

            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            Mock.Get(credentialService)
                .Verify(
                    x => x.GetCredentialsAsync(
                        packageSource.SourceUri,
                        It.IsAny<IWebProxy>(),
                        CredentialRequestType.Unauthorized,
                        It.IsAny<string>(),
                        It.IsAny<CancellationToken>()),
                    Times.Once());
        }

        [Fact]
        public async Task SendAsync_WithAcquiredCredentialsOn403_RetriesRequest()
        {
            // Arrange
            var packageSource = new PackageSource("http://package.source.test");
            var clientHandler = new HttpClientHandler();

            var credentialService = Mock.Of<ICredentialService>();
            Mock.Get(credentialService)
                .Setup(
                    x => x.GetCredentialsAsync(
                        packageSource.SourceUri,
                        It.IsAny<IWebProxy>(),
                        CredentialRequestType.Forbidden,
                        It.IsAny<string>(),
                        It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult<ICredentials>(new NetworkCredential()));

            var handler = new HttpSourceAuthenticationHandler(packageSource, clientHandler, credentialService)
            {
                InnerHandler = GetLambdaMessageHandler(
                    HttpStatusCode.Forbidden, HttpStatusCode.OK)
            };

            // Act
            var response = await SendAsync(handler);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            Mock.Get(credentialService)
                .Verify(
                    x => x.GetCredentialsAsync(
                        packageSource.SourceUri,
                        It.IsAny<IWebProxy>(),
                        CredentialRequestType.Forbidden,
                        It.IsAny<string>(),
                        It.IsAny<CancellationToken>()),
                    Times.Once());
        }

        [Fact]
        public async Task SendAsync_With403PromptDisabled_Returns403()
        {
            // Arrange
            var packageSource = new PackageSource("http://package.source.test");
            var clientHandler = new HttpClientHandler();

            var credentialService = new Mock<ICredentialService>(MockBehavior.Strict);
            credentialService.SetupGet(x => x.HandlesDefaultCredentials)
                .Returns(false);
            var handler = new HttpSourceAuthenticationHandler(packageSource, clientHandler, credentialService.Object)
            {
                InnerHandler = GetLambdaMessageHandler(HttpStatusCode.Forbidden)
            };

            var request = new HttpRequestMessage(HttpMethod.Get, "http://foo");
            request.SetConfiguration(new HttpRequestMessageConfiguration(promptOn403: false));

            // Act
            var response = await SendAsync(handler, request: request);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task SendAsync_WhenTaskCanceledExceptionThrownDuringAcquiringCredentials_Throws()
        {
            // Arrange
            var packageSource = new PackageSource("http://package.source.test");
            var clientHandler = new HttpClientHandler();

            var credentialService = Mock.Of<ICredentialService>();
            Mock.Get(credentialService)
                .Setup(
                    x => x.GetCredentialsAsync(
                        packageSource.SourceUri,
                        It.IsAny<IWebProxy>(),
                        CredentialRequestType.Unauthorized,
                        It.IsAny<string>(),
                        It.IsAny<CancellationToken>()))
                .ThrowsAsync(new TaskCanceledException());

            var handler = new HttpSourceAuthenticationHandler(packageSource, clientHandler, credentialService);

            int retryCount = 0;
            var innerHandler = new LambdaMessageHandler(
                _ =>
                {
                    retryCount++;
                    return new HttpResponseMessage(HttpStatusCode.Unauthorized);
                });
            handler.InnerHandler = innerHandler;

            // Act & Assert
            await Assert.ThrowsAsync<TaskCanceledException>(
                () => SendAsync(handler));

            Assert.Equal(1, retryCount);

            Mock.Get(credentialService)
                .Verify(
                    x => x.GetCredentialsAsync(
                        packageSource.SourceUri,
                        It.IsAny<IWebProxy>(),
                        CredentialRequestType.Unauthorized,
                        It.IsAny<string>(),
                        It.IsAny<CancellationToken>()),
                    Times.Once);
        }

        [Fact]
        public async Task SendAsync_WhenOperationCanceledExceptionThrownDuringAcquiringCredentials_Throws()
        {
            // Arrange
            var packageSource = new PackageSource("http://package.source.test");
            var clientHandler = new HttpClientHandler();

            var cts = new CancellationTokenSource();

            var credentialService = Mock.Of<ICredentialService>();
            Mock.Get(credentialService)
                .Setup(
                    x => x.GetCredentialsAsync(
                        packageSource.SourceUri,
                        It.IsAny<IWebProxy>(),
                        CredentialRequestType.Unauthorized,
                        It.IsAny<string>(),
                        It.IsAny<CancellationToken>()))
                .ThrowsAsync(new OperationCanceledException())
                .Callback(() => cts.Cancel());

            var handler = new HttpSourceAuthenticationHandler(packageSource, clientHandler, credentialService);

            int retryCount = 0;
            var innerHandler = new LambdaMessageHandler(
                _ =>
                {
                    retryCount++;
                    return new HttpResponseMessage(HttpStatusCode.Unauthorized);
                });
            handler.InnerHandler = innerHandler;

            // Act & Assert
            await Assert.ThrowsAnyAsync<OperationCanceledException>(
                () => SendAsync(handler));

            Assert.Equal(1, retryCount);

            Mock.Get(credentialService)
                .Verify(
                    x => x.GetCredentialsAsync(
                        packageSource.SourceUri,
                        It.IsAny<IWebProxy>(),
                        CredentialRequestType.Unauthorized,
                        It.IsAny<string>(),
                        It.IsAny<CancellationToken>()),
                    Times.Once);
        }

        [Fact]
        public async Task SendAsync_WithWrongCredentials_StopsRetryingAfter3Times()
        {
            // Arrange
            var packageSource = new PackageSource("http://package.source.test");
            var clientHandler = new HttpClientHandler();

            var credentialService = Mock.Of<ICredentialService>();
            Mock.Get(credentialService)
                .Setup(
                    x => x.GetCredentialsAsync(
                        packageSource.SourceUri,
                        It.IsAny<IWebProxy>(),
                        CredentialRequestType.Unauthorized,
                        It.IsAny<string>(),
                        It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult<ICredentials>(new NetworkCredential()));

            var handler = new HttpSourceAuthenticationHandler(packageSource, clientHandler, credentialService);

            int retryCount = 0;
            var innerHandler = new LambdaMessageHandler(
                _ =>
                {
                    retryCount++;
                    return new HttpResponseMessage(HttpStatusCode.Unauthorized);
                });
            handler.InnerHandler = innerHandler;

            // Act
            var response = await SendAsync(handler);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);

            Assert.Equal(HttpSourceAuthenticationHandler.MaxAuthRetries + 1, retryCount);

            Mock.Get(credentialService)
                .Verify(
                    x => x.GetCredentialsAsync(
                        packageSource.SourceUri,
                        It.IsAny<IWebProxy>(),
                        CredentialRequestType.Unauthorized,
                        It.IsAny<string>(),
                        It.IsAny<CancellationToken>()),
                    Times.Exactly(HttpSourceAuthenticationHandler.MaxAuthRetries));
        }

        [Fact]
        public async Task SendAsync_WithMissingCredentials_Returns401()
        {
            // Arrange
            var packageSource = new PackageSource("http://package.source.test");
            var clientHandler = new HttpClientHandler();

            var credentialService = Mock.Of<ICredentialService>();

            var handler = new HttpSourceAuthenticationHandler(packageSource, clientHandler, credentialService);

            int retryCount = 0;
            var innerHandler = new LambdaMessageHandler(
                _ =>
                {
                    retryCount++;
                    return new HttpResponseMessage(HttpStatusCode.Unauthorized);
                });
            handler.InnerHandler = innerHandler;

            // Act
            var response = await SendAsync(handler);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);

            Assert.Equal(1, retryCount);

            Mock.Get(credentialService)
                .Verify(
                    x => x.GetCredentialsAsync(
                        packageSource.SourceUri,
                        It.IsAny<IWebProxy>(),
                        CredentialRequestType.Unauthorized,
                        It.IsAny<string>(),
                        It.IsAny<CancellationToken>()),
                    Times.Once());
        }

        [Fact]
        public async Task SendAsync_WhenCredentialServiceThrows_Returns401()
        {
            // Arrange
            var packageSource = new PackageSource("http://package.source.test");
            var clientHandler = new HttpClientHandler();

            var credentialService = Mock.Of<ICredentialService>();
            Mock.Get(credentialService)
               .Setup(
                   x => x.GetCredentialsAsync(
                       packageSource.SourceUri,
                       It.IsAny<IWebProxy>(),
                       CredentialRequestType.Unauthorized,
                       It.IsAny<string>(),
                       It.IsAny<CancellationToken>()))
               .Throws(new InvalidOperationException("Credential service failed acquring user credentials"));

            var handler = new HttpSourceAuthenticationHandler(packageSource, clientHandler, credentialService);

            int retryCount = 0;
            var innerHandler = new LambdaMessageHandler(
                _ =>
                {
                    retryCount++;
                    return new HttpResponseMessage(HttpStatusCode.Unauthorized);
                });
            handler.InnerHandler = innerHandler;

            // Act
            var response = await SendAsync(handler);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);

            Assert.Equal(1, retryCount);

            Mock.Get(credentialService)
                .Verify(
                    x => x.GetCredentialsAsync(
                        packageSource.SourceUri,
                        It.IsAny<IWebProxy>(),
                        CredentialRequestType.Unauthorized,
                        It.IsAny<string>(),
                        It.IsAny<CancellationToken>()),
                    Times.Once());
        }

        [Fact]
        public async Task SendAsync_WithProtocolDiagnosticsStopwatches_PausesStopwatches()
        {
            var packageSource = new PackageSource("http://package.source.test");
            Stopwatch stopwatch = Stopwatch.StartNew();
            var clientHandler = new HttpClientHandler();

            var credentialService = Mock.Of<ICredentialService>();
            Mock.Get(credentialService)
                .Setup(
                    x => x.GetCredentialsAsync(
                        packageSource.SourceUri,
                        It.IsAny<IWebProxy>(),
                        CredentialRequestType.Unauthorized,
                        It.IsAny<string>(),
                        It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult<ICredentials>(new NetworkCredential()))
                .Callback(() =>
                {
                    Assert.False(stopwatch.IsRunning, "Stopwatch should be stopped during " + nameof(credentialService.GetCredentialsAsync));
                });

            var handler = new HttpSourceAuthenticationHandler(packageSource, clientHandler, credentialService)
            {
                InnerHandler = GetLambdaMessageHandler(
                    HttpStatusCode.Unauthorized, HttpStatusCode.OK)
            };

            var response = await SendAsync(handler);

            Assert.True(stopwatch.IsRunning, "Stopwatch should be running after SendAsync returns");

            Mock.Get(credentialService)
                .Verify(
                    x => x.GetCredentialsAsync(
                        It.IsAny<Uri>(),
                        It.IsAny<IWebProxy>(),
                        It.IsAny<CredentialRequestType>(),
                        It.IsAny<string>(),
                        It.IsAny<CancellationToken>()),
                    Times.Once());
        }

        [Fact]
        public async Task SendAsync_RetryWithClonedGetRequest()
        {
            var packageSource = new PackageSource("http://package.source.test");
            var clientHandler = new HttpClientHandler();

            var credentialService = Mock.Of<ICredentialService>();
            Mock.Get(credentialService)
                .Setup(
                    x => x.GetCredentialsAsync(
                        packageSource.SourceUri,
                        It.IsAny<IWebProxy>(),
                        CredentialRequestType.Unauthorized,
                        It.IsAny<string>(),
                        It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult<ICredentials>(new NetworkCredential()));

            var requests = 0;
            var handler = new HttpSourceAuthenticationHandler(packageSource, clientHandler, credentialService)
            {
                InnerHandler = new LambdaMessageHandler(
                    request =>
                    {
                        Assert.Null(request.Headers.Authorization);
                        request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "TEST");
                        requests++;
                        return new HttpResponseMessage(HttpStatusCode.Unauthorized);
                    })
            };

            var response = await SendAsync(handler);

            Assert.True(requests > 1, "No retries");
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        public static IEnumerable<object[]> GetHttpContent()
        {
            yield return new object[] { new StringContent("abc") };
            yield return new object[] { new ByteArrayContent(Encoding.UTF8.GetBytes("abc")) };
            yield return new object[] { new StreamContent(new MemoryStream()) };
            yield return new object[] { new FormUrlEncodedContent(new[] { new KeyValuePair<string, string>("test", "abc") }) };

            var multipartFormDataContent = new MultipartFormDataContent();
            multipartFormDataContent.Add(new StringContent("abc"), "test");
            multipartFormDataContent.Add(new StreamContent(new MemoryStream()), "test", "file");
            yield return new object[] { multipartFormDataContent };
        }

        //Skipped Linux: https://github.com/NuGet/Home/issues/9685
        [PlatformTheory(Platform.Windows, Platform.Darwin)]
        [MemberData(nameof(GetHttpContent))]
        public async Task SendAsync_RetryWithClonedPostRequest(HttpContent httpContent)
        {
            var packageSource = new PackageSource("http://package.source.test");
            var clientHandler = new HttpClientHandler();

            var credentialService = Mock.Of<ICredentialService>();
            Mock.Get(credentialService)
                .Setup(
                    x => x.GetCredentialsAsync(
                        packageSource.SourceUri,
                        It.IsAny<IWebProxy>(),
                        CredentialRequestType.Unauthorized,
                        It.IsAny<string>(),
                        It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult<ICredentials>(new NetworkCredential()));

            var requests = 0;
            var handler = new HttpSourceAuthenticationHandler(packageSource, clientHandler, credentialService)
            {
                InnerHandler = new LambdaMessageHandler(
                    async request =>
                    {
                        Assert.Null(request.Headers.Authorization);
                        request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "TEST");
                        await request.Content.ReadAsStringAsync();
                        requests++;
                        return new HttpResponseMessage(HttpStatusCode.Unauthorized);
                    })
            };

            var response = await SendAsync(handler, new HttpRequestMessage(HttpMethod.Post, "http://package.source.test") { Content = httpContent });

            Assert.True(requests > 1, "No retries");
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Dispose_CalledMultipleTimes_DisposesInstanceAsync()
        {
            // Arrange
            var packageSource = new PackageSource("http://package.source.test");
            var clientHandler = new HttpClientHandler();
            var credentialService = Mock.Of<ICredentialService>();
            Mock.Get(credentialService)
                .Setup(x => x.HandlesDefaultCredentials)
                .Returns(true);
            Mock.Get(credentialService)
                .Setup(
                    x => x.GetCredentialsAsync(
                        It.IsAny<Uri>(),
                        It.IsAny<IWebProxy>(),
                        It.IsAny<CredentialRequestType>(),
                        It.IsAny<string>(),
                        It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult<ICredentials>(new NetworkCredential()));
            var handler = new HttpSourceAuthenticationHandler(packageSource, clientHandler, credentialService)
            {
                InnerHandler = GetLambdaMessageHandler(HttpStatusCode.OK) // avoid a network call
            };

            // Simulate some work
            await SendAsync(handler);

            // Act and Assert: Nothing should throw
            handler.Dispose();
            handler.Dispose();
            handler.Dispose();
            handler.Dispose();
        }

        [Fact]
        public async Task Dispose_CalledInMultipleObjectsConcurrently_NoLocksAsync()
        {
            // Arrange
            var packageSource = new PackageSource("http://package.source.test");
            var clientHandler = new HttpClientHandler();
            var credentialService = Mock.Of<ICredentialService>();
            Mock.Get(credentialService)
                .Setup(x => x.HandlesDefaultCredentials)
                .Returns(true);
            Mock.Get(credentialService)
                .Setup(x => x.GetCredentialsAsync(
                        It.IsAny<Uri>(),
                        It.IsAny<IWebProxy>(),
                        It.IsAny<CredentialRequestType>(),
                        It.IsAny<string>(),
                        It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult<ICredentials>(new NetworkCredential()));

            var handler1 = new HttpSourceAuthenticationHandler(packageSource, clientHandler, credentialService)
            {
                InnerHandler = GetLambdaMessageHandler(HttpStatusCode.OK) // avoid a network call
            };
            var handler2 = new HttpSourceAuthenticationHandler(packageSource, clientHandler, credentialService)
            {
                InnerHandler = GetLambdaMessageHandler(HttpStatusCode.OK) // avoid a network call
            };
            var handler3 = new HttpSourceAuthenticationHandler(packageSource, clientHandler, credentialService)
            {
                InnerHandler = GetLambdaMessageHandler(HttpStatusCode.OK) // avoid a network call
            };

            var workTasks = new Task[]
            {
                new Task(async () => { try { await SendAsync(handler1); } catch { throw; } }),
                new Task(async () => { try { await SendAsync(handler2); } catch { throw; } }),
                new Task(async () => { try { await SendAsync(handler3); } catch { throw; } }),
            };

            var disposeTasks = new Task[]
            {
                new Task(() => { try { handler1.Dispose(); } catch { throw; } }),
                new Task(() => { try { handler2.Dispose(); } catch { throw; } }),
                new Task(() => { try { handler3.Dispose(); } catch { throw; } }),
                new Task(() => { try { handler1.Dispose(); } catch { throw; } }),
                new Task(() => { try { handler2.Dispose(); } catch { throw; } }),
                new Task(() => { try { handler3.Dispose(); } catch { throw; } }),
                new Task(() => { try { handler1.Dispose(); } catch { throw; } }),
                new Task(() => { try { handler2.Dispose(); } catch { throw; } }),
                new Task(() => { try { handler3.Dispose(); } catch { throw; } }),
            };

            StartTasks(workTasks);
            await Task.WhenAll(workTasks);

            // Act
            StartTasks(disposeTasks);
            await Task.WhenAll(disposeTasks);

            // Assert: Nothing should throw
            Assert.All(workTasks, tsk => AssertTask(tsk));
            Assert.All(disposeTasks, tsk => AssertTask(tsk));
        }

        private static void AssertTask(Task task)
        {
            Assert.Equal(TaskStatus.RanToCompletion, task.Status);
            Assert.Null(task.Exception);
        }

        private static void StartTasks(IEnumerable<Task> tasks)
        {
            // Act and Assert, nothing should throw
            Parallel.ForEach(tasks, t =>
            {
                if (t.Status == TaskStatus.Created)
                {
                    t.Start();
                }
            });
        }

        [Fact]
        public async Task SendAsync_AfterDispose_ThrowsAsync()
        {
            // Arrange
            var packageSource = new PackageSource("http://package.source.test");
            var clientHandler = new HttpClientHandler();

            var handler = new HttpSourceAuthenticationHandler(packageSource, clientHandler, credentialService: null);

            // Act
            handler.Dispose();

            // Assert
            await Assert.ThrowsAsync<ObjectDisposedException>(async () => await SendAsync(handler));
        }

        private static LambdaMessageHandler GetLambdaMessageHandler(HttpStatusCode statusCode)
        {
            return new LambdaMessageHandler(
                _ => new HttpResponseMessage(statusCode));
        }

        private static LambdaMessageHandler GetLambdaMessageHandler(params HttpStatusCode[] statusCodes)
        {
            var responses = new Queue<HttpStatusCode>(statusCodes);
            return new LambdaMessageHandler(
                _ => new HttpResponseMessage(responses.Dequeue()));
        }

        private static async Task<HttpResponseMessage> SendAsync(
            HttpMessageHandler handler,
            HttpRequestMessage request = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            using (var client = new HttpClient(handler))
            {
                return await client.SendAsync(request ?? new HttpRequestMessage(HttpMethod.Get, "http://foo"), cancellationToken);
            }
        }
    }
}
