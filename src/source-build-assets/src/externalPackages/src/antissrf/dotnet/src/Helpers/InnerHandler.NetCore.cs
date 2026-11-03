// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

#if NET5_0_OR_GREATER

using System.Net;
using System.Net.Http;
using System.Net.Sockets;

namespace Microsoft.Security.AntiSSRF
{
    internal class InnerHandler
    {
        internal static SocketsHttpHandler GetHandler(AntiSSRFPolicy policy)
        {
            return new SocketsHttpHandler()
            {
                AllowAutoRedirect = false,
                ConnectCallback = async (ConnectionContext, CancellationToken) =>
                {
                    IPAddress[] resolvedIPs = await Dns.GetHostAddressesAsync(ConnectionContext.DnsEndPoint.Host, CancellationToken);
                    if (resolvedIPs.Length == 0)
                        throw new AntiSSRFException($"DNS lookup failed for {ConnectionContext.DnsEndPoint.Host}.");

                    if (!policy.IsNetworkConnectionAllowed(resolvedIPs))
                        throw new AntiSSRFException($"The connection to {ConnectionContext.DnsEndPoint.Host} is not allowed per policy.");

                    Socket socket = new(SocketType.Stream, ProtocolType.Tcp) { NoDelay = true };
                    try
                    {
                        await socket.ConnectAsync(resolvedIPs, ConnectionContext.DnsEndPoint.Port, CancellationToken);
                        return new NetworkStream(socket, ownsSocket: true);
                    }
                    catch
                    {
                        socket.Dispose();
                        throw;
                    }
                }
            };
        }
    }
}

#endif