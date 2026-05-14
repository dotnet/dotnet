// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NuGet.Commands;
using NuGet.Common;
using Xunit;

namespace NuGet.Protocol.Plugins.Tests
{
    public class LogRequestHandlerTests
    {
        [Fact]
        public void Constructor_ThrowsForNullLogger()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new LogRequestHandler(logger: null));

            Assert.Equal("logger", exception.ParamName);
        }

        [Fact]
        public void CancellationToken_IsNone()
        {
            var test = new LogRequestHandlerTest();

            Assert.Equal(CancellationToken.None, test.Handler.CancellationToken);
        }

        [Fact]
        public async Task HandleResponseAsync_ThrowsForNullConnection()
        {
            var test = new LogRequestHandlerTest();

            var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                () => test.Handler.HandleResponseAsync(
                    connection: null,
                    request: test.Request,
                    responseHandler: Mock.Of<IResponseHandler>(),
                    cancellationToken: CancellationToken.None));

            Assert.Equal("connection", exception.ParamName);
        }

        [Fact]
        public async Task HandleResponseAsync_ThrowsForNullRequest()
        {
            var test = new LogRequestHandlerTest();

            var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                () => test.Handler.HandleResponseAsync(
                    Mock.Of<IConnection>(),
                    request: null,
                    responseHandler: Mock.Of<IResponseHandler>(),
                    cancellationToken: CancellationToken.None));

            Assert.Equal("request", exception.ParamName);
        }

        [Fact]
        public async Task HandleResponseAsync_ThrowsForNullResponseHandler()
        {
            var test = new LogRequestHandlerTest();

            var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                () => test.Handler.HandleResponseAsync(
                    Mock.Of<IConnection>(),
                    test.Request,
                    responseHandler: null,
                    cancellationToken: CancellationToken.None));

            Assert.Equal("responseHandler", exception.ParamName);
        }

        [Fact]
        public async Task HandleResponseAsync_ThrowsIfCancelled()
        {
            var test = new LogRequestHandlerTest();

            await Assert.ThrowsAsync<OperationCanceledException>(
                () => test.Handler.HandleResponseAsync(
                    Mock.Of<IConnection>(),
                    test.Request,
                    Mock.Of<IResponseHandler>(),
                    new CancellationToken(canceled: true)));
        }

        [Fact]
        public async Task HandleResponseAsync_LogsDebug()
        {
            await HandleResponseAsync(
                LogLevel.Debug,
                LogLevel.Debug,
                MessageResponseCode.Success,
                expectLog: true);
        }

        [Fact]
        public async Task HandleResponseAsync_LogsVerbose()
        {
            await HandleResponseAsync(
                LogLevel.Verbose,
                LogLevel.Verbose,
                MessageResponseCode.Success,
                expectLog: true);
        }

        [Fact]
        public async Task HandleResponseAsync_LogsInformation()
        {
            await HandleResponseAsync(
                LogLevel.Information,
                LogLevel.Information,
                MessageResponseCode.Success,
                expectLog: true);
        }

        [Fact]
        public async Task HandleResponseAsync_LogsMinimal()
        {
            await HandleResponseAsync(
                LogLevel.Minimal,
                LogLevel.Minimal,
                MessageResponseCode.Success,
                expectLog: true);
        }

        [Fact]
        public async Task HandleResponseAsync_LogsWarning()
        {
            await HandleResponseAsync(
                LogLevel.Warning,
                LogLevel.Warning,
                MessageResponseCode.Success,
                expectLog: true);
        }

        [Fact]
        public async Task HandleResponseAsync_LogsError()
        {
            await HandleResponseAsync(
                LogLevel.Error,
                LogLevel.Error,
                MessageResponseCode.Success,
                expectLog: true);
        }

        [Theory]
        [InlineData(LogLevel.Verbose)]
        [InlineData(LogLevel.Information)]
        [InlineData(LogLevel.Minimal)]
        [InlineData(LogLevel.Warning)]
        [InlineData(LogLevel.Error)]
        public async Task HandleResponseAsync_FailsOnRequestWithMoreDetailedLogLevel(LogLevel handlerLogLevel)
        {
            var requestedLogLevel = handlerLogLevel - 1;

            await HandleResponseAsync(
                handlerLogLevel,
                requestedLogLevel,
                MessageResponseCode.Error,
                expectLog: false);
        }

        [Fact]
        public void SetLogger_ThrowsForNullLogger()
        {
            var test = new LogRequestHandlerTest();
            var exception = Assert.Throws<ArgumentNullException>(
                () => test.Handler.SetLogger(logger: null));

            Assert.Equal("logger", exception.ParamName);
        }

        [Fact]
        public async Task SetLogger_UpdatesLogger()
        {
            var test = new LogRequestHandlerTest();
            var secondLogger = new Mock<ILogger>(MockBehavior.Strict);

            test.Handler.SetLogger(secondLogger.Object);

            var responseHandler = new Mock<IResponseHandler>(MockBehavior.Strict);

            responseHandler.Setup(x => x.SendResponseAsync(
                    It.IsNotNull<Message>(),
                    It.IsNotNull<LogResponse>(),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            await test.Handler.HandleResponseAsync(
                Mock.Of<IConnection>(),
                test.Request,
                responseHandler.Object,
                CancellationToken.None);

            test.Logger.Verify();
            responseHandler.Verify();
            secondLogger.Verify();
        }

        [Fact]
        public void GetLogLevel_ThrowsForNullLogger()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => LogRequestHandler.GetLogLevel(logger: null));

            Assert.Equal("logger", exception.ParamName);
        }

        [Theory]
        [InlineData(LogLevel.Debug)]
        [InlineData(LogLevel.Warning)]
        public void GetLogLevel_ReturnsLoggerLogLevel(LogLevel expectedLogLevel)
        {
            var logger = new RestoreCollectorLogger(Mock.Of<ILogger>(), expectedLogLevel);
            var actualLogLevel = LogRequestHandler.GetLogLevel(logger);

            Assert.Equal(expectedLogLevel, actualLogLevel);
        }

        [Fact]
        public void GetLogLevel_ReturnsInformationIfNotLoggerBase()
        {
            var logLevel = LogRequestHandler.GetLogLevel(Mock.Of<ILogger>());

            Assert.Equal(LogLevel.Information, logLevel);
        }

        private async Task HandleResponseAsync(
            LogLevel handlerLogLevel,
            LogLevel requestLogLevel,
            MessageResponseCode responseCode,
            bool expectLog)
        {
            var test = new LogRequestHandlerTest(handlerLogLevel);
            var payload = new LogRequest(requestLogLevel, message: "a");
            var request = MessageUtilities.Create(
                requestId: "b",
                type: MessageType.Request,
                method: MessageMethod.Log,
                payload: payload);

            var responseHandler = new Mock<IResponseHandler>(MockBehavior.Strict);

            responseHandler.Setup(x => x.SendResponseAsync(
                    It.Is<Message>(message => message == request),
                    It.Is<LogResponse>(response => response.ResponseCode == responseCode),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            if (expectLog)
            {
                test.Logger.Setup(
                    x => x.Log(It.Is<ILogMessage>(m => m.Message == payload.Message && m.Level == requestLogLevel)));
            }

            await test.Handler.HandleResponseAsync(
                Mock.Of<IConnection>(),
                request,
                responseHandler.Object,
                CancellationToken.None);

            test.Logger.Verify();
        }

        private sealed class LogRequestHandlerTest
        {
            internal LogRequestHandler Handler { get; }
            internal Mock<ILogger> Logger { get; }
            internal Message Request { get; }

            internal LogRequestHandlerTest(LogLevel logLevel = LogLevel.Debug)
            {
                Logger = new Mock<ILogger>(MockBehavior.Strict);
                Handler = new LogRequestHandler(new RestoreCollectorLogger(Logger.Object, logLevel));
                Request = MessageUtilities.Create(
                    requestId: "a",
                    type: MessageType.Request,
                    method: MessageMethod.Log,
                    payload: new LogRequest(LogLevel.Debug, "b"));
            }
        }
    }
}
