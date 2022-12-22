// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Discovery;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;

namespace Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Adapter;

/// <summary>
/// Provides user specified runSettings and framework provided context of the run.
/// </summary>
public class RunContext : DiscoveryContext, IRunContext
{
    /// <summary>
    /// Gets a value indicating whether the execution process should be kept alive after the run is finished.
    /// </summary>
    public bool KeepAlive { get; internal set; }

    /// <summary>
    /// Gets a value indicating whether the discovery or execution is happening in-process or out-of-process.
    /// </summary>
    public bool InIsolation { get; internal set; }

    /// <summary>
    /// Gets a value indicating whether data collection is enabled.
    /// </summary>
    public bool IsDataCollectionEnabled { get; internal set; }

    /// <summary>
    /// Gets a value indicating whether the test is being debugged.
    /// </summary>
    public bool IsBeingDebugged { get; internal set; }

    /// <summary>
    /// Gets the directory which should be used for storing result files/deployment files etc.
    /// </summary>
    public string? TestRunDirectory { get; internal set; }

    /// <summary>
    /// Gets the directory for Solution.
    /// </summary>
    public string? SolutionDirectory { get; internal set; }
}
