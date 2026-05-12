// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Sockets;

namespace OpenTelemetry.Exporter.Geneva.Transports;

internal sealed class UnixDomainSocketDataTransport : IDataTransport, IDisposable
{
    public const int DefaultTimeoutMilliseconds = 15000;
    private readonly EndPoint unixEndpoint;
    private readonly int timeoutMilliseconds;
    private Socket socket;

    /// <summary>
    /// Initializes a new instance of the <see cref="UnixDomainSocketDataTransport"/> class.
    /// The class for transporting data over Unix domain socket.
    /// </summary>
    /// <param name="unixDomainSocketPath">The path to connect a unix domain socket over.</param>
    /// <param name="timeoutMilliseconds">
    /// The time-out value, in milliseconds.
    /// If you set the property with a value between 1 and 499, the value will be changed to 500.
    /// The default value is 15,000 milliseconds.
    /// </param>
    public UnixDomainSocketDataTransport(
        string unixDomainSocketPath,
        int timeoutMilliseconds = DefaultTimeoutMilliseconds)
    {
        this.unixEndpoint = new UnixDomainSocketEndPoint(unixDomainSocketPath);
        this.timeoutMilliseconds = timeoutMilliseconds;

        this.Connect();
    }

    public bool IsEnabled()
    {
        return true;
    }

    public void Send(byte[] data, int size)
    {
        if (!this.socket.Connected
            && !this.Reconnect())
        {
            throw new InvalidOperationException("UDS not connected.");
        }

        this.socket.Send(data, size, SocketFlags.None);
    }

    public void Dispose()
    {
        this.socket.Dispose();
    }

    [MemberNotNull(nameof(socket))]
    private bool Connect()
    {
        this.socket = new Socket(AddressFamily.Unix, SocketType.Stream, ProtocolType.IP)
        {
            SendTimeout = this.timeoutMilliseconds,
        };

        try
        {
            this.socket.Connect(this.unixEndpoint);

            return true;
        }
        catch (Exception ex)
        {
            ExporterEventSource.Log.TransportException(nameof(UnixDomainSocketDataTransport), "Attempt to connect to socket failed. Connection will be retried periodically until established.", ex);

            return false;
        }
    }

    private bool Reconnect()
    {
        this.socket.Close();
        return this.Connect();
    }
}
