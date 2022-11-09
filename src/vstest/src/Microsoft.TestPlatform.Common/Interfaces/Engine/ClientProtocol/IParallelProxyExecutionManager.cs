// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine;

/// <summary>
/// Interface defining the parallel execution manager
/// </summary>
public interface IParallelProxyExecutionManager : IProxyExecutionManager
{
    /// <summary>
    /// Handles Partial Run Complete event coming from a specific concurrent proxy execution manager
    /// Each concurrent proxy execution manager will signal the parallel execution manager when its complete
    /// </summary>
    /// <param name="proxyExecutionManager">Concurrent Execution manager that completed the run</param>
    /// <param name="testRunCompleteArgs">RunCompleteArgs for the concurrent run</param>
    /// <param name="lastChunkArgs">LastChunk testresults for the concurrent run</param>
    /// <param name="runContextAttachments">RunAttachments for the concurrent run</param>
    /// <param name="executorUris">ExecutorURIs of the adapters involved in executing the tests</param>
    /// <returns>True if parallel run is complete</returns>
    bool HandlePartialRunComplete(
        IProxyExecutionManager proxyExecutionManager,
        TestRunCompleteEventArgs testRunCompleteArgs,
        TestRunChangedEventArgs? lastChunkArgs,
        ICollection<AttachmentSet>? runContextAttachments,
        ICollection<string>? executorUris);
}
