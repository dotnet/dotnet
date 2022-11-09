// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Net;

using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Interfaces;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;

/// <inheritdoc/>
public sealed class SocketTransport : ITransport
{
    private readonly TestHostConnectionInfo _connectionInfo;
    private readonly ICommunicationManager _communicationManager;

    /// <summary>
    /// Specifies whether the resolver is disposed or not
    /// </summary>
    private bool _disposed;

    public SocketTransport(ICommunicationManager communicationManager, TestHostConnectionInfo connectionInfo)
    {
        _communicationManager = communicationManager;
        _connectionInfo = connectionInfo;
    }

    /// <inheritdoc/>
    public IPEndPoint Initialize()
    {
        var endpoint = GetIpEndPoint(_connectionInfo.Endpoint);
        TPDebug.Assert(endpoint is not null, "endpoint is null");
        switch (_connectionInfo.Role)
        {
            case ConnectionRole.Host:
                {
                    // In case users passes endpoint Port as 0 HostServer will allocate endpoint at appropriate port,
                    // So reassign endpoint to point to correct endpoint.
                    endpoint = _communicationManager.HostServer(endpoint);
                    _communicationManager.AcceptClientAsync();
                    return endpoint;
                }

            case ConnectionRole.Client:
                {
                    _communicationManager.SetupClientAsync(endpoint);
                    return endpoint;
                }

            default:
                throw new NotImplementedException("Unsupported Connection Role");
        }
    }

    /// <inheritdoc/>
    public bool WaitForConnection(int connectionTimeout)
    {
        return _connectionInfo.Role == ConnectionRole.Client ? _communicationManager.WaitForServerConnection(connectionTimeout) : _communicationManager.WaitForClientConnection(connectionTimeout);
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        if (_connectionInfo.Role == ConnectionRole.Client)
        {
            _communicationManager.StopClient();
        }
        else
        {
            _communicationManager.StopServer();
        }

        _disposed = true;
    }

    /// <summary>
    /// Converts a given string endpoint address to valid Ipv4, Ipv6 IPEndpoint
    /// </summary>
    /// <param name="endpointAddress">Input endpoint address</param>
    /// <returns>IPEndpoint from give string</returns>
    private static IPEndPoint? GetIpEndPoint(string endpointAddress)
    {
        return Uri.TryCreate(string.Concat("tcp://", endpointAddress), UriKind.Absolute, out Uri? uri)
            ? new IPEndPoint(IPAddress.Parse(uri.Host), uri.Port < 0 ? 0 : uri.Port)
            : null;
    }
}
