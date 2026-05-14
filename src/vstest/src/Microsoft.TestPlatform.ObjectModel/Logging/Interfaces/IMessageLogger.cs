// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;

/// <summary>
/// Used for logging error warning and informational messages.
/// </summary>
public interface IMessageLogger
{
    /// <summary>
    /// Sends a message to the enabled loggers.
    /// </summary>
    /// <param name="testMessageLevel">Level of the message.</param>
    /// <param name="message">The message to be sent.</param>
    void SendMessage(TestMessageLevel testMessageLevel, string message);

}
