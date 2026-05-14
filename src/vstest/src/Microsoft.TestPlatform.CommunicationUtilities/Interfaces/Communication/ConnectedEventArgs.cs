// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Interfaces;

/// <summary>
/// Provides properties for the connected communication channel.
/// </summary>
public class ConnectedEventArgs : EventArgs
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ConnectedEventArgs"/> class.
    /// </summary>
    // TODO: Do we need this constructor?
    public ConnectedEventArgs()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ConnectedEventArgs"/> class.
    /// </summary>
    /// <param name="channel">Communication channel for this connection.</param>
    public ConnectedEventArgs(ICommunicationChannel channel)
    {
        Channel = channel;
        Connected = true;
    }

    public ConnectedEventArgs(Exception? faultException)
    {
        Connected = false;
        Fault = faultException;
    }

    /// <summary>
    /// Gets the communication channel based on this connection.
    /// </summary>
    public ICommunicationChannel? Channel { get; private set; }

    /// <summary>
    /// Gets a value indicating whether channel is connected or not, true if it's connected.
    /// </summary>
    [MemberNotNullWhen(true, nameof(Channel))]
    [MemberNotNullWhen(false, nameof(Fault))]
    public bool Connected { get; private set; }

    /// <summary>
    /// Gets the exception if it's not connected.
    /// </summary>
    public Exception? Fault { get; private set; }
}
