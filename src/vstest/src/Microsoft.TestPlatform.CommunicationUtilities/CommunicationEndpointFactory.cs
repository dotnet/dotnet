// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Interfaces;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;

/// <summary>
/// Implements ICommunicationEndpointFactory.
/// </summary>
public class CommunicationEndpointFactory : ICommunicationEndpointFactory
{
    /// <inheritdoc />
    public ICommunicationEndPoint Create(ConnectionRole role)
    {
        ICommunicationEndPoint endPoint = role == ConnectionRole.Host ? new SocketServer() : new SocketClient();
        return endPoint;
    }
}
