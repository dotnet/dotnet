// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualStudio.TestPlatform.Utilities;

/// <summary>
/// Interface to output information under the command line.
/// </summary>
public interface IOutput
{
    /// <summary>
    /// Writes the message with a new line.
    /// </summary>
    /// <param name="message">Message to be output.</param>
    /// <param name="level">Level of the message.</param>
    void WriteLine(string? message, OutputLevel level);

    /// <summary>
    /// Writes the message with no new line.
    /// </summary>
    /// <param name="message">Message to be output.</param>
    /// <param name="level">Level of the message.</param>
    void Write(string? message, OutputLevel level);
}
