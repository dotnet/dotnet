// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IO;
using System.Net.Sockets;
using System.Threading;

using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Interfaces;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.CommunicationUtilities.PlatformTests;

[TestClass]
public abstract class SocketTestsBase
{
    protected const string Dummydata = "Dummy Data";
    protected const int Timeout = 10 * 1000;

    protected abstract TcpClient? Client { get; }

    [TestMethod]
    public void SocketEndpointStartShouldRaiseServerConnectedEventOnServerConnection()
    {
        SetupChannel(out ConnectedEventArgs? connectedEventArgs);

        Assert.IsNotNull(connectedEventArgs);
    }

    [TestMethod]
    public void SocketEndpointShouldNotifyChannelOnDataAvailable()
    {
        var message = string.Empty;
        ManualResetEvent waitForMessage = new(false);
        SetupChannel(out ConnectedEventArgs? _)!.MessageReceived += (s, e) =>
        {
            message = e.Data;
            waitForMessage.Set();
        };

        WriteData(Client!);

        waitForMessage.WaitOne();
        Assert.AreEqual(Dummydata, message);
    }

    protected static string ReadData(TcpClient client)
    {
        using BinaryReader reader = new(client.GetStream());
        return reader.ReadString();
    }

    protected static void WriteData(TcpClient client)
    {
        using BinaryWriter writer = new(client.GetStream());
        writer.Write(Dummydata);
    }

    protected abstract ICommunicationChannel? SetupChannel(out ConnectedEventArgs? connectedEventArgs);
}
