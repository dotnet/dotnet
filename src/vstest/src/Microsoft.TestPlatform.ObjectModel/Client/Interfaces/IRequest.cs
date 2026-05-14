// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;

/// <summary>
/// Represents any request to discover or run tests.
/// </summary>
public interface IRequest : IDisposable
{
    /// <summary>
    ///  Handler for receiving raw messages directly from host without any deserialization or morphing
    ///  This is required if one wants to re-direct the message over the process boundary without any processing overhead
    ///  All events should come as raw messages as well as actual serialized events
    /// </summary>
    event EventHandler<string> OnRawMessageReceived;

    /// <summary>
    /// Waits for the request to complete
    /// </summary>
    /// <param name="timeout">Time out</param>
    /// <returns>True if the request timeouts</returns>
    bool WaitForCompletion(int timeout);
}

/// <summary>
/// Extensions for <see cref="IRequest"/>.
/// </summary>
public static class RequestExtensions
{
    /// <summary>
    /// Waits for the request to complete.
    /// </summary>
    /// <param name="request">Request to wait on.</param>
    public static void WaitForCompletion(this IRequest request)
    {
        request.WaitForCompletion(Timeout.Infinite);
    }
}
