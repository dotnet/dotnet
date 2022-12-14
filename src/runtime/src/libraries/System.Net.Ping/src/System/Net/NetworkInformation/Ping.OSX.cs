// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.NetworkInformation
{
    public partial class Ping
    {
        private static bool SendIpHeader => true;
        private static bool NeedsConnect => false;
        private static bool SupportsDualMode => false;

        private static PingReply SendPingCore(IPAddress address, byte[] buffer, int timeout, PingOptions? options)
            => SendIcmpEchoRequestOverRawSocket(address, buffer, timeout, options);

        private Task<PingReply> SendPingAsyncCore(IPAddress address, byte[] buffer, int timeout, PingOptions? options)
            => SendIcmpEchoRequestOverRawSocketAsync(address, buffer, timeout, options);
    }
}
