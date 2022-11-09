// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Interfaces;

/// <summary>
/// Provides information on disconnection of a communication channel.
/// </summary>
public class DisconnectedEventArgs : EventArgs
{
    /// <summary>
    /// Gets or sets if there's an error on disconnection.
    /// </summary>
    public Exception? Error { get; set; }
}
