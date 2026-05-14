// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.Host;

/// <summary>
/// Interface to define a TestRuntimeProvider with support for attaching the debugger to the
/// default testhost process.
/// </summary>
public interface ITestRuntimeProvider2 : ITestRuntimeProvider
{
    /// <summary>
    /// Attach the debugger to an already running testhost process.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> if the debugger was successfully attached to the running testhost
    /// process, <see langword="false"/> otherwise.
    /// </returns>
    bool AttachDebuggerToTestHost();
}
