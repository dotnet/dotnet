// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

#if !NET

using System.Text;
using OpenTelemetry.Internal;

namespace System.Net.Sockets;

internal sealed class UnixDomainSocketEndPoint : EndPoint
{
    // sockaddr_un.sun_path at http://pubs.opengroup.org/onlinepubs/9699919799/basedefs/sys_un.h.html, -1 for terminator
    private const int MaximumNativePathLength = 92 - 1;

    // The first 2 bytes of the underlying buffer are reserved for the AddressFamily enumerated value.
    // https://docs.microsoft.com/dotnet/api/system.net.socketaddress
    private const int NativePathOffset = 2;
    private readonly string path;
    private readonly byte[] nativePath;

    public UnixDomainSocketEndPoint(string path)
    {
        Guard.ThrowIfNull(path);

        this.path = path;
        this.nativePath = Encoding.UTF8.GetBytes(path);
        if (this.nativePath.Length is 0 or > MaximumNativePathLength)
        {
            throw new ArgumentOutOfRangeException(nameof(path), "Path is of an invalid length for use with domain sockets.");
        }
    }

    private UnixDomainSocketEndPoint(SocketAddress socketAddress)
    {
        Guard.ThrowIfNull(socketAddress);

        if (socketAddress.Family != this.AddressFamily ||
            socketAddress.Size > NativePathOffset + MaximumNativePathLength)
        {
            throw new ArgumentOutOfRangeException(
                nameof(socketAddress),
                "The path of SocketAddress is of an invalid length for use with domain sockets.");
        }

        if (socketAddress.Size > NativePathOffset)
        {
            this.nativePath = new byte[socketAddress.Size - NativePathOffset];
            for (var i = 0; i < this.nativePath.Length; ++i)
            {
                this.nativePath[i] = socketAddress[NativePathOffset + i];
            }

            this.path = Encoding.UTF8.GetString(this.nativePath);
        }
        else
        {
            this.path = string.Empty;
            this.nativePath = [];
        }
    }

    public override AddressFamily AddressFamily => AddressFamily.Unix;

    public override EndPoint Create(SocketAddress socketAddress) => new UnixDomainSocketEndPoint(socketAddress);

    public override SocketAddress Serialize()
    {
        var socketAddress = new SocketAddress(AddressFamily.Unix, NativePathOffset + this.nativePath.Length + 1);
        for (var i = 0; i < this.nativePath.Length; ++i)
        {
            socketAddress[NativePathOffset + i] = this.nativePath[i];
        }

        socketAddress[NativePathOffset + this.nativePath.Length] = 0;  // SocketAddress should be NULL terminated
        return socketAddress;
    }

    public override string ToString() => this.path;
}

#endif
