// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Interfaces;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;

internal static class TcpClientExtensions
{
    // Timeout for polling stream in micro seconds.
    private const int Streamreadtimeout = 1000 * 1000;

    internal static Task MessageLoopAsync(
        this TcpClient client,
        ICommunicationChannel channel,
        Action<Exception?> errorHandler,
        CancellationToken cancellationToken)
    {
        Exception? error = null;

        var remoteEndPoint = string.Empty;
        var localEndPoint = string.Empty;
        try
        {
            remoteEndPoint = client.Client.RemoteEndPoint?.ToString();
            localEndPoint = client.Client.LocalEndPoint?.ToString();
        }
        catch (SocketException socketException)
        {
            EqtTrace.Error(
                "TcpClientExtensions.MessageLoopAsync: Failed to access the endpoint due to socket error: {0}",
                socketException);
        }

        // PERF: check if verbose is enabled once, and re-use for all calls in the tight loop below. The check for verbose is shows in perf traces
        // below and we are wasting resources re-checking when user does not have it open. Downside of this is that if you change the verbosity level
        // during runtime (e.g. in VS options), you won't update here. Which is imho an okay tradeoff.
        var isVerboseEnabled = EqtTrace.IsVerboseEnabled;

        var sw = Stopwatch.StartNew();
        // Set read timeout to avoid blocking receive raw message
        while (channel != null && !cancellationToken.IsCancellationRequested)
        {
            if (isVerboseEnabled)
            {
                EqtTrace.Verbose("TcpClientExtensions.MessageLoopAsync: Polling on remoteEndPoint: {0} localEndPoint: {1} after {2} ms", remoteEndPoint, localEndPoint, sw.ElapsedMilliseconds);
                sw.Restart();
            }

            try
            {
                if (client.Client.Poll(Streamreadtimeout, SelectMode.SelectRead))
                {
                    if (isVerboseEnabled)
                    {
                        EqtTrace.Verbose("TcpClientExtensions.MessageLoopAsync: NotifyDataAvailable remoteEndPoint: {0} localEndPoint: {1}", remoteEndPoint, localEndPoint);
                    }
                    channel.NotifyDataAvailable(cancellationToken);
                }
            }
            catch (IOException ioException)
            {
                if (ioException.InnerException is SocketException socketException
                    && socketException.SocketErrorCode == SocketError.TimedOut)
                {
                    EqtTrace.Info(
                        "Socket: Message loop: failed to receive message due to read timeout {0}, remoteEndPoint: {1} localEndPoint: {2}",
                        ioException,
                        remoteEndPoint,
                        localEndPoint);
                }
                else
                {
                    EqtTrace.Error(
                        "Socket: Message loop: failed to receive message due to socket error {0}, remoteEndPoint: {1} localEndPoint: {2}",
                        ioException,
                        remoteEndPoint,
                        localEndPoint);
                    // Do not pass the error to the caller, the transport was torn down because testhost
                    // disconnected, in 99% of the cases. This error ends up confusing developers
                    // even though it just means "testhost crashed", look at testhost to see what happened.
                    // https://github.com/microsoft/vstest/issues/4461
                    // error = ioException;
                    break;
                }
            }
            catch (Exception exception)
            {
                EqtTrace.Error(
                    "Socket: Message loop: failed to receive message {0}, remoteEndPoint: {1} localEndPoint: {2}",
                    exception,
                    remoteEndPoint,
                    localEndPoint);
                error = exception;
                break;
            }
        }

        // Try clean up and raise client disconnected events
        errorHandler(error);

        EqtTrace.Verbose("TcpClientExtensions.MessageLoopAsync: exiting MessageLoopAsync remoteEndPoint: {0} localEndPoint: {1}", remoteEndPoint, localEndPoint);

        return Task.FromResult(0);
    }

    /// <summary>
    /// Converts a given string endpoint address to valid Ipv4, Ipv6 IPEndpoint
    /// </summary>
    /// <param name="value">Input endpoint address</param>
    /// <returns>IPEndpoint from give string, if its not a valid string. It will create endpoint with loop back address with port 0</returns>
    internal static IPEndPoint GetIpEndPoint(this string? value)
    {
        return Uri.TryCreate(string.Concat("tcp://", value), UriKind.Absolute, out Uri? uri)
            ? new IPEndPoint(IPAddress.Parse(uri.Host), uri.Port < 0 ? 0 : uri.Port)
            : new IPEndPoint(IPAddress.Loopback, 0);
    }
}
