// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;

[assembly: Debuggable(DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: AllowPartiallyTrustedCallers]
[assembly: ReferenceAssembly]
[assembly: AssemblyTitle("System.Net.Sockets")]
[assembly: AssemblyDescription("System.Net.Sockets")]
[assembly: AssemblyDefaultAlias("System.Net.Sockets")]
[assembly: AssemblyCompany("Microsoft Corporation")]
[assembly: AssemblyProduct("Microsoft® .NET Framework")]
[assembly: AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: AssemblyFileVersion("4.6.24705.01")]
[assembly: AssemblyInformationalVersion("4.6.24705.01 built by: SOURCEBUILD")]
[assembly: CLSCompliant(true)]
[assembly: AssemblyMetadata("", "")]
[assembly: AssemblyVersion("4.1.1.0")]

[assembly: TypeForwardedTo(typeof(System.Net.Sockets.IOControlCode))]
[assembly: TypeForwardedTo(typeof(System.Net.Sockets.IPPacketInformation))]
[assembly: TypeForwardedTo(typeof(System.Net.Sockets.IPProtectionLevel))]
[assembly: TypeForwardedTo(typeof(System.Net.Sockets.IPv6MulticastOption))]
[assembly: TypeForwardedTo(typeof(System.Net.Sockets.LingerOption))]
[assembly: TypeForwardedTo(typeof(System.Net.Sockets.MulticastOption))]
[assembly: TypeForwardedTo(typeof(System.Net.Sockets.NetworkStream))]
[assembly: TypeForwardedTo(typeof(System.Net.Sockets.ProtocolType))]
[assembly: TypeForwardedTo(typeof(System.Net.Sockets.SelectMode))]
[assembly: TypeForwardedTo(typeof(System.Net.Sockets.SendPacketsElement))]
[assembly: TypeForwardedTo(typeof(System.Net.Sockets.Socket))]
[assembly: TypeForwardedTo(typeof(System.Net.Sockets.SocketAsyncEventArgs))]
[assembly: TypeForwardedTo(typeof(System.Net.Sockets.SocketAsyncOperation))]
[assembly: TypeForwardedTo(typeof(System.Net.Sockets.SocketFlags))]
[assembly: TypeForwardedTo(typeof(System.Net.Sockets.SocketOptionLevel))]
[assembly: TypeForwardedTo(typeof(System.Net.Sockets.SocketOptionName))]
[assembly: TypeForwardedTo(typeof(System.Net.Sockets.SocketShutdown))]
[assembly: TypeForwardedTo(typeof(System.Net.Sockets.SocketType))]
[assembly: TypeForwardedTo(typeof(System.Net.Sockets.TcpClient))]
[assembly: TypeForwardedTo(typeof(System.Net.Sockets.TcpListener))]
[assembly: TypeForwardedTo(typeof(System.Net.Sockets.UdpClient))]
[assembly: TypeForwardedTo(typeof(System.Net.Sockets.UdpReceiveResult))]



namespace System.Net.Sockets
{
    public partial struct SocketReceiveFromResult
    {
        public int ReceivedBytes;
        public System.Net.EndPoint RemoteEndPoint;
    }
    public partial struct SocketReceiveMessageFromResult
    {
        public System.Net.Sockets.IPPacketInformation PacketInformation;
        public int ReceivedBytes;
        public System.Net.EndPoint RemoteEndPoint;
        public System.Net.Sockets.SocketFlags SocketFlags;
    }
    public static partial class SocketTaskExtensions
    {
        public static System.Threading.Tasks.Task<System.Net.Sockets.Socket> AcceptAsync(this System.Net.Sockets.Socket socket) { throw null; }
        public static System.Threading.Tasks.Task<System.Net.Sockets.Socket> AcceptAsync(this System.Net.Sockets.Socket socket, System.Net.Sockets.Socket acceptSocket) { throw null; }
        public static System.Threading.Tasks.Task ConnectAsync(this System.Net.Sockets.Socket socket, System.Net.EndPoint remoteEndPoint) { throw null; }
        public static System.Threading.Tasks.Task ConnectAsync(this System.Net.Sockets.Socket socket, System.Net.IPAddress address, int port) { throw null; }
        public static System.Threading.Tasks.Task ConnectAsync(this System.Net.Sockets.Socket socket, System.Net.IPAddress[] addresses, int port) { throw null; }
        public static System.Threading.Tasks.Task ConnectAsync(this System.Net.Sockets.Socket socket, string host, int port) { throw null; }
        public static System.Threading.Tasks.Task<int> ReceiveAsync(this System.Net.Sockets.Socket socket, System.ArraySegment<byte> buffer, System.Net.Sockets.SocketFlags socketFlags) { throw null; }
        public static System.Threading.Tasks.Task<int> ReceiveAsync(this System.Net.Sockets.Socket socket, System.Collections.Generic.IList<System.ArraySegment<byte>> buffers, System.Net.Sockets.SocketFlags socketFlags) { throw null; }
        public static System.Threading.Tasks.Task<System.Net.Sockets.SocketReceiveFromResult> ReceiveFromAsync(this System.Net.Sockets.Socket socket, System.ArraySegment<byte> buffer, System.Net.Sockets.SocketFlags socketFlags, System.Net.EndPoint remoteEndPoint) { throw null; }
        public static System.Threading.Tasks.Task<System.Net.Sockets.SocketReceiveMessageFromResult> ReceiveMessageFromAsync(this System.Net.Sockets.Socket socket, System.ArraySegment<byte> buffer, System.Net.Sockets.SocketFlags socketFlags, System.Net.EndPoint remoteEndPoint) { throw null; }
        public static System.Threading.Tasks.Task<int> SendAsync(this System.Net.Sockets.Socket socket, System.ArraySegment<byte> buffer, System.Net.Sockets.SocketFlags socketFlags) { throw null; }
        public static System.Threading.Tasks.Task<int> SendAsync(this System.Net.Sockets.Socket socket, System.Collections.Generic.IList<System.ArraySegment<byte>> buffers, System.Net.Sockets.SocketFlags socketFlags) { throw null; }
        public static System.Threading.Tasks.Task<int> SendToAsync(this System.Net.Sockets.Socket socket, System.ArraySegment<byte> buffer, System.Net.Sockets.SocketFlags socketFlags, System.Net.EndPoint remoteEndPoint) { throw null; }
    }
}
