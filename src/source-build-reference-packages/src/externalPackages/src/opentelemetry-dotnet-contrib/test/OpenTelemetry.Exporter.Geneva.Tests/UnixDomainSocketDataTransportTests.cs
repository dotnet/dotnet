// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

#nullable disable

using System.Net.Sockets;
using System.Reflection;
using OpenTelemetry.Exporter.Geneva.Transports;
using OpenTelemetry.Tests;
using Xunit;

namespace OpenTelemetry.Exporter.Geneva.Tests;

public class UnixDomainSocketDataTransportTests
{
    [SkipUnlessPlatformMatchesFact(TestPlatform.Linux)]
    public void UnixDomainSocketDataTransport_Success_Linux()
    {
        var path = GetRandomFilePath();
        var endpoint = new UnixDomainSocketEndPoint(path);
        try
        {
            using var server = new Socket(AddressFamily.Unix, SocketType.Stream, ProtocolType.IP);
            server.Bind(endpoint);
            server.Listen(1);

            // Client
            using var dataTransport = new UnixDomainSocketDataTransport(path);
            using var serverSocket = server.Accept();
            var data = new byte[] { 12, 34, 56 };
            dataTransport.Send(data, data.Length);
            var receivedData = new byte[5];
            serverSocket.Receive(receivedData);
            Assert.Equal(data[0], receivedData[0]);
            Assert.Equal(data[1], receivedData[1]);
            Assert.Equal(data[2], receivedData[2]);
        }
        catch (Exception)
        {
            throw;
        }
        finally
        {
            try
            {
                File.Delete(path);
            }
            catch
            {
            }
        }
    }

    [SkipUnlessPlatformMatchesFact(TestPlatform.Linux)]
    public void UnixDomainSocketDataTransport_SendTimesOutIfSocketBufferFull_Linux()
    {
        var path = GetRandomFilePath();
        var endpoint = new UnixDomainSocketEndPoint(path);
        using var server = new Socket(AddressFamily.Unix, SocketType.Stream, ProtocolType.IP);
        server.Bind(endpoint);
        server.Listen(1);
        var data = new byte[1024];
        var i = 0;
        using var dataTransport = new UnixDomainSocketDataTransport(path, 5000);  // Set low timeout for faster tests
        var socket = typeof(UnixDomainSocketDataTransport).GetField("socket", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(dataTransport) as Socket;
        try
        {
            // Client
            using var serverSocket = server.Accept();
            while (true)
            {
                Console.WriteLine($"Sending request #{i++}.");
                socket.Send(data, data.Length, SocketFlags.None);
            }

            // The server is not processing sent data (because of heavy load, etc.)
        }
        catch (Exception)
        {
            // At this point, the outgoing buffer for the socket must be full,
            // because the last Send failed.
            // Send again and assert the exception to confirm:
            Assert.Throws<SocketException>(() =>
            {
                Console.WriteLine($"Sending request #{i}.");
                socket.Send(data, data.Length, SocketFlags.None);
            });
        }
        finally
        {
            try
            {
                File.Delete(path);
            }
            catch
            {
            }
        }
    }

    [SkipUnlessPlatformMatchesFact(TestPlatform.Linux)]
    public void UnixDomainSocketDataTransport_ServerRestart_Linux()
    {
        Console.WriteLine("Test starts.");
        var path = GetRandomFilePath();
        var endpoint = new UnixDomainSocketEndPoint(path);
        try
        {
            var server = new Socket(AddressFamily.Unix, SocketType.Stream, ProtocolType.IP);

            // LingerOption lo = new LingerOption(false, 0);
            // server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Linger, lo);
            server.Bind(endpoint);
            server.Listen(1);

            // Client
            using var dataTransport = new UnixDomainSocketDataTransport(path);
            var serverSocket = server.Accept();
            var data = new byte[] { 12, 34, 56 };
            dataTransport.Send(data, data.Length);
            var receivedData = new byte[5];
            serverSocket.Receive(receivedData);
            Assert.Equal(data[0], receivedData[0]);
            Assert.Equal(data[1], receivedData[1]);
            Assert.Equal(data[2], receivedData[2]);

            Console.WriteLine("Successfully sent a message.");

            // Emulate server stops
            serverSocket.Shutdown(SocketShutdown.Both);
            serverSocket.Disconnect(false);
            serverSocket.Dispose();

            if (server.Connected)
            {
                // On MacOS the 'server' socket never shows up as connected
                // so trying to shutdown/disconnect throws an exception.
                server.Shutdown(SocketShutdown.Both);
                server.Disconnect(false);
            }

            server.Dispose();
            try
            {
                File.Delete(path);
            }
            catch
            {
            }

            Console.WriteLine("Destroyed server.");

            Console.WriteLine("Client will fail during Send, and should throw an Exception");
            Assert.ThrowsAny<Exception>(() => dataTransport.Send(data, data.Length));
            Console.WriteLine("Client will fail during Reconnect, and should throw an Exception");
            Assert.ThrowsAny<Exception>(() => dataTransport.Send(data, data.Length));

            using var server2 = new Socket(AddressFamily.Unix, SocketType.Stream, ProtocolType.IP);
            server2.Bind(endpoint);
            server2.Listen(1);
            Console.WriteLine("Started a new server and listening.");

#pragma warning disable IDE0230 // Use UTF-8 string literal
            var data2 = new byte[] { 34, 56, 78 };
#pragma warning restore IDE0230 // Use UTF-8 string literal
            dataTransport.Send(data2, data2.Length);
            Console.WriteLine("The same client sent a new message. Internally it should reconnect if server ever stopped and the socket is not connected anymore.");

            using var serverSocket2 = server2.Accept();
            Console.WriteLine("The new server is ready and accepting connections.");
            var receivedData2 = new byte[5];
            serverSocket2.Receive(receivedData2);
            Console.WriteLine("Server received a messge.");
            Assert.Equal(data2[0], receivedData2[0]);
            Assert.Equal(data2[1], receivedData2[1]);
            Assert.Equal(data2[2], receivedData2[2]);
        }
        catch (Exception)
        {
            throw;
        }
        finally
        {
            try
            {
                File.Delete(path);
            }
            catch
            {
            }
        }
    }

    private static string GetRandomFilePath()
    {
        while (true)
        {
            var path = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            if (!File.Exists(path))
            {
                return path;
            }
        }
    }
}
