// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.Client.Interfaces;

public interface ITestHostLauncher3 : ITestHostLauncher2
{
    /// <summary>
    /// Attach debugger to already running custom test host process.
    /// </summary>
    /// <param name="attachDebuggerInfo">Process ID and target framework of the process to which the debugger should be attached.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns><see langword="true"/> if the debugger was successfully attached to the requested process, <see langword="false"/> otherwise.</returns>
    bool AttachDebuggerToProcess(AttachDebuggerInfo attachDebuggerInfo, CancellationToken cancellationToken);
}
