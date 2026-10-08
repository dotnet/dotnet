// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.TestPlatform.VsTestConsole.TranslationLayer.Interfaces;

/// <summary>
/// Defines the interface that can manage a process
/// </summary>
internal interface IProcessManager
{
    /// <summary>
    /// Starts the Process
    /// </summary>
    void StartProcess(ConsoleParameters consoleParameters);

    /// <summary>
    /// Is Process Initialized
    /// </summary>
    /// <returns>True, if process initialized</returns>
    bool IsProcessInitialized();

    /// <summary>
    /// Shutdown Process
    /// </summary>
    void ShutdownProcess();

    /// <summary>
    /// Raise event on process exit
    /// </summary>
    event EventHandler ProcessExited;

    /// <summary>
    /// Start time captured immediately from the process that was launched.
    /// </summary>
    DateTime? ProcessStartTimeUtc { get; }

    /// <summary>
    /// Gets a value indicating whether the vstest.console this manager launches performs the
    /// connection identity check. Because we start the console ourselves and know its path, we can
    /// read the marker compiled into it, which is a fact about that build rather than a prediction
    /// about the patch number it shipped as. A console that does not carry the marker is refused,
    /// because there is no unauthenticated mode to fall back to.
    /// </summary>
    bool SupportsConnectionSecrets { get; }

    /// <summary>
    /// Gets the product version of the vstest.console this manager launches, used to tell the user
    /// which version was found when it is too old. Null when the version could not be determined.
    /// </summary>
    string? ProcessVersion { get; }

    /// <summary>
    /// Gets the full path of the vstest.console this manager launches, so the user is told which
    /// installation was used rather than being left to guess between the standalone package, the
    /// .NET SDK and Visual Studio. Null when the path is not known.
    /// </summary>
    string? ProcessPath { get; }
}
