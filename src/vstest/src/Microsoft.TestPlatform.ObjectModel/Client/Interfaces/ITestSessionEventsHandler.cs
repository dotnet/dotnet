// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;

/// <summary>
/// Interface contract for handling test session events.
/// </summary>
public interface ITestSessionEventsHandler : ITestMessageEventHandler
{
    /// <summary>
    /// Dispatch StartTestSession complete event to listeners.
    /// </summary>
    ///
    /// <param name="eventArgs">The event args.</param>
    void HandleStartTestSessionComplete(StartTestSessionCompleteEventArgs? eventArgs);

    /// <summary>
    /// Dispatch StopTestSession complete event to listeners.
    /// </summary>
    ///
    /// <param name="eventArgs">The event args.</param>
    void HandleStopTestSessionComplete(StopTestSessionCompleteEventArgs? eventArgs);
}
