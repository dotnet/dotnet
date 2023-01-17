// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.Client.Interfaces;

/// <summary>
/// Interface defining contract for custom test host launcher implementation.
/// </summary>
public interface ITestHostLauncher2 : ITestHostLauncher
{
    /// <summary>
    /// Attach debugger to already running custom test host process.
    /// </summary>
    /// <param name="pid">Process ID of the process to which the debugger should be attached.</param>
    /// <returns><see langword="true"/> if the debugger was successfully attached to the requested process, <see langword="false"/> otherwise.</returns>
    bool AttachDebuggerToProcess(int pid);

    /// <summary>
    /// Attach debugger to already running custom test host process.
    /// </summary>
    /// <param name="pid">Process ID of the process to which the debugger should be attached.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns><see langword="true"/> if the debugger was successfully attached to the requested process, <see langword="false"/> otherwise.</returns>
    bool AttachDebuggerToProcess(int pid, CancellationToken cancellationToken);
}
