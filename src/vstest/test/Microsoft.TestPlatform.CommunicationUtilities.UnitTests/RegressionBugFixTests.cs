// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace Microsoft.TestPlatform.CommunicationUtilities.UnitTests;

/// <summary>
/// Regression test for GH-4461:
/// After a non-timeout IOException with a SocketException inner exception,
/// the error output parameter must remain null (not the exception).
/// Previously, the code set <c>error = ioException</c>, which propagated
/// confusing socket errors to developers when testhost crashed.
/// </summary>
[TestClass]
public class RegressionBugFixTests
{
    public TestContext TestContext { get; set; } = null!;

    private static (TcpClient client, TcpClient serverSide, TcpListener listener) CreateConnectedTcpPair()
    {
        var listener = new TcpListener(IPAddress.Loopback, 0);
        listener.Start();
        var port = ((IPEndPoint)listener.LocalEndpoint).Port;

        var client = new TcpClient();
        client.Connect(IPAddress.Loopback, port);
        var serverSide = listener.AcceptTcpClient();

        return (client, serverSide, listener);
    }

    [TestMethod]
    public async Task MessageLoopAsync_NonTimeoutSocketIOException_ErrorMustBeNull()
    {
        // GH-4461: When a non-timeout IOException wrapping a SocketException occurs,
        // the error handler must receive null (not the exception).
        // If the fix were reverted (error = ioException uncommented), capturedError would be non-null.
        var (client, serverSide, listener) = CreateConnectedTcpPair();
        try
        {
            var channel = new Mock<ICommunicationChannel>();
            var socketException = new SocketException((int)SocketError.ConnectionReset);
            var ioException = new IOException("Connection forcibly closed", socketException);

            channel.Setup(c => c.NotifyDataAvailable(It.IsAny<CancellationToken>()))
                .Throws(ioException);

            Exception? capturedError = null;

            // Write data so Poll returns true and NotifyDataAvailable is invoked.
            var serverStream = serverSide.GetStream();
            await serverStream.WriteAsync(new byte[] { 0x1 }, 0, 1, TestContext.CancellationToken);
            await serverStream.FlushAsync(TestContext.CancellationToken);

            using var cts = CancellationTokenSource.CreateLinkedTokenSource(TestContext.CancellationToken);
            cts.CancelAfter(TimeSpan.FromSeconds(5));

            await client.MessageLoopAsync(channel.Object, error => capturedError = error, cts.Token);

            // The fix commented out `error = ioException`, so capturedError must be null.
            Assert.IsNull(capturedError,
                "GH-4461: Non-timeout IOException with SocketException must NOT be propagated to the error handler.");
        }
        finally
        {
            client.Dispose();
            serverSide.Dispose();
            listener.Stop();
        }
    }

    [TestMethod]
    public async Task MessageLoopAsync_NonIOException_ErrorMustBeSet()
    {
        // Contrast test: non-IOException exceptions must still be propagated.
        var (client, serverSide, listener) = CreateConnectedTcpPair();
        try
        {
            var channel = new Mock<ICommunicationChannel>();
            var expectedException = new InvalidOperationException("Non-IO failure");
            channel.Setup(c => c.NotifyDataAvailable(It.IsAny<CancellationToken>()))
                .Throws(expectedException);

            Exception? capturedError = null;

            var serverStream = serverSide.GetStream();
            await serverStream.WriteAsync(new byte[] { 1 }, 0, 1, TestContext.CancellationToken);
            await serverStream.FlushAsync(TestContext.CancellationToken);

            using var cts = CancellationTokenSource.CreateLinkedTokenSource(TestContext.CancellationToken);
            cts.CancelAfter(TimeSpan.FromSeconds(5));

            await client.MessageLoopAsync(channel.Object, error => capturedError = error, cts.Token);

            Assert.AreSame(expectedException, capturedError,
                "Non-IOException must still be propagated to the error handler.");
        }
        finally
        {
            client.Dispose();
            serverSide.Dispose();
            listener.Stop();
        }
    }
}
