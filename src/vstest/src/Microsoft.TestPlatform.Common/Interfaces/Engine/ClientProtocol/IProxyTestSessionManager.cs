// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine;

/// <summary>
/// Orchestrates test session related functionality for the engine communicating with the
/// client.
/// </summary>
public interface IProxyTestSessionManager
{
    /// <summary>
    /// Starts the test session based on the test session criteria.
    /// </summary>
    ///
    /// <param name="eventsHandler">
    /// Event handler for handling events fired during test session management operations.
    /// </param>
    /// <param name="requestData">The request data.</param>
    ///
    /// <returns>True if the operation succeeded, false otherwise.</returns>
    bool StartSession(ITestSessionEventsHandler eventsHandler, IRequestData requestData);

    /// <summary>
    /// Stops the test session.
    /// </summary>
    ///
    /// <param name="requestData">The request data.</param>
    ///
    /// <returns>True if the operation succeeded, false otherwise.</returns>
    bool StopSession(IRequestData requestData);
}
