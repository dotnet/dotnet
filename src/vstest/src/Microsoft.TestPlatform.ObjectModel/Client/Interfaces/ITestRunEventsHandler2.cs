// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;

/// <summary>
/// Interface contract for handling test run events during run operation.
/// </summary>
[Obsolete("You don't have to implement this interface, AttachDebuggerToProcess it is never called back. To attach to debugger implement ITestHostLauncher2 or ITestHostLauncher3.")]
// /!\ Possible future interface should not be based on this interface, use ITestRunEventsHandler instead.
public interface ITestRunEventsHandler2 : ITestRunEventsHandler
{
    /// <summary>
    /// Attach debugger to an already running process.
    /// </summary>
    /// <param name="pid">Process ID of the process to which the debugger should be attached.</param>
    /// <returns><see langword="true"/> if the debugger was successfully attached to the requested process, <see langword="false"/> otherwise.</returns>
    [Obsolete("You don't have to implement this it is never called back. To attach to debugger implement ITestHostLauncher2 or ITestHostLauncher3.")]
    bool AttachDebuggerToProcess(int pid);
}
