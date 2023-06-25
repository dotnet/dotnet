// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Xunit;

namespace NuGet.Protocol.Plugins.Tests
{
    public class MonitorNuGetProcessExitRequestHandlerTests : IDisposable
    {
        private readonly MonitorNuGetProcessExitRequestHandler _handler;
        private readonly Mock<IPlugin> _plugin;

        public MonitorNuGetProcessExitRequestHandlerTests()
        {
            _plugin = new Mock<IPlugin>(MockBehavior.Strict);
            _handler = new MonitorNuGetProcessExitRequestHandler(_plugin.Object);
        }

        public void Dispose()
        {
            _handler.Dispose();
            GC.SuppressFinalize(this);
        }

        [Fact]
        public void Constructor_ThrowsForNullPlugin()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new MonitorNuGetProcessExitRequestHandler(plugin: null));

            Assert.Equal("plugin", exception.ParamName);
        }

        [Fact]
        public void CancellationToken_IsNone()
        {
            var handler = new MonitorNuGetProcessExitRequestHandler(Mock.Of<IPlugin>());

            Assert.Equal(CancellationToken.None, handler.CancellationToken);
        }

        [Fact]
        public async Task HandleResponseAsync_ThrowsForNullConnection()
        {
            var request = CreateRequest(MessageType.Request);

            var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                () => _handler.HandleResponseAsync(
                    connection: null,
                    request: request,
                    responseHandler: Mock.Of<IResponseHandler>(),
                    cancellationToken: CancellationToken.None));

            Assert.Equal("connection", exception.ParamName);
        }

        [Fact]
        public async Task HandleResponseAsync_ThrowsForNullRequest()
        {
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                () => _handler.HandleResponseAsync(
                    Mock.Of<IConnection>(),
                    request: null,
                    responseHandler: Mock.Of<IResponseHandler>(),
                    cancellationToken: CancellationToken.None));

            Assert.Equal("request", exception.ParamName);
        }

        [Fact]
        public async Task HandleResponseAsync_ThrowsForNullResponseHandler()
        {
            var request = CreateRequest(MessageType.Request);

            var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                () => _handler.HandleResponseAsync(
                    Mock.Of<IConnection>(),
                    request,
                    responseHandler: null,
                    cancellationToken: CancellationToken.None));

            Assert.Equal("responseHandler", exception.ParamName);
        }

        [Fact]
        public async Task HandleResponseAsync_ThrowsIfCancelled()
        {
            var request = CreateRequest(MessageType.Request);

            await Assert.ThrowsAsync<OperationCanceledException>(
                () => _handler.HandleResponseAsync(
                    Mock.Of<IConnection>(),
                    request,
                    Mock.Of<IResponseHandler>(),
                    new CancellationToken(canceled: true)));
        }

        [Fact]
        public async Task HandleResponseAsync_RespondsWithNotFoundIfProcessNotFound()
        {
            var invalidProcessId = GetInvalidProcessId();
            var request = CreateRequest(MessageType.Request, invalidProcessId);
            var responseHandler = new Mock<IResponseHandler>(MockBehavior.Strict);

            responseHandler.Setup(x => x.SendResponseAsync(
                    It.Is<Message>(r => r == request),
                    It.Is<MonitorNuGetProcessExitResponse>(r => r.ResponseCode == MessageResponseCode.NotFound),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            await _handler.HandleResponseAsync(
                Mock.Of<IConnection>(),
                request,
                responseHandler.Object,
                CancellationToken.None);
        }

        [Fact]
        public async Task HandleResponseAsync_RespondsWithSuccessIfProcessFound()
        {
            var request = CreateRequest(MessageType.Request, processId: GetCurrentProcessId());
            var responseHandler = new Mock<IResponseHandler>(MockBehavior.Strict);

            responseHandler.Setup(x => x.SendResponseAsync(
                    It.Is<Message>(r => r == request),
                    It.Is<MonitorNuGetProcessExitResponse>(r => r.ResponseCode == MessageResponseCode.Success),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            await _handler.HandleResponseAsync(
                Mock.Of<IConnection>(),
                request,
                responseHandler.Object,
                CancellationToken.None);
        }

        private static Message CreateRequest(MessageType type, int? processId = null)
        {
            if (processId.HasValue)
            {
                var payload = new MonitorNuGetProcessExitRequest(processId.Value);

                return MessageUtilities.Create(
                    requestId: "a",
                    type: MessageType.Request,
                    method: MessageMethod.MonitorNuGetProcessExit,
                    payload: payload);
            }

            return new Message(
                requestId: "a",
                type: type,
                method: MessageMethod.MonitorNuGetProcessExit,
                payload: null);
        }

        private static int GetCurrentProcessId()
        {
            using (var process = Process.GetCurrentProcess())
            {
                return process.Id;
            }
        }

        private static int GetInvalidProcessId()
        {
            for (var processId = -1; processId > int.MinValue; --processId)
            {
                try
                {
                    using (Process.GetProcessById(processId))
                    {
                    }
                }
                catch (Exception)
                {
                    return processId;
                }
            }

            throw new Exception("Unable to find an invalid process ID.");
        }
    }
}
